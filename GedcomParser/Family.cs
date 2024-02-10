namespace GedcomParser
{
    public record Family (
        string Id,
        Individual? Husband,
        Individual? Wife,
        string MarriageDate,
        List<Individual> ChildrenIds
    );
}
