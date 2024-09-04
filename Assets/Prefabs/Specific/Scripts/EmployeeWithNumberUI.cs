using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EmployeeWithNumberUI : EmployeeUI
{
    [SerializeField] private TextMeshProUGUI number;
  
    public  void Initialize(Employee employee, string number)
  {
      base.Initialize(employee.seniority, employee.charge);
      this.number.text = number;
  }
    

}
