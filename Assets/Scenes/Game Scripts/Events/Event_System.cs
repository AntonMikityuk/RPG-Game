using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Event_System : MonoBehaviour
{
    /*Списки с ивентами*/
    [Header("Events Lists")]
    public List<Event> Common_Events;
    public List<Event> Rare_Events;
    public List<Event> Epic_Events;
    public List<Event> Special_Events;

    /*Загрузка ивентов при запуске окна игры*/
    private void Awake()
    {
        Load_Events();
    }
    /*Получение рандомного ивента*/
    public Event Get_Event()
    {
        int roll = Random.Range(0, 100);
        Event Selected_Event = null;
        Debug.Log($"Rolling event: {roll}");

        if (roll <= 60 && Common_Events.Count > 0)
        {
            Selected_Event = Common_Events[Random.Range(0, Common_Events.Count)];
            Selected_Event.Tier = Battle_System.Event_Tier.Common;
        }
        else if (roll <= 95 && Rare_Events.Count > 0)
        {
            Selected_Event = Rare_Events[Random.Range(0, Rare_Events.Count)];
            Selected_Event.Tier = Battle_System.Event_Tier.Rare;
        }
        else if (Epic_Events.Count > 0)
        {
            Selected_Event = Epic_Events[Random.Range(0, Epic_Events.Count)];
            Selected_Event.Tier = Battle_System.Event_Tier.Epic;
        }
        
        if (Selected_Event == null)
        {
            Debug.LogWarning("No events available for selection!");
        }
        return Selected_Event;
    }
    /*Загрузка ивентов из json файла, вывод количества событий каждого типа + их название и описание*/
    private void Load_Events()
    {
        Debug.Log("[Event_System] Load_Events() called");
        string path = Path.Combine(Application.streamingAssetsPath, "Events.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Event_Data data = JsonUtility.FromJson<Event_Data>(json);

            Common_Events = data.common ?? new List<Event>();
            Rare_Events = data.rare ?? new List<Event>();
            Epic_Events = data.epic ?? new List<Event>();
            Special_Events = data.special ?? new List<Event>();

            Debug.Log("Events loaded successfully!");
            Debug.Log($"Events count: Common: {Common_Events.Count}, Rare: {Rare_Events.Count}, Epic: {Epic_Events.Count}");

            PrintEventList("Common events", Common_Events);
            PrintEventList("Rare events", Rare_Events);
            PrintEventList("Epic events", Epic_Events);
            PrintEventList("Special events", Special_Events);

            Event randomCommonEvent = GetRandom_Event(Common_Events);
            Debug.Log($"Selected event: {randomCommonEvent.Event_name} - {randomCommonEvent.Event_description}");
        }
        else
        {
            Debug.LogError("Event file not found: " + path);
        }
    }
    /*Вывод всех элементов списка определенного типа в консоль*/
    private void PrintEventList(string Event_type, List<Event> Event_list)
    {
        if (Event_list.Count == 0)
        {
            Debug.LogWarning($"{Event_type} events list is empty!");
            return;
        }

        Debug.Log($"--- {Event_type} Events ---");
        foreach (var Event in Event_list)
        {
            Debug.Log($"Name: {Event.Event_name}, Description: {Event.Event_description}, Tag: {Event.Event_tag}, enemy type: {Event.Enemy_type}");
        }
    }
    /*Получение рандомного ивента*/
    public Event GetRandom_Event(List<Event> Event_list)
    {
        if (Event_list.Count == 0 || Event_list == null)
        {
            Debug.LogWarning("Events list is empty!");
            return null;
        }
        return Event_list[Random.Range(0, Event_list.Count)];
    }
}
