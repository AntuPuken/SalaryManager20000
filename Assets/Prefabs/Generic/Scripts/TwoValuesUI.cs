using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TwoValuesUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI
        first;

    [SerializeField] TextMeshProUGUI
        second;

    public void Initialize(string item1, string item2)
    {
        this.first.text = item1;
        this.second.text = item2;
    }

}
