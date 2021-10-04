using System;
using System.Collections;
namespace List
{
    class Program
    {
        static void Main(string[] args)
        {
            _List l = new _List();
            l.Add(1);
            l.Add(2);
            l.Add(3);
            l.Add(4);
            foreach (var item in l)
            {
                Console.WriteLine(item);
            }

        }
    }
    public class _List : IEnumerable
    {
        private const int _defaultCapacity = 4;

        private int[] _items;
        private int _size;
        private int _version;

        static readonly int[] _emptyArray = new int[0];

        public _List()
        {
            _items = _emptyArray;
        }

        public _List(int capacity)
        {
            if (capacity < 0)
                return;

            if (capacity == 0)
                _items = _emptyArray;
            else
                _items = new int[capacity];
        }
        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        int[] newItems = new int[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }
        public int Count
        {
            get
            {
                return _size;
            }
        }
        public int this[int index]
        {
            get
            {
                return _items[index];
            }

            set
            {
                _items[index] = value;
                _version++;
            }
        }
        public void Add(int item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
            _version++;
        }
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > 0X7FEFFFFF) newCapacity = 0X7FEFFFFF;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }
        public _List GetRange(int index, int count)
        {

            _List list = new _List(count);
            Array.Copy(_items, index, list._items, 0, count);
            list._size = count;
            return list;
        }
        public int IndexOf(int item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }
        public void Insert(int index, int item)
        {
            if ((uint)index > (uint)_size)
            {
                return;
            }
            if (_size == _items.Length)
                EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;
            _version++;
        }
        public int LastIndexOf(int item, int index, int count)
        {
            if (_size == 0)
            {
                return -1;
            }
            else
            {
                return Array.LastIndexOf(_items, item, index, count);
            }
        }
        public bool Remove(int item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                return;
            }
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default;
            _version++;
        }

        public void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                return;
            }

            if (count < 0)
            {
                return;
            }

            if (_size - index < count)
            {
                return;
            }

            if (count > 0)
            {
                int i = _size;
                _size -= count;
                if (index < _size)
                {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }
                Array.Clear(_items, _size, count);
                _version++;
            }
        }
        public void Reverse()
        {
            Reverse(0, Count);
        }
        public void Reverse(int index, int count)
        {
            if (index < 0)
            {
                return;
            }

            if (count < 0)
            {
                return;
            }

            if (_size - index < count)
                return;
            Array.Reverse(_items, index, count);
            _version++;
        }

        public IEnumerator GetEnumerator()
        {
            return new _ListEnumerator(_items, _size);
        }
    }
    public class _ListEnumerator : IEnumerator
    {
        private int _counter = 0;
        private readonly int _size;
        private readonly int[] _source;
        public _ListEnumerator(int[] source, int size)
        {
            _source = source;
            _size = size;
        }
        public object Current => _source[_counter++];

        public bool MoveNext()
        {
            return _counter < _size;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}