using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Schedulee.Core.Extensions;
using Shouldly;

namespace Schedulee.Core.Tests.Extensions
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void InsertSortedWithoutComparer()
        {
            var initialItems = new List<int> {1, 2, 3, 4, 5};
            var toInsert = new[] {6, 7, 8, 9};
            var expectedResult = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
            initialItems.InsertSorted(toInsert);
            expectedResult.Length.ShouldBe(expectedResult.Length);
            for(int i = 0; i < expectedResult.Length; i++)
            {
                initialItems[i].ShouldBe(expectedResult[i]);
            }
        }

        [Test]
        public void InsertSortedWithComparer()
        {
            var initialItems = new List<int> {1, 5, 8, 10, 12};
            var toInsert = new[] {11, 2, 7, 5, 6, 4};
            var expectedResult = new[] {1, 2, 4, 5, 6, 7, 8, 10, 11, 12};
            var comparer = new Comparison<int>((x, y) => x.CompareTo(y));
            initialItems.InsertSorted(toInsert, comparer);
            expectedResult.Length.ShouldBe(expectedResult.Length);
            for(int i = 0; i < expectedResult.Length; i++)
            {
                initialItems[i].ShouldBe(expectedResult[i]);
            }
        }

        [Test]
        public void CountTest()
        {
            var items = new List<int> {1, 2, 3, 4, 5} as IEnumerable;
            var count = items.Count();
            count.ShouldBe(5);
        }

        [Test]
        public void IndexOfWhenAvailableTest()
        {
            var items = new List<int> {1, 2, 3, 4, 5} as IEnumerable;
            var position = items.IndexOf(3);
            position.ShouldBe(2);
        }

        [Test]
        public void IndexOfnWhenNotAvailableTest()
        {
            var items = new List<int> {1, 2, 3, 4, 5} as IEnumerable;
            var position = items.IndexOf(10);
            position.ShouldBe(-1);
        }

        [Test]
        public void ElementAtTest()
        {
            var items = new List<int> {1, 2, 3, 4, 5} as IEnumerable;
            var item = items.ElementAt(2);
            item.ShouldBe(3);
        }

        [Test]
        public void GetItemAtIndexTest()
        {
            var items = new List<int> {1, 2, 3, 4, 5} as IEnumerable<int>;
            var item = items.GetItemAtIndex(2);
            item.ShouldBe(3);
        }

        [Test]
        public void AddRangeTest()
        {
            var initialItems = new List<int> {1, 2, 3, 4, 5};
            var toInsert = new[] {6, 7, 8, 9};
            var expectedResult = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
            initialItems.AddRange(toInsert);
            expectedResult.Length.ShouldBe(expectedResult.Length);
            for(int i = 0; i < expectedResult.Length; i++)
            {
                initialItems[i].ShouldBe(expectedResult[i]);
            }
        }

        [Test]
        public void ToArrayTest()
        {
            var initialItems = new List<int> {1, 2, 3, 4, 5};
            var result = ((IEnumerable) initialItems).ToArray();
            result.Length.ShouldBe(initialItems.Count());
            for(int i = 0; i < initialItems.Count; i++)
            {
                initialItems[i].ShouldBe(result[i]);
            }
        }

        [Test]
        public void EnumerableEqualReturnsFalseForDifferentArraysTest()
        {
            var items1 = Enumerable.Range(1, 10).ToArray();
            var items2 = Enumerable.Range(11, 10).ToArray();
            var result = items1.EnumerableEqual(items2);
            result.ShouldBe(false);
        }

        [Test]
        public void EnumerableEqualReturnsFalseForDifferentLengthArraysTest()
        {
            var items1 = Enumerable.Range(1, 10).ToArray();
            var items2 = Enumerable.Range(1, 20).ToArray();
            var result = items1.EnumerableEqual(items2);
            result.ShouldBe(false);
        }

        [Test]
        public void EnumerableEqualReturnsTrueForEqualArraysTest()
        {
            var items1 = Enumerable.Range(1, 10).ToArray();
            var items2 = Enumerable.Range(1, 10).ToArray();
            var result = items1.EnumerableEqual(items2);
            result.ShouldBe(true);
        }
    }
}