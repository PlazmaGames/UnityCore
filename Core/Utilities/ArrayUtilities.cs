using System;
using System.Collections;

namespace PlazmaGames.Core.Utils
{
    public sealed class ArrayUtilities
    {
        /// <summary>
        /// Replaces a elements in the array with another.
        /// </summary>
        public static void Replace<T>(T[,] arr, T val, T newVal) where T : IEnumerable
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j].Equals(val)) arr[i, j] = newVal;
                }
            }
        }

        /// <summary>
        /// Count the number of elements equal to val in the 2D array.
        /// </summary>
        public static int Count<T>(T[,] arr, T val) where T : IEnumerable
        {
            int count = 0;

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j].Equals(val)) count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Count the number of elements equal to val in the 2D array.
        /// </summary>
        public static int Count<T>(T[,] arr, T val, int offset) where T : IEnumerable
        {
            int count = 0;

            for (int i = offset; i < arr.GetLength(0) - offset; i++)
            {
                for (int j = offset; j < arr.GetLength(1) - offset; j++)
                {
                    if (arr[i, j].Equals(val)) count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        public static T[] Concat<T>(T[] x, T[] y) where T : IEnumerable
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }

        /// <summary>
        /// Fill an array with a single value
        /// </summary>
        public static void Fill<T>(ref T[] arr, T val) where T : IEnumerable
        {
            for (int i = 0; i < arr.Length; i++) arr[i] = val;
        }

        /// <summary>
        /// Fill a 2D array with a single value
        /// </summary>
        public static void Fill<T>(ref T[,] arr, T val)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = val;
                }
            }
        }

        /// <summary>
        /// Creates an array and fills it with a default value 
        /// </summary>
        public static T[] CreateAndFill<T>(int size, T val) where T : IEnumerable
        {
            T[] arr = new T[size];
            Fill(ref arr, val);
            return arr;
        }

        /// <summary>
        /// Creates a 2D array and fills it with a default value 
        /// </summary>
        public static T[,] CreateAndFill<T>(int sizeX, int sizeY, T val) where T : IEnumerable
        {
            T[,] arr = new T[sizeX, sizeY];
            Fill(ref arr, val);
            return arr;
        }
    }
}

