namespace CSharpTest.Configuration
{
    public class Probabilities
    {

        private readonly Random _random;
        private static readonly int[] _possibleCoinsInView = { 0, 0, 0, 0, 1, 2, 3, 4 };    // 50 % for 0,  12.5 % for other values
        private readonly ReelSymbol[] _initialReelSymbols = { ReelSymbol.Jack, ReelSymbol.King, ReelSymbol.Wild, ReelSymbol.Ten }; // Exclude Coin and None

        public Probabilities()
        {
            _random = new Random();
        }

        public ReelSymbol GetRandomInitialReelSymbolExcludingCoinAndNone()
        {
            int randomIndex = _random.Next(0, _initialReelSymbols.Length);
            return _initialReelSymbols[randomIndex];
        }

        public int GenerateRandomCoinsInView()
        {
            int randomIndex = _random.Next(0, _possibleCoinsInView.Length);
            return _possibleCoinsInView[randomIndex];
        }

        public int GenerateRandomCoinStackHeight(int coinsInView)
        {
            if (coinsInView == 0) return 0;

            else if (coinsInView >= 1 && coinsInView <= 3) return _random.Next(3, 8);
            else if (coinsInView == 4) return _random.Next(4, 8);

            return 0;
        }

        public bool ApplyCoinStackFromTopFirstOrBottomFirst()
        {
            return _random.Next(0, 2) == 1;
        }

    }
}
