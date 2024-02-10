namespace GedcomParser
{
    public record IndividualFrame(
        int Top,
        int Left,
        int Width,
        int Height,
        string Text,
        Individual Individual
    );
}