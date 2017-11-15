using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;

namespace UnitTest
{
    [TestClass]
    public class Task1Test
    {
        [TestMethod]
        public void PushMethodAddItem()
        {
            var quequeRefType = new Task1.SynchronizedQueue<object>();
            var item = new object();
            quequeRefType.Push(item);
            Assert.IsTrue(quequeRefType._queue.Count == 1);
            Assert.IsTrue(quequeRefType._queue.Contains(item));


            var quequeValueType = new Task1.SynchronizedQueue<int>();
            int value = 1;
            quequeValueType.Push(value);
            Assert.IsTrue(quequeValueType._queue.Count == 1);
            Assert.IsTrue(quequeValueType._queue.Contains(value));
        }

        [TestMethod]
        public void PopMethodReturnAndDeleteItem()
        {
            var itemRefType = new object();
            var initCollectionRefType = new object[] { itemRefType };
            var quequeRefType = new Task1.SynchronizedQueue<object>(initCollectionRefType);
            var returnedRefType = quequeRefType.Pop();
            Assert.AreEqual(itemRefType, returnedRefType);
            Assert.IsTrue(quequeRefType._queue.Count == 0);

            int itemValueType = 1;
            var initCollectionValuefType = new int[] { itemValueType };
            var quequeValueType = new Task1.SynchronizedQueue<int>(initCollectionValuefType);
            var returnedValueType = quequeValueType.Pop();
            Assert.AreEqual(itemValueType, returnedValueType);
            Assert.IsTrue(quequeValueType._queue.Count == 0);
        }


        [TestMethod]
        public void PopMethodOnEmptyQuequeWaitPush()
        {
            var queque = new Task1.SynchronizedQueue<object>();
            var item = new object();
            var stopWatch = new Stopwatch();

            var pushThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                queque.Push(item);
            });

            pushThread.Start();
            stopWatch.Start();
            var returned = queque.Pop();
            stopWatch.Stop();

            Assert.IsTrue(stopWatch.ElapsedMilliseconds >= 1000);
            Assert.AreEqual(returned, item);
        }

        [TestMethod]
        public void PopMethodSeveralThreadWaitPush()
        {
            var queque = new Task1.SynchronizedQueue<object>();
            var stopWatch = new Stopwatch();

            var pushThread = new Thread(() =>
            {
                Thread.Sleep(1000);
                queque.Push(new object());
                Thread.Sleep(1000);
                queque.Push(new object());
            });

            var popThread1 = new Thread(() =>
            {
                queque.Pop();
            });
            var popThread2 = new Thread(() =>
            {
                queque.Pop();
            });

            pushThread.Start();
            popThread1.Start();
            popThread2.Start();
            stopWatch.Start();

            popThread1.Join();
            popThread2.Join();
            stopWatch.Stop();

            Assert.IsTrue(stopWatch.ElapsedMilliseconds >= 2000);
        }

    }
}
