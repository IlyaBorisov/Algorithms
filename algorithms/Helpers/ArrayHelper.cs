using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Helpers
{
    public static class ArrayHelper
    {
        public static void Show<Tin>(this Tin[] arr, bool check, bool show, Tin[] sourcearr) where Tin : IComparable<Tin>
        {
            if (show)
            {
                Console.WriteLine(string.Join(" ", arr));
            }            
            if (check)
            {
                bool? direction = null;
                for (int i = 1; i < arr.Length; i++)
                {
                    if (!(arr[i].CompareTo(arr[i - 1]) == 0))
                    {
                        if (direction.HasValue)
                        {
                            if ((arr[i].CompareTo(arr[i - 1]) < 0) == direction.Value)
                            {
                                Console.WriteLine("Sorted : NO");
                                return;
                            }                           
                        }
                        else
                        {
                            direction = arr[i].CompareTo(arr[i - 1]) > 0;
                        }                                                
                    }
                }

                if (direction.HasValue)
                {
                    Console.WriteLine("Sorted : OK");
                }
                else
                {
                    Console.WriteLine("All array elements are the same");
                }

                if (sourcearr != null)
                {
                    if (sourcearr.Length == arr.Length)
                    {
                        if (direction.HasValue)
                        {
                            var sortedSourceArr = direction.Value ? sourcearr.OrderBy(e => e).ToArray() : 
                                                                    sourcearr.OrderByDescending(e => e).ToArray();
                            if (arr.SequenceEqual(sortedSourceArr))
                            {
                                Console.WriteLine("Source : OK");
                                return;
                            }
                        }
                        else
                        {
                            if (sourcearr[0].CompareTo(arr[0]) == 0)
                            {
                                Console.WriteLine("Source : OK");
                                return;
                            }
                        }                        
                    }
                    Console.WriteLine("Source : NO");
                }
            }            
        }

        public static int[] CreateRandomArray(int length, int minElement, int maxElement)
        {
            var random = new Random();
            return Enumerable.Range(0, length).Select(i => random.Next(minElement, maxElement)).ToArray();
        }

        public static int[] CreateRandomSet(int length, int minElement = int.MinValue, int maxElement = int.MaxValue)
        {
            var range = (long)maxElement - minElement;
            if (range < length)
            {
                Console.WriteLine("unable to create set");
                return null;
            }

            var random = new Random();
            var arr = new int[length];
            var elements = new HashSet<int>();

            while (length >= 1)
            {
                var newElement = random.Next(minElement, maxElement);
                if (!elements.Contains(newElement))
                {
                    elements.Add(newElement);
                    arr[length - 1] = newElement;
                    length--;
                }
            }
            return arr;
        }

        public static void Swap(this int[] arr, int index1, int index2)
        {
            int temp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = temp;
        }

        public static void Swap<Tin>(this Tin[] arr, int index1, int index2) where Tin : IComparable<Tin>
        {
            Tin temp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = temp;
        }
    }
}
