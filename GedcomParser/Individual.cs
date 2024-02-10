namespace GedcomParser
{
    public record Individual(
        string Id,
        string GivenName,
        string SurName,
        IndividualSex Sex,
        string BirthDate,
        string DeathDate)
    {
        public Family? FamilyChild { get; set; }
        public List<Family> FamilySpouse { get; set; } = [];
    }
}
