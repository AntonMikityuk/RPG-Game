using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Encounters_Events : MonoBehaviour
{
    [Header("Text Typer")]
    public Text_Typer Typer;

    [Header("Game Manager")]
    public Game_Management Game_Manager;

    [Header("Event_System")]
    public Event_Buttons_Changer Event_Changer;

    [Header("Encounters Loader")]
    public Encounters_Loader Loader;

    private Hero Player_hero;

    private Encounter_data Current_EncounterData;

    [NonSerialized]
    public string Encounter_Type;


    private void Awake()
    {
        Player_hero = FindAnyObjectByType<Hero>();
        if (Player_hero == null)
        {
            Debug.LogError("Hero not found!");
            return;
        }
    }

    public void Initiate_Encounter(string encounter_name)
    {
        if (Loader == null || Loader.Encounters_List == null)
        {
            Debug.LogError("Encounters_Loader or Encounters_List not initialised");
            return;
        }
        int Encounter_Index = Loader.Search_Encounter(encounter_name);
        if (Encounter_Index == -1)
        {
            Debug.LogError("Encounter not found");
            return;
        }
        Current_EncounterData = Loader.Encounters_List[Encounter_Index];
    }

    public string Encounter_SelectType()
    {
        Encounter_Type = Current_EncounterData.Encounter_Type;
        Debug.Log($"Current encounter type - {Encounter_Type}");
        return Encounter_Type;
    }
}
