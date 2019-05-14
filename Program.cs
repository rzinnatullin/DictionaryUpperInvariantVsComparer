using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DictionaryUpperInvariantVsComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 120;

            const int keyNumber = 10000;
            const int keyLength = 10;
            const int repeats = 1000;
            const int probes = 10000;
            var keys = new List<string>();
            var random = new Random();
            AddRandomKeys(keys, keyNumber, keyLength, random);
            var keysToProbe = new List<string>();
            keysToProbe.AddRange(keys);
            AddRandomKeys(keysToProbe, keyNumber, keyLength, random);

            var sw = new Stopwatch();
            var matches = 0;
            var unmatches = 0;
            sw.Start();
            for (var i = 0; i < repeats; i++)
            {
                var dictionary1 = keys.ToDictionary(x => x.ToUpperInvariant(), x => x);
                for (var p = 0; p < probes; p++)
                {
                    if (dictionary1.ContainsKey(keysToProbe[random.Next(keysToProbe.Count)].ToUpperInvariant()))
                    {
                        matches++;
                    }
                    else
                    {
                        unmatches++;
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("ToUpperInvariant, matches {0}, unmatches {1}, time {2}", matches, unmatches, sw.Elapsed);

            matches = 0;
            unmatches = 0;
            sw.Reset();
            sw.Start();
            for (var i = 0; i < repeats; i++)
            {
                var dictionary1 = keys.ToDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);
                for (var p = 0; p < probes; p++)
                {
                    if (dictionary1.ContainsKey(keysToProbe[random.Next(keysToProbe.Count)]))
                    {
                        matches++;
                    }
                    else
                    {
                        unmatches++;
                    }
                }
            }
            sw.Stop();
            Console.WriteLine("InvariantCultureIgnoreCase comparer, matches {0}, unmatches {1}, time {2}", matches, unmatches, sw.Elapsed);
            Console.ReadLine();
        }

        private static void AddRandomKeys(List<string> target, int keyNumber, int keyLength, Random random)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < keyNumber; i++)
            {
                var keyBuilder = new StringBuilder();
                for (var n = 0; n < keyLength; n++)
                {
                    keyBuilder.Append(chars[random.Next(chars.Length)]);
                }
                target.Add(keyBuilder.ToString());
            }
        }
    }
}
