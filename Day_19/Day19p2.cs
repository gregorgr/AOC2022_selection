using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day_19
{
    internal class Day19p2
    {





        public long Run(string bluePrintFileName, int maxTime, int linesFromBluePrint)
        {


            List<string> blueprintLines = File.ReadAllLines($"../../../{bluePrintFileName}").ToList(); // Read the

            long total = 1;
            int i = 1;
            foreach (var line in blueprintLines.Take(linesFromBluePrint))
            {
                var bp = new List<List<(int amount, int type)>>();
                var maxSpend = new int[3]; // For ore, clay, and obsidian
               
                foreach (var section in line.Split(": ")[1].Split(". "))
                {
                    var recipe = new List<(int amount, int type)>();
                    var matches = Regex.Matches(section, @"(\d+) (\w+)");

                    foreach (Match match in matches)
                    {
                        int x = int.Parse(match.Groups[1].Value);
                        int y = new[] { "ore", "clay", "obsidian" }.ToList().IndexOf(match.Groups[2].Value);
                        recipe.Add((x, y));
                        if (y >= 0)
                        {
                            maxSpend[y] = Math.Max(maxSpend[y], x);
                        }
                    }

                    bp.Add(recipe);
                }

                var cache = new Dictionary<string, int>();
                int result = Dfs(bp, maxSpend, cache, maxTime, new[] { 1, 0, 0, 0 }, new[] { 0, 0, 0, 0 });
                Console.WriteLine($"Blueprint {i} can make {result} geodes \n");
                total *= result;
                i++;
            }


            return total;
        }

        private int Dfs(List<List<(int amount, int type)>> bp, int[] maxSpend, Dictionary<string, int> cache, int time, int[] bots, int[] amt)
        {
            if (time == 0)
            {
                return amt[3];
            }

            // Cache key: concatenate time, bots, and amt
            string key = $"{time},{string.Join(",", bots)},{string.Join(",", amt)}";
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }

            int maxVal = amt[3] + bots[3] * time;

            for (int btype = 0; btype < bp.Count; btype++)
            {
                if (btype != 3 && bots[btype] >= maxSpend[btype])
                {
                    continue;
                }

                int wait = 0;
                foreach (var (ramt, rtype) in bp[btype])
                {
                    if (bots[rtype] == 0)
                    {
                        wait = -1;
                        break;
                    }
                    wait = Math.Max(wait, (int)Math.Ceiling((double)(ramt - amt[rtype]) / bots[rtype]));
                }

                if (wait == -1) continue; // Skip if unable to wait

                int remainingTime = time - wait - 1;
                if (remainingTime <= 0) continue;

                int[] newBots = (int[])bots.Clone();
                int[] newAmt = amt.Select((x, i) => x + bots[i] * (wait + 1)).ToArray();

                foreach (var (ramt, rtype) in bp[btype])
                {
                    newAmt[rtype] -= ramt;
                }

                newBots[btype]++;

                for (int i = 0; i < 3; i++)
                {
                    newAmt[i] = Math.Min(newAmt[i], maxSpend[i] * remainingTime);
                }

                maxVal = Math.Max(maxVal, Dfs(bp, maxSpend, cache, remainingTime, newBots, newAmt));
            }

            cache[key] = maxVal;
            return maxVal;
        }
    }


}
