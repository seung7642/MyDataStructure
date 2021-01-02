using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyDataStructureLibrary
{
    public class MyList<T> : IEnumerable<T>
    {
        private readonly int _defaultCapacity = 4;
        private readonly int _maxArrayLength = 1_000_000;
        
        private T[] _array;
        
        [ContractPublicPropertyName("Count")]
        private int _size;
        private int position = -1;

        private static readonly T[] _emptyArray = new T[0];

        public MyList() : this(4)
        {
        }

        public MyList(int capacity)
        {
            _size = 0;
            _array = new T[capacity];
        }

        public MyList(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            var c = collection as ICollection<T>;
            if (c != null) {
                var count = c.Count;
                if (count == 0) {
                    _array = _emptyArray;
                }
                else {
                    _array = new T[count];
                    c.CopyTo(_array, 0);
                }
            }
            else {
                _size = 0;
                _array = _emptyArray;
            }
        }

        public int Count
        {
            get {
                return _size;
            }
        }

        public int Capacity
        {
            get {
                return _array.Length;
            }
            set {
                if (value < _size)
                    throw new ArgumentOutOfRangeException();

                if (value != _array.Length) {
                    if (value > 0) {
                        var newItems = new T[value];
                        if (_size > 0)
                            Array.Copy(_array, 0, newItems, 0, _size);
                        _array = newItems;
                    }
                    else {
                        _array = _emptyArray;
                    }
                }
            }
        }

        public bool IsReadOnly
        {
            get {
                return false;
            }
        }

        public T this[int index]
        {
            get {
                if (index < 0 || index >= _size)
                    throw new ArgumentOutOfRangeException();
                return _array[index];
            }
            set {
                if (index < 0 || index >= _size)
                    throw new ArgumentOutOfRangeException();
                _array[index] = value;
            }
        }

        public int Add(T item)
        {
            if (_size == _array.Length)
                EnsureCapacity(_size + 1);

            _array[_size] = item;
            return _size++;
        }

        public int BinarySearch(T item)
        {
            return BinarySearch(0, Count, item, null);
        }

        public int BinarySearch(T item, IComparer comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException();
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            if (_size - index < count)
                throw new ArgumentException();

            return Array.BinarySearch(_array, index, count, item, comparer);
        }

        public void Clear()
        {
            if (_size > 0) {
                Array.Clear(_array, 0, _size);
            }
        }

        public MyList<T> Clone()
        {
            var list = new MyList<T>(_size);
            list._size = _size;
            Array.Copy(_array, 0, list._array, 0, _size);
            return list;
        }

        public bool Contains(T item)
        {
            if (item == null) {
                foreach (var e in _array) {
                    if (e == null)
                        return true;
                }
            }
            else {
                foreach (var e in _array) {
                    if (e != null && e.Equals(item))
                        return true;
                }
            }

            return false;
        }

        public void CopyTo(Array array)
        {
            CopyTo(0, array, 0, _size);
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, _size);
        }

        public void CopyTo(int index, Array array, int arrayIndex, int count)
        {
            if (_size - index < count)
                throw new ArgumentException();
            
            Array.Copy(_array, index, array, arrayIndex, count);
        }
        
        private void EnsureCapacity(int min)
        {
            if (_array.Length < min) {
                int newCapacity = _array.Length == 0 ? _defaultCapacity : _array.Length * 2;

                if (newCapacity > _maxArrayLength)
                    newCapacity = _maxArrayLength;

                if (newCapacity < min)
                    newCapacity = min;

                Capacity = newCapacity;
            }
        }

        public int IndexOf(T item)
        {
            return IndexOf(item, 0, _size);
        }

        public int IndexOf(T item, int startIndex)
        {
            return IndexOf(item, startIndex, _size);
        }

        public int IndexOf(T item, int startIndex, int count)
        {
            if (startIndex > _size)
                throw new ArgumentOutOfRangeException();
            if (count < 0 || _size - startIndex < count)
                throw new ArgumentOutOfRangeException();

            return Array.IndexOf(_array, item, startIndex, count);
        }

        public int LastIndexOf(T item)
        {
            return LastIndexOf(item, 0, _size);
        }

        public int LastIndexOf(T item, int startIndex)
        {
            return LastIndexOf(item, startIndex, _size);
        }

        public int LastIndexOf(T item, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= _size || _size - startIndex < count)
                throw new ArgumentOutOfRangeException();
            if (_size == 0)
                return -1;

            return Array.LastIndexOf(_array, item, startIndex, count);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > _size)
                throw new ArgumentOutOfRangeException();
            
            if (_size == _array.Length)
                EnsureCapacity(_size + 1);
            
            if (index < _size)
                Array.Copy(_array, index, _array, index + 1, _size - index);

            _array[index] = item;
            _size++;
        }

        public void Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
                RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();
            
            Array.Copy(_array, index + 1, _array, index, _size - (index + 1));
            _size--;
            _array[_size] = default(T);
        }

        public void RemoveRange(int index, int count)
        {
            if (count < 0 || index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();
            if (_size - index < count)
                throw new ArgumentOutOfRangeException();
            
            Array.Copy(_array, index + count, _array, index, _size - (index + count));
            var i = _size;
            _size -= count;
            while (i > _size) {
                _array[--i] = default(T);
            }
        }

        public void Reverse()
        {
            Reverse(0, _size);
        }

        public void Reverse(int index, int count)
        {
            if (count < 0 || index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();
            if (_size - index < count)
                throw new ArgumentOutOfRangeException();
            
            Array.Reverse(_array, index, count);
        }

        public void Sort()
        {
            Sort(0, _size, Comparer.Default);
        }

        public void Sort(IComparer comparer)
        {
            Sort(0, _size, comparer);
        }

        public void Sort(int index, int count, IComparer comparer)
        {
            if (count < 0 || index < 0 || index >= _size)
                throw new ArgumentOutOfRangeException();
            if (_size - index < count)
                throw new ArgumentOutOfRangeException();
            
            Array.Sort(_array, index, count, comparer);
        }

        public T[] ToArray()
        {
            var newArray = new T[_size];
            Array.Copy(_array, 0, newArray, 0, _size);
            return newArray;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new MyListEnumerator<T>(this);
        }
    }

    public sealed class MyListEnumerator<T> : IEnumerator<T>
    {
        private MyList<T> list;
        private int index;
        private T current;
        
        public MyListEnumerator(MyList<T> list)
        {
            this.list = list;
            this.index = -1;
            current = default(T);
        }

        public bool MoveNext()
        {
            if (++index >= list.Count) {
                return false;
            }

            return true;
        }

        public T Current
        {
            get {
                if (index < 0 || index >= list.Count)
                    throw new ArgumentOutOfRangeException();
                
                return list[index];
            }
        }

        Object IEnumerator.Current
        {
            get {
                return Current;
            }
        }

        public void Reset()
        {
            index = -1;
            current = default(T);
        }

        public void Dispose()
        {
            
        }
    }
}