namespace GedcomParser
{
    public static class GedcomTreeExtensionMethods
    {
        public static string GivenName(this GedcomBlock indiBlock) =>
            ((GedcomBlockWithText?)indiBlock
                .SubBlocks.First(e => e.BlockType == BlockType.NAME)
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.GIVN))?.Text ?? "";

        public static string Surname(this GedcomBlock indiBlock) =>
            ((GedcomBlockWithText?)indiBlock
                .SubBlocks.First(e => e.BlockType == BlockType.NAME)
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.SURN))?.Text ?? "";

        public static string Id(this GedcomBlock indiBlock) => ((GedcomBlockWithId)indiBlock).Id;

        public static IndividualSex Sex(this GedcomBlock indiBlock) =>
            ((GedcomBlockSex?)indiBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.SEX))?.Sex.ToIndividualSex() ?? IndividualSex.Unknown;

        public static string BirthDate(this GedcomBlock indiBlock) =>
            ((GedcomBlockWithText?)indiBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.BIRT)
                ?.SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.DATE))?.Text ?? "";

        public static string DeathDate(this GedcomBlock indiBlock) =>
            ((GedcomBlockWithText?)indiBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.DEAT)
                ?.SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.DATE))?.Text ?? "";

        public static string FamilyChildId(this GedcomBlock indiBlock) =>
            ((GedcomBlockWithId?)indiBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.FAMC))?.Id ?? "";

        public static IEnumerable<string> FamilySpouseIds(this GedcomBlock indiBlock) =>
            indiBlock
                .SubBlocks.Where(e => e.BlockType == BlockType.FAMS).Select(e => ((GedcomBlockWithId)e).Id);

        public static string FatherId(this GedcomBlock famBlock) =>
            ((GedcomBlockWithId?)famBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.HUSB))?.Id ?? "";

        public static string MotherId(this GedcomBlock famBlock) =>
            ((GedcomBlockWithId?)famBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.WIFE))?.Id ?? "";

        public static IEnumerable<string> ChildIds(this GedcomBlock famBlock) =>
            famBlock
                .SubBlocks.Where(e => e.BlockType == BlockType.CHIL).Select(e => ((GedcomBlockWithId)e).Id);

        public static string MarriageDate(this GedcomBlock famBlock) =>
            ((GedcomBlockWithText?)famBlock
                .SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.MARR)
                ?.SubBlocks.FirstOrDefault(e => e.BlockType == BlockType.DATE))?.Text ?? "";
    }
}
