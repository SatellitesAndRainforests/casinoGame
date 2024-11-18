using CSharpTest.Interfaces;
using CSharpTest.Application.Exceptions;

namespace CSharpTest.Application
{
    public class GameWin : IGameWin
    {
        private readonly long _maximumWin;
        private readonly bool _unlimitedWins;
        private long _currentWin;

        public GameWin(long maximumWin, bool unlimitedWins)
        {
            _maximumWin = maximumWin;
            _unlimitedWins = unlimitedWins;
            _currentWin = 0;
        }

        public (long CappedWin, bool IsCapped) AddWin(long win)
        {

            if (_unlimitedWins)
            {
                _currentWin += win;
                return (win, false);
            }
            else
            {

                var newTotal = _currentWin + win;

                if (newTotal > _maximumWin)
                {
                    var actualWin = _maximumWin - _currentWin;
                    _currentWin = _maximumWin;

                    return (actualWin, true);
                }
                else
                {
                    _currentWin = newTotal;
                    return (win, false);
                }

            }

        }

        public bool WouldCapIfAdded(long win)
        {
            if (_unlimitedWins) return false;
            return _currentWin + win > _maximumWin;
        }

        public long CurrentWin => _currentWin;

        public bool IsCapped => !_unlimitedWins && _currentWin >= _maximumWin;

        public void VerifyMaxWin()
        {
            if (!_unlimitedWins && _currentWin > _maximumWin)
            {
                throw new GameException($"CurrentWin: {_currentWin} exceeds the maximum win: {_maximumWin}.");
            }
        }

        public IGameWin Copy()
        {
            return new GameWin(_maximumWin, _unlimitedWins) { _currentWin = _currentWin };
        }

    }
}
