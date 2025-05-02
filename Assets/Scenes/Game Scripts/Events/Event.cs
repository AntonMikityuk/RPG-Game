using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Структура ивента*/
[System.Serializable]
public class Event
{
    public string Event_name;
    public string Event_description;
    public string Event_tag;
    public string Enemy_type;
    public string Trap_type;

    [System.NonSerialized]
    public Battle_System.Event_Tier Tier;
}

/*Списки для иевнтов*/
[System.Serializable]
public class Event_Data
{
    public List<Event> common;
    public List<Event> rare;
    public List<Event> epic;
    public List<Event> special;
}
