using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Policy;

namespace MyDataStructureLibrary
{
    /// <summary>
    /// Dictionary<T>와 유사하게 배열 기반 구현이고, 버킷 배열을 사용하여 해시 값을 Slots
    /// 배열에 매핑한다. 동일한 해시 값에 대해선 해당 인덱스의 리스트에 추가한다.
    ///
    /// capacity는 항상 소수(prime)다. 따라서, 원소가 추가됨에 따라 크기를 리사이징하게되면
    /// capacity는 마지막 capacity의 두 배보다 큰 다음 소수(prime)로 선택된다. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyHashSet<T> : IEnumerable<T>
    {
        private MyList<T>[] _buckets;
        
        [ContractPublicPropertyName("Count")]
        private int _count;

        [ContractPublicPropertyName("Comparer")]
        private IEqualityComparer<T> _comparer;

        #region [Constructors]

        public MyHashSet() 
            : this(12, EqualityComparer<T>.Default)
        {
            
        }

        public MyHashSet(int capacity) 
            : this(capacity, EqualityComparer<T>.Default)
        {
            
        }
        
        public MyHashSet(IEqualityComparer<T> comparer = null) 
            : this(12, comparer)
        {
            _comparer = comparer;
            _count = 0;
        }

        public MyHashSet(int capacity, IEqualityComparer<T> comparer = null)
        {
            var size = HashHelpers.GetPrime(capacity);
            _buckets = new MyList<T>[size];
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }
        
        #endregion

        private MyList<T> FindBucketList(T item)
        {
            var hashCode = _comparer.GetHashCode(item) & 0x7fffffff;
            var index = hashCode % _buckets.Length;
            return _buckets[index];
        }

        private void Resize(int capacity)
        {
            var newSize = HashHelpers.GetPrime(capacity);
            var newBucket = new MyList<T>[newSize];

            foreach (var bucket in _buckets) {
                foreach (var item in bucket) {
                    var hashCode = _comparer.GetHashCode(item) & 0x7fffffff;
                    var index = hashCode % newSize;
                    if (newBucket[index] == null) {
                        newBucket[index] = new MyList<T>();
                    }

                    newBucket[index].Add(item);
                }
            }

            _buckets = newBucket;
        }

        public bool Add(T item)
        {
            if (_count >= _buckets.Length * HashHelpers.RESIZE_FACTOR) {
                Resize(_buckets.Length * HashHelpers.PRIME_FACTOR);
            }
            
            var hashCode = _comparer.GetHashCode(item) & 0x7fffffff;
            var index = hashCode % _buckets.Length;
            var bucket = _buckets[index];
            if (bucket == null) {
                bucket = new MyList<T>();
                _buckets[index] = bucket;
            }

            if (!_buckets[index].Contains(item)) {
                _buckets[index].Add(item);
                _count++;
                return true;
            }

            return false;
        }

        public bool Remove(T item)
        {
            var bucket = FindBucketList(item);
            if (bucket == null)
                return false;

            if (bucket.Remove(item)) {
                _count--;
                return true;
            }

            return false;
        }
        
        private void Initialize(int capacity)
        {
            var size = HashHelpers.GetPrime(capacity);
            _buckets = new MyList<T>[size];
        }
        

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(T item)
        {
            var bucket = FindBucketList(item);
            if (bucket == null) {
                return false;
            }

            return bucket.Contains(T => _comparer.Equals(T, item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MyHashSetEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #region [Enumerator class]
        public class MyHashSetEnumerator<T> : IEnumerator<T>
        {
            private MyHashSet<T> _hashSet;
            private IEnumerator<T> _iterator;
            private int _index;

            public MyHashSetEnumerator(MyHashSet<T> hashSet)
            {
                _hashSet = hashSet;
                _index = 0;
                _iterator = FindNextIterator();
            }

            public IEnumerator<T> FindNextIterator()
            {
                // TODO: 현재 인덱스가 해시셋의 버킷배열의 크기보다 작을 때까지 반복한다.
                // 버킷배열에 할당된 연결리스트를 가져온 후 현재 인덱스를 하나 증가시킨다.
                // 연결리스트가 존재하고 리스트에 추가되어 있는 항목의 갯수가 0보다 크다면
                // 연결리스트의 GetEnumerator() 결과를 리턴한다. 
                while (_index < _hashSet._count) {
                
                }
            
                return null;
            }

            public T Current
            {
                get;
            }

            public bool MoveNext()
            {
                // _iterator가 null이 아니고 _iterator의 MoveNext() 결과값이 false 일때까지
                // FindNextEnumerator를 호출하여 다음 버킷에 있는 연결리스트를 찾는다. 
                while (_iterator != null && !_iterator.MoveNext()) {
                
                }

                // IEnumerator가 null이 아니면 MoveNext()가 성공한 것이므로 Current를 호출할 수 있다.
                return _iterator != null;
            }

            public void Reset()
            {
                _index = 0;
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
        #endregion
    }
}