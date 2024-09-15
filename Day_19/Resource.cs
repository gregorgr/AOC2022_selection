using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19
{
    internal class Resource
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public Resource(string name, int count)
        {
            Name = name;
            Count = count;
        }

        // Method to perform a task
        public void UseResource(int amount)
        {
            if (Count >= amount)
            {
                Count -= amount;
            }
            else
            {
                throw new InvalidOperationException("Not enough resources.");
            }
        }
    }
}
