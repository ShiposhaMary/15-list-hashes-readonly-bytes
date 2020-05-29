using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace hashes
{
    public class My_ReadonlyBytes : IEnumerable<byte>
    {
        public int Length => list.Length;
        private readonly byte[] list;
        private int hash;

        public My_ReadonlyBytes(params byte[] array)
        {   if (array == null) throw new ArgumentNullException();
            else list = array;
            HashCodeInside();
        }

        public byte this[int index]
        {
            get
            {
                if (index >= list.Length) throw new IndexOutOfRangeException();
                return list[index];
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            
             if (!(obj!=null&&obj.GetType() == typeof(My_ReadonlyBytes) &&
                  Length == ((My_ReadonlyBytes)obj).Length)) return false;

            return Equals((My_ReadonlyBytes)obj);
        }

        private bool Equals(My_ReadonlyBytes other)
        {
            var isItemsEqual = true;
            var count = 0;
            while (isItemsEqual && count < Length)
            {
                isItemsEqual = this[count] == other[count];
                ++count;
            }

            return isItemsEqual;
        }

        private void HashCodeInside()
        {
            const int fnvPrime = 3123423;
            foreach (var item in list)
            {                            //FNV (англ. Fowler–Noll–Vo) — простая хеш-функция для общего применения
                unchecked
                {
                    hash *= fnvPrime;
                    hash ^= item;
                }
            }
        }

        public override int GetHashCode()
        {
            return hash;
        }

        public override string ToString()
        {
            string stToString = "[";
            for (var i = 0; i < list.Length; i++)
            {   if (i == list.Length - 1)
                    stToString = stToString + list[i].ToString() + "";
                else stToString = stToString + list[i].ToString() + ", ";
        }
            return stToString + "]";
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
