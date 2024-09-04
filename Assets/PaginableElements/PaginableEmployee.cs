using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;


public struct EmployeeElementData : IPaginableElementData
{
    public Employee employee { get; private set; }
   
    public EmployeeElementData(Employee employee)
    {
        this.employee = employee;
    }
}
public class PaginableEmployee : PaginableElement<EmployeeElementData>
{
    [SerializeField] private EmployeeUI employeeUI;

    protected override void OnDataUpdated(EmployeeElementData employeesElementData)
    {
        employeeUI.Initialize(employeesElementData.employee);
    }
}
