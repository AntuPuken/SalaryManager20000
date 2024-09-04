using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SalaryBaseAmounts", menuName = "ScriptableObjects/SalaryBaseAmounts")]
public class SalaryBaseAmounts : ScriptableObject
{
    public List<SalaryAmmounts> baseSalaryAmmountsList;
}
