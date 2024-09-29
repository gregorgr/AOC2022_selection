namespace Day_19
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;




    internal class Program
    {
        private static  bool DebugMe = true;

        private static int maxTime = 0;
        private static uint currentGeodeMax = 0;
        private static int currentMaxTime = 0;
        private static int[] brakPointAt = { 10, 11,18 }; 
        
        /*
        Blueprint 1:
        Each ore robot costs 4 ore.
        Each clay robot costs 2 ore.
        Each obsidian robot costs 3 ore and 14 clay.
        Each geode robot costs 2 ore and 7 obsidian
         * */
        static void Main(string[] args)
        {
            Console.WriteLine("This is not working as it should. Recursion works for first half of demo but constraints are not working for second blueprint.");
            Console.WriteLine("AOC 2022 Day 19: Not Enough Minerals---");
            string blueprint = "        Blueprint 1:\r\n        Each ore robot costs 4 ore.\r\n        Each clay robot costs 2 ore.\r\n        Each obsidian robot costs 3 ore and 14 clay.\r\n        Each geode robot costs 2 ore and 7 obsidian";

            string blueprintA = "blueprint19-demo.txt";
            string blueprintB = "blueprint19.txt";
            string blueprintE = "blueprint19ERR.txt"; // only record wit a wrong result
            maxTime = 24;
            DebugMe = false;
            uint total = getTotalOfAllBluepints(blueprintE);
            
            Console.WriteLine($"Part one!\nResult: {total} (should be 33 or 87 or 1599) diff={total-1599}");   //   22
            Console.WriteLine("");

        }

        static uint getTotalOfAllBluepints(string blueprintFile) {


            string[] blueprintLines = System.IO.File.ReadAllLines($"../../../{blueprintFile}");
            uint total = 0;
            int i = 1;
            string result = "==============================\n";
            // Parsing example, adjust based on your AoC Day 19 task
            foreach (var line in blueprintLines)
            {
                Console.WriteLine($"blueprint: {i}");

                // get blueprint
                List <RobotWorkshop> ws = ParseBlueprintLine(line);

                // optimize maxcount
                uint[] res = { 0, 0, 0, 0 };
                GetOptimizedRobotMaxCount(ws);
                currentGeodeMax = 0;
                uint maxCount = ProcessBlueprintLevel(ws, res, 0);
                result += $"Blueprint {i} has {maxCount} geodes and quality level of {maxCount * i}\n";
                Console.WriteLine($"\n*******************************\nBlueprint {i} can make {maxCount} geodes and quality level of {maxCount * i}\n\n");
                total += maxCount * (uint)i;
                i++;
            }
            result += $"--------------------------------------------\nTotal={total} should be 33 or 1599";
            Console.WriteLine(result); 

            return total;
           
        }


        public static void GetOptimizedRobotMaxCount(List<RobotWorkshop> rws)
        {

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                // int count = resources[(int)resource];
                //  {resources[(int)resource]},
                // Console.Write($"{resource}: ");
                int i = 0;
                int indexOfResourceRobot = 0;
                int price = 0;
                foreach (RobotWorkshop rw in rws)
                {
                    if (rw.Product == resource.ToString())
                    {
                        indexOfResourceRobot = i;
                    }
                    price = Math.Max(price, rw.GetPriceFor(resource.ToString()));
                    i++;
                }
                price =(int)( 1.1 * price);
                rws[indexOfResourceRobot].MaxCount = price;
            }
        }



        // static int StartMaxHarvesting(int currentBlueprintNo, List<RobotWorkshop> robotWorkshops, HashSet<(string, int)> resources, int time, int maxTime)
        // HarvestBlueprintMax

        static uint ProcessBlueprintLevel(List<RobotWorkshop> rwsl,
            uint[] resources, int time)
        {

            if (DebugMe)
            {
                Console.Write($"\n== Minute {time + 1} ==\n");
            }


         
            int robotID = rwsl.Count - 1; // is a geode robotID
            // always check if we can make geode robot?
            bool botMade = false;
            
            if (rwsl[robotID].CheckResources(resources) && (time )<maxTime)
            {
                bool tmp = DebugMe;
                DebugMe = true;
               // DebugWorkshopsLine(rwsl, resources, $"Before GEODEBOT creation: t={(time+1)}:{maxTime-time-1} ");
                resources = rwsl[robotID].TryToCreateRobot(resources, time);
                botMade = true;
               // DebugWorkshopsLine(rwsl, resources, $"After GEODEBOT creation: t={(time + 1)}:");
                DebugMe = tmp;

            }
            
            uint currentMax = ProcessBlueprintLeaf( rwsl,  resources, time, botMade);

            return currentMax;
        }


        static uint ProcesBlueprinetLeafOption(int createRobotID,
            List<RobotWorkshop> rwsl, uint[] resources, int time)
        {
            // have to clone all data in this option
            List<RobotWorkshop> cloneRwsl = CloneRobotWorkshopsList(rwsl);
            uint[] cloneRes = CloneResources(resources);

            DebugWorkshops(cloneRwsl, cloneRes, $"RobotWorkshop before creating  {rwsl[createRobotID].Name} time: {(time + 1)}");
            cloneRes = cloneRwsl[createRobotID].TryToCreateRobot(cloneRes, time );
            DebugWorkshops(cloneRwsl, cloneRes, $"RobotWorkshop after creating  {rwsl[createRobotID].Name} time: {(time + 1)}");

            return HarvestLeaf(cloneRwsl, cloneRes,time);
        }

        static uint ProcessBlueprintLeaf( 
            List<RobotWorkshop> rwsl,
            uint[] resources, int time, bool botMade)
        {
            int Max = 0;


            // ReasonableChoices
            int robotID = rwsl.Count-2;
            uint currentMax = 0;

            // Option 1: Create a bot
            // are we in time to make another bot?
            //(time + 3) < maxTime to decide whether to build a bot
            if (time > 2 && (time + 1) < maxTime && !botMade) {
                // can we create a bot


                while (robotID >= 0)
                {
                    
                    string product = rwsl[robotID].Product;

                    bool canCreateBot = rwsl[robotID].CheckResources(resources);
                    if (time == 4)
                    {
                      //  Console.WriteLine($"at 3 robotID={robotID} {product} Bot canCreateBot={canCreateBot}");
                    }

                    if (canCreateBot)
                    {
                        uint currentOptionMax = ProcesBlueprinetLeafOption(robotID, rwsl, resources, time);
                        currentMax = Math.Max(currentMax, currentOptionMax);
                    }
                    

                    if (time == 4)
                    {
                      //  Console.WriteLine($"After  3 robotID={robotID} {product} Bot canCreateBot= {canCreateBot}");
                    }
                    robotID--;
                }
            }

            // Option 2: Don't create a bot
            //List<RobotWorkshop> cloneRwsl = CloneRobotWorkshopsList(rwsl);
            //int[] cloneRes = CloneResources(resources);

            uint dontCreateMax = HarvestLeaf(rwsl, resources, time);

            return Math.Max(dontCreateMax, currentMax);
       
            // return currentMax;
        }

        static uint HarvestLeaf(List<RobotWorkshop> rwsl,
                                uint[] resources, int time)
        {

            

            // harvest
            resources = HarvestProducts(rwsl, resources);
            /// stat harvesting with new robots
            CheckRobotStatuses(rwsl);

            uint newCurrentMax = resources[3];
            time++;        

            
            if (time < maxTime)
            {

                //TODO: some optimisation if leaf has bad perspective
                // branch has pottential
                if (rwsl[rwsl.Count - 1].RobotCount() >= 1) {

                    int currentPotetnial = rwsl[rwsl.Count - 1].RobotCount() * (maxTime - time);
                }
                    


                return ProcessBlueprintLevel(rwsl, resources, time);

            }

            
            if (newCurrentMax > 0) {
                if (newCurrentMax > currentGeodeMax)
                {
                    bool tmp = DebugMe;
                    DebugMe = true;
                    DebugWorkshopsLine(rwsl, resources, $"New MAX at t={(time )}, geode={newCurrentMax};");

                    DebugMe = tmp;
                    currentGeodeMax = Math.Max(currentGeodeMax,newCurrentMax);

                }
                else if (newCurrentMax == 0) {
                    bool tempdebug = DebugMe;

                }

            }

            return newCurrentMax;
        }


        public static uint[] CloneResources(uint[] resources)
        {
            return (uint[])resources.Clone();
        }

 
        public static List<RobotWorkshop> CloneRobotWorkshopsList(List<RobotWorkshop> originalList)
        {

            // Deep clone the list of RobotWorkshop
            List<RobotWorkshop> deepClonedList = new List<RobotWorkshop>();
            foreach (var workshop in originalList)
            {
                deepClonedList.Add(workshop.Clone());
            }
            return deepClonedList;
        }

        static void DebugWorkshopsLine(List<RobotWorkshop> workshop, uint[] resources, string message)
        {

            if (DebugMe)
            {
                string output = "";

                foreach (RobotWorkshop w in workshop)
                {
                    if (output.Length > 0) {
                        output += ", ";

                    }
                    output += $"{w.Product}Bots: {w.RobotCount()}";
                }
                int i = 0;
                output += " Resources=[";
                foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
                {
                    // int count = resources[(int)resource];
                    if (i > 0)
                    {
                        output += ", ";
                    }
                    output += $"{resources[(int)resource]}";
                    i++;
                }
                output += "]";

                Console.WriteLine($"{message} {output}");

            }
        }
        static void DebugWorkshops(List<RobotWorkshop> workshop, uint[] resources, string message)
        {
            
            if (DebugMe) {
                
                Console.WriteLine(message);
                foreach (RobotWorkshop w in workshop)
                {
                    bool tempDebug = w.DebugMe;
                    w.DebugMe = DebugMe;
                   // string s = resources.ContainsKey(w.Product) ? $" {w.Product}: {resources[w.Product]}" : "";
                    w.DebugPrint(".");
                    w.DebugMe = tempDebug;
                }
                int index = 0;
                string str = "";
                foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
                {
                    // int count = resources[(int)resource];
                    if (str.Length > 0)
                    {
                        str += ", ";
                    }
                    str += $" {resource}: {resources[(int)resource]} ";
                    index++;
                }
                Console.WriteLine($"Resources:[{str}]");
             }
        }

        static void DebugResources(uint[] resources, string message)
        {
            if (DebugMe)
            {
                Console.Write($"Resources: {message}: ");
                foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
                 {
                    uint count = resources[(int)resource];
                    Console.Write($"{resource}: {resources[(int)resource]}, ");
                }
                Console.WriteLine();
            }
        }

        static void DebugWorkshops (List<RobotWorkshop> workshop, string message)
        {
            if (DebugMe)
            {
                Console.WriteLine(message);
                foreach (RobotWorkshop w in workshop)
                {
                    w.DebugPrint();
                }

            }
   
        }

   
        static void CreateRobots(int currentBlueprintNo, List<RobotWorkshop> robotWorkshops, uint[] resources, int time)
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
                    robotWorkshops[k].TryToCreateRobot(resources, time);
                }
            }
           // return nextRobot;
        }

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

        static uint[] HarvestProducts(List<RobotWorkshop> robotWorkshops, 
            uint[] resources ) {

                 

                for (int k = 0; k < robotWorkshops.Count ; k++)
                {
                    string product = robotWorkshops[k].Product;
                    if (robotWorkshops[k].RobotCount() > 0) {

                        string name = robotWorkshops[k].Name;
                        
                        // create

                        uint newItems = robotWorkshops[k].Harvest();

                        if (newItems > 0)
                        {
                            resources = AddOrUpdateResource(resources, product, newItems);
                        }     
                }

                if (DebugMe)
                {
                    uint total = GetResourceCount(resources, product);
                    if (total > 0)
                    {
                        Console.Write($" you now have {total} {product}.\n");
                    }
                }
           }
            return resources;
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
        static uint GetResourceCount(uint[] resources, string resourceName)
        {
            uint index = GetResourceIndex(resourceName);
            return resources[index];
            //return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
       
        }


        // Method to add or update resource count in a HashSet
        static uint[] AddOrUpdateResource(uint[] resources, string resourceName, uint count)
        {
            uint index = GetResourceIndex(resourceName);
            resources[index] += count;
            return resources;
        }

        // Method to get the resource index based on its name
        static uint GetResourceIndex(string resourceName)
        {
            switch (resourceName.ToLower())
            {
                case "ore": case "Ore": return 0;
                case "clay": case "Clay": return 1;
                case "obsidian": case "Obsidian": return 2;
                case "geode": case "Geode": return 3;
                default: throw new ArgumentException("Unknown resource name");
            }
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



    }
}