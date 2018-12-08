using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ToolsRUs.Data.Entities;
using ToolsRUsSystem.DAL;
using System.ComponentModel;
using ToolsRUs.Data.POCOs;

#endregion

namespace ToolsRUsSystem.BLL
{
    [DataObject]
    public class EmployeeController
    {

        public Employee Employee_Get(int employeeID)
        {
            using (var context = new ToolsRUsContext())
            {
                return context.Employees.Find(employeeID);
            }
        }
    }
}
