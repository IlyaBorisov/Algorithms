using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Helpers;

namespace Algorithms.Sorting
{
    #region Bucket

    public class Bucket
    {
        public double Value;
        public Bucket Next;
        public Bucket(double value)
        {
            Value = value;
        }
    }

    #endregion
    public static class SortExtensions
    {
        private static readonly Random rnd = new Random();

        #region InsertionSort
        public static void InsertionSortAsc(this int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int j = i - 1;
                int key = arr[i];

                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = key;
            }
        }

        public static void InsertionSortDesc(this int[] arr)
        {
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                int j = i + 1;
                int key = arr[i];

                while (j <= arr.Length - 1 && arr[j] > key)
                {
                    arr[j - 1] = arr[j];
                    j++;
                }

                arr[j - 1] = key;
            }
        }

        public static void InsertionSortWithBinarySearch(this int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];

                int index = BinarySearch(arr, key, 0, i - 1);

                if (arr[index] <= key)
                {
                    index++;
                }

                Buffer.BlockCopy(arr, 4 * index, arr, 4 * (index + 1), 4 * (i - index));

                arr[index] = key;
            }
        }

        private static int BinarySearch(int[] arr, int key, int low, int high)
        {
            if (low == high)
            {
                return low;
            }

            int mid = low + ((high - low) / 2);

            if (arr[mid] > key)
            {
                return BinarySearch(arr, key, low, mid);
            }
            else if (arr[mid] < key)
            {
                return BinarySearch(arr, key, mid + 1, high);
            }
            else
            {
                return mid + 1;
            }
        }

        #endregion

        #region SelectionSort
        public static void SelectionSortAsc(this int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int min = int.MaxValue;
                int minIndex = i;
                for (int j = i; j < arr.Length; j++)
                {
                    if (arr[j] < min)
                    {
                        min = arr[j];
                        minIndex = j;
                    }
                }
                arr.Swap(i, minIndex);
            }
        }
        #endregion

        #region BubbleSort
        public static void BubbleSortAsc(this int[] arr)
        {            
            for (int i = 1; i < arr.Length; i++)
            {
                var swapCount = 0;
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        arr.Swap(j, j - 1);
                        swapCount++;
                    }
                }
                if (swapCount == 0)
                {
                    break;
                }
            }
        }
        #endregion

        #region MergeSort
        public static int[] MergeSort(this int[] arr, out int inversions)
        {
            var sortedarr = new int[arr.Length];
            Array.Copy(arr, sortedarr, arr.Length);
            inversions = 0;
            sortedarr.MergeSort(0, arr.Length - 1, ref inversions);
            return sortedarr;
        }
        
        private static void MergeSort(this int[] arr, int first, int last, ref int inversions)
        {
            if (first < last)
            {
                int middle = (first + last) / 2;
                arr.MergeSort(first, middle, ref inversions);
                arr.MergeSort(middle + 1, last, ref inversions);
                arr.Merge(first, middle, last, ref inversions);
            }
        }

        private static void Merge(this int[] arr, int first, int middle, int last, ref int inversions)
        {
            var arr1Length = middle - first + 1;
            var arr2Length = last - middle;

            var arr1 = new int[arr1Length];
            var arr2 = new int[arr2Length];

            Array.Copy(arr, first, arr1, 0, arr1Length);
            Array.Copy(arr, middle + 1, arr2, 0, arr2Length);

            var i = 0;
            var j = 0;
            var k = first;

            while (i < arr1Length && j < arr2Length)
            {
                if (arr1[i] <= arr2[j])
                {
                    arr[k] = arr1[i];
                    i++;
                }
                else
                {
                    arr[k] = arr2[j];
                    j++;
                    inversions += middle - i;
                }
                k++;
            }

            Array.Copy(arr1, i, arr, k, arr1Length - i);
            Array.Copy(arr2, j, arr, k, arr2Length - j);
        }
        #endregion

        #region HeapSort

        private static int Left(int i)
        {
            return 2 * i;
        }

        private static int Right(int i)
        {
            return 2 * i + 1;
        }

        private static void MaxHeapify(int[] arr, int heapSize, int i)
        {
            var left = Left(i);
            var right = Right(i);

            int largest;

            if (left < heapSize && arr[left] > arr[i])
            {
                largest = left;
            }
            else
            {
                largest = i;
            }

            if (right < heapSize && arr[right] > arr[largest])
            {
                largest = right;
            }

            if (largest != i)
            {
                ArrayHelper.Swap(arr, largest, i);
                MaxHeapify(arr, heapSize, largest);
            }
        }

        private static void BuildMaxHeap(int[] arr)
        {            
            for (int i = arr.Length / 2; i >= 0; i--)
            {
                MaxHeapify(arr, arr.Length, i);
            }
        }

        public static void HeapSortRef(int[] arr)
        {
            BuildMaxHeap(arr);
            var heapSize = arr.Length;
            for (int i = arr.Length - 1; i >= 1; i--)
            {
                ArrayHelper.Swap(arr, 0, i);
                heapSize--;
                MaxHeapify(arr, heapSize, 0);
            }
        }

        public static int[] HeapSort(this int[] arr)
        {
            var arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);
            HeapSortRef(arrCopy);
            return arrCopy;
        }

        #endregion

        #region QuickSort

        public static int[] QuickSortIns(this int[] arr, int ins)
        {
            var arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);
            QuickSortIns(arrCopy, 0, arrCopy.Length - 1, ins);
            arrCopy.InsertionSortAsc();
            return arrCopy;
        }

        private static void QuickSortIns(int[] arr, int low, int high, int ins)
        {
            if (high - low > ins)
            {
                int mid = PartitionStandard(arr, low, high);
                QuickSortIns(arr, low, mid - 1, ins);
                QuickSortIns(arr, mid + 1, high, ins);
            }
        }

        public static Tin[] QuickSort<Tin>(this Tin[] arr, Action<Tin[], int, int> action) where Tin : IComparable<Tin>
        {
            var arrCopy = new Tin[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);
            action(arrCopy, 0, arrCopy.Length - 1);
            return arrCopy;
        }

        public static void QuickSortHoare<Tin>(Tin[] arr, int low, int high) where Tin : IComparable<Tin>
        {
            if (low < high)
            {
                int mid = PartitionHoare(arr, low, high);
                QuickSortHoare(arr, low, mid - 1);
                QuickSortHoare(arr, mid + 1, high);
            }            
        }

        public static void QuickSortSameElements(int[] arr, int low, int high)
        {
            if (low < high)
            {
                (int q, int t) = PartitionSameElements(arr, low, high);
                QuickSortSameElements(arr, low, q - 1);
                QuickSortSameElements(arr, t + 1, high);
            }
        }

        public static void QuickSortTailRecursive(int[] arr, int low, int high)
        {
            while (low < high)
            {
                int mid = PartitionHoare(arr, low, high);
                QuickSortHoare(arr, low, mid - 1);
                low = mid + 1;
            }
        }

        private static int PartitionStandard(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;
            for (int j = low; j <= high - 1; j++)
            {
                if (arr[j] <= pivot)
                {
                    i++;
                    ArrayHelper.Swap(arr, i, j);
                }
            }
            ArrayHelper.Swap(arr, i + 1, high);
            return i + 1;
        }

        private static (int, int) PartitionSameElements(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;
            var t = 0;
            for (int j = low; j <= high - 1 - t; j++)
            {
                if (arr[j] < pivot)
                {                    
                    i++;
                    ArrayHelper.Swap(arr, i, j);
                }
                else if (arr[j] == pivot)
                {
                    t++;
                    ArrayHelper.Swap(arr, j, high - t);
                    j--;
                }
            }
            Array.Copy(arr, i + 1, arr, i + t + 2, high - t - i - 1);
            for (int j = i + 1; j <= i + t + 1; j++)
            {
                arr[j] = pivot;
            }
            return (i + 1, i + t + 1);
        }

        private static int PartitionHoare<Tin>(Tin[] arr, int low, int high) where Tin : IComparable<Tin>
        {
            //var mid = LomutoChoise(arr, low, high);
            //ArrayHelper.Swap(arr, mid, low);
            Tin pivot = arr[low];
            int i = low;
            int j = high + 1;

            while (true)
            {
                do
                {
                    j--;
                } while (arr[j].CompareTo(pivot) > 0);

                do
                {
                    i++;
                } while (i <= high && arr[i].CompareTo(pivot) < 0);

                if (i < j)
                {
                    arr.Swap(i, j);
                }
                else
                {
                    arr.Swap(j, low);
                    return j;
                }
            }
        }

        private static int LomutoChoise(int[] arr, int low, int high)
        {
            var indicies = Enumerable.Range(0, 3).Select(i => rnd.Next(low, high)).ToArray();
            var mid = indicies.Select(i => arr[i]).OrderBy(i => i).ToArray()[1];
            return indicies.Where(i => arr[i] == mid).First();
        }

        #endregion

        #region CountingSort

        public delegate void CountingSortDelegate(ref int[] arr, int low, int high);

        public static int[] CountingSort(this int[] arr, int low, int high, CountingSortDelegate action)
        {
            var arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);
            action(ref arrCopy, low, high);
            return arrCopy;
        }

        public static void CountingSortStable(ref int[] arr, int low, int high)
        {
            var counting = new int[high - low + 1];

            var outputArr = new int[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                counting[arr[i - low]]++;
            }

            for (int i = 1; i < counting.Length; i++)
            {
                counting[i] += counting[i - 1];
            }

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                outputArr[counting[arr[i] - low] - 1] = arr[i];
                counting[arr[i] - low]--;
            }
            arr = outputArr;
        }

        public static void CountingSortUnstable(ref int[] arr, int low, int high)
        {
            var counting = new int[high - low + 1];

            for (int i = 0; i < arr.Length; i++)
            {
                counting[arr[i] - low]++;
            }

            int index = 0;

            for (int i = 0; i < counting.Length; i++)
            {
                for (int j = 0; j < counting[i]; j++)
                {
                    arr[index] = low + i;
                    index++;
                }
            }
        }

        #endregion

        #region RadixSort

        public static int[] RadixSort(int[] arr)
        {
            for (int p = 0; p < 4; p++)
            {
                var outputArr = new int[arr.Length];

                var counting = new int[256];

                for (int i = 0; i < arr.Length; i++)
                {
                    counting[arr[i].Digit(p)]++;
                }

                for (ushort i = 1; i <= 255; i++)
                {
                    counting[i] += counting[i - 1];
                }

                for (int i = arr.Length - 1; i >= 0; i--)
                {
                    var digit = arr[i].Digit(p);
                    counting[digit]--;
                    outputArr[counting[digit]] = arr[i];
                }

                Array.Copy(outputArr, arr, arr.Length);
            }

            return arr;
        }

        public static int Digit(this int n, int p)
        {
            return n >> (8 * p) & 255;
        }

        #endregion

        #region BucketSort

        public static double[] BucketSort(double[] arr)
        {
            var n = arr.Length;
            var buckets = new Bucket[n];
            Bucket temp;
            Bucket prev;
            foreach (var item in arr)
            {
                var index = (int)(n * item);

                if (buckets[index] == null)
                {
                    buckets[index] = new Bucket(item);
                }
                else
                {
                    temp = buckets[index];
                    prev = null;

                    while (temp.Value < item)
                    {
                        prev = temp;
                        temp = temp.Next;
                        if (temp == null)
                        {
                            break;
                        }
                    }

                    var inserted = new Bucket(item);
                    inserted.Next = temp;

                    if (prev == null)
                    {
                        buckets[index] = inserted;
                    }
                    else
                    {
                        prev.Next = inserted;
                    }
                }
            }
            var outputArr = new double[n];
            int outputIndex = 0;
            foreach (var bucket in buckets)
            {
                if (bucket != null)
                {
                    temp = bucket;

                    do
                    {
                        outputArr[outputIndex] = temp.Value;
                        outputIndex++;
                        temp = temp.Next;

                    } while (temp != null);
                }
            }
            return outputArr;
        }

        #endregion

        #region SelectStatistic

        public static Tin SelectStatistic<Tin>(Tin[] arr, int low, int high, int i) where Tin : IComparable<Tin>
        {
            if (low == high)
            {
                return arr[low];
            }

            int mid = PartitionHoare(arr, low, high);

            int stat = mid - low + 1;

            if (stat == i)
            {
                return arr[mid];
            }
            else if (i < stat)
            {
                return SelectStatistic(arr, low, mid - 1, i);
            }
            else
            {
                return SelectStatistic(arr, mid + 1, high, i - stat);
            }
        }

        #endregion
    }
}
