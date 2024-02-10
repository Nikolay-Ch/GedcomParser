using GedcomParser;

var parser = new GedcomParser.GedcomParser();
parser.ParseGedcomZip(@"Example.gdz");

var Individuals = new List<Individual>(parser.Individuals);
