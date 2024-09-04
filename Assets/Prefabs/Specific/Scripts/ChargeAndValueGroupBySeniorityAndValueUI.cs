using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargeAndValueGroupBySeniorityAndValueUI : ChargeGroupBySeniorityAndValueUI 
{
    [SerializeField] protected TextMeshProUGUI
        second;
    public void Initialize(ChargeWithInt chargeTotal, SeniorityWithInt[] seniorityTotals)
    {
        second.text = chargeTotal.value.ToString(); 
        base.Initialize(chargeTotal.charge, seniorityTotals);
    }
}
