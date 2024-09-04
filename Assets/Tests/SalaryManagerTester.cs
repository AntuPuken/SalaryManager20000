using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

[TestFixture]
public class SalaryManagerTests
{
    private GameObject gameObject;
    private SalaryManager salaryManager;

    [SetUp]
    public void SetUp()
    {
        gameObject = new GameObject();
        salaryManager = gameObject.AddComponent<SalaryManager>();
    }


    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void GenerateEmployeeList_ShouldGenerateEmployeesCorrectly()
    {
        //Arrange 
         var employeeSeniorityAmounts = new Dictionary<Tuple<Charge, Seniority>, int>
        {
            { Tuple.Create(Charge.Engineering, Seniority.Senior), 2 },
            { Tuple.Create(Charge.Engineering, Seniority.Junior), 3 }
        };
         var employees = new List<Employee>();
         
        // Act
        salaryManager.GenerateEmployeeList(employees, employeeSeniorityAmounts);
        // Assert
        Assert.AreEqual(5, employees.Count);
        Assert.AreEqual(2,
            employees.FindAll(e => e.charge == Charge.Engineering && e.seniority == Seniority.Senior)
                .Count);
        Assert.AreEqual(3,
            employees.FindAll(e => e.charge == Charge.Engineering && e.seniority == Seniority.Junior)
                .Count);
    }

    [Test]
    public void EmployeeAmountGroupedByCharge_ShouldGroupEmployeesCorrectly()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee { charge = Charge.Engineering, seniority = Seniority.Senior },
            new Employee { charge = Charge.Engineering, seniority = Seniority.Junior },
            new Employee { charge = Charge.Engineering, seniority = Seniority.Junior },
            new Employee { charge = Charge.HumanResources, seniority = Seniority.Senior },
            new Employee { charge = Charge.HumanResources, seniority = Seniority.SemiSenior }
        };

        // Act
        var result = salaryManager.EmployeeAmountGroupedByCharge(employees);

        // Assert
        Assert.AreEqual(2, result.Count); // 2 groups: Engineering and HumanResources

        var engineeringGroup = result.Single(g => g.charge == Charge.Engineering);
        Assert.AreEqual(3, engineeringGroup.count); // 3 employees in Engineering
        Assert.AreEqual(2, engineeringGroup.seniorityGroups.Length); // 2 seniority levels in Engineering
        Assert.AreEqual(Seniority.Senior,
            engineeringGroup.seniorityGroups[0].seniority); // Senior should come before Junior
        Assert.AreEqual(1, engineeringGroup.seniorityGroups[0].value); // 1 Senior
        Assert.AreEqual(2, engineeringGroup.seniorityGroups[1].value); // 2 Juniors

        var hrGroup = result.Single(g => g.charge == Charge.HumanResources);
        Assert.AreEqual(2, hrGroup.count); // 2 employees in HumanResources
        Assert.AreEqual(2, hrGroup.seniorityGroups.Length); // 2 seniority levels in HumanResources
        Assert.AreEqual(Seniority.Senior, hrGroup.seniorityGroups[0].seniority); // Senior should come before SemiSenior
        Assert.AreEqual(1, hrGroup.seniorityGroups[0].value); // 1 Senior
        Assert.AreEqual(1, hrGroup.seniorityGroups[1].value); // 1 SemiSenior
    }


    [Test]
    public void ApplyRaises_ShouldCorrectlyApplyRaises()
    {
        // Arrange
        var baseSalaryAmounts = new List<SalaryAmmounts>
        {
            new SalaryAmmounts
            {
                charge = Charge.Engineering,
                baseSalaryBySeniority = new SeniorityWithInt[]
                {
                    new SeniorityWithInt { seniority = Seniority.Senior, value = 1000 },
                    new SeniorityWithInt { seniority = Seniority.Junior, value = 500 }
                }
            }
        };

        var incrementPercentages = new List<SalaryIncrements>
        {
            new SalaryIncrements
            {
                charge = Charge.Engineering,
                incrementBySeniority = new SeniorityWithFloat[]
                {
                    new SeniorityWithFloat { seniority = Seniority.Senior, value = 10f },
                    new SeniorityWithFloat { seniority = Seniority.Junior, value = 20f }
                }
            }
        };

        // Act
        var result = salaryManager.ApplyRaises(baseSalaryAmounts, incrementPercentages);

        // Assert
        Assert.AreEqual(1100, result[0].baseSalaryBySeniority[0].value);
        Assert.AreEqual(600, result[0].baseSalaryBySeniority[1].value);
    }
[Test]
    public void ApplyRaisesToEachEmployee_ShouldApplyRaisesCorrectly()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee { charge = Charge.Engineering, seniority = Seniority.Senior },
            new Employee { charge = Charge.Engineering, seniority = Seniority.Junior },
            new Employee { charge = Charge.HumanResources, seniority = Seniority.SemiSenior }
        };

        var baseSalaryAmmountsList = new List<SalaryAmmounts>
        {
            new SalaryAmmounts
            {
                charge = Charge.Engineering,
                baseSalaryBySeniority = new SeniorityWithInt[]
                {
                    new SeniorityWithInt { seniority = Seniority.Senior, value = 1000 },
                    new SeniorityWithInt { seniority = Seniority.Junior, value = 500 }
                }
            },
            new SalaryAmmounts
            {
                charge = Charge.HumanResources,
                baseSalaryBySeniority = new SeniorityWithInt[]
                {
                    new SeniorityWithInt { seniority = Seniority.SemiSenior, value = 800 }
                }
            }
        };

        var incrementPercentagesList = new List<SalaryIncrements>
        {
            new SalaryIncrements
            {
                charge = Charge.Engineering,
                incrementBySeniority = new SeniorityWithFloat[]
                {
                    new SeniorityWithFloat { seniority = Seniority.Senior, value = 10 },
                    new SeniorityWithFloat { seniority = Seniority.Junior, value = 5 }
                }
            },
            new SalaryIncrements
            {
                charge = Charge.HumanResources,
                incrementBySeniority = new SeniorityWithFloat[]
                {
                    new SeniorityWithFloat { seniority = Seniority.SemiSenior, value = 7.5f }
                }
            }
        };
        
        var expectedSalaryAmountsAfterRaise = new List<SalaryAmmountsWithFloat>
        {
            new SalaryAmmountsWithFloat
            {
                charge = Charge.Engineering,
                baseSalaryBySeniority = new SeniorityWithFloat[]
                {
                    new SeniorityWithFloat { seniority = Seniority.Senior, value = 1100 }, // 1000 + 10%
                    new SeniorityWithFloat { seniority = Seniority.Junior, value = 525 }   // 500 + 5%
                }
            },
            new SalaryAmmountsWithFloat
            {
                charge = Charge.HumanResources,
                baseSalaryBySeniority = new SeniorityWithFloat[]
                {
                    new SeniorityWithFloat { seniority = Seniority.SemiSenior, value = 860 } // 800 + 7.5%
                }
            }
        };
       List<SalaryAmmountsWithFloat> salaryAmountsWithFloats = new List<SalaryAmmountsWithFloat>();
        // Act
        var employeeWithSalaries = salaryManager.ApplyRaisesToEachEmployee(employees, salaryAmountsWithFloats, baseSalaryAmmountsList, incrementPercentagesList);

        // Assert
       
        // Assert the salaryAmountsWithFloats list is generated correctly
        Assert.AreEqual(expectedSalaryAmountsAfterRaise.Count, salaryAmountsWithFloats.Count);
        for (int i = 0; i < expectedSalaryAmountsAfterRaise.Count; i++)
        {
            Assert.AreEqual(expectedSalaryAmountsAfterRaise[i].charge, salaryAmountsWithFloats[i].charge);
            for (int j = 0; j < expectedSalaryAmountsAfterRaise[i].baseSalaryBySeniority.Length; j++)
            {
                Assert.AreEqual(expectedSalaryAmountsAfterRaise[i].baseSalaryBySeniority[j].seniority, salaryAmountsWithFloats[i].baseSalaryBySeniority[j].seniority);
                Assert.AreEqual(expectedSalaryAmountsAfterRaise[i].baseSalaryBySeniority[j].value, salaryAmountsWithFloats[i].baseSalaryBySeniority[j].value);
            }
        }
        
        Assert.AreEqual(3, employeeWithSalaries.Count);

        Assert.AreEqual(1100, employeeWithSalaries.Single(e => e.employee.charge == Charge.Engineering && e.employee.seniority == Seniority.Senior).salary);
        Assert.AreEqual(525, employeeWithSalaries.Single(e => e.employee.charge == Charge.Engineering && e.employee.seniority == Seniority.Junior).salary);
        Assert.AreEqual(860, employeeWithSalaries.Single(e => e.employee.charge == Charge.HumanResources && e.employee.seniority == Seniority.SemiSenior).salary);
    }
    [Test]
    public void OrderEmployeesByCharge_ShouldSortEmployeesByChargeDescending()
    {
        // Arrange
        salaryManager._employees = new List<Employee>
        {
            new Employee { charge = Charge.Engineering, seniority = Seniority.Junior },
            new Employee { charge = Charge.Design, seniority = Seniority.Senior },
            new Employee { charge = Charge.HumanResources, seniority = Seniority.Senior }
        };

        // Act
        var result = salaryManager.OrderEmployeesByCharge(salaryManager._employees);

        // Assert
        Assert.AreEqual(Charge.Design, result[0].charge);
        Assert.AreEqual(Charge.Engineering, result[1].charge);
    }
    [Test]
    public void Shuffle_ShouldRandomizeEmployeeList()
    {
        // Arrange
        salaryManager._employees = new List<Employee>
        {
            new Employee { charge = Charge.Engineering, seniority = Seniority.Senior },
            new Employee { charge = Charge.Engineering, seniority = Seniority.Junior },
            new Employee { charge = Charge.Engineering, seniority = Seniority.SemiSenior },
            new Employee { charge = Charge.HumanResources, seniority = Seniority.SemiSenior },
            new Employee { charge = Charge.Artist, seniority = Seniority.SemiSenior },
            new Employee { charge = Charge.ProjectManager, seniority = Seniority.SemiSenior },
        };

        var originalList = new List<Employee>(salaryManager._employees);

        // Act
        salaryManager.Shuffle(salaryManager._employees);

        // Assert
        Assert.AreNotEqual(originalList, salaryManager._employees);
    }

  

}