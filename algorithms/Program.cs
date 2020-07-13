using Algorithms.Helpers;
using Algorithms.Sorting;
using Algorithms.DivideAndRule;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Algorithms.SimpleDataStructures;
using Algorithms.DynamicProgramming;
using System.IO;

namespace Algorithms
{
	class Program
	{
		static readonly Dictionary<char, byte> _symbMap=new Dictionary<char, byte>();
		static int _baseStart;
		static int _baseEnd = int.MaxValue;
		static readonly Dictionary<string, long> _baseGenesMap = new Dictionary<string, long>();
		struct Range
		{
			public int Start { get; set; }
			public int End { get; set; }
			public string Text { get; set; }
			public Range(string text, int start, int end)
			{
				Start = start;
				End = end;
				Text = text;
			}
		}
		class Vertex
		{
			public Vertex[] NextVertices = new Vertex[_symbMap.Count];
			public Vertex[] Go;
			public Vertex Parent;
			public byte CharToParent;
			public List<int> HealthIndicies;
			public bool IsLeaf;
			public Vertex SuffLink;
			public Vertex GoodSuffLink;
			public Vertex(Vertex parent, byte charToParent)
			{
				Parent = parent;
				CharToParent = charToParent;
			}
		}

		private static Vertex GetSuffLink(Vertex v, Vertex root)
		{
			if (v.SuffLink == null)
			{
				if (v == root || v.Parent == root)
				{
					v.SuffLink = root;
				}
				else
				{
					v.SuffLink = GetLink(GetSuffLink(v.Parent, root), v.CharToParent, root);
				}
			}
			return v.SuffLink;
		}

		private static Vertex GetLink(Vertex v, byte index, Vertex root)
		{
			if (v.Go == null)
			{
				v.Go = new Vertex[_symbMap.Count];
			}
			if (v.Go[index] == null)
			{
				if (v.NextVertices[index] != null)
				{
					v.Go[index] = v.NextVertices[index];
				}
				else if (v == root)
				{
					v.Go[index] = root;
				}
				else
				{
					v.Go[index] = GetLink(GetSuffLink(v, root), index, root);
				}
			}
			return v.Go[index];
		}

		private static Vertex GetGoodLink(Vertex v, Vertex root)
		{
			if (v.GoodSuffLink == null)
			{
				var temp = GetSuffLink(v, root);
				if (temp == root)
				{
					v.GoodSuffLink = root;
				}
				else
				{
					v.GoodSuffLink = temp.IsLeaf ? temp : GetGoodLink(temp, root);
				}
			}
			return v.GoodSuffLink;
		}

		private static void UpdateRoot(string[] genes, ref Vertex root)
		{
			Vertex curr;
			var healthMap = new Dictionary<string, Vertex>();
            for (int i = 0; i < genes.Length; i++)
            {
				if (healthMap.ContainsKey(genes[i]))
                {
					healthMap[genes[i]].HealthIndicies.Add(i);
                }
                else
                {
					curr = root;
					for (int j = 0; j < genes[i].Length; j++)
					{
						var index = _symbMap[genes[i][j]];
						if (curr.NextVertices[index] == null)
						{
							curr.NextVertices[index] = new Vertex(curr, index);
						}
						curr = curr.NextVertices[index];
					}
					curr.IsLeaf = true;
					if (curr.HealthIndicies == null)
					{
						curr.HealthIndicies = new List<int>();
					}
					curr.HealthIndicies.Add(i);
					healthMap.Add(genes[i], curr);
				}				
			}
		}

		private static long GetGeneHealth(int first, int last, string d, long[] health, Vertex root)
		{
			long result = 0;
			long baseresult = 0;
			var curr = root;
			bool textNotInMap = !_baseGenesMap.ContainsKey(d);

			for (int i = 0; i < d.Length; i++)
			{
				if (_symbMap.ContainsKey(d[i]))
				{
					curr = GetLink(curr, _symbMap[d[i]], root);

					for (var v = curr; v != root; v = GetGoodLink(v, root))
					{
						if (v.IsLeaf)
						{
							var firstHealth = v.HealthIndicies.First();
							var lastHealth = v.HealthIndicies.Last();

							if (firstHealth <= last && lastHealth >= first)
                            {
								int leftStart = firstHealth < _baseStart && lastHealth > first ? v.HealthIndicies.FindIndex(h => h >= first && h < _baseStart) : -1;
								int leftEnd = leftStart != -1 ? v.HealthIndicies.FindIndex(leftStart + 1, h => h >= _baseStart) - 1 : -2;

								int rightEnd = lastHealth > _baseEnd && firstHealth < last ? v.HealthIndicies.FindLastIndex(h => h <= last && h > _baseEnd) : -2;
								int rightStart = rightEnd > -1 ? v.HealthIndicies.FindLastIndex(rightEnd, h => h <= _baseEnd) + 1 : -1;

								if (textNotInMap)
								{
									int middleStart = leftEnd != -2 ? leftEnd + 1 : (lastHealth > _baseStart && firstHealth < _baseEnd ? v.HealthIndicies.FindIndex(h => h >= _baseStart) : -1);
									int middleEnd = rightStart != -1 ? rightStart - 1 : (middleStart != -1 ? v.HealthIndicies.FindLastIndex(h => h <= _baseEnd) : -2);

									if (middleStart != -1)
									{
										for (int j = middleStart; j <= middleEnd; j++)
										{
											baseresult += health[v.HealthIndicies[j]];
										}
									}
								}														

								if (leftStart != -1)
                                {
									for (int j = leftStart; j <= leftEnd; j++)
									{
										result += health[v.HealthIndicies[j]];
									}
								}

								if (rightStart != -1)
                                {
									for (int j = rightStart; j <= rightEnd; j++)
									{
										result += health[v.HealthIndicies[j]];
									}
								}								
							}
						}
					}
				}
				else
				{
					curr = root;
				}
			}

			if (textNotInMap)
			{				
				_baseGenesMap.Add(d, baseresult);
			}
			else
			{
				baseresult = _baseGenesMap[d];
			}

			return baseresult + result;
		}
		static void Main(string[] args)
		{
			var rdr = new StreamReader(@"d:\input13.txt");
			var timer = new Stopwatch();
			timer.Start();

			int n = Convert.ToInt32(rdr.ReadLine());
			string[] genes = rdr.ReadLine().Split(' ');
			long[] health = Array.ConvertAll(rdr.ReadLine().Split(' '), healthTemp => Convert.ToInt64(healthTemp));

			byte count = 0;
			foreach (var gene in genes)
			{
				foreach (var symb in gene)
				{
					if (!_symbMap.ContainsKey(symb))
					{
						_symbMap.Add(symb, count++);
					}
				}
			}			

			var root = new Vertex(null, (byte)'*');
			UpdateRoot(genes, ref root);

			int s = Convert.ToInt32(rdr.ReadLine());

			long min = long.MaxValue;
			long max = 0;
			long curr;

			var ranges = new List<Range>();

			for (int sItr = 0; sItr < s; sItr++)
			{
				string[] firstLastd = rdr.ReadLine().Split(' ');
				int first = Convert.ToInt32(firstLastd[0]);
				int last = Convert.ToInt32(firstLastd[1]);
				string d = firstLastd[2];
				ranges.Add(new Range(d, first, last));
				if (first > _baseStart)
					_baseStart = first;
				if (last < _baseEnd)
					_baseEnd = last;
			}

            foreach (var range in ranges)
            {
				curr = GetGeneHealth(range.Start, range.End, range.Text, health, root);
				if (curr > max)
					max = curr;
				if (curr < min)
					min = curr;
			}

			Console.WriteLine($"{min} {max}");

			timer.Stop();
			Console.WriteLine((double)timer.ElapsedMilliseconds / 1000 + "s");
			rdr.Close();
		}
	}
}