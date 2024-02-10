namespace GedcomParser
{
    /// <summary>
    /// Extension to allow peek one line for StreamReader
    /// based on answer https://stackoverflow.com/a/842554
    /// </summary>
    public class StreamReaderPeekLineAdapter(StreamReader underlying)
    {
        private readonly StreamReader Underlying = underlying;
        private readonly Queue<string> BufferedLines = new();

        public bool EndOfStream => BufferedLines.Count == 0 && Underlying.EndOfStream;

        public string? PeekLine()
        {
            string? line = Underlying.ReadLine();
            if (line != null)
                BufferedLines.Enqueue(line);

            return line;
        }

        public string? ReadLine()
        {
            if (BufferedLines.Count > 0)
                return BufferedLines.Dequeue();

            return Underlying.ReadLine();
        }

    }
}
