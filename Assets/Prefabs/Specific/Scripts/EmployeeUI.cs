using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EmployeeUI : TwoValuesUI
{
  public void Initialize(Employee employee)
  {
      Initialize(employee.seniority, employee.charge);
  }
  public void Initialize(Seniority seniority, Charge charge)
  {
     base.Initialize(seniority.ToString(), charge.ToString());
  }
}
