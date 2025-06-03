using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Encounter_data
{
    public string Encounter_Name;
    public string Encounter_Type;
}

[System.Serializable]
public class Encounter_DataListWrapper
{
    public List<Encounter_data> encounters;
}
