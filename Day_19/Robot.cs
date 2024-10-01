using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19
{
    internal class Robot
    {
        public string Name { get; set; }
        //public int Count { get; set; }

        private bool DebugMe = true;
        public bool IsBuilding { get; set; }

        public Robot (string name, bool debug)
        {
            DebugMe = debug;
            Name = name;
            // robot is in the process of being build
            IsBuilding = true;
        }

        public Robot(string name, bool isBulding, bool debug)
        {
            DebugMe = debug;
            Name = name;
            IsBuilding = isBulding;
            // robot is in the process of being build
     
        }

        public int Harvest()
        {
            if (IsBuilding)
            {
                return 0;
            }
            return 1; 
        }

        public void GetStatus() {

            
            if (DebugMe && IsBuilding)
            {
                Console.Write($"The new {Name} robot is ready; ");
            }
            if (IsBuilding)
            {
                IsBuilding = false;
            }
        }

        public Robot Clone()
        {
            return new Robot(this.Name, this.IsBuilding, this.DebugMe);
        }

    }
}
