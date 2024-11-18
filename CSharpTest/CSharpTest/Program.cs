//using CSharpTest.Application;

//namespace CSharpTest;

//internal class Program
//{
    //private static void Main(string[] args)
    //{
        //Game game = new Game();
        //game.Run();

        // INSTRUCTIONS
        //
        //  Read the INFORMATION sections and complete the tasks in the TASK sections.
        
        // INFORMATION:
        //   A ReelStopWindow is a 5 x 4 grid of ReelSymbol. An basic implementation is provided in the Core namespace.
        //
        //   The API is:
        //
        //   GetSymbolAt(reel, offset)
        //     - Return the symbol on the given reel and offset (i.e. column and row in the grid)
        //   SetSymbolAt(reel, offset, symbol)
        //     - Set the symbol on the given reel and offset (i.e. column and row in the grid)
        //   ReelCount
        //     - The number of reels in the stop window. (i.e. number of columns - this is 5.)
        //   ReelHeight
        //     - The number of symbols per reel in the stop window. (i.e. number of rows - this is 4.)
        
        // INFORMATION:
        //   Random numbers:
        //     Use the built-in .NET random number generator - do not worry about random number quality.
        
        // TASK: 
        //   Create an instance of ReelStopWindow from the Core namespace and assign a random ReelSymbol, excluding
        //   Coin and None, with equal probability to each grid position.
        
        // INFORMATION:
        //  Coin stacks:
        //    A coin stack is 3 to 7 Coin symbols that can be applied to a reel (column) in full or in part. A
        //    coin stack must appear on consecutive rows (offsets) and therefore cannot be split.
        //
        //    Where a coin stack is partially in view the out of view Coin symbols should conceptually be above or below
        //    the reel with equal probability. That is, the in view Coins are anchored to the top or bottom of the reel.
        //
        //    Adding a Coin will overwrite the existing symbol. However, a Coin may not overwrite a Wild symbol.

        // TASK:
        //   Using the ReelStopWindow in the previous task, for each reel:
        //     * Pick the number of coins that will be in view. This should be a number between
        //       0 and 4. 0 should have 50% probability, 1 to 4 should have 12.5% probability each.
        //
        //     * Pick with equal probability the coin stack size based on the number of coins that
        //       will be in view: 
        //          0 -> No coin stack
        //          1 -> Coin stack should be 3, 4, 5, 6 or 7.
        //          2 -> Coin stack should be 3, 4, 5, 6 or 7.
        //          3 -> Coin stack should be 3, 4, 5, 6 or 7.
        //          4 -> Coin stack should be 4, 5, 6 or 7.
        //
        //      * Apply the coin stack to the reel ensuring the number of coins on the reel is correct.
        //
        //         Rule: If it is not possible to apply the selected coin stack because of Wild symbols then no coins
        //               should be applied to that reel.
        //
        // Example: The correct way to apply a coin stack of 6 to reel 1 where only 2 coins may be in view:
        //     C X X X X
        //     C X X X X
        //     W X X X X 
        //     X X X X X
        //     C = Coin, W = Wild, X = any other symbol
        // 
        // Notes:
        //   Assume ReelHeight, ReelCount and the 'number of coins' selection options may be changed by configuration.
        //   Ensure that the code continues to function for any value of ReelHeight, ReelCount and 'number of coins'
        //   options. If the configuration conflicts with the rules of applying a coin stack throw a GameException
        //   containing a useful error message. It is OK to throw the exception while running the implementation of 
        //   the logic do not worry about pre-validation unless that is the easiest way to detect the error.
        
        // INFORMATION:
        //   Win Lines
        //    A win line is a squence of offsets on the grid starting from the left most reel. For example:
        //    the win line [0, 1, 2, 3, 2] covers the following grid positions:  
        //       X - - - -
        //       - X - - -
        //       - - X - X
        //       - - - X -
        //
        //    A win is awarded for consecutive symbols found on the win line from left to right:
        //       J - - - -
        //       - J - - -
        //       - - J - W
        //       - - - K -
        //    In this example there are 3 consecutive Jack symbols and so the win is 3x Jacks.
        // 
        //    The Wild symbol can substitue for any other symbol except Coin:
        //       W - - - -
        //       - J - - -
        //       - - J - W
        //       - - - K -
        //    In this example the win is still 3x Jack symbols.
        // 
        //    The Wild symbol itself may also pay a win. This can lead to there being multiple possible wins on a
        //    single win line. In this case only the highest win should be paid. In the event two wins are of equal
        //    value the longest consecutive sequence should be paid. E.G. 5x Jack should be paid over 3x Wild.
        //
        //    The Wild symbol can substitue for any other symbol except Coin:
        //       W - - - -
        //       - W - - -
        //       - - W - K
        //       - - - K -
        //    In this example there are two wins 3x Wild and 5x King. Since 5x King awards more than 3x Wild the win
        //    for 5x King is paid.
        //
        //   Pay Table
        //
        //   The pay table determines how much should be awarded for each consecutive sequence of symbols on a pay line:
        //    Ten  x3 awards 2, x4 awards 5, x5 awards 10
        //    Jack x3 awards 10, x4 awards 15, x5 awards 20
        //    King x3 awards 20, x4 awards 30, x5 awards 50
        //    Wild x3 awards 20, x4 awards 30, x5 awards 50
        //
        //   If a Symbol is not in the pay table then no win is paid for consecutive sequences of that symbol.
        
        // TASK:
        //   * Using the stop window constructed in previous tasks check for wins on each of the following win lines
        //     and sum the total win:
        //      [ 0, 0, 0, 0, 0 ]
        //      [ 1, 1, 1, 1, 1 ]
        //      [ 2, 2, 2, 2, 2 ]
        //      [ 3, 3, 3, 3, 3 ]
        //  Notes:
        //   Assume the win lines and the pay table can be changed by configuration. Ensure the code continues to
        //   function correctly for any given configuration. If a configuration is invalid throw a GameException
        //   containing a useful error message. It is OK to throw the exception while running the implementation
        //   of the logic do not worry about pre-validation unless that is the easiest way to detect the error.
        
        // INFORMATION:
        //  Maximum Win 
        //   A game may accumulate multiple wins from a variety of sources the sum of which may not exceed the game's
        //   maximum win, if enabled.
        
        //  TASK:
        //   Review and create an implementation of the IGameWin interface.
        //    The implementation should accept a maximum win value along with an option to enable
        //    unlimited wins.
//    }
//}