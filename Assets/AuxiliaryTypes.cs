using System;
using System.Collections.Generic;

[System.Serializable]
public enum Charge// for new charges add them at the end of the enum(append) so we dont loose the serialized values
{
    HumanResources,
    Engineering,
    Artist,
    Design,
    ProjectManager,
    CEO
}
[System.Serializable]
public enum Seniority// for new seniorities add them at the end of the enum(append) so we dont loose the serialized values
{
    Senior,
    SemiSenior,
    Junior
}
[System.Serializable]
public struct Employee
{
    public Charge charge;
    public Seniority seniority;
}
[System.Serializable]
public struct EmployeeWithSalary
{
    public Employee employee;
    public float salary;
}
[Serializable]
public struct SeniorityWithFloat
{
    public Seniority seniority;
    public float value;
}
public struct EmployeeGroup
{
    public Charge charge { get; set; }
    public int count { get; set; }
    public SeniorityWithInt[] seniorityGroups { get; set; }
}
[Serializable]
public struct SeniorityWithInt
{
    public Seniority seniority;
    public int value;
}
public struct ChargeWithInt
{
    public Charge charge;
    public int value;
}
[System.Serializable]
public struct SalaryIncrements
{ 
    public Charge charge;
    public SeniorityWithFloat[] incrementBySeniority;
}
[System.Serializable]
public struct SalaryAmmounts
{
    public Charge charge;
    public SeniorityWithInt[] baseSalaryBySeniority;
}
public struct SalaryAmmountsWithFloat
{
    public Charge charge;
    public SeniorityWithFloat[] baseSalaryBySeniority;
}