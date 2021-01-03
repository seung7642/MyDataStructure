using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MyDataStructureLibrary
{
    public class MyHashSet<T> : ICollection<T>, ISet<T>
    {
        private MyList<T>[] _buckets;
        private int _count;
        
        [ContractPublicPropertyName("Comparer")]
        private IEqualityComparer<T> _comparer;
        
        #region Constructors

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

        void ICollection<T>.Add(T item)
        {
            throw new System.NotImplementedException();
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

            foreach (var element in bucket) {
                
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
        public IEnumerator<T> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ISet<T>.Add(T item)
        {
            throw new System.NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new System.NotImplementedException();
        }
    }
}