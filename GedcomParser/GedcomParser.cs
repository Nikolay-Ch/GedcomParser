// See https://aka.ms/new-console-template for more information

using System.IO.Compression;

namespace GedcomParser
{
    public class GedcomParser
    {
        public const string GedcomFileNameInZip = "gedcom.ged";
        public List<Individual> Individuals { get; } = [];
        public List<Family> Families { get; } = [];

        public void ParseGedcomZip(string fileName)
        {
            var zip = ZipFile.OpenRead(fileName);
            var entry = zip.GetEntry(GedcomFileNameInZip)
                ?? throw new FileNotFoundException("File not found!", GedcomFileNameInZip);

            using var fs = entry.Open();
            ParseGedcom(fs);
        }

        public void ParseGedcom(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Open);
            ParseGedcom(fs);
        }

        protected void ParseGedcom(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var readerAdapter = new StreamReaderPeekLineAdapter(reader);

            var blocks = new List<GedcomBlock>();
            while (!readerAdapter.EndOfStream)
            {
                var block = ReadBlock(readerAdapter);
                if (block != null)
                    blocks.Add(block);
            }

            CreateIndividuals(blocks);

            CreateFamilies(blocks);

            FillFamilies(blocks);
        }

        private void CreateIndividuals(List<GedcomBlock> blocks)
        {
            foreach (var block in blocks.Where(e => e.BlockType == BlockType.INDI))
            {
                var indi = new Individual(
                    block.Id(),
                    block.GivenName(),
                    block.Surname(),
                    block.Sex(),
                    block.BirthDate(),
                    block.DeathDate()
                );

                Individuals.Add(indi);
            }
        }

        private void CreateFamilies(List<GedcomBlock> blocks)
        {
            foreach (var block in blocks.Where(e => e.BlockType == BlockType.FAM))
            {
                var family = new Family(
                    block.Id(),
                    block.FatherId() != "" ? Individuals.First(e => e.Id == block.FatherId()) : null,
                    block.MotherId() != "" ? Individuals.First(e => e.Id == block.MotherId()) : null,
                    block.MarriageDate(),
                    Individuals.Where(e => block.ChildIds().Contains(e.Id)).ToList()
                );

                Families.Add(family);
            }
        }

        private void FillFamilies(List<GedcomBlock> blocks)
        {
            foreach (var block in blocks.Where(e => e.BlockType == BlockType.INDI))
            {
                var indi = Individuals.Find(e => e.Id == block.Id())!;

                indi.FamilyChild = Families.FirstOrDefault(e => e.Id == block.FamilyChildId());
                indi.FamilySpouse = Families.Where(e => block.FamilySpouseIds().Contains(e.Id)).ToList();
            }
        }

        private GedcomBlock? ReadBlock(StreamReaderPeekLineAdapter reader, int level = 0)
        {
            var blockLines = new List<string>();
            do
            {
                blockLines.Add(reader.ReadLine() ?? "");

            } while (!reader.EndOfStream&& int.Parse(reader.PeekLine()?.Split(' ')[0]!) != level);

            return new GedcomBlockTreeCreator().ParseBlock(blockLines, 0);
        }
    }
}
