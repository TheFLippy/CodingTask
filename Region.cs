using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Task
{
    public class Region
    {
        public string RegionID { get; set; }
        public string Name { get; set; }
        public string ParentRegionID { get; set; }
        public bool Visited { get; set; }
        public List<Edge> Edges { get; set; }
        public List<Employee> Employees { get; set; }

        public Region(string regionID, string name, string parentID)
        {
            Visited = false;
            Edges = new List<Edge>();
            Employees = new List<Employee>();
            RegionID = regionID;
            Name = name;
            
            if(string.IsNullOrEmpty(parentID))
            {
                ParentRegionID = null;
            }
            else
            {
                ParentRegionID = parentID;
            }
        }

        public void AddEdge(Region child)
        {
            Edges.Add(new Edge { Child = child, Parent = this });
        }

        public void AddEmployee(Employee employee)
        {
            Employees.Add(employee);
        }
    }
}
