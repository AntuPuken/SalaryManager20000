using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ChargeGroupBySeniorityAndValueUI : ValueGroupedByTwoValuesUI
{
    public void Initialize(Charge value, SeniorityWithFloat[] twoValuesArray)
    {
        first.text = value.ToString();
        foreach (var twoValue in twoValuesArray)
        {
            var tTHG = Instantiate(groupedFieldsPrefab).GetComponent<TwoValuesUI>();
            tTHG.Initialize(twoValue.seniority.ToString(), twoValue.value.ToString());
            tTHG.transform.SetParent(groupsContainer);
        }
    }   
    public void Initialize(Charge value, SeniorityWithInt[] twoValuesArray)
    {
        first.text = value.ToString();
        foreach (var twoValue in twoValuesArray)
        {
            var tTHG = Instantiate(groupedFieldsPrefab).GetComponent<TwoValuesUI>();
            tTHG.Initialize(twoValue.seniority.ToString(), twoValue.value.ToString());
            tTHG.transform.SetParent(groupsContainer);
        }
    } 
   
}
