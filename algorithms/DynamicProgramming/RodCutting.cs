using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.DynamicProgramming
{
    public static class RodCutting
    {
        public static int CutRodNaive(int[] prices,int size)
        {
            if (size == 0)
            {
                return 0;
            }

            var localMax = int.MinValue;

            for (int i = 1; i <= size; i++)
            {
                localMax = Math.Max(localMax, prices[i] + CutRodNaive(prices, size - i));
            }
            return localMax;
        }

        public static int CutRodMemoized(int[] prices, int size)
        {
            var memos = Enumerable.Range(0, size + 1).Select(m => int.MinValue).ToArray();

            return CutRodMemoizedAux(prices, size, memos); 
        }

        private static int CutRodMemoizedAux(int[] prices, int size, int[] memos)
        {
            if (memos[size] >= 0)
            {
                return memos[size];
            }

            int localMax;
            if (size == 0)
            {
                localMax = 0;
            }
            else
            {
                localMax = int.MinValue;

                for (int i = 1; i <= size; i++)
                {
                    localMax = Math.Max(localMax, prices[i] + CutRodMemoizedAux(prices, size - i, memos));
                }
            }

            memos[size] = localMax;

            return localMax;
        }

        public static (int,List<int>) CutRodBottomUpWithPieces(int[] prices, int size)
        {
            var memos = new int[size + 1];
            var pieces = new int[size + 1];
            memos[0] = 0;            

            for (int j = 1; j <= size; j++)
            {
                var localMax = int.MinValue;

                for (int i = 1; i <= j; i++)
                {
                    if (localMax < prices[i] + memos[j - i])
                    {
                        localMax = prices[i] + memos[j - i];
                        pieces[j] = i;
                    }
                }
                memos[j] = localMax;
            }

            var piecesResult = new List<int>();
            while (size > 0)
            {
                piecesResult.Add(pieces[size]);
                size -= pieces[size];
            }
            return (memos[size],piecesResult);
        }
    }
}
