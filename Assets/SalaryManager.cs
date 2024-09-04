
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;




public class SalaryManager : MonoBehaviour
{
    [SerializeField] private PaginableEmployee employeePrefab;
    [SerializeField] private PaginableEmployeeWithSalary employeeWithSalaryPrefab;
    [SerializeField] private PaginableChargeBySeniorityWithFloat paginableChargeBySeniorityWithFloatPrefab;
    [SerializeField] private PaginableChargeBySeniorityWithInt paginableChargeBySeniorityWithIntPrefab;
    [SerializeField] private PaginableGroupedChargeAndSeniorities chargeAndSenioritiesPrefab;
    [SerializeField] private Transform employeesContainer;
    [SerializeField] private Button fillEmployeesBtn;
    [SerializeField] private Button shuffleEmployeesBtn;
    [SerializeField] private Button orderEmployeesBtn;
     [SerializeField] private Button employeesWithPercentagesBtn;
    [SerializeField] private Button groupEmployeesByChargeBtn;
    [SerializeField] private Button showSalariesBtn;
    [SerializeField] private Button applyRaisesBtn;
    [SerializeField] private Button applyRaisesToEachEmployeeBtn;
    [SerializeField] private SalaryIncrementPercentages salaryIncrementPercentagesSO;
    [SerializeField] private SalaryBaseAmounts salaryBaseAmountsSO;

    public Dictionary<Tuple<Charge, Seniority>, int> employeeSeniorityAmounts = new Dictionary<Tuple<Charge, Seniority>, int> // here we will store the amount of employees for each charge and their seniority, this is done to speed up the data generation process but it is assumed that the list will be provided.
   {
       { Tuple.Create(Charge.HumanResources, Seniority.Senior), 5 },
       { Tuple.Create(Charge.HumanResources, Seniority.SemiSenior), 2 },
       { Tuple.Create(Charge.HumanResources, Seniority.Junior), 13 },
       { Tuple.Create(Charge.Engineering, Seniority.Senior), 50 },
       { Tuple.Create(Charge.Engineering, Seniority.SemiSenior), 68 },
       { Tuple.Create(Charge.Engineering, Seniority.Junior), 32 },
       { Tuple.Create(Charge.Artist, Seniority.Senior), 5 },
       { Tuple.Create(Charge.Artist, Seniority.SemiSenior), 20 },
       { Tuple.Create(Charge.Design, Seniority.Senior), 10 },
       { Tuple.Create(Charge.Design, Seniority.Junior), 15 },
       { Tuple.Create(Charge.ProjectManager, Seniority.Senior), 10 },
       { Tuple.Create(Charge.ProjectManager, Seniority.SemiSenior), 20 },
       { Tuple.Create(Charge.CEO, Seniority.Senior), 1 }
   };

   public List<Employee> _employees = new List<Employee>();
   public bool isShufeled;
   public List<SalaryAmmountsWithFloat> salaryAmountsWithRaise = new List<SalaryAmmountsWithFloat>();
   public List<EmployeeWithSalary> employeeWithSalaries = new List<EmployeeWithSalary>();
   public List<EmployeeGroup> employeeGroupedBySenority = new List<EmployeeGroup>();
   private void Start()
   {
       GenerateEmployeeList(_employees, employeeSeniorityAmounts);
   }

   public void GenerateEmployeeList(List<Employee> employees, Dictionary<Tuple<Charge, Seniority>, int> employeeSeniorityAmounts)
   {
       foreach (var employeeSeniorityAmount in employeeSeniorityAmounts)//generating employees list
       {
           for (int i = 0; i < employeeSeniorityAmount.Value; i++)
           {
               employees.Add(new Employee()
                   {
                       charge = employeeSeniorityAmount.Key.Item1,
                       seniority = employeeSeniorityAmount.Key.Item2
                   }
               );
           }
       }
   }
   
    private void OnEnable()
    {
        RegisterActions();
    }

    private void OnDisable()
    {
        UnregisterActions();
    }
    private void RegisterActions()
    {
        fillEmployeesBtn.onClick.AddListener(ResetEmployeeSliderContent);
        shuffleEmployeesBtn.onClick.AddListener(ShuffleAndRefillEmployeeList);
        orderEmployeesBtn.onClick.AddListener(OrderAndRefillEmployeeList);
        employeesWithPercentagesBtn.onClick.AddListener(ShowEmployeesWithPercentages);
        groupEmployeesByChargeBtn.onClick.AddListener(ShowEmployeeAmountGroupedByCharge);
        showSalariesBtn.onClick.AddListener(ShowSalaries);
        applyRaisesBtn.onClick.AddListener(ApplyRaisesAndRefreshUI);
        applyRaisesToEachEmployeeBtn.onClick.AddListener(ApplyRaisesToEachEmployeeAndRefreshUI);
    }

    private void ShowEmployeeAmountGroupedByCharge()
    {
        employeeGroupedBySenority = EmployeeAmountGroupedByCharge(_employees);
        UpdateEmployeeContainer(
            employeeGroupedBySenority, 
            chargeAndSenioritiesPrefab.gameObject, 
            employee => new ChargeAndSenioritiesElementData(
                new ChargeWithInt()
                {
                    charge = employee.charge,
                    value = employee.count,
                },
                employee.seniorityGroups
                    .ToArray()
            )
        );
    }

    private void UnregisterActions()
    {
        fillEmployeesBtn.onClick.RemoveListener(ResetEmployeeSliderContent);
        shuffleEmployeesBtn.onClick.RemoveListener(ShuffleAndRefillEmployeeList);
        orderEmployeesBtn.onClick.RemoveListener(OrderAndRefillEmployeeList);
        employeesWithPercentagesBtn.onClick.RemoveListener(ShowEmployeesWithPercentages);
        groupEmployeesByChargeBtn.onClick.RemoveListener(ShowEmployeeAmountGroupedByCharge);
        showSalariesBtn.onClick.RemoveListener(ShowSalaries);
        applyRaisesBtn.onClick.RemoveListener(ApplyRaisesAndRefreshUI);
        applyRaisesToEachEmployeeBtn.onClick.RemoveListener(ApplyRaisesToEachEmployeeAndRefreshUI);

    }
  
    private void ApplyRaisesToEachEmployeeAndRefreshUI()
    {
        employeeWithSalaries = ApplyRaisesToEachEmployee(_employees, salaryAmountsWithRaise, salaryBaseAmountsSO.baseSalaryAmmountsList, salaryIncrementPercentagesSO.incrementsList);
        UpdateEmployeeContainer(employeeWithSalaries, employeeWithSalaryPrefab.gameObject,increment => new EmployeeWithSalaryElementData(increment.employee, increment.salary));
    }
  

    private void ApplyRaisesAndRefreshUI()
    {
        salaryAmountsWithRaise = ApplyRaises(salaryBaseAmountsSO.baseSalaryAmmountsList, salaryIncrementPercentagesSO.incrementsList);
        UpdateEmployeeContainer(salaryAmountsWithRaise, paginableChargeBySeniorityWithFloatPrefab.gameObject,
            increment =>
                new ChargeBySeniorityWithFloatValueElementData(increment.charge, increment.baseSalaryBySeniority));
    }

    private void ShowSalaries()
    {
       UpdateEmployeeContainer(salaryBaseAmountsSO.baseSalaryAmmountsList, paginableChargeBySeniorityWithIntPrefab.gameObject, increment => new ChargeBySeniorityWithIntValueElementData(increment.charge, increment.baseSalaryBySeniority));
    }
    private void OrderAndRefillEmployeeList()
    {
        if (CheckAndToggleShuffle(true))
        {
            _employees = OrderEmployeesByCharge(_employees);
            ResetEmployeeSliderContent();
        }
    }
    private void ShuffleAndRefillEmployeeList()
    {
        if (CheckAndToggleShuffle(false))
        {
            Shuffle(_employees);
            ResetEmployeeSliderContent();
        }
       
    }
    private void ShowEmployeesWithPercentages()
    {
        UpdateEmployeeContainer(
            salaryIncrementPercentagesSO.incrementsList, 
            paginableChargeBySeniorityWithFloatPrefab.gameObject, 
            increment => new ChargeBySeniorityWithFloatValueElementData(increment.charge, increment.incrementBySeniority)
        );
    }
    
    private void ResetEmployeeSliderContent()
    { 
        UpdateEmployeeContainer(
            _employees, 
            employeePrefab.gameObject, 
            employee => new EmployeeElementData(employee)
        );
    }
    private bool CheckAndToggleShuffle(bool expectedState)
    {
        if (isShufeled == expectedState)
        {
            isShufeled = !expectedState;
            return true;
        }
        return false;
    }
    private void UpdateEmployeeContainer<T>(IEnumerable<T> employeeDataList, GameObject prefab, Func<T, IPaginableElementData> createData)
    {
        if (employeesContainer.childCount != 0)
            DestroyContainerChildren();

        foreach (var data in employeeDataList)
        {
            var eU = Instantiate(prefab).GetComponent<IPaginableElement>();
            eU.SetData(createData(data));
            eU.transform.SetParent(employeesContainer);
        }
        
    }
    private void DestroyContainerChildren()
    {
        foreach(Transform child in employeesContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public  List<EmployeeGroup> EmployeeAmountGroupedByCharge(List<Employee> employees)
    {
        return employees
            .GroupBy(e => e.charge)
            .Select(g => new EmployeeGroup
            {
                charge = g.Key,
                count = g.Count(),
                seniorityGroups = g.GroupBy(e => e.seniority)
                    .Select(sg => new SeniorityWithInt()
                    {
                        seniority = sg.Key,
                        value = sg.Count()
                    })
                    .OrderBy(sg => sg.seniority)
                    .ToArray()
            })
            .OrderBy(g => g.charge)
            .ToList();
    }
    public  List<SalaryAmmountsWithFloat> ApplyRaises(List<SalaryAmmounts> baseSalaryAmmountsList, List<SalaryIncrements> incrementPercentagesList)
    {
        var employeesWithRaisesList = new List<SalaryAmmountsWithFloat>();

        for (int i = 0; i < baseSalaryAmmountsList.Count; i++)
        {
            SalaryAmmountsWithFloat salaryAmmountsWithFloat = new SalaryAmmountsWithFloat();
            salaryAmmountsWithFloat.charge = baseSalaryAmmountsList[i].charge;
            SeniorityWithFloat[] seniorityWithFloats =
                new SeniorityWithFloat[baseSalaryAmmountsList[i].baseSalaryBySeniority.Length];
            for (int j = 0; j < baseSalaryAmmountsList[i].baseSalaryBySeniority.Length; j++)
            {
                seniorityWithFloats[j] = new SeniorityWithFloat()
                {
                    seniority = baseSalaryAmmountsList[i].baseSalaryBySeniority[j].seniority,
                    value = baseSalaryAmmountsList[i].baseSalaryBySeniority[j].value +
                            baseSalaryAmmountsList[i].baseSalaryBySeniority[j].value *
                            (incrementPercentagesList[i].incrementBySeniority[j].value / 100)
                };
                salaryAmmountsWithFloat.baseSalaryBySeniority = seniorityWithFloats;
            }

            employeesWithRaisesList.Add(salaryAmmountsWithFloat);
        }
        return employeesWithRaisesList;
    }
      public List<EmployeeWithSalary> ApplyRaisesToEachEmployee(List<Employee> employees, List<SalaryAmmountsWithFloat> salaryAmountsAfterRaise,  List<SalaryAmmounts> baseSalaryAmmountsList, List<SalaryIncrements> incrementPercentagesList)
    {
        if (salaryAmountsAfterRaise.Count == 0)
        {
            Debug.LogWarning($"The base list hasnt been generated yet, it will be generated automatically, dont worry");
           var calculatedSalaryAmountsAfterRaise = ApplyRaises(baseSalaryAmmountsList, incrementPercentagesList);
           salaryAmountsAfterRaise.AddRange(calculatedSalaryAmountsAfterRaise);
        }
        
        var employeeWithSalaries = new List<EmployeeWithSalary>();
        foreach (var employee in employees)
        {
            employeeWithSalaries.Add(new EmployeeWithSalary()
            {
                employee = employee,
                salary =  salaryAmountsAfterRaise.Single(x => x.charge == employee.charge).baseSalaryBySeniority.Single(x=> x.seniority == employee.seniority).value
            });
        }
        return employeeWithSalaries;
    }
    public List<Employee> OrderEmployeesByCharge(List<Employee> employees)
    {
       return employees.OrderByDescending(x => x.charge).ToList();
    } 
    public void Shuffle<T>(IList<T> list)
      {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
      }
}
