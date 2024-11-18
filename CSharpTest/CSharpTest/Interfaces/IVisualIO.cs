using CSharpTest.Application;

namespace CSharpTest.Interfaces
{
    public interface IVisualIO
    {

        public void DisplayPaddedMessage(string message);

        public void DisplayMessage(string message);

        public void DisplayReelsVertical(ReelStopWindow window);

        public void DisplayWinReport(WinReport winReport);

        public void ClearDisplayScreen();

    }
}
