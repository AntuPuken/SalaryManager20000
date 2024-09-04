using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;


public struct ChargeAndSenioritiesElementData : IPaginableElementData
{
    public ChargeWithInt chargeTotal { get; private set; }
    public SeniorityWithInt[] senioritiesTotals { get; private set; }
   
    public ChargeAndSenioritiesElementData(ChargeWithInt chargeTotal, SeniorityWithInt[] senioritiesTotals)
    {
        this.chargeTotal = chargeTotal;
        this.senioritiesTotals = senioritiesTotals;
    }
}
public class PaginableGroupedChargeAndSeniorities : PaginableElement<ChargeAndSenioritiesElementData>
{
    [SerializeField] private ChargeAndValueGroupBySeniorityAndValueUI chargeAndSenioritiesUI;

    protected override void OnDataUpdated(ChargeAndSenioritiesElementData employeesElementData)
    {
        chargeAndSenioritiesUI.Initialize(employeesElementData.chargeTotal, employeesElementData.senioritiesTotals);
    }
}
