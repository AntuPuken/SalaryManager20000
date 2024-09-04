using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChargeBySeniorityWithFloatValueElementData : ChargeElementData
{
    public SeniorityWithFloat[] seniority { get; private set; }

    public ChargeBySeniorityWithFloatValueElementData(Charge charge, SeniorityWithFloat[] seniority)
        : base(charge)
    {
        this.seniority = seniority;
    }
}
public class PaginableChargeBySeniorityWithFloat : PaginableElement<ChargeBySeniorityWithFloatValueElementData>
{
    [SerializeField] private ChargeGroupBySeniorityAndValueUI employeeUI;

    protected override void OnDataUpdated(ChargeBySeniorityWithFloatValueElementData employeesElementData)
    {
        employeeUI.Initialize(employeesElementData.charge, employeesElementData.seniority);
    }
}
