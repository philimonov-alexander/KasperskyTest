using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("UnitTest")]
namespace Task1
{
    /// <summary>
    /// Представляет потокобезопасную очередь
    /// </summary>
    public class SynchronizedQueue<T>
    {
        private object _lockObj;
        private AutoResetEvent _waitEvent;

        internal Queue<T> _queue;

        public SynchronizedQueue()
        {
            _queue = new Queue<T>();
            _lockObj = new object();
            _waitEvent = new AutoResetEvent(false);
        }
        
        /// <param name="collection">Коллекция, элементы которой будут скопированы в новый экзепляр коллекции</param>
        public SynchronizedQueue(IEnumerable<T> collection) : this()
        {
            _queue =  new Queue<T>(collection);
        }

        public void Push(T item)
        {
            Monitor.Enter(_lockObj);
            try 
            {
                _waitEvent.Reset();
                _queue.Enqueue(item);
                _waitEvent.Set();
            }
            finally
            {
                Monitor.Exit(_lockObj);
            }
        }

        /// <summary>
        /// Возвращает элемент очереди или ожидает добавление нового, если очередь пуста
        /// </summary>
        public T Pop()
        {
            Monitor.Enter(_lockObj);
            try
            {
                return _queue.Dequeue(); 
            }
            catch (InvalidOperationException) { }
            finally
            {
                Monitor.Exit(_lockObj);
            }

            #warning Уточнить постановку, при многопоточном доступе не гарантируется очередность потоков
            _waitEvent.WaitOne();
            return Pop();
        }
    }
}
