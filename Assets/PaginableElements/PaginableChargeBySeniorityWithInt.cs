using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChargeBySeniorityWithIntValueElementData : ChargeElementData
{
    public SeniorityWithInt[] seniority { get; private set; }

    public ChargeBySeniorityWithIntValueElementData(Charge charge, SeniorityWithInt[] seniority)
        : base(charge)
    {
        this.seniority = seniority;
    }
}

public class PaginableChargeBySeniorityWithInt : PaginableElement<ChargeBySeniorityWithIntValueElementData>
{
    [SerializeField] private ChargeGroupBySeniorityAndValueUI employeeUI;

    protected override void OnDataUpdated(ChargeBySeniorityWithIntValueElementData employeesElementData)
    {
        employeeUI.Initialize(employeesElementData.charge, employeesElementData.seniority);
    }
}
