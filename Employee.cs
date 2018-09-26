using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Task
{
    public class Employee
    {
        public string RegionID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Employee(string regionId, string name, string surname)
        {
            RegionID = regionId;
            Name = name;
            Surname = surname;
        }
    }
}
