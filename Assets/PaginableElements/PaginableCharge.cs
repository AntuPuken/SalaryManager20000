using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChargeElementData : IPaginableElementData
{
    public Charge charge { get; private set; }

    public ChargeElementData(Charge charge)
    {
        this.charge = charge;
    }
}