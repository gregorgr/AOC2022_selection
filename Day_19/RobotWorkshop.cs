using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Day_19
{
    internal class RobotWorkshop
    {
        public string Name { get; set; }
        public string Product { get; set; }
  
        private int maxCount { get; set; }

        public bool DebugMe { get; set; }

        private List<Robot> Robots = new List<Robot>();

        public HashSet<(string, int)> RobotPrice = new HashSet<(string, int)>();



        public RobotWorkshop(string name, string product)
        {

            Name = name == "geode" ? name + "-cracking" : name + "-collecting";
            Product = product;
            //Items = 0;
            DebugMe = false;

            this.maxCount = name == "clay" ? 4 : 100;
       
        }

        public RobotWorkshop(string name, string product, int maxCount)
        {
            Name = name == "geode" ? name + "-cracking" : name + "-collecting";
            Product = product;
           // Items = 0;
            DebugMe = false;
            this.maxCount = maxCount;
 
            if (maxCount == 1)
            {
                Robot r = new Robot(Product, DebugMe);
                r.IsBuilding = false;
                Robots.Add(r);
  
            }
        }
        // Assuming Robot class is cloneable
        // RobotWorkshop(this.Name, this.Product, this.Items, this.maxCount, clonedRobots, DebugMe);
        //new RobotWorkshop(this.Name, this.Product, this.Items, this.maxCount, DebugMe, clonedRobots );
        public RobotWorkshop(string name, string product, int maxCount,   bool debugme, List<Robot> robots, HashSet<(string, int)> robotPrice)
        {
            Name = name;
            Product = product;
            Robots = robots;
            // Items = items;
            this.maxCount = maxCount;
            DebugMe = debugme;
            RobotPrice = robotPrice;

        }

        public void AddPrice(string name, int cost)
        {
            RobotPrice.Add((name, cost));
        }



        public bool TryToCreateRobot(Dictionary<string, int> resources) {
            
            int i = 0;
            foreach (var price in RobotPrice)
            {
                string resourceToGet = price.Item1;
                int robotPrice = price.Item2;

                UseResources(resources, resourceToGet, robotPrice);
                if (DebugMe)
                {
                    string text = i==0 ? "Spend":" and";
                    Console.Write($"{text} {robotPrice} {resourceToGet}");
                }

                i++;
            }

            Robot r = new Robot(Product, DebugMe);
            //r.IsBuilding = ;
            if (DebugMe)
            {
                Console.Write($" to start building an {r.Name} robot.\n");
            }
            Robots.Add(r);

            return false;       
        }

        private void UseResources(Dictionary<string, int> resources, string resourceName, int price) {

            //var existingResource = (resourceName, 0);
            bool resourceExists = false;

            if (resources.ContainsKey(resourceName) && resources[resourceName] >= price)
            {
                resources[resourceName] = resources[resourceName]- price;
            }
            else {
                Console.WriteLine($"ERROR: RobotWorkshop({Name})->[UseResources] Resource {resourceName} not found.");
            }

        }

        public bool CanCreateRobot(Dictionary<string, int> resources) {
            string currentRobot = Name;
            bool canCreate =false; // for not able
            if (Robots.Count < this.maxCount)
            {
                foreach (var price in RobotPrice)
                {

                    string resourceToGet = price.Item1;
                    int robotPrice = price.Item2;

                    int count = resources.ContainsKey(resourceToGet) ? resources[resourceToGet] : 0;
       
                    canCreate = count >= robotPrice;
                    if (!canCreate) {
                        break;
                    }

                }
            }
            return canCreate;
        }
        public bool CheckResources(Dictionary<string, int> resources) {
            // this code is for linear programing but we dont need it
            string currentRobot = Name;
            bool canCreate = false; // for not able

            if (Robots.Count < this.maxCount) {

                foreach (var price in RobotPrice) {

                    string resourceToGet = price.Item1;
                    int robotPrice = price.Item2;

                    // int count = HasResource(resources, resourceToGet);
                    int count = resources.ContainsKey(resourceToGet) ? resources[resourceToGet] : 0;
                    if (count >= robotPrice)
                    {
                        canCreate = true;
                        //Console.WriteLine($"a) Robot {Name}: {price.Item1}, Price: {price.Item2}");
                    }
                    else
                    {
                        return false;
                    }        
                }
            }
            return canCreate;
        }

        public int RobotCount()
        {
            return Robots.Count;
        }
        
        /*
        public int HasResource(Dictionary<string, int> resources, string resourceName)
        {
            

            foreach (var resource in resources)
            {
                if (resource.Item1 == resourceName)
                {
                    return resource.Item2;
                   // existingResource = resource;
                   // resourceExists = true;
                    break;
                }
            }
            return 0;
        }
        
        */

        public int Harvest() {
            int count = 0;

            int items = 0;
            foreach (Robot robot in Robots)
            {
                if(!robot.IsBuilding)
                {
                    count++;
                }
                items+=robot.Harvest();
            }
           // Items += items;
            if (DebugMe && count>0)
            {
                Console.Write($"{count} {Name} robots collects {items} {Product}; ");
            }
            return count;
        }

        public void GetStatus()
        {
            bool isBuilding=false;
            foreach (Robot robot in Robots)
            {
                if (robot.IsBuilding) { isBuilding = true; }
                robot.GetStatus();
            }
            int count = Robots.Count;
            if (DebugMe && count >0 && isBuilding && Product !="ore")
            {
                
                Console.Write($"you now have {count} of {Name}.\n");
            }
        }

        // Deep clone for RobotWorkshop
        public RobotWorkshop Clone()
        {
            // Deep clone the list of robots
            List<Robot> clonedRobots = new List<Robot>();

            foreach (var robot in Robots)
            {
                clonedRobots.Add(robot.Clone()); // Assuming Robot class has a Clone() method

            }

           
            HashSet<(string, int)> p = new HashSet<(string, int)>();
            foreach (var price in RobotPrice)
            {
                p.Add((price.Item1, price.Item2)); // Add a new resource
            }

            return new RobotWorkshop(this.Name, this.Product, this.maxCount,  DebugMe, clonedRobots, p );
        }
        public void DebugPrint(string message = "")
        {
            if (DebugMe)
            {

                Console.WriteLine($"Workshop: {Name}, Robots: {RobotCount()}: {message}");
            }
        }
    }
}
