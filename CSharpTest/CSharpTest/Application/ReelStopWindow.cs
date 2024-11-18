using CSharpTest.Configuration;
using CSharpTest.Application.Exceptions;

namespace CSharpTest.Application;

public class ReelStopWindow
{
    private readonly ReelSymbol[][] _symbols;

    public ReelStopWindow()
    {
        _symbols = new ReelSymbol[ReelCount][];
        for (var reel = 0; reel < ReelCount; reel++)
        {
            _symbols[reel] = new ReelSymbol[ReelHeight];
            Array.Fill(_symbols[reel], ReelSymbol.None);
        }
    }

    public int ReelCount => 5;
    public int ReelHeight => 4;

    public ReelSymbol GetSymbolAt(int reel, int offset)
    {
        if (reel >= ReelCount || offset >= ReelHeight)
            throw new GameException($"reel {reel} offset {offset} is out of range {ReelCount} {ReelHeight}");
        return _symbols[reel][offset];
    }

    public void SetSymbolAt(int reel, int offset, ReelSymbol symbol)
    {
        if (reel >= ReelCount || offset >= ReelHeight)
            throw new GameException($"reel {reel} offset {offset} is out of range {ReelCount} {ReelHeight}");
        _symbols[reel][offset] = symbol;
    }
}
