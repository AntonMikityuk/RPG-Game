using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell
{
    public string Spell_Name;
    public string Spell_Description;
    public int Level;
    public string Type;
    public int Amount;
    public int Mana_Cost;
    public string Spell_Icon;
}

[System.Serializable]
public class Spell_DataListWrapper
{
    public List<Spell> spells;
}
