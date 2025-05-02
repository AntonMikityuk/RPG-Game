using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trap_Data
{
    public string trap_name;
    public string Stat_ToChek;
    public int Requirement;
    public int Damage;
}

[System.Serializable]
public class Trap_DataListWrapper
{
    public List<Trap_Data> traps;
}
