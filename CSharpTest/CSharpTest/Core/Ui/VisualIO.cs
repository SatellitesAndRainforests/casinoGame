using CSharpTest.Core.Interfaces;

namespace CSharpTest.Core.Ui
{
    public class VisualIO : IVisualIO
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine();
        }

        public void DisplayReelsVertical(ReelStopWindow window)
        {

            Console.WriteLine("");

            string[] rowStrings = new string[window.ReelHeight];

            for (int row = 0; row < window.ReelHeight; row++)
            {
                for (int columns = 0; columns < window.ReelCount; columns++)
                {
                    rowStrings[row] += window.GetSymbolAt(columns, row).ToString() + "\t";
                }
            }

            foreach (string row in rowStrings) Console.WriteLine(row);

        }

    }
}
