using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task2;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class Task2Test
    {
        [TestMethod]
        public void GetPairsForIntReturnsPairs()
        {
            var targetArray = new List<int> { -1, 0, 1, 2, 3, 4, 5, 6, 7 };
            var searchedPairs = PairSearch<int>.GetPairs(targetArray.ToArray(), 6);

            Assert.IsTrue(searchedPairs.Count == 4);
            foreach (Tuple<int,int> pair in searchedPairs)
            {
                Assert.IsTrue(targetArray.Contains(pair.Item1));
                Assert.IsTrue(targetArray.Contains(pair.Item2));
            }
        }

        [TestMethod]
        public void GetPairsReturnsPairsOfUniqueElements()
        {
            var targetArray = new List<int> {  1, 2, 1, 1, 0 };
            var searchedPairs = PairSearch<int>.GetPairs(targetArray.ToArray(), 2);

            Assert.IsTrue(searchedPairs.Count == 2);

            var elementsFrequencyInPairs = searchedPairs
                .SelectMany(x => new int[] { x.Item1, x.Item2 })
                .GroupBy(x => x)
                .ToList();

            var elementsFrequencyInTargetArray = targetArray
                .GroupBy(x => x)
                .ToList();

            foreach (var elementsFrequency in elementsFrequencyInPairs)
            {
                Assert.IsTrue(targetArray.Contains(elementsFrequency.Key));
                Assert.IsTrue(elementsFrequencyInTargetArray.First(x => x.Key == elementsFrequency.Key).Count() >= elementsFrequency.Count());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPairsForEnumNot()
        {
            var targetArray = new List<TestPairSearch> { TestPairSearch.a, TestPairSearch.b, TestPairSearch.b };
            var searchedPairs = PairSearch<TestPairSearch>.GetPairs(targetArray.ToArray(), TestPairSearch.b);
        }

        [TestMethod]
        public void GetPairsForDecimalReturnsPairs()
        {
            var targetArray = new List<decimal> { -0.1M, 1.2M, 2.5M, 3.5M, 4.8M, 5M, 6.1M };
            var searchedPairs = PairSearch<decimal>.GetPairs(targetArray.ToArray(), 6);

            Assert.IsTrue(searchedPairs.Count == 3);
            foreach (Tuple<decimal, decimal> pair in searchedPairs)
            {
                Assert.IsTrue(targetArray.Contains(pair.Item1));
                Assert.IsTrue(targetArray.Contains(pair.Item2));
            }
        }
    }

    enum TestPairSearch
    {
        a = 1,
        b = 2
    }
}
