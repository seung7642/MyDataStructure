using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyDataStructureLibrary
{
    public class MyArrayList : IEnumerable
    {
        private const int _defaultCapacity = 4;
        private const int _maxArrayLength = 1_000_000;
        
        private Object[] _array;
        
        [ContractPublicPropertyName("Count")]
        private int _size;
        private int position = -1;

        private static readonly object[] _emptyArray = new object[0];

        public MyArrayList() : this(4)
        {
        }

        public MyArrayList(int capacity)
        {
            _size = 0;
            _array = new object[capacity];
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
                        var newItems = new Object[value];
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

        public object this[int index]
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

        public int Add(Object item)
        {
            if (_size == _array.Length)
                EnsureCapacity(_size + 1);

            _array[_size] = item;
            return _size++;
        }

        public int BinarySearch(Object item)
        {
            return BinarySearch(0, Count, item, null);
        }

        public int BinarySearch(Object item, IComparer comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        public int BinarySearch(int index, int count, Object item, IComparer comparer)
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

        public Object Clone()
        {
            var list = new MyArrayList(_size);
            list._size = _size;
            Array.Copy(_array, 0, list._array, 0, _size);
            return list;
        }

        public bool Contains(Object item)
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

        public int IndexOf(Object item)
        {
            return IndexOf(item, 0, _size);
        }

        public int IndexOf(Object item, int startIndex)
        {
            return IndexOf(item, startIndex, _size);
        }

        public int IndexOf(Object item, int startIndex, int count)
        {
            if (startIndex > _size)
                throw new ArgumentOutOfRangeException();
            if (count < 0 || _size - startIndex < count)
                throw new ArgumentOutOfRangeException();

            return Array.IndexOf(_array, item, startIndex, count);
        }

        public int LastIndexOf(Object item)
        {
            return LastIndexOf(item, 0, _size);
        }

        public int LastIndexOf(Object item, int startIndex)
        {
            return LastIndexOf(item, startIndex, _size);
        }

        public int LastIndexOf(Object item, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= _size || _size - startIndex < count)
                throw new ArgumentOutOfRangeException();
            if (_size == 0)
                return -1;

            return Array.LastIndexOf(_array, item, startIndex, count);
        }

        public void Insert(int index, Object item)
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

        public void Remove(Object item)
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
            _array[_size] = null;
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
                _array[--i] = null;
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

        public Object[] ToArray()
        {
            var newArray = new Object[_size];
            Array.Copy(_array, 0, newArray, 0, _size);
            return newArray;
        }

        public IEnumerator GetEnumerator()
        {
            return new MyArrayListEnumerator(this);
        }
    }

    public sealed class MyArrayListEnumerator : IEnumerator
    {
        private MyArrayList list;
        private int index;
        private Object currentElement;
        private bool isMyArrayList;
        
        private static Object dummyObject = new Object();

        public MyArrayListEnumerator(MyArrayList list)
        {
            this.list = list;
            this.index = -1;
            currentElement = dummyObject;
            isMyArrayList = list.GetType() == typeof(MyArrayList);
        }

        public bool MoveNext()
        {
            if (isMyArrayList) {
                if (index < list.Count - 1) {
                    currentElement = list[++index];
                    return true;
                }
                else {
                    currentElement = dummyObject;
                    index = list.Count;
                    return false;
                }
            }
            else {
                if (index < list.Count - 1) {
                    currentElement = list[++index];
                    return true;
                }
                else {
                    currentElement = dummyObject;
                    index = list.Count;
                    return false;
                }
            }
        }

        public Object Current
        {
            get {
                var temp = currentElement;
                if (dummyObject == temp) {
                    if (index == -1) {
                        throw new InvalidOperationException();
                    }
                    else {
                        throw new InvalidOperationException();
                    }
                }

                return temp;
            }
        }

        public void Reset()
        {
            currentElement = dummyObject;
            index = -1;
        }
    }
}