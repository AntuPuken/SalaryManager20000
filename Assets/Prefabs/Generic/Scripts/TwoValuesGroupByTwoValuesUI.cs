using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwoValuesGroupedByTwoValuesUI : ValueGroupedByTwoValuesUI
{
   [SerializeField] protected TextMeshProUGUI
      second;

   public void Initialize(Tuple<string, string> chargeTotal, Tuple<string, string>[] seniorityTotals)
   {
      second.text = chargeTotal.Item2; 
      base.Initialize(chargeTotal.Item1, seniorityTotals);
   }
}