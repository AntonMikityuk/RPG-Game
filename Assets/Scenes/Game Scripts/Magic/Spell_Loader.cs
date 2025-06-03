// Spell_Loader.cs
using UnityEngine;
using System.Collections.Generic;
using System.IO; // Для Path.Combine и File

public class Spell_Loader : MonoBehaviour
{
    [Header("Spells List")]
    public List<Spell> Spells_List;

    private void Awake()
    {
        Spells_List = new List<Spell>(); // Инициализируем список
        Load_Spells();
    }

    private void Load_Spells()
    {
        Debug.Log("[Spell_Loader] LoadSpells() called");
        string path = Path.Combine(Application.streamingAssetsPath, "Spells.json");

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            Spell_DataListWrapper spellDataWrapper = JsonUtility.FromJson<Spell_DataListWrapper>(jsonString);

            if (spellDataWrapper != null && spellDataWrapper.spells != null)
            {
                Spells_List = spellDataWrapper.spells;
                Debug.Log($"[Spell_Loader] Spells loaded successfully! Count: {Spells_List.Count}");
                Print_SpellList(Spells_List);
            }
            else
            {
                Debug.LogError("[Spell_Loader] Failed to parse " + "Spells.json" + " or spell list is empty.");
            }
        }
        else
        {
            Debug.LogError("[Spell_Loader] Spell file not found: " + path);
        }
    }

    private void Print_SpellList(List<Spell> spells)
    {
        Debug.Log("[Spell_Loader] Loaded spells:");
        foreach (var spell in spells)
        {
            Debug.Log($"- {spell.Spell_Name} | Lvl: {spell.Level} | Type: {spell.Type} | Amount: {spell.Amount} | Cost: {spell.Mana_Cost} | Icon: {spell.Spell_Icon}");
        }
    }

    public Spell Search_Spell(string name)
    {
        foreach (var spell in Spells_List)
        {
            if (spell.Spell_Name == name)
            {
                return spell;
            }
        }
        Debug.LogWarning($"[Spell_Loader] Spell with name '{name}' not found.");
        return null;
    }

    public int Get_SpellIndexByName(string name) // Если нужен индекс
    {
        for (int i = 0; i < Spells_List.Count; i++)
        {
            if (Spells_List[i].Spell_Name == name)
            {
                return i;
            }
        }
        Debug.LogWarning($"[Spell_Loader] Spell with name '{name}' not found. Returning -1.");
        return -1;
    }
}