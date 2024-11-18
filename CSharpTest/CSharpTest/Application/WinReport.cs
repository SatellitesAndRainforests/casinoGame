namespace CSharpTest.Application
{
    public class WinReport
    {
        public int SpinScore { get; set; }
        public long GameScore { get; set; }
        public IList<TopScoringLine>? TopScoringLines { get; set; }
    }
}
