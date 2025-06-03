using UnityEngine;
using System.Collections.Generic;


public class SpellbookManager : MonoBehaviour
{
    [Header("Spellbook slots")]
    public List<Spellbook_Slot> Spellbook_Slots;

    private List<Spell> Spells_ToDisplay = new List<Spell>();

    [Header("Pages Info")]
    public int spellsPerPage = 6;
    private int currentPage = 0;

    void Start()
    {
        Spell_Loader Loader = FindAnyObjectByType<Spell_Loader>();
        if (Loader != null && Loader.Spells_List != null)
        {
            Spells_ToDisplay = Loader.Spells_List;
            Debug.Log($"[SpellbookManager] Received {Spells_ToDisplay.Count} spells from Spell_Loader.");
        }
        else
        {
            Debug.LogError("[SpellbookManager] Spell_Loader instance not found or no spells loaded. Ensure Spell_Loader is in the scene and configured.");
            return;
        }

        if (Spellbook_Slots == null || Spellbook_Slots.Count == 0)
        {
            Debug.LogError("[SpellbookManager] Spellbook slots not assigned or empty!");
            return;
        }
        DisplayCurrentPage();
        SetupSlotButtons();
    }

    void DisplayCurrentPage()
    {
        int startIndex = currentPage * spellsPerPage;

        for (int i = 0; i < Spellbook_Slots.Count; i++)
        {
            int spellIndex = startIndex + i;
            if (spellIndex < Spells_ToDisplay.Count)
            {
                Spellbook_Slots[i].DisplaySpell(Spells_ToDisplay[spellIndex]);
            }
            else
            {
                Spellbook_Slots[i].Clear_Slot();
            }
        }
    }

    void SetupSlotButtons()
    {
        if (Spellbook_Slots == null) return;

        foreach (Spellbook_Slot slot in Spellbook_Slots)
        {
            if (slot.Slot_Button != null)
            {
                slot.Slot_Button.onClick.RemoveAllListeners();
                Spell Spell_ForButton = slot.Current_SpellData;
                slot.Slot_Button.onClick.AddListener(() => OnSpellSlot_Clicked(Spell_ForButton, slot));
            }
        }
    }

    void OnSpellSlot_Clicked(Spell Spell_Data, Spellbook_Slot Selected_Slot)
    {
        if (Spell_Data == null)
        {
            Debug.Log($"[SpellbookManager] Clicked on an empty slot (UI Slot Index: {Spellbook_Slots.IndexOf(Selected_Slot)}).");
            return;
        }

        Debug.Log($"[SpellbookManager] Clicked on spell: {Spell_Data.Spell_Name}, Level: {Spell_Data.Level}, Cost: {Spell_Data.Mana_Cost}");
        /*Тут логику написать*/
    }

    public void Next_Page()
    {
        if (Spells_ToDisplay.Count == 0)
            return;

        int maxPage = Mathf.CeilToInt((float)Spells_ToDisplay.Count / spellsPerPage) - 1;
        if (maxPage < 0)
            maxPage = 0;

        if (currentPage < maxPage)
        {
            currentPage++;
            DisplayCurrentPage();
            SetupSlotButtons();
        }
    }

    public void Previous_Page()
    {
        if (currentPage > 0)
        {
            currentPage--;
            DisplayCurrentPage();
            SetupSlotButtons();
        }
    }
}