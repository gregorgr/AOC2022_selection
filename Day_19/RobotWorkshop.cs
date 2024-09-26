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
        

        public int MaxCount { get; set; }

        public int LastRobotCreationTime { get; set; }

        public bool DebugMe { get; set; }

        private List<Robot> Robots = new List<Robot>();

        public HashSet<(string, int)> RobotPrice = new HashSet<(string, int)>();



        public RobotWorkshop(string name, string product)
        {

            Name = name == "geode" ? name + "-cracking" : name + "-collecting";
            Product = product;
            //Items = 0;
            DebugMe = false;

            this.MaxCount = name == "clay" ? 4 : 100;
       
        }

        public RobotWorkshop(string name, string product, int maxCount)
        {
            Name = name == "geode" ? name + "-cracking" : name + "-collecting";
            Product = product;
           // Items = 0;
            DebugMe = false;
            this.MaxCount = maxCount;
 
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
            this.MaxCount = maxCount;
            DebugMe = debugme;
            RobotPrice = robotPrice;

        }

        public void AddPrice(string name, int cost)
        {
            RobotPrice.Add((name, cost));
        }

        public int GetPriceFor(string name) {

            foreach (var price in RobotPrice)
            {
                //string resourceToGet = price.Item1;
                if (price.Item1 == name.ToLower()) {
                    return price.Item2;
                }
               
            }
            return 0;
        
        }




        // public bool TryToCreateRobot(Dictionary<string, int> resources) {
        public bool TryToCreateRobot(int[] resources, int time) {
            
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
            LastRobotCreationTime = time;

            return false;       
        }
        // private void UseResources(Dictionary<string, int> resources, string resourceName, int price) {
        private void UseResources(int[] resources, string resourceName, int price) {

            //var existingResource = (resourceName, 0);
            bool resourceExists = false;
            if (Enum.TryParse(resourceName, true, out ResourceType res)){   
               // Resource, resourceName, out Resource res) {
                int resourceIndex = (int)res;
                if (resources[resourceIndex] >= price)
                {
                    resources[resourceIndex] -=price;
                }
                else {
                    Console.WriteLine($"ERROR not enough resources {resourceName}: have {resources[resourceIndex]}, need {price}");
                }
            }
            else
            {
                Console.WriteLine($"Invalid resource name: {resourceName}.");
            }
        }

        public bool CanCreateRobot(int[] resources) {
 
            return CheckResources(resources);
        }
        public bool CheckResources(int[] resources) {
            // this code is for linear programing but we dont need it
            string currentRobot = Name;
            bool canCreate = false; // for not able



            if (Robots.Count < this.MaxCount || this.MaxCount==0) {

                // check if we need this robot or we have allready enough resources
                if (Enum.TryParse(Product, true, out ResourceType resource1)) {
                    // we do not need resources
                    if (resources[(int)resource1] > MaxCount * 1.2 && MaxCount != 0)
                    {
                        return false;
                    }

                }

                foreach (var price in RobotPrice)
                {
                    string resourceToGet = price.Item1;
                    int robotPrice = price.Item2;

                    if (Enum.TryParse(resourceToGet, true, out ResourceType resource)) {

                        int resourceIndex = (int)resource;
                        // we do not need resources
                        if (resources[(int)resource] > MaxCount * 1.2 && MaxCount != 0) {
                            return false;
                        }
                        // we have enbough resources & resource is needed 
                        canCreate = resources[(int)resource] >= robotPrice ;
                        bool doWeNeedIt = true; // resources[(int)resource] < MaxCount * 1.2 || MaxCount==0;
                        if (!canCreate) { 
                            return false;         
                        }
                    }
                }
            }
            return canCreate;
        }

        public int RobotCount()
        {
            return Robots.Count;
        }
        

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

            return new RobotWorkshop(this.Name, this.Product, this.MaxCount,  DebugMe, clonedRobots, p );
        }
        public void DebugPrint(string message = "")
        {
            if (DebugMe)
            {
                string price = "";
                foreach (var p in  RobotPrice)
                {
                    price += $"{p.Item1}: {p.Item2}; ";

                }
                if (price.Length > 0) {
                    price = $"; Price: {price}";
                }

                Console.WriteLine($"Workshop: {Name}, Robots: {RobotCount()}: {message}{price}");
            }
        }
    }
}
public enum ResourceType {  
    ore = 0, 
    clay = 1, 
    obsidian=2, 
    geode=3 
};
