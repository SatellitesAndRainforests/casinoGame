using CSharpTest.Configuration;

namespace CSharpTest.Application
{
    public class TopScoringLine
    {
        public int Row { get; set; }
        public int Score { get; set; }
        public int StartingIndex { get; set; }                  // planned to use in win highlight animation
        public List<ReelSymbol> Symbols { get; set; } = [];
    }
}
