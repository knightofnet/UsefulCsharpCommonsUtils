using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulCsharpCommonsUtils.collection
{
    public class CircularStack<T>
    {
        private readonly List<T> _innerList;
        private readonly int _maxSize;

        public CircularStack(int size)
        {
            _maxSize = size;
            _innerList = new List<T>(size);
        }

        public void Push(T item)
        {
            if (_innerList.Count >= _maxSize)
            {
                _innerList.RemoveAt(0);
            }

            _innerList.Add(item);
        }

        public T Peek()
        {
            return _innerList.LastOrDefault();
          
        }

        public T Pop()
        {
            T last = Peek();
            if (last != null)
            {
                _innerList.RemoveAt(_innerList.Count - 1);
            }

            return last;
        }



        public IEnumerable<T> Items => _innerList;
    }
}
