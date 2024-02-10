namespace GedcomParser
{
    public enum BlockType
    {
        None,
        INDI,
        FAM,
        FAMC,
        FAMS,
        NAME,
        TYPE,
        GIVN,
        SURN,
        SEX,
        BIRT,
        DEAT,
        DATE,
        PLAC,
        HUSB,
        WIFE,
        CHIL,
        MARR,
        DIV
    }

    public enum BlockTypeName
    {
        None,
        BIRTH,
        MARRIED,
        MARRIAGE
    }

    public enum BlockTypeSex
    {
        U,
        M,
        F
    }

    public class GedcomBlock
    {
        public required BlockType BlockType { get; init; }

        public List<GedcomBlock> SubBlocks { get; } = [];
    }

    public class GedcomBlockWithId : GedcomBlock
    {
        public required string Id { get; init; }
    }

    public class GedcomBlockWithText : GedcomBlock
    {
        public required string Text { get; init; }
    }

    public class GedcomBlockTypeName : GedcomBlock
    {
        public required BlockTypeName TypeName { get; init; }
    }

    public class GedcomBlockSex : GedcomBlock
    {
        public required BlockTypeSex Sex { get; init; }
    }

    public class GedcomBlockTreeCreator
    {
        private BlockType GetBlockType(string value)
        {
            _ = Enum.TryParse<BlockType>(value, out var blockType);
            return blockType;
        }

        public GedcomBlock? ParseBlock(List<string> blockLines, int level)
        {
            // parse block type
            var topBlockLine = blockLines[0].Split();
            var blockType = GetBlockType(topBlockLine[1]);
            if (blockType == BlockType.None && topBlockLine.Length > 2)
                blockType = GetBlockType(topBlockLine[2]);

            blockLines.RemoveAt(0);

            GedcomBlock? retVal = null;

            switch (blockType)
            {
                case BlockType.None:
                    break;
                case BlockType.INDI:
                case BlockType.FAM:
                    retVal = new GedcomBlockWithId { BlockType = blockType, Id = topBlockLine[1].Trim('@') };
                    break;
                case BlockType.FAMC:
                case BlockType.FAMS:
                case BlockType.HUSB:
                case BlockType.WIFE:
                case BlockType.CHIL:
                    retVal = new GedcomBlockWithId { BlockType = blockType, Id = topBlockLine[2].Trim('@') };
                    break;
                case BlockType.NAME:
                case BlockType.GIVN:
                case BlockType.SURN:
                case BlockType.DATE:
                case BlockType.PLAC:
                    retVal = new GedcomBlockWithText { BlockType = blockType, Text = String.Join(' ', topBlockLine[2..]) };
                    break;
                case BlockType.TYPE:
                    _ = Enum.TryParse<BlockTypeName>(topBlockLine[2], true, out var typeName);
                    retVal = new GedcomBlockTypeName { BlockType = blockType, TypeName = typeName };
                    break;
                case BlockType.SEX:
                    _ = Enum.TryParse<BlockTypeSex>(topBlockLine[2], out var sex);
                    retVal = new GedcomBlockSex { BlockType = blockType, Sex = sex };
                    break;
                case BlockType.BIRT:
                case BlockType.DEAT:
                case BlockType.MARR:
                case BlockType.DIV:
                    retVal = new GedcomBlock { BlockType = blockType };
                    break;
                default:
                    break;
            }

            ParseSubBlocks(blockLines, level, retVal);

            return retVal;
        }

        private void ParseSubBlocks(List<string> blockLines, int level, GedcomBlock? block)
        {
            for (int i = 0; i < blockLines.Count;)
            {
                List<string> subBlockLines = [];
                do
                {
                    subBlockLines.Add(blockLines[i++]);

                } while (i != blockLines.Count && int.Parse(blockLines[i]?.Split()[0]!) != level + 1);

                var subBlock = ParseBlock(subBlockLines, level + 1);
                if (subBlock != null)
                    block?.SubBlocks.Add(subBlock);
            }
        }
    }
}
