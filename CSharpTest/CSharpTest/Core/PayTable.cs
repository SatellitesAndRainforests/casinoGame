namespace CSharpTest.Core
{
    public class PayTable
    {

        private readonly Dictionary<ReelSymbol, Dictionary<int, int>> _table = new()
        {
            { ReelSymbol.Ten, new() {
                    { 3, 2 },
                    { 4, 5 },
                    { 5, 10 },
                }
            },
            { ReelSymbol.Jack, new() {
                    { 3, 10 },
                    { 4, 15 },
                    { 5, 20 },
                }
            },
            { ReelSymbol.King, new() {
                    { 3, 20 },
                    { 4, 30 },
                    { 5, 50 },
                }
            },
            { ReelSymbol.Wild, new() {
                    { 3, 20 },
                    { 4, 30 },
                    { 5, 50 },
                }
            },
        };

        public bool TableContains(ReelSymbol symbol, int consecutiveCount)
        {
            return _table.ContainsKey(symbol) && _table[symbol].ContainsKey(consecutiveCount);
        }

        public int GetPayout(ReelSymbol symbol, int consecutiveCount)
        {
            if (TableContains(symbol, consecutiveCount)) return _table[symbol][consecutiveCount];
            else return 0;
        }

    }
}
