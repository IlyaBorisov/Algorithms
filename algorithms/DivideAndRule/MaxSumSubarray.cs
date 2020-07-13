using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms.DivideAndRule
{
    public static class MaxSumSubarray
    {
        /// <summary>
        /// divide and rule O(nlogn) algorithm for finding max sum of subarray
        /// </summary>
        public static (int low, int high, int sum) MaxSumSubarray1(this int[] arr)
        {
            return FindMaxSubarray(arr, 0, arr.Length - 1);
        }

        private static (int low, int high, int sum) FindMaxSubarray(int[] arr, int low, int high)
        {
            if (low == high)
            {
                return (low, high, arr[low]);
            }
            else
            {
                int mid = (low + high) / 2;

                (int left_low, int left_high, int left_sum) = FindMaxSubarray(arr, low, mid);
                (int right_low, int right_high, int right_sum) = FindMaxSubarray(arr, mid + 1, high);
                (int cross_low, int cross_high, int cross_sum) = FindMaxCrossingSubarray(arr, low, mid, high);

                if (left_sum >= right_sum && left_sum >= cross_sum)
                {
                    return (left_low, left_high, left_sum);
                }
                else if (right_sum >= left_sum && right_sum >= cross_sum)
                {
                    return (right_low, right_high, right_sum);
                }
                else
                {
                    return (cross_low, cross_high, cross_sum);
                }
            }
        }

        private static (int cross_low, int cross_high, int cross_sum) FindMaxCrossingSubarray(int[] arr, int low, int mid, int high)
        {
            int left_sum = int.MinValue;
            int max_left = mid;
            int sum = 0;
            for (int i = mid; i >= low; i--)
            {
                sum += arr[i];
                if (sum > left_sum)
                {
                    left_sum = sum;
                    max_left = i;
                }
            }
            int right_sum = int.MinValue;
            int max_right = mid + 1;
            sum = 0;
            for (int i = mid + 1; i <= high; i++)
            {
                sum += arr[i];
                if (sum > right_sum)
                {
                    right_sum = sum;
                    max_right = i;
                }
            }
            return (max_left, max_right, left_sum + right_sum);
        }

        /// <summary>
        /// O(n) algorithm for finding max sum of subarray
        /// </summary>
        public static (int low, int high, int sum) MaxSumSubarray2(this int[] arr)
        {
            int low = 0;
            int lowcurr = 0;
            int high = 0;
            int maxsubArrCurr = 0;
            int maxsubArr = arr[0];
            int maxMinus = int.MinValue;
            int maxMinusIndex = 0;
            bool withPlus = false;
            for (int i = 0; i < arr.Length; i++)
            {
                maxsubArrCurr += arr[i];

                if (maxsubArrCurr > maxsubArr)
                {
                    maxsubArr = maxsubArrCurr;
                    low = lowcurr;
                    high = i;                    
                }                    
                if (maxsubArrCurr < 0)
                {
                    maxsubArrCurr = 0;
                    lowcurr = i + 1;
                }                    
                if (arr[i] > 0)
                {
                    withPlus = true;
                }
                else if (!withPlus && arr[i] > maxMinus)
                {
                    maxMinus = arr[i];
                    maxMinusIndex = i;
                }                    
            }
            if (!withPlus)
                return (maxMinusIndex, maxMinusIndex, maxMinus);
            else
                return (low, high, maxsubArr);
        }
    }
}
