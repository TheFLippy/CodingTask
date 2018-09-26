using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Coding_Task;

namespace Coding_Task
{
    public class Config
    {
        //Initialize dictionary
        Dictionary<string, Region> RegionDictionary = new Dictionary<string, Region>();
        List<Region> roots;

        public void GenerateGraph()
        {
            roots = new List<Region>();

            foreach(var region in RegionDictionary)
            {
                if(region.Value.ParentRegionID != null)
                {
                    RegionDictionary[region.Value.ParentRegionID].AddEdge(region.Value);
                }
                //Found root
                else
                {
                    roots.Add(region.Value);
                }
            }
        }

        public string FindPath(Region goalNode)
        {
            foreach(Region root in roots)
            {
                if (BFS(root, goalNode))
                {
                    return root.RegionID;
                }
            }

            return null;
        }

        //Method which takes in path to CSV, loads its contents into a dictionary and passes it back
        public Dictionary<string, Region> LoadRegionsCsv(string pathRegions)
        {
            using (TextFieldParser parser = new TextFieldParser(pathRegions))
            {
                //Set the delimiter for the parser (takes an array of delimiters, we only need comma)
                parser.Delimiters = new string[] { "," };
                while (!parser.EndOfData)
                {
                    try
                    {
                        //Read csv row into an array (thats what the method returns)
                        string[] fields = parser.ReadFields();

                        //Get the region details out of the array and create an object
                        //Not a good idea to hard code array indexes, but thats what the parser returns...
                        var name = fields[0];
                        var id = fields[1];
                        var parentID = fields[2];

                        var RegionObject = new Region(id, name, parentID);

                        //Add ID and Region object to dictionary
                        if (!RegionDictionary.ContainsKey(id))
                        {
                            RegionDictionary.Add(id, RegionObject);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        
                    }
                }
                GenerateGraph();
                PopulateRegionsFromCVS("employees.csv");
                return RegionDictionary;

            }
        }

        public void PopulateRegionsFromCVS(string pathRegions)
        {
            using (TextFieldParser parser = new TextFieldParser(pathRegions))
            {
                //Set the delimiter for the parser (takes an array of delimiters, we only need comma)
                parser.Delimiters = new string[] { "," };
                while (!parser.EndOfData)
                {
                    try
                    {
                        //Read csv row into an array (thats what the method returns)
                        string[] fields = parser.ReadFields();

                        //Get the employee details out of the array and create an object
                        string RegionID = fields[0];
                        string Name = fields[1];
                        string Surname = fields[2];
                        Employee employee = new Employee(RegionID, Name, Surname);

                        if (RegionDictionary.ContainsKey(RegionID))
                        {
                            RegionDictionary[RegionID].AddEmployee(employee);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }


        public bool BFS(Region root, Region goal)
        {

            Queue<Region> queue = new Queue<Region>();
            queue.Enqueue(root);

            while(queue.Count != 0)
            {
                Region r = (Region)queue.Dequeue();

                foreach(Edge edge in r.Edges)
                {
                    if(edge.Child.Name == goal.Name)
                    {
                        edge.Child.Visited = true;
                        return true;
                    }
                    else if(edge.Child.Visited == false)
                    {
                        edge.Child.Visited = true;
                        queue.Enqueue(edge.Child);
                    }
                }
            }
            return false;
        }

        public Dictionary<string, Region> ResetEdges()
        {
            foreach(var region in RegionDictionary)
            {
                region.Value.Visited = false;
            }

            return RegionDictionary;
        }
        
        //Method which takes in path to CSV, loads its contents into a dictionary and passes it back
        /*
        public List<Employee> LoadEmployeesCsv(string pathRegions, string regionCode)
        {
            //Initialize dictionary
            List<Employee> EmployeesList = new List<Employee>();

            using (TextFieldParser parser = new TextFieldParser(pathRegions))
            {
                //Set the delimiter for the parser (takes an array of delimiters, we only need comma)
                parser.Delimiters = new string[] { "," };
                while (!parser.EndOfData)
                {
                    try
                    {
                        //Read csv row into an array (thats what the method returns)
                        string[] fields = parser.ReadFields();

                        //Get the employee details out of the array and create an object
                        string RegionID = fields[0];

                        //If the region code from the text box matches the one parsed, add it to list
                        if (RegionID == regionCode)
                        {
                            string Name = fields[1];
                            string Surname = fields[2];
                            Employee employee = new Employee(RegionID, Name, Surname);

                            //Add object to list
                            EmployeesList.Add(employee);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                return EmployeesList;

            }
        }
        */

        /*
         public List<Region> RecurseAdd(Region Current, List<Region> Holder)
        {
            //Exit statement: if there is no parent, we found the root. Assign the list to it
            if (Current.ParentRegionID == null)
            {
                Holder.Add(Current);
                RegionDictionary[Current.RegionID].Children = Holder;
                return Holder;
            }
            //If region has a parent, concatinate the name to return string
            else
            {
                Holder.Add(Current);

                //Retrieve parent region and recurse
                Region next = RegionDictionary[Current.ParentRegionID];
                return RecurseAdd(next, Holder);
            }
        }
        */

    }
}
