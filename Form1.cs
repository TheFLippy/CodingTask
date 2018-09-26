using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coding_Task
{
    public partial class Form1 : Form
    {
        //Initialize config 
        Config config = new Config();
        
        //Initialize dictionary and list to hold regions and relevant employees
        Dictionary<string, Region> regionsDict = new Dictionary<string, Region>();
        List<Employee> employeesList = new List<Employee>();

        public Form1()
        {
            InitializeComponent();
            regionsDict = config.LoadRegionsCsv("regions.csv");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Reset text in case we are searching for not the first time
            richTextBox1.Text = "";
            regionsDict = config.ResetEdges();

            //List for employees
            List<Employee> employeeList;

            //If the entered region is in the dictionary:
            if (regionsDict.ContainsKey(textBox1.Text))
            {
                var root = config.FindPath(regionsDict[textBox1.Text]);

                //Means its a whole region (i.e root)
                if (root == null)
                {
                    employeeList = ExtractEmployees(regionsDict[textBox1.Text], new List<Employee>());
                    RunForAllStaff(employeeList);
                }
                //Else, recurse upwards 
                else
                {
                    RunForAllStaff(regionsDict[textBox1.Text].Employees);
                }
            }
            else
            {
                richTextBox1.Text = $"Region {textBox1.Text} does not exist!";
            }
        }

        //Extract all employees by traversing the tree 
        public List<Employee> ExtractEmployees(Region current, List<Employee> employeeList)
        {
            //Run on all edges on current region
            foreach(var edge in current.Edges)
            {
                //If the region has a child, add all it's employees to list
                if(edge.Child != null)
                {
                    foreach(Employee emp in edge.Child.Employees)
                    {
                        employeeList.Add(emp);
                    }

                    //If a child has edges, recurse and repeat
                    if(edge.Child.Edges.Count > 0)
                    {
                        return ExtractEmployees(edge.Child, employeeList);
                    }
                }
            }

            return employeeList;
        }

        //Run recursion on all staff 
        public void RunForAllStaff(List<Employee> employees)
        {
            //String builder init
            var stringBuilder = new StringBuilder("");

            //Store region path for easier syntax
            string regionPath;

            //
            foreach (Employee employee in employees)
            {
                regionPath = RecursionMethod(regionsDict[employee.RegionID], "");
                stringBuilder.Append($"{employee.Name} {employee.Surname}:\t\t{regionPath}\n");
            }

            //Check if there are any results
            if(string.IsNullOrWhiteSpace(stringBuilder.ToString()))
            {
                richTextBox1.Text = "No employees found!";
            }
            else
            {
                richTextBox1.Text = stringBuilder.ToString();
            }
        }

        //Recursive method to construct a path
        public string RecursionMethod(Region current, string path)
        {
            if(current.ParentRegionID == null)
            {
                path += $"{current.Name}.";
                return path;
            }
            else
            {
                path += $"{current.Name}, ";
                return RecursionMethod(regionsDict[current.ParentRegionID], path);
            }          
        }
    }
}
