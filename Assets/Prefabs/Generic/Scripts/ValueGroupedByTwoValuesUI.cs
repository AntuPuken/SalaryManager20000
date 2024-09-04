using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueGroupedByTwoValuesUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI
        first;
    [SerializeField] protected Transform
        groupsContainer;
    [SerializeField] protected TwoValuesUI groupedFieldsPrefab;
    public void Initialize(string value, Tuple<string, string>[] twoValuesArray)
    {
        first.text = value;
        foreach (var twoValue in twoValuesArray)
        {
            var tTHG = Instantiate(groupedFieldsPrefab).GetComponent<TwoValuesUI>();
            tTHG.Initialize(twoValue.Item1, twoValue.Item2);
            tTHG.transform.SetParent(groupsContainer);
        }
    }
}

public class TMP_Text
{
}
