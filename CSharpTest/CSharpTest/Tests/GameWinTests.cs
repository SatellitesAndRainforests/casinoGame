using CSharpTest.Application;
using CSharpTest.Application.Exceptions;
using CSharpTest.Interfaces;
using NUnit.Framework;

namespace CSharpTest.Tests
{
    [TestFixture]
    public class GameWinTests
    {

        private IGameWin? _gameWin;

        [Test]
        public void GameWin_SetToUnlimitedWins_AddsUncappedWinsToTotal()
        {
            // Arrange
            var maxTotalWinValue = 100;
            var currentTotalWinValue = 99;
            bool setGameToUnlinmitedWins = true;

            _gameWin = new GameWin(maxTotalWinValue, setGameToUnlinmitedWins);
            _gameWin.AddWin(currentTotalWinValue);

            var newWinAmount = 200;

            // Act
            var valueAddedToTotalWin = _gameWin.AddWin(newWinAmount);

            // Assert
            Assert.False(valueAddedToTotalWin.IsCapped);
            Assert.AreEqual(newWinAmount, valueAddedToTotalWin.CappedWin);
            Assert.AreEqual(currentTotalWinValue + newWinAmount, _gameWin.CurrentWin);
        }

        [Test]
        public void GameWin_SetToCapTotalWin_CapsNewWins()
        {
            // Arrange
            var maxTotalWinValue = 100;
            bool unlimtedWins = false;

            _gameWin = new GameWin(maxTotalWinValue, unlimtedWins);

            var newWinAmount = 500;

            // Act
            var valueAddedToTotalWin = _gameWin.AddWin(newWinAmount);

            // Assert
            Assert.True(valueAddedToTotalWin.IsCapped);
            Assert.AreEqual(maxTotalWinValue, valueAddedToTotalWin.CappedWin);
        }

        [Test]
        public void VerifyMaxWin_ThrowsGameException_WhenCurrentWinExceedsMaxWin()
        {

            // Arrange

            var maxTotalWinValue = 100;
            bool unlimtedWins = false;

            _gameWin = new GameWin(maxTotalWinValue, unlimtedWins);

            var currentWinValueThatExceedsMaxTotalWin = 500;

            typeof(GameWin).GetField("_currentWin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
               .SetValue(_gameWin, currentWinValueThatExceedsMaxTotalWin);

            // Act + Assert

            Assert.Throws<GameException>(() => _gameWin.VerifyMaxWin());

        }


        [Test]
        public void IsCapped_ReturnsTrue_IfCurrentWinIsTheMaxWin()
        {
            // Arrange
            var maxTotalWinValue = 100;
            bool unlimtedWins = false;

            _gameWin = new GameWin(maxTotalWinValue, unlimtedWins);

            var newWinAmount = 500;

            // Act
            var valueAddedToTotalWin = _gameWin.AddWin(newWinAmount);

            // Assert
            Assert.True(_gameWin.IsCapped);
        }

        [Test]
        public void WouldCapIfAdded_ReturnsTrue_IfAddWinWasCalledWithTheSameWin()
        {
            // Arrange
            var maxTotalWinValue = 100;
            bool unlimtedWins = false;

            _gameWin = new GameWin(maxTotalWinValue, unlimtedWins);

            var newWinAmount = 500;

            // Act
            bool result = _gameWin.WouldCapIfAdded(newWinAmount);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void Copy_CreatesANewInstanceWithMatchingState()
        {

            // Arrange
            var maxTotalWinValue = 100;
            bool unlimitedWins = false;
            _gameWin = new GameWin(maxTotalWinValue, unlimitedWins);
            _gameWin.AddWin(50);

            // Act
            var copiedGameWin = _gameWin.Copy();

            // Assert
            Assert.AreEqual(_gameWin.CurrentWin, copiedGameWin.CurrentWin);
            Assert.AreEqual(_gameWin.IsCapped, copiedGameWin.IsCapped);
            Assert.IsFalse(ReferenceEquals(_gameWin, copiedGameWin));

        }

    }
}
