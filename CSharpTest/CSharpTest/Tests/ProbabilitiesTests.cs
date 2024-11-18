using CSharpTest.Configuration;
using NUnit.Framework;

namespace CSharpTest.Tests
{
    [TestFixture]
    public class ProbabilitiesTests
    {

        private readonly Probabilities _probabilities = new Probabilities();

        [Test]
        public void ApplyCoinStackFromTopFirstOrBottomFirst_Returns5050Probability()
        {

            // Arrange

            int trueCount = 0;
            int falseCount = 0;

            int iterations = 10000;
            int tolerance = 200;     // 2% for randomness

            // Act

            for (int i = 0; i < iterations; i++)
            {
                bool topFirst = _probabilities.ApplyCoinStackFromTopFirstOrBottomFirst();

                if (topFirst) trueCount++;
                else falseCount++;

            }

            int difference = Math.Abs(trueCount - falseCount);

            // Assert

            Assert.True(difference <= tolerance);

        }

        [Test]
        public void GenerateRandomCoinsInView_ReturnsCorrectProbabilityForEachValue()
        {

            // Arrange

            double expectedProbabilityFor0 = 0.5;               // 50 %
            double expectedProbabilityOtherValues = 0.125;      // 12.5 % 

            double tolerance = 0.03;    // 3 % for randomness

            var coinsInViewCounts = new Dictionary<int, int>()
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
            };

            int iterations = 10000;

            // Act

            for (int i = 0; i < iterations; i++)
            {
                int coinsInView = _probabilities.GenerateRandomCoinsInView();
                coinsInViewCounts[coinsInView]++;
            }

            double actualProbabilityFor0 = coinsInViewCounts[0] / (double)iterations;
            double actualProbabilityFor1 = coinsInViewCounts[1] / (double)iterations;
            double actualProbabilityFor2 = coinsInViewCounts[2] / (double)iterations;
            double actualProbabilityFor3 = coinsInViewCounts[3] / (double)iterations;
            double actualProbabilityFor4 = coinsInViewCounts[4] / (double)iterations;

            double diffrenceFor0 = Math.Abs(expectedProbabilityFor0 - actualProbabilityFor0);
            double diffrenceFor1 = Math.Abs(expectedProbabilityOtherValues - actualProbabilityFor1);
            double diffrenceFor2 = Math.Abs(expectedProbabilityOtherValues - actualProbabilityFor2);
            double diffrenceFor3 = Math.Abs(expectedProbabilityOtherValues - actualProbabilityFor3);
            double diffrenceFor4 = Math.Abs(expectedProbabilityOtherValues - actualProbabilityFor4);

            // Assert

            Assert.IsTrue(diffrenceFor0 <= tolerance);
            Assert.IsTrue(diffrenceFor1 <= tolerance);
            Assert.IsTrue(diffrenceFor2 <= tolerance);
            Assert.IsTrue(diffrenceFor3 <= tolerance);
            Assert.IsTrue(diffrenceFor4 <= tolerance);

        }






    }
}
