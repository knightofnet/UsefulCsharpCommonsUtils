using System.Collections.Generic;

namespace UsefulCsharpCommonsUtils.collection
{
    internal class CircularQueue<T>
    {

        private readonly Queue<T> _queue;
        private readonly int _maxSize;

        public CircularQueue(int size)
        {
            _maxSize = size;
            _queue = new Queue<T>(size);
        }

        public void Enqueue(T item)
        {
            if (_queue.Count >= _maxSize)
            {
                _queue.Dequeue();
            }

            _queue.Enqueue(item);
        }

       

        public IEnumerable<T> Items => _queue;
    }
}
