namespace GedcomParser
{
    public static class BlockTypeSexExtensions
    {
        public static IndividualSex ToIndividualSex(this BlockTypeSex value) =>
        value switch
        {
            BlockTypeSex.M => IndividualSex.Male,
            BlockTypeSex.F => IndividualSex.Female,
            _ => IndividualSex.Unknown,
        };
    }

    public enum IndividualSex
    {
        Unknown,
        Male,
        Female
    }
}
