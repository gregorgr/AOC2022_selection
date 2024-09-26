namespace Day_19
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;




    internal class Programx
    {
        private static  bool DebugMe = true;

        private static int maxTime = 0;
        private static int currentMax = 0;
        private static int currentMaxTime = 0;
        private static int[] brakPointAt = { 10, 11,18 }; 
        
        /*
        Blueprint 1:
        Each ore robot costs 4 ore.
        Each clay robot costs 2 ore.
        Each obsidian robot costs 3 ore and 14 clay.
        Each geode robot costs 2 ore and 7 obsidian
         * */
        static void Mainx(string[] args)
        {
            Console.WriteLine("This is not working as it should. Recursion works for first half of demo but constraints are not working for second blueprint.");
            Console.WriteLine("AOC 2022 Day 19: Not Enough Minerals---");
            string blueprint = "        Blueprint 1:\r\n        Each ore robot costs 4 ore.\r\n        Each clay robot costs 2 ore.\r\n        Each obsidian robot costs 3 ore and 14 clay.\r\n        Each geode robot costs 2 ore and 7 obsidian";

            string blueprintA = "blueprint19-demo.txt";
            maxTime = 24;
            DebugMe = true;
           // int total = getTotalOfAllBluepints(blueprintA);
           // Console.WriteLine($"Part one!\nDEMO Result: {total} (should be 33)");   //   22

        }
        /*
        static int getTotalOfAllBluepints(string blueprintFile) {


            string[] blueprintLines = System.IO.File.ReadAllLines($"../../../{blueprintFile}");
            int total = 0;
            int i = 1;
            string result = "==============================\n";
            // Parsing example, adjust based on your AoC Day 19 task
            foreach (var line in blueprintLines)
            {
                Console.WriteLine($"blueprint: {i}");

                // get blueprint
                List <RobotWorkshop> ws = ParseBlueprintLine(line);

                // optimize maxcount

                

                //  int currentBlueprintNo,

                Dictionary<string, int> res = new Dictionary<string, int>();


                AddOrUpdateResource(res, "ore", 0);
                AddOrUpdateResource(res, "clay", 0);
                AddOrUpdateResource(res, "obsidian", 0);
                AddOrUpdateResource(res, "geode", 0);
               // int[4] res= {0, 0, 0, 0 };
                OptimizeMaxCount(res, ws);

                int maxCount = HarvestBlueprintMax(ws, res, 0);
                result += $"\n*******************************\nBlueprint {i} has {maxCount} geodes and quality level of {maxCount * i}\n";
                Console.WriteLine($"\n*******************************\nBlueprint {i} has {maxCount} geodes and quality level of {maxCount * i}\n\n");
                total += maxCount * i;
                i++;
            }
            Console.WriteLine(result); 

            return total;
           
        }
*/
    /*
        public static void OptimizeMaxCount(Dictionary<string, int> r, List<RobotWorkshop> rws)
        {

            foreach (var x in r)
            {

                int price = 0;
                string name = x.Key.ToString();
                int i = 0;
                int index = 0;
                foreach (RobotWorkshop rw in rws)
                {

                    if (rw.Product == name)
                    {
                        index = i;
                    }
                    price = Math.Max(price, rw.GetPriceFor(name));


                    i++;

                }
                rws[index].MaxCount = price;
            }
        }
  */


       // static int StartMaxHarvesting(int currentBlueprintNo, List<RobotWorkshop> robotWorkshops, HashSet<(string, int)> resources, int time, int maxTime)
  /*
        static int HarvestBlueprintMax(List<RobotWorkshop> rwsl,
            int[] resources, int time)
        {

            if (DebugMe)
            {
                Console.Write($"\n== Minute {time + 1} ==\n");
            }


         
            int robotID = rwsl.Count - 1; // is a geode robotID
            // always check if we can make geode robot?
            if (rwsl[robotID].CheckResources(resources) && (time+1)<maxTime)
            {
                DebugWorkshops(rwsl, resources, $"3 Resources before robot creation: Time:{(time+1)}:");

                rwsl[robotID].TryToCreateRobot(resources);
                DebugWorkshops(rwsl, resources, $"3 Resources after robot creation Time:{(time+1)}:");

                if (brakPointAt.Contains(time + 1)) {
                    Console.Write($"\n...Break minute {time + 1} ==\n");
                }
                
            }

            int currentMax = ProcessBlueprintLeaf(robotID-1, rwsl,  resources, time);

            return currentMax;
        }
  */
  
        /*
        static int ProcessBlueprintLeaf(int robotID, List<RobotWorkshop> rwsl,
            Dictionary<string, int> resources, int time)
        {
            int currentMax = 0;
            bool canCreate1=false;

            // we still have a robot to process and we dont produce robots onm 22nd sec
            if (robotID >= 0 && time > 1 && (time + 2) < maxTime) {

                // can create a robot
                bool canCreate = rwsl[robotID].CheckResources(resources);

                if (canCreate)
                {
                    int notCreateMax = 0;

                    // Option 1: Don't create robot
                    List<RobotWorkshop> cloneRwsl = CloneRobotWorkshopsList(rwsl);
                    int[] cloneRes = CloneResources(resources);
                    DebugWorkshops(cloneRwsl, cloneRes, $"RobotWorkshop after cloning  {rwsl[robotID].Name} time:{(time + 1)}:");

                    Console.Write("-------------------\n");
                    notCreateMax = ProcessBlueprintLeaf(robotID - 1, cloneRwsl, cloneRes, time);


                    if (brakPointAt.Contains(time + 1))
                    {
                        //Console.Write($"\n== Back from not create {rwsl[robotID].Name} at minute {time + 1} ==\n");
                        DebugWorkshops(cloneRwsl, cloneRes, $"After leaf  {rwsl[robotID].Name} cloning Time:{(time + 1)}:");
                    }

                    // Option 2: Create robot
                    DebugWorkshops(rwsl, resources, $"RobotWorkshop before creating  {rwsl[robotID].Name} time: {(time + 1)}");
                    rwsl[robotID].TryToCreateRobot(resources);
                    DebugWorkshops(rwsl, resources, $"RobotWorkshop after creating  {rwsl[robotID].Name} time: {(time + 1)}\"");

                    currentMax = ProcessBlueprintLeaf(robotID - 1, rwsl, resources, time);

                    // compare results
                    // Return the best outcome between both options
                    // return currentMax > notCreateMax ? currentMax : notCreateMax;
                    return Math.Max(currentMax, notCreateMax);

                }
                else { 
                
                
                }
            }

      



                if (canCreate1)
                {
                    
                    int notCreateMax = 0;

                    // Option 1: Don't create robot
                    if ( rwsl[robotID].RobotCount()>1) { // zakaj to?
                        

                        
                       //  List<RobotWorkshop> cloneRwsl = CloneRobotWorkshopsList(rwsl);
                       //  Dictionary<string, int> cloneRes = CloneResources(resources);
                       //  DebugWorkshops(cloneRwsl,cloneRes, $"RobotWorkshop after cloning  {rwsl[robotID].Name} time:{(time + 1)}:");
     
                       // Console.Write("-------------------\n");
                       // notCreateMax = ProcessBlueprintLeaf(robotID - 1, cloneRwsl, cloneRes, time);

                       // if (brakPointAt.Contains(time + 1))
                       // {
                       //     //Console.Write($"\n== Back from not create {rwsl[robotID].Name} at minute {time + 1} ==\n");
                       //     DebugWorkshops(cloneRwsl, cloneRes, $"After leaf  {rwsl[robotID].Name} cloning Time:{(time + 1)}:");
                       // }
                    }

                    if (DebugMe)
                    {
                        Console.Write($"__ minute {time+1} Option 2: Create robot {rwsl[robotID].Name}==\n");
                    }

                    // Option 2: Create robot
                    DebugWorkshops(rwsl, resources, $"RobotWorkshop before creating  {rwsl[robotID].Name} time: {(time+1)}");
                    rwsl[robotID].TryToCreateRobot(resources);
                    DebugWorkshops(rwsl, resources, $"RobotWorkshop after creating  {rwsl[robotID].Name} time: {(time+1)}\"");

                    currentMax = ProcessBlueprintLeaf(robotID-1, rwsl, resources, time);


                    // compare results
                    // Return the best outcome between both options
                    // return currentMax > notCreateMax ? currentMax : notCreateMax;
                    return Math.Max(currentMax, notCreateMax);

                }
                else {
                    // Proceed to next robot if creation not possible
                    currentMax = ProcessBlueprintLeaf(robotID - 1, rwsl, resources, time);
                }

             //   return currentMax;

           // if (robotID >= 0 && time > 1 && (time + 1) < maxTime)
           // {
                // }

                if (DebugMe)
            {
                string s = "";
                DebugWorkshops(rwsl, resources, $"RobotWorkshop before harvest at {time + 1}");
            //    foreach (RobotWorkshop wsl in rwsl) {
            //        s += $"{wsl.Name} -  Robots: {wsl.RobotCount().ToString()}; \n";
            //    }
             //   Console.Write($"... minute {time + 1} {s} Harvest at {time+1} ==\n");
            }
            // harvest
            HarvestProducts(rwsl, resources);
            /// stat harvesting with new robots
            CheckRobotStatuses(rwsl);

            // CheckRobotStatuses(currentBlueprintNo, robotWorkshops, resources);
            if (DebugMe)
            {
                string s = "";
                DebugWorkshops(rwsl, $"RobotWorkshop AFTER harvest at {time + 1}");
                //    foreach (RobotWorkshop wsl in rwsl) {
                //        s += $"{wsl.Name} -  Robots: {wsl.RobotCount().ToString()}; \n";
                //    }
                //   Console.Write($"... minute {time + 1} {s} Harvest at {time+1} ==\n");
            }

            time++;
            if (time < maxTime)
            {
                // we still have time to process
                currentMax = HarvestBlueprintMax(rwsl, resources, time);
            }
            else {
                // the end of the time
                // we count
                 int newCurrentMax = GetResourceCount(resources, "geode");
                Console.Write("NEW MAX:\n");
                DebugWorkshops(rwsl, "RobotWorkshop:");
                DebugResources(resources, "Resources:");
                Console.Write("====================================================================\n");
                return newCurrentMax;
               // currentMax = newCurrentMax>currentMax ? newCurrentMax : currentMax;

            }
 
            return currentMax;
        }
        */


        static Dictionary<string, int> CloneResources1(Dictionary<string, int> resources)
        {
            return new Dictionary<string, int>(resources);
        }
        /*
        static HashSet<(string, int)> CloneResources(HashSet<(string, int)> resources) {
            // Clone the HashSet (shallow clone, which is fine for value types like tuples)
            HashSet<(string, int)> c = new HashSet<(string, int)>(resources);
            foreach (var resource in resources)
            {
                c.Add((resource.Item1, resource.Item2)); // Add a new resource
            }

            return c;
        }*/
        /*
        static List<RobotWorkshop> CloneRobotWorkshopsList(List<RobotWorkshop> originalList)
        {

            // Deep clone the list of RobotWorkshop
            List<RobotWorkshop> deepClonedList = new List<RobotWorkshop>();
            foreach (var workshop in originalList)
            {
                deepClonedList.Add(workshop.Clone());
            }
            return deepClonedList;
        }
        */
        static void DebugWorkshops(List<RobotWorkshop> workshop, Dictionary<string, int> resources, string message)
        {
            Console.WriteLine(message);
            foreach (RobotWorkshop w in workshop)
            {
                string s = resources.ContainsKey(w.Product) ? $" {w.Product}: {resources[w.Product]}" : ""; 
                w.DebugPrint(s);
            }
        }

        static void DebugResources(Dictionary<string, int> resources, string message)
        {
            Console.WriteLine(message);
            foreach (var resource in resources)
            {
                Console.WriteLine($"{resource.Key}: {resource.Value}");
            }
            Console.WriteLine();
        }

        static void DebugWorkshops (List<RobotWorkshop> workshop, string message)
        {
            Console.WriteLine(message);
            foreach (RobotWorkshop w in workshop)
            {
                w.DebugPrint();
            }
        }
        /*
   
        static void CreateRobots(int currentBlueprintNo, List<RobotWorkshop> robotWorkshops, Dictionary<string, int> resources)
        {
            string nextRobot = "";
            for (int k = robotWorkshops.Count - 1; k >= 0; k--)
            {
                bool canCreate = robotWorkshops[k].CheckResources(resources);
                if (!canCreate)
                {
                    // can not ccreate jet! sop skip checking next robot
                    break;

                }
                else {
                    robotWorkshops[k].TryToCreateRobot(resources);
                }
            }
           // return nextRobot;
        }
        */
        static void CheckRobotStatuses(List<RobotWorkshop> robotWorkshops)
        {
            for (int k = 0; k < robotWorkshops.Count; k++)
            {
                robotWorkshops[k].GetStatus();
            }
        }

  

        static int GetRobotOrCreateWorkshopIdByProduct(List<RobotWorkshop> robotWorkshops, string product)
        {

            //  var workshop = robotWorkshops.FirstOrDefault(w => w.Product == product);
            int index = robotWorkshops.FindIndex(w => w.Product == product);
            if (index < 0)
            {
                RobotWorkshop rw = new RobotWorkshop(product, product);
                rw.DebugMe = DebugMe;
                robotWorkshops.Add(rw);
                index = robotWorkshops.Count - 1;
            }
            return index;
        }

        static void HarvestProducts(List<RobotWorkshop> robotWorkshops, Dictionary<string, int> resources ) {

                 

                for (int k = 0; k < robotWorkshops.Count ; k++)
                {

                    string name = robotWorkshops[k].Name;
                    string product = robotWorkshops[k].Product;
                    // create

                    int newItems = robotWorkshops[k].Harvest();

                    AddOrUpdateResource(resources, product, newItems);

                   

                    if (DebugMe)
                    {
                        int total = GetResourceCount(resources, product);
                        if (total > 0)
                        {
                            Console.Write($" you now have {total} {product}.\n");
                        }
                    }
                // work

                    /*
                int canCreate = robotWorkshops[k].CheckResources(resources);
                if (canCreate == 0)
                {
                    // can not ccreate jet! sop skip checking next robot
                    break;

                }
*/


            }
        }

        static void ProcessRobotCost(List<Resource> res, string costDescription)
        {
            // Example of further parsing, you can customize this based on how you want to handle resources
            if (costDescription.Contains("ore robot"))
            {
                var oreCost = Regex.Match(costDescription, @"costs (\d+) ore").Groups[1].Value;
                Console.WriteLine($"Ore Robot costs {oreCost} ore.");
            }
            else if (costDescription.Contains("clay robot"))
            {
                var clayCost = Regex.Match(costDescription, @"costs (\d+) ore").Groups[1].Value;
                Console.WriteLine($"Clay Robot costs {clayCost} ore.");
            }
            else if (costDescription.Contains("obsidian robot"))
            {
                var match = Regex.Match(costDescription, @"costs (\d+) ore and (\d+) clay");
                var oreCost = match.Groups[1].Value;
                var clayCost = match.Groups[2].Value;
                Console.WriteLine($"Obsidian Robot costs {oreCost} ore and {clayCost} clay.");
            }
            else if (costDescription.Contains("geode robot"))
            {
                var match = Regex.Match(costDescription, @"costs (\d+) ore and (\d+) obsidian");
                var oreCost = match.Groups[1].Value;
                var obsidianCost = match.Groups[2].Value;
                Console.WriteLine($"Geode Robot costs {oreCost} ore and {obsidianCost} obsidian.");
            }
        }

        // Method to get the count of a resource from the HashSet
        static int GetResourceCount(Dictionary<string, int> resources, string resourceName)
        {
            return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
            /*
            foreach (var resource in resources)
            {
                if (resource.Item1 == resourceName)
                {
                    return resource.Item2; // Return the count (int) if the resource exists
                }
            }
            return 0; // Return 0 if the resource is not found
            */
        }


        // Method to add or update resource count in a HashSet
        static void AddOrUpdateResource(Dictionary<string, int> resources, string resourceName, int count)
        {
            // Check if the resource exists in the HashSet
            if (resources.ContainsKey(resourceName))
            {
                resources[resourceName] += count;
            }
            else
            {
                resources[resourceName] = count;
            }
            /*
            var existingResource = (resourceName, 0);
            bool resourceExists = false;

            foreach (var resource in resources)
            {
                if (resource.Item1 == resourceName)
                {
                    existingResource = resource;
                    resourceExists = true;
                    break;
                }
            }

            // Remove the existing resource and re-add it with the updated count
            if (resourceExists)
            {
                resources.Remove(existingResource); // Remove the old entry
                resources.Add((resourceName, existingResource.Item2 + count)); // Add with updated count
            }
            else
            {
                resources.Add((resourceName, count)); // Add new resource
            }
            */
        }

        private static List<RobotWorkshop> ParseBlueprintLine(string line)
        {

            List<RobotWorkshop> robotWorkshops = new List<RobotWorkshop>();

            // add initial ore collecting robot
            RobotWorkshop rw = new RobotWorkshop("ore", "ore", 1);
            rw.DebugMe = DebugMe;
            robotWorkshops.Add(rw);

            // List<Resource> resources = new List<Resource>();
            var match = Regex.Match(line, @"Blueprint \d+: (.*)");
            if (match.Success)
            {
                string robotDescriptions = match.Groups[1].Value;

                // Step 2: Split by ". " to get each robot's cost description
                var robotCosts = robotDescriptions.Split(new[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cost in robotCosts)
                {
                    // Print or process each robot's cost
                    if (!DebugMe)
                    {
                        Console.WriteLine($"Robot cost: {cost}");
                    }

                    // Optionally further process each sentence (for example, you could extract specific resources like ore, clay, etc.)
                    ProcessBlueprintItem(robotWorkshops, cost);
                }
            }

            return robotWorkshops;

        }



        static void ProcessBlueprintItem(List<RobotWorkshop> robotWorkshops, string costDescription)
        {
            int index = 0;
            string product = "ore";

            // Example of further parsing, you can customize this based on how you want to handle resources
            if (costDescription.Contains("ore robot"))
            {
                int oreCost = int.Parse(Regex.Match(costDescription, @"costs (\d+) ore").Groups[1].Value);
                index = GetRobotOrCreateWorkshopIdByProduct(robotWorkshops, product);
                robotWorkshops[index].AddPrice("ore", oreCost);
                if (DebugMe)
                {
                    Console.WriteLine($"Ore Robot costs {oreCost} ore.");
                }

            }
            else if (costDescription.Contains("clay robot"))
            {
                product = "clay";
                int clayCost = int.Parse(Regex.Match(costDescription, @"costs (\d+) ore").Groups[1].Value);

                index = GetRobotOrCreateWorkshopIdByProduct(robotWorkshops, product);
                robotWorkshops[index].AddPrice("ore", clayCost);
                if (DebugMe)
                {
                    Console.WriteLine($"Clay Robot costs {clayCost} ore.");
                }
            }
            else if (costDescription.Contains("obsidian robot"))
            {
                product = "obsidian";
                index = GetRobotOrCreateWorkshopIdByProduct(robotWorkshops, product);
                var match = Regex.Match(costDescription, @"costs (\d+) ore and (\d+) clay");
                if (match.Success)
                {
                    int oreCost = int.Parse(match.Groups[1].Value);
                    int clayCost = int.Parse(match.Groups[2].Value);
                    robotWorkshops[index].AddPrice("clay", clayCost);
                    robotWorkshops[index].AddPrice("ore", oreCost);
                    if (DebugMe)
                    {
                        Console.WriteLine($"Obsidian Robot costs {oreCost} ore and {clayCost} clay.");
                    }
                }

            }
            else if (costDescription.Contains("geode robot"))
            {
                product = "geode";
                index = GetRobotOrCreateWorkshopIdByProduct(robotWorkshops, product);
                var match = Regex.Match(costDescription, @"costs (\d+) ore and (\d+) obsidian");
                if (match.Success)
                {
                    var oreCost = int.Parse(match.Groups[1].Value);
                    var obsidianCost = int.Parse(match.Groups[2].Value);

                    robotWorkshops[index].AddPrice("obsidian", obsidianCost);
                    robotWorkshops[index].AddPrice("ore", oreCost);
                    if (DebugMe)
                    {
                        Console.WriteLine($"Geode Robot costs {oreCost} ore and {obsidianCost} obsidian.");
                    }
                }

            }
        }

/*
        public static List<RobotWorkshop> CloneRobotWorkshopsList1(List<RobotWorkshop> originalList)
        {

            // Deep clone the list of RobotWorkshop
            List<RobotWorkshop> deepClonedList = new List<RobotWorkshop>();
            foreach (var workshop in originalList)
            {
                deepClonedList.Add(workshop.Clone());
            }
            return deepClonedList;
        }

*/




    }
}