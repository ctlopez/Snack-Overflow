﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
       
    public interface IEmployeeManager
    {

		Employee RetrieveEmployeeByUserName(string userName);

        /// <summary>
        /// Daniel brown
        /// Created 02/08/2017
        /// 
        /// Retrieve a single employee
        /// </summary>
        /// <param name="employeeID">The Id of the employee to be retrieved</param>
        /// <returns>A single Employee Object</returns>
        Employee RetrieveEmployee(int employeeID);

        /// <summary>
        /// Daniel Brown
        /// Created 02/08/2017
        /// 
        /// Retrieve a list of all employees
        /// </summary>
        /// <returns>List of employees</returns>
        List<Employee> RetrieveEmployeeList();

        bool UpdateEmployee(Employee oldEmployee, Employee newEmployee);

        int CreateEmployee(Employee employeeInstance);

        List<Employee> SearchEmployees(Employee searchParameters);
    }
}
