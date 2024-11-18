namespace CSharpTest.Application
{
    public class WinLineInfo
    {
        public int LineIndex { get; set; }
        public int StartLocation { get; set; }
        public int Score { get; set; }

        public WinLineInfo(int lineIndex, int startLocation)
        {
            LineIndex = lineIndex;
            StartLocation = startLocation;
        }
    }
}
