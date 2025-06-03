using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Encounters_Loader : MonoBehaviour
{

    [Header("Encounters List")]
    public List<Encounter_data> Encounters_List;

    /*Загрузка событий при старте*/
    private void Start()
    {
        Load_Encounters();
    }
    /*Загрузка событий из файла*/
    private void Load_Encounters()
    {
        Debug.Log("[Encounters_Loader] Load_Encounters() called");
        string path = Path.Combine(Application.streamingAssetsPath, "Encounters.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Encounter_DataListWrapper data = JsonUtility.FromJson<Encounter_DataListWrapper>(json);

            if (data != null && data.encounters != null)
            {
                Encounters_List = data.encounters;
                Debug.Log("Encounters loaded successfully!");
                Debug.Log($"Encounters count: {Encounters_List.Count}");

                Print_EncountersList(Encounters_List);
            }
            else
            {
                Debug.LogError("Failed to parse Traps.json or list is empty.");
            }
        }
        else
        {
            Debug.LogError("Traps file not found: " + path);
        }
    }

    // Метод для вывода списка загруженных событий
    private void Print_EncountersList(List<Encounter_data> encounters)
    {
        Debug.Log("[Encounters_Loader] Loaded encounters:");
        foreach (var encounter in encounters)
        {
            Debug.Log($"- {encounter.Encounter_Name} | Type: {encounter.Encounter_Type}");
        }
    }
    /*Поиск события*/
    public int Search_Encounter(string name)
    {
        foreach (var encounter in Encounters_List)
        {
            if (encounter.Encounter_Name == name)
            {
                Debug.Log($"Encounter found, name: {encounter.Encounter_Name}");
                return Encounters_List.IndexOf(encounter);
            }
        }
        Debug.Log("Encounter not found");
        return -1;
    }
}