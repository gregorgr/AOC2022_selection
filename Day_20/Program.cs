using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AOC 2022 Day 20: Grove Positioning System");

            string puzzleA = "input_day20demo.txt";
            string puzzleB = "input_day20.txt";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // day 20 part1
            int rezult= part1(puzzleB);

            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;

            Console.WriteLine($"\nPart one!\nResult: {rezult} (3 for demo and 14526 for puzzle) in {elapsed.Seconds} seconds and {elapsed.Milliseconds} milliseconds");

            stopwatch.Restart();

            // day 20 part1
            const long decryptionKey = 811589153;
            long rezult2 = part2(puzzleB, decryptionKey);

            stopwatch.Stop();
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"\nPart two!\nResult: {rezult2} (3 for demo and 14526 for puzzle) in {elapsed.Seconds} seconds and {elapsed.Milliseconds} milliseconds");



        }


        static long part2(string file,  long decryptionKey, int mixRounds = 10)
        {
            // O(n)
            var numbers = File.ReadAllLines($"../../../{file}")
                .Select(int.Parse)
                .Select(x => x * decryptionKey)
                .ToList();

            var originalIndices = Enumerable.Range(0, numbers.Count).ToList();

            // O(n^2)
            for (int round = 0; round < mixRounds; round++)
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                     int currentIndex = originalIndices.IndexOf(i);
                     originalIndices.RemoveAt(currentIndex);

                    // calcualte new circular index around list
                     long newIndex = (currentIndex + numbers[i]) % originalIndices.Count;
                     if (newIndex < 0) newIndex += originalIndices.Count;

                     // insert number into new list
                     originalIndices.Insert((int)newIndex, i);

                }
            }

            // O(1)
            // find index of number 0 (zero) to find startin coordiantes
            int zeroIndex = originalIndices.IndexOf(numbers.IndexOf(0));

            long result = numbers[originalIndices[(zeroIndex + 1000) % numbers.Count]];
            result += numbers[originalIndices[(zeroIndex + 2000) % numbers.Count]];
            result += numbers[originalIndices[(zeroIndex + 3000) % numbers.Count]];

            return result;
        }

        static int part1(string file)
        {
            // O(n)
            var numbers = File.ReadAllLines($"../../../{file}").Select(int.Parse).ToList();

            var originalIndices = Enumerable.Range(0, numbers.Count).ToList();

            // O(n^2)
            for (int i = 0; i < numbers.Count; i++)
            {
                int currentIndex = originalIndices.IndexOf(i);
                originalIndices.RemoveAt(currentIndex);

                // calcualte new circular index around list
                int newIndex = (currentIndex + numbers[i]) % originalIndices.Count;
                if (newIndex < 0) newIndex += originalIndices.Count;

                // insert number into new list
                originalIndices.Insert(newIndex, i);
            }
            // O(1)
            // find index of number 0 (zero) to find startin coordiantes
            int zeroIndex = originalIndices.IndexOf(numbers.IndexOf(0));

            int result = numbers[originalIndices[(zeroIndex + 1000) % numbers.Count]];
            result += numbers[originalIndices[(zeroIndex + 2000) % numbers.Count]];
            result += numbers[originalIndices[(zeroIndex + 3000) % numbers.Count]];

            return result;
        }
    }


}