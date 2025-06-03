using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Trap_Loader : MonoBehaviour
{

    [Header("Traps List")]
    public List<Trap_Data> Trap_List;

    /*Загрузка ловушек при старте*/
    private void Start()
    {
        Load_Traps();
    }
    /*Загрузка ловушек из файла*/
    private void Load_Traps()
    {
        Debug.Log("[Trap_Loader] Load_Traps() called");
        string path = Path.Combine(Application.streamingAssetsPath, "Traps.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Trap_DataListWrapper data = JsonUtility.FromJson<Trap_DataListWrapper>(json);

            if (data != null && data.traps != null)
            {
                Trap_List = data.traps;
                Debug.Log("Traps loaded successfully!");
                Debug.Log($"Traps count: {Trap_List.Count}");

                Print_TrapList(Trap_List);
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

    // Метод для вывода списка загруженных ловушек
    private void Print_TrapList(List<Trap_Data> traps)
    {
        Debug.Log("[Trap_Loader] Loaded traps:");
        foreach (var trap in traps)
        {
            Debug.Log($"- {trap.trap_name} | Stat: {trap.Stat_ToChek} | Requirement: {trap.Requirement} | Damage: {trap.Damage}");
        }
    }
    /*Поиск ловушки*/
    public int Search_Trap(string name)
    {
        foreach (var trap in Trap_List)
        {
            if (trap.trap_name == name)
            {
                Debug.Log($"Trap found, name: {trap.trap_name}");
                return Trap_List.IndexOf(trap);
            }
        }
        Debug.Log("Trap not found");
        return -1;
    }
}