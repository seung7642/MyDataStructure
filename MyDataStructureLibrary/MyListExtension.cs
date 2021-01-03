using System;

namespace MyDataStructureLibrary
{
    public static class MyListExtension
    {
        public static T Find<T>(this MyList<T> list, Predicate<T> match)
        {
            var index = list.FindIndex(0, list.Count, match);

            if (index != -1) {
                return list[index];
            }

            return default(T);
        }

        public static int FindIndex<T>(this MyList<T> list, Predicate<T> match)
        {
            return FindIndex(list, 0, list.Count, match);
        }

        public static int FindIndex<T>(this MyList<T> list, int startIndex, int count, Predicate<T> match)
        {
            if (count < 0 || startIndex < 0 || startIndex >= list.Count)
                throw new ArgumentOutOfRangeException();
            if (list.Count - startIndex < count)
                throw new ArgumentOutOfRangeException();

            for (var index = startIndex; index < (startIndex + count); index++) {
                if (match(list[index]))
                    return index;
            }

            return -1;
        }

        public static MyList<T> FindAll<T>(this MyList<T> list, Predicate<T> match)
        {
            var objList = new MyList<T>();

            foreach (var item in list) {
                if (match(item))
                    objList.Add(item);
            }
            
            return objList;
        }

        public static bool Contains<T>(this MyList<T> list, Predicate<T> match)
        {
            var index = FindIndex(list, 0, list.Count, match);

            if (index != -1)
                return true;

            return false;
        }
        
        public static void ForEach<T>(this MyList<T> list, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException();
            
            foreach (var item in list) {
                action(item);
            }
        }
    }
}