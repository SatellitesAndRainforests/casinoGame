
using CSharpTest.Core.Constants;
using CSharpTest.Core.Interfaces;
using CSharpTest.Core.Ui;
using System.Threading.Tasks.Sources;

namespace CSharpTest.Core
{
    public class Game
    {

        private ReelStopWindow _window = new ReelStopWindow();
        private Random _random = new Random();
        private PayTable _payTable = new PayTable();
        private int[][] _winLines = [[0, 0, 0, 0, 0], [1, 1, 1, 1, 1], [2, 2, 2, 2, 2], [3, 3, 3, 3, 3]];
        private IVisualIO _visualIo = new VisualIO();
        private IGameWin _gameWin = new GameWin(0, true);



        private void IntroMessagePrint()
        {
            _visualIo.DisplayMessage("welcome");
        }

        private void InitReelsRandom()
        {
            for (int reel = 0; reel < _window.ReelCount; reel++)
            {
                for (int ofset = 0; ofset < _window.ReelHeight; ofset++)
                {
                    int randomInt = _random.Next(1, 5);      // check < - - 
                    _window.SetSymbolAt(reel, ofset, (ReelSymbol)randomInt);
                }
            }
        }

        //private void PrintReelsVertical()
        //{
        //    Console.WriteLine("");

        //    string[] rowStrings = new string[_window.ReelHeight];

        //    for (int row = 0; row < _window.ReelHeight; row++)
        //    {
        //        for (int columns = 0; columns < _window.ReelCount; columns++)
        //        {
        //            rowStrings[row] += _window.GetSymbolAt(columns, row).ToString() + "\t";
        //        }
        //    }
        //    foreach (string row in rowStrings) Console.WriteLine(row);
        //}

        private void ApplyCoinStacksToReels()
        {

            for (int reel = 0; reel < _window.ReelCount; reel++)
            {
                int coinsInView = GenerateCoinsInView();
                int coinStackHeight = GenerateCoinStackHeight(coinsInView);
                bool fromTop = EqualProbabilityTryApplyCoinStackFromTopFirstOrBottomFirst();

                //Console.WriteLine();
                //Console.WriteLine("coins in view  : " + coinsInView);
                //Console.WriteLine("coins in height: " + coinStackHeight);
                //Console.WriteLine("from top       : " + fromTop);

                if (fromTop)
                {
                    if (TryApplyToTopOfReel(reel, coinsInView)) continue;
                    else if (TryApplyToBottomOfReel(reel, coinsInView)) continue;
                    else if (TryApplyToMiddleOfReelFromTop(reel, coinsInView, coinStackHeight)) continue;
                    else continue;
                    // else throw new GameException("could not apply");
                }
                else
                {
                    if (TryApplyToBottomOfReel(reel, coinsInView)) continue;
                    else if (TryApplyToTopOfReel(reel, coinsInView)) continue;
                    else if (TryApplyToMiddleOfReelFromBottom(reel, coinsInView, coinStackHeight)) continue;
                    else continue;
                    // else throw new GameException("could not apply");
                }

            }

        }

        private bool EqualProbabilityTryApplyCoinStackFromTopFirstOrBottomFirst()
        {
            return _random.Next(0, 2) == 1;
        }

        private bool TryApplyToTopOfReel(int reel, int coinsInView)
        {
            for (int i = 0; i < coinsInView; i++)
            {
                if (_window.GetSymbolAt(reel, i) == ReelSymbol.Wild) return false;
            }

            for (int i = 0; i < coinsInView; i++)
            {
                _window.SetSymbolAt(reel, i, ReelSymbol.Coin);
            }

            return true;
        }

        private bool TryApplyToBottomOfReel(int reel, int coinsInView)
        {

            for (int i = 0; i < coinsInView; i++)
            {
                if (_window.GetSymbolAt(reel, (_window.ReelHeight - 1 - i)) == ReelSymbol.Wild) return false;
            }

            for (int i = 0; i < coinsInView; i++)
            {
                _window.SetSymbolAt(reel, (_window.ReelHeight - 1 - i), ReelSymbol.Coin);
            }
            return true;
        }

        private bool TryApplyToMiddleOfReelFromTop(int reel, int coinsInView, int coinStackHeight)
        {
            for (int index = 0; index <= _window.ReelHeight - coinStackHeight; index++)
            {
                for (int coinCount = 0; coinCount < coinStackHeight; coinCount++)
                {
                    if (_window.GetSymbolAt(reel, index + coinCount) == ReelSymbol.Wild) break;

                    if (coinCount == coinStackHeight - 1)
                    {
                        for (int coin = 0; coin < coinStackHeight; coin++) _window.SetSymbolAt(reel, index + coin, ReelSymbol.Coin);

                        return true;
                    };
                }
            }
            return false;
        }

        private bool TryApplyToMiddleOfReelFromBottom(int reel, int coinsInView, int coinStackHeight)
        {
            if (coinsInView == 0 || coinsInView >= _window.ReelHeight) return false;

            for (int index = _window.ReelHeight - 1; index >= 0 + coinStackHeight - 1; index--)
            {
                for (int coinCount = 0; coinCount < coinStackHeight; coinCount++)
                {
                    if (_window.GetSymbolAt(reel, index - coinCount) == ReelSymbol.Wild) break;

                    if (coinCount == coinStackHeight - 1)
                    {
                        for (int coin = 0; coin < coinStackHeight; coin++)
                        {
                            _window.SetSymbolAt(reel, index - coin, ReelSymbol.Coin);
                        }

                        return true;
                    };
                }
            }
            return false;
        }

        private int GenerateCoinsInView()
        {
            int[] possibleCoinsInViewProbability = [0, 0, 1, 2, 3, 4];

            return possibleCoinsInViewProbability[_random.Next(0, possibleCoinsInViewProbability.Length)];
        }

        private int GenerateCoinStackHeight(int coinsInView)
        {
            if (coinsInView == 0) return 0;

            else if (coinsInView >= 1 && coinsInView <= 3) return _random.Next(3, 8);
            else if (coinsInView == 4) return _random.Next(4, 8);

            return 0;
        }

        public class WinLineInfo
        {
            public int? _lineIndex;
            public int? _score;

            public WinLineInfo(int lineIndex)
            {
                _lineIndex = lineIndex;
            }

        }

        private int CalculateSpinWinTotal()
        {

            //if (InvalidWinLinConfiguration()) throw new GameException("invalid win line configuration");

            ReelSymbol[][] winLineValues = new ReelSymbol[_winLines.Length][];

            for (int i = 0; i < _winLines.Length; i++)
            {
                winLineValues[i] = new ReelSymbol[_winLines[i].Length];
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("winlines");

            for (int winLine = 0; winLine < _winLines.Length; winLine++)
            {
                for (int winLineIndex = 0; winLineIndex < _winLines[winLine].Length; winLineIndex++)
                {

                    winLineValues[winLine][winLineIndex] = _window.GetSymbolAt(winLineIndex, winLine);
                    //Console.Write(winLineValues[winLine][winLineIndex] + "\t");
                }
                //Console.WriteLine();
            }

            List<Dictionary<WinLineInfo, List<ReelSymbol>>> consecutivesFound = [];

            //foreach (ReelSymbol[] line in winLineValues)
            for (int lineIndex = 0; lineIndex < winLineValues.Length; lineIndex++)
            {
                ReelSymbol[] line = winLineValues[lineIndex];

                for (int i = 0; i < line.Length - 1; i++)  // check
                {
                    List<ReelSymbol> currentConsecative = [];

                    ReelSymbol currentSymbol = line[i];
                    ReelSymbol nextSymbol = line[i + 1];

                    if (SymbolsMatch(currentSymbol, nextSymbol))
                    {
                        currentConsecative.Add(currentSymbol);
                        currentConsecative.Add(nextSymbol);

                        consecutivesFound.Add(new() { { new WinLineInfo(lineIndex), new List<ReelSymbol>(currentConsecative) } });

                        for (int j = i + 2; j < line.Length; j++)
                        {
                            ReelSymbol subsequentSymbol = line[j];

                            if (NewSymbolMatchesCurrentConsecuative(currentConsecative, subsequentSymbol))
                            {
                                currentConsecative.Add(subsequentSymbol);
                                consecutivesFound.Add(new() { { new WinLineInfo(lineIndex), new List<ReelSymbol>(currentConsecative) } });
                            }
                            else break;
                        }

                    }
                }
            }

            //Console.WriteLine();
            //foreach (var entry in consecutivesFound)
            //{
            //    foreach (var kvp in entry)

            //        Console.WriteLine("key: " + kvp.Key._lineIndex + ", score: " + kvp.Key._score + "\t" + string.Join("\t", kvp.Value));
            //}

            Console.WriteLine();
            foreach (var entry in consecutivesFound)
            {
                foreach (var kvp in entry)
                {
                    ReplaceWildCardsWithTheConsecuativeSymbol(kvp.Value);
                    kvp.Key._score = CalculateScores(kvp.Value);

                    //Console.WriteLine("key: " + kvp.Key._lineIndex + ", score: " + kvp.Key._score + "\t" + string.Join("\t", kvp.Value));

                }
            }

            var highestScoresByLine = consecutivesFound
                .SelectMany(entry => entry)
                .GroupBy(kvp => kvp.Key._lineIndex)
                .Select(group => new
                {
                    LineIndex = group.Key,
                    MaxScore = group
                        .OrderByDescending(kvp => kvp.Key._score)
                        .ThenByDescending(kvp => kvp.Value.Count)
                        .First()
                        .Key._score
                })
                .ToList();

            var sum = highestScoresByLine.Sum(hsbl => hsbl.MaxScore);

            Console.WriteLine();
            string winSummary = "total score: ";
            foreach (var result in highestScoresByLine)
            {

                if (result.MaxScore > 0)
                {
                    Console.WriteLine($"Winline: {result.LineIndex}, Highest Score: {result.MaxScore}");
                    winSummary += result.MaxScore + " + ";
                }
            }
            if (winSummary == "total score: ") winSummary = "";
            else if (winSummary.EndsWith(" + ")) winSummary = winSummary.Substring(0, winSummary.Length - 2) + " = ";


            Console.WriteLine();

            if (sum > 0) Console.WriteLine(winSummary + sum);
            else Console.WriteLine();

            Console.WriteLine();

            if (sum == null) return 0;
            else return (int)sum;

        }

        private int CalculateScores(List<ReelSymbol> symbols)
        {
            return _payTable.GetPayout(symbols[0], symbols.Count);
        }

        private void ReplaceWildCardsWithTheConsecuativeSymbol(List<ReelSymbol> symbols)
        {
            ReelSymbol consecuativeSymbol = FindConsecuativeSymbolOrReturnWildCard(symbols);

            if (consecuativeSymbol != ReelSymbol.Wild)
            {
                for (int i = 0; i < symbols.Count; i++)
                {
                    if (symbols[i] == ReelSymbol.Wild) symbols[i] = consecuativeSymbol;
                }
            }
        }

        private ReelSymbol FindConsecuativeSymbolOrReturnWildCard(List<ReelSymbol> symbols)
        {
            ReelSymbol consecuativeSymbol = ReelSymbol.Wild;

            foreach (var symbol in symbols)
            {
                if (symbol != ReelSymbol.Wild)
                {
                    consecuativeSymbol = symbol;
                    break;
                }
            }

            return consecuativeSymbol;

        }

        private bool SymbolsMatch(ReelSymbol a, ReelSymbol b)
        {
            return (a == b || a == ReelSymbol.Wild || b == ReelSymbol.Wild);
        }

        private bool NewSymbolMatchesCurrentConsecuative(List<ReelSymbol> currentConsecuative, ReelSymbol newSymbol)
        {
            if (currentConsecuative.Count < 2) return false;
            if (newSymbol == ReelSymbol.Wild) return true;

            ReelSymbol exsistingSymbol = ReelSymbol.Wild;

            foreach (ReelSymbol symbol in currentConsecuative)
            {
                if (symbol != ReelSymbol.Wild)
                {
                    exsistingSymbol = symbol;
                    break;
                }
            }

            if (exsistingSymbol == ReelSymbol.Wild) return true;

            return (newSymbol == exsistingSymbol);

        }

        private ConsoleKey WaitForUserInput()
        {
            _visualIo.DisplayMessage("press enter to spin");

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter) return ConsoleKey.Enter;

                Thread.Sleep(100);
            }

        }

        private void SimulateAndDisplayNewReelsSpin()
        {

            for (int i = 0; i < 3; i++)
            {
                Console.Clear();

                InitReelsRandom();
                ApplyCoinStacksToReels();
                //PrintReelsVertical();
                _visualIo.DisplayReelsVertical(_window);

                Thread.Sleep(200);
            }

        }

        // reel  0       1       2       3       4 
        //     [ [0],  [ [ ],  [ [ ],  [ [ ],  [ [ ],
        //       [1],    [ ],    [ ],    [ ],    [ ],
        //       [2],    [ ],    [ ],    [ ],    [ ],
        //       [3] ],  [ ] ],  [ ] ],  [ ] ],  [ ] ] ;

        public void Run()
        {

            IntroMessagePrint();

            while (true)
            {
                var userInput = WaitForUserInput();

                if (userInput == UserInput.spin)
                {

                    SimulateAndDisplayNewReelsSpin();
                    int spinWinTotal = CalculateSpinWinTotal();
                    _gameWin.AddWin(spinWinTotal);
                    _visualIo.DisplayMessage("accumulated: " + _gameWin.CurrentWin);
                }
            }

        }

    }

}
