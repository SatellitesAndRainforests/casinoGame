namespace CSharpTest.Interfaces;

public interface IGameWin
{
    // Adds the 'win' to the accumulator.
    // If the 'win' would cause the accumulated win to exceed the maximum win
    // then the win is capped such that the maximum win is not exceeded.
    // Returns: CappedWin - the actual win amount added to the accumulator.
    //          IsCapped - true if CappedWin is lower than 'win'.
    public (long CappedWin, bool IsCapped) AddWin(long win);

    // Returns true if IsCapped would be true if AddWin was called with the same 'win'.
    bool WouldCapIfAdded(long win);

    // The current value of the accumulated win
    long CurrentWin { get; }

    // Returns true if Current Win is the maximum win.
    bool IsCapped { get; }

    // Throws GameException if CurrentWin exceeds the maximum win.
    void VerifyMaxWin();

    // Create a copy of this instance.
    IGameWin Copy();
}

