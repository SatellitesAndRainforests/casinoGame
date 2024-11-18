using CSharpTest.Application.Constants;
using CSharpTest.Ui;
using CSharpTest.Interfaces;
using CSharpTest.Configuration;

var game = new CSharpTest.Application.Game();
game.Run();

namespace CSharpTest.Application
{
    public class Game
    {

        private IGameWin _gameWin = new GameWin(0, true);
        private ReelStopWindow _window = new ReelStopWindow();

        private readonly IVisualIO _visualIo = new VisualIO();
        private readonly WinLines _winLines = new WinLines();
        private readonly PayTable _payTable = new PayTable();
        private readonly Probabilities _probabilities = new Probabilities();



        private void DisplayIntroMessage()
        {
            _visualIo.DisplayPaddedMessage(IoMessages.WelcomeMessage);
        }

        private void InitReelsWithRandomSymbols()
        {
            for (int reel = 0; reel < _window.ReelCount; reel++)
            {
                for (int offset = 0; offset < _window.ReelHeight; offset++)
                {
                    ReelSymbol randomSymbol = _probabilities.GetRandomInitialReelSymbolExcludingCoinAndNone();
                    _window.SetSymbolAt(reel, offset, randomSymbol);
                }
            }
        }

        private void InitCoinsAndTryApplyToReels()
        {

            for (int reel = 0; reel < _window.ReelCount; reel++)
            {
                int coinsInView = _probabilities.GenerateRandomCoinsInView();
                int coinStackHeight = _probabilities.GenerateRandomCoinStackHeight(coinsInView);

                bool fromTop = _probabilities.ApplyCoinStackFromTopFirstOrBottomFirst();

                if (fromTop)
                {
                    if (TryApplyCoinsInViewFromTopOfReel(reel, coinsInView)) continue;
                    else if (TryApplyCoinsInViewFromBottomOfReel(reel, coinsInView)) continue;
                    else if (TryApplyCoinsInViewToMiddleOfReelFromTop(reel, coinStackHeight)) continue;
                    else continue;
                    // else throw new GameException("could not apply");
                }
                else
                {
                    if (TryApplyCoinsInViewFromBottomOfReel(reel, coinsInView)) continue;
                    else if (TryApplyCoinsInViewFromTopOfReel(reel, coinsInView)) continue;
                    else if (TryApplyCoinsInViewToMiddleOfReelFromBottom(reel, coinStackHeight)) continue;
                    else continue;
                    // else throw new GameException("could not apply");
                }

            }

        }

        private bool TryApplyCoinsInViewFromTopOfReel(int reel, int coinsInView)
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

        private bool TryApplyCoinsInViewFromBottomOfReel(int reel, int coinsInView)
        {
            for (int i = 0; i < coinsInView; i++)
            {
                if (_window.GetSymbolAt(reel, _window.ReelHeight - 1 - i) == ReelSymbol.Wild) return false;
            }

            for (int i = 0; i < coinsInView; i++)
            {
                _window.SetSymbolAt(reel, _window.ReelHeight - 1 - i, ReelSymbol.Coin);
            }

            return true;
        }

        private bool TryApplyCoinsInViewToMiddleOfReelFromBottom(int reel, int coinStackHeight)
        {
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

        private bool TryApplyCoinsInViewToMiddleOfReelFromTop(int reel, int coinStackHeight)
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

        private ReelSymbol[][] GetWinLineSymbolsFromReels()
        {
            ReelSymbol[][] winLineMatrix = _winLines.InitEmptyWinLineMatrix();

            for (int winLine = 0; winLine < winLineMatrix.Length; winLine++)
            {
                for (int index = 0; index < winLineMatrix[winLine].Length; index++)
                {
                    winLineMatrix[winLine][index] = _window.GetSymbolAt(index, winLine);
                }
            }

            return winLineMatrix;
        }

        private List<Dictionary<WinLineInfo, List<ReelSymbol>>> FindAllConsecutiveSymbolsInEachRow(ReelSymbol[][] winLineMatrix)
        {

            List<Dictionary<WinLineInfo, List<ReelSymbol>>> consecutivesFound = [];

            for (int lineIndex = 0; lineIndex < winLineMatrix.Length; lineIndex++)
            {
                ReelSymbol[] line = winLineMatrix[lineIndex];

                for (int i = 0; i < line.Length - 1; i++)
                {
                    List<ReelSymbol> currentConsecative = [];

                    ReelSymbol currentSymbol = line[i];
                    ReelSymbol nextSymbol = line[i + 1];

                    if (SymbolsMatch(currentSymbol, nextSymbol))
                    {
                        currentConsecative.Add(currentSymbol);
                        currentConsecative.Add(nextSymbol);

                        consecutivesFound.Add(new() { { new WinLineInfo(lineIndex, i), new List<ReelSymbol>(currentConsecative) } });

                        for (int j = i + 2; j < line.Length; j++)
                        {
                            ReelSymbol subsequentSymbol = line[j];

                            if (NewSymbolMatchesCurrentConsecuativeSymbols(currentConsecative, subsequentSymbol))
                            {
                                currentConsecative.Add(subsequentSymbol);
                                consecutivesFound.Add(new() { { new WinLineInfo(lineIndex, i), new List<ReelSymbol>(currentConsecative) } });
                            }
                            else break;
                        }

                    }
                }
            }

            return consecutivesFound;

        }

        private WinReport GenerateSpinWinReport()
        {

            //if (InvalidWinLinConfiguration()) throw new GameException("invalid win line configuration");

            var winLineMatrix = GetWinLineSymbolsFromReels();
            var winLinesFound = FindAllConsecutiveSymbolsInEachRow(winLineMatrix);

            ProcessWinLinesForScoringAndCalculateScores(winLinesFound);

            var topScoringWinningLinesForEachRow = SelectTopScoringLinesByScoreThenLength(winLinesFound);

            return new WinReport()
            {
                SpinScore = topScoringWinningLinesForEachRow.Sum(line => line.Score),
                TopScoringLines = topScoringWinningLinesForEachRow,
            };

        }

        private void ProcessWinLinesForScoringAndCalculateScores(List<Dictionary<WinLineInfo, List<ReelSymbol>>> winLinesFound)
        {
            foreach (var winLine in winLinesFound)
            {
                foreach (var kvp in winLine)
                {
                    ReplaceWildCardsWithTheConsecuativeSymbol(kvp.Value);
                    kvp.Key.Score = CalculateScores(kvp.Value);
                }
            }
        }

        private List<TopScoringLine> SelectTopScoringLinesByScoreThenLength(List<Dictionary<WinLineInfo, List<ReelSymbol>>> winLinesFound)
        {
            return winLinesFound
                .SelectMany(winLine => winLine)
                .GroupBy(kvp => kvp.Key.LineIndex)
                .Select(group =>
                {
                    var topLine = group
                        .OrderByDescending(kvp => kvp.Key.Score)
                        .ThenByDescending(kvp => kvp.Value.Count)
                        .First();

                    return new TopScoringLine()
                    {
                        Row = topLine.Key.LineIndex,
                        StartingIndex = topLine.Key.StartLocation,
                        Score = topLine.Key.Score,
                        Symbols = topLine.Value
                    };

                })
                .ToList();
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
            return a == b || a == ReelSymbol.Wild || b == ReelSymbol.Wild;
        }

        private bool NewSymbolMatchesCurrentConsecuativeSymbols(List<ReelSymbol> currentConsecuative, ReelSymbol newSymbol)
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

            return newSymbol == exsistingSymbol;

        }

        private ConsoleKey WaitForUserInput()
        {
            _visualIo.DisplayPaddedMessage(IoMessages.PressEnterToSpin);

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter) return ConsoleKey.Enter;

                Thread.Sleep(100);
            }

        }

        private void SimulateAndDisplayNewReelSpin()
        {
            for (int i = 0; i < 3; i++)
            {
                _visualIo.ClearDisplayScreen();

                InitReelsWithRandomSymbols();
                InitCoinsAndTryApplyToReels();

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

            DisplayIntroMessage();

            while (true)
            {
                var userInput = WaitForUserInput();

                if (userInput == UserInput.Spin)
                {

                    SimulateAndDisplayNewReelSpin();
                    var winReport = GenerateSpinWinReport();

                    _gameWin.AddWin(winReport.SpinScore);
                    winReport.GameScore = _gameWin.CurrentWin;

                    _visualIo.DisplayWinReport(winReport);

                }
            }

        }


    }

}
