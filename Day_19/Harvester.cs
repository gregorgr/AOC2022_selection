using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Day_19
{
    internal class Harvester
    {
        List<RobotWorkshop> RwList {  get; set; }
        public int[] Resources {  get; set; }
        int Time {  get; set; }

        public Harvester(List<RobotWorkshop> rwList1, 
            int[] resources1,
            int time) {

            //RwList = CloneRobotWorkshopsList(rwList1);
           // Resources = CloneResources(resources1);
            Time = time; 
        }


    }


}
