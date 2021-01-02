using System;
using MyDataStructureLibrary;

namespace MyDataStructure.Test
{
    public class MyArrayListTest
    {
        public static void Test()
        {
            var list = new MyArrayList();
            list.Add(1);
            list.Add(2);
            list.Add(4);
            list.Add(5);
            list.Insert(2, 3);
            
            list.Reverse();
            Print(list);
        }

        private static void Print(MyArrayList list)
        {
            foreach (var item in list) {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
    }
}