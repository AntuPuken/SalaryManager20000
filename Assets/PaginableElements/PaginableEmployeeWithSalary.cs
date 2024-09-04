using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EmployeeWithSalaryElementData : IPaginableElementData
{
    public Employee employee { get; private set; }
    public float salary { get; private set; }
   
    public EmployeeWithSalaryElementData(Employee employee, float salary)
    {
        this.employee = employee;
        this.salary = salary;
    }
}
public class PaginableEmployeeWithSalary : PaginableElement<EmployeeWithSalaryElementData>
{
    [SerializeField] private EmployeeWithNumberUI employeeUI;

    protected override void OnDataUpdated(EmployeeWithSalaryElementData employeesElementData)
    {
        employeeUI.Initialize(employeesElementData.employee, employeesElementData.salary.ToString());
    }
}
