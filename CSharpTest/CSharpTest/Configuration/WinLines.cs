namespace CSharpTest.Configuration
{
    public class WinLines
    {

        private readonly int[][] _winLines =
            [
                [0, 0, 0, 0, 0],
                [1, 1, 1, 1, 1],
                [2, 2, 2, 2, 2],
                [3, 3, 3, 3, 3]
            ];

        public int[][] Lines => _winLines;

        public ReelSymbol[][] InitEmptyWinLineMatrix()
        {

            ReelSymbol[][] emptyWinLineMatrix = new ReelSymbol[_winLines.Length][];

            for (int i = 0; i < _winLines.Length; i++)
            {
                emptyWinLineMatrix[i] = new ReelSymbol[_winLines[i].Length];
            }

            return emptyWinLineMatrix;

        }

    }
}
