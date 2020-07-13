using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.SimpleDataStructures
{
    public class ArrayLinkedList
    {
        public int? Head { get; set; }
        public int? Tail { get; set; }

        private readonly int?[] Prev;
        private readonly int?[] Next;
        private readonly int?[] Key;
        private int? Free;
        public ArrayLinkedList(int length)
        {
            Prev = new int?[length];
            Next = new int?[length];
            Key = new int?[length];
            for (int i = 0; i < Next.Length; i++)
            {
                Next[i] = i + 1;
            }
            Next[^1] = null;
            Free = 0;
        }

        public void Add(int key)
        {
            if (Free == null)
            {
                throw new Exception("Overflow");
            }
            Prev[Free.Value] = Tail;
            if (Tail != null)
            {
                Next[Tail.Value] = Free;
            }            
            Tail = Free;
            if (Head == null)
            {
                Head = Tail;
            }
            Key[Tail.Value] = key;            
            Free = Next[Free.Value];
            Next[Tail.Value] = null;
        }

        private void RemoveByIndex(int index)
        {
            Next[Prev[index].Value] = Next[index];
            Prev[Next[index].Value] = Prev[index];
            Key[index] = null;
            Prev[index] = null;
            Next[index] = Free;
            Free = index;
        }

        public void Remove(int keyToRemove)
        {
            var indicies = Key.Where(key => key == keyToRemove).Select(key => key.Value);
            foreach (var index in indicies)
            {
                RemoveByIndex(index);
            }
        }
    }
}
