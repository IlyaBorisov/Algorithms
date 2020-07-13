using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Sorting
{
    public static class DifficultChallenges
    {
        /// <summary>
        /// p.62 ch.2.3.7
        /// </summary>
        public static bool IsSetContainsElementsSumIsNumber(int[] arr, int number)
        {
            if (arr.Length < 2)
            {
                return false;
            }
            else if (arr.Length == 2)
            {
                return arr.Sum() == number;
            }

            Array.Sort(arr);

            int first = 0;
            int second = 0;

            int sub = number - arr[first];

            while (second < arr.Length - 1 && arr[second + 1] < sub)
            {
                second++;
            }

            if (arr[second + 1] == sub)
            {
                return true;
            }

            var expect = number - arr[second];

            do
            {
                first++;
                if (arr[first] == expect)
                {
                    return true;
                }
            } while (first < arr.Length - 2);

            return false;
        }
    }
}
