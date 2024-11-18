using CSharpTest.Application;
using CSharpTest.Interfaces;

namespace CSharpTest.Ui
{
    public class VisualIO : IVisualIO
    {
        public void DisplayPaddedMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" " + message);
            Console.WriteLine();
            Console.WriteLine();
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(" " + message);
        }

        public void DisplayReelsVertical(ReelStopWindow window)
        {

            ClearDisplayScreen();
            DisplayMessage("");

            string[] rowStrings = new string[window.ReelHeight];

            for (int row = 0; row < window.ReelHeight; row++)
            {
                for (int columns = 0; columns < window.ReelCount; columns++)
                {
                    rowStrings[row] += window.GetSymbolAt(columns, row).ToString() + "\t";
                }
            }

            foreach (string row in rowStrings) DisplayMessage(row);

        }

        public void DisplayWinReport(WinReport winReport)
        {

            DisplayMessage("");
            DisplayMessage("");

            string winSummary = " ";
            string totalScoreSummary = " total score: \t" + winReport.SpinScore;

            if (winReport.TopScoringLines?.Count > 0)
            {
                winSummary += "\n" + " - - winning lines - - - - - - - - - - - - - - - -" + "\n\n";

                foreach (var topScoringLine in winReport.TopScoringLines)
                {
                    if (topScoringLine.Score > 0)
                    {
                        winSummary += ($" line: {topScoringLine.Row}, score: {topScoringLine.Score} " +
                            $"\t " + string.Join("\t", topScoringLine.Symbols) +
                            $"\n");
                    }
                }

                winSummary += "\n";
            }

            winSummary += totalScoreSummary;
            winSummary += "\n\n" + " game score: \t" + winReport.GameScore;

            DisplayMessage(winSummary);
            DisplayMessage("");
            DisplayMessage("- - - - - - - - - - - - - - - - - - - - - - - - - ");

        }

        public void ClearDisplayScreen()
        {
            Console.Clear();
        }

    }
}
