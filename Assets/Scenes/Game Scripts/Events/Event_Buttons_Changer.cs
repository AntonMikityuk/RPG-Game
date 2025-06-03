using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Event_Buttons_Changer : MonoBehaviour
{
    [Header("Event System")]
    public Event_System Event_Changer;
    [Header("Text Typer")]
    public Text_Typer Text_Reference;
    [Header("Windows")]
    public GameObject Combat_Window;
    [Header("Combat Manager")]
    public Battle_System Combat_Manager;
    [Header("Trap Events")]
    public Trap_Events Traps;
    [Header("Encounters")]
    public Encounters_Events Encounters;
    [Header("Game_Manager")]
    public Game_Management Manager;
    [Header("Hero")]
    public Hero Player_Hero;

    [Header("Events for Loading")]
    public Event Event_1;
    public Event Event_2;
    public Event Event_3;

    /*Кнопки ивентов*/
    [System.Serializable]
    public class Event_Button
    {
        public Button Button_Event;
        public TextMeshProUGUI Event_Text;
        public Image Event_Icon;
    }

    [Header("Event Buttons")]
    public List<Event_Button> Event_buttons = new List<Event_Button>();
    /*Списки ивентов*/
    private List<Event> Events_list = new List<Event>();

    /*Ролл первых ивентов при запуске*/
    private void Start()
    {
        Debug.Log("[Event_Buttons_Changer] Start() called");
        Roll_events();
    }

    /*Ролл ивентов и их вывод на кнопки в окне игры*/
    public void Roll_events()
    {
        Events_list.Clear();

        for (int i = 0; i < Event_buttons.Count; i++)
        {
            Event New_event = Event_Changer.Get_Event();

            if (New_event == null)
            {
                Debug.LogError($"[Roll_events] Get_Event() returned NULL for button {i}");
                continue;
            }

            Events_list.Add(New_event);
            Event_buttons[i].Event_Text.text = New_event.Event_tag;
            Event_buttons[i].Event_Icon.sprite = GetEvent_Icon(New_event);

            Debug.Log($"[Roll_events] Found event '{New_event.Event_name}' for button {i}");

            int localIndex = i; // Фиксируем индекс
            Event_buttons[i].Button_Event.onClick.RemoveAllListeners();
            Event_buttons[i].Button_Event.onClick.AddListener(delegate { OnEvent_Selected(localIndex); });

            if (i == 0)
                Event_1 = New_event;
            else if (i == 1)
                Event_2 = New_event;
            else
                Event_3 = New_event;
        }
    }

    public void Load_EventsSave(Event Event_1, Event Event_2, Event Event_3)
    {
        Events_list.Clear();

        Events_list.Add(Event_1);
        Events_list.Add(Event_2);
        Events_list.Add(Event_3);

        Event_buttons[0].Event_Text.text = Event_1.Event_tag;
        Event_buttons[0].Event_Icon.sprite = GetEvent_Icon(Event_1);
        int localIndex_0 = 0;
        Event_buttons[0].Button_Event.onClick.RemoveAllListeners();
        Event_buttons[0].Button_Event.onClick.AddListener(delegate { OnEvent_Selected(localIndex_0); });

        Event_buttons[1].Event_Text.text = Event_2.Event_tag;
        Event_buttons[1].Event_Icon.sprite = GetEvent_Icon(Event_2);
        int localIndex_1 = 1;
        Event_buttons[1].Button_Event.onClick.RemoveAllListeners();
        Event_buttons[1].Button_Event.onClick.AddListener(delegate { OnEvent_Selected(localIndex_1); });

        Event_buttons[2].Event_Text.text = Event_3.Event_tag;
        Event_buttons[2].Event_Icon.sprite = GetEvent_Icon(Event_3);
        int localIndex_2 = 2;
        Event_buttons[2].Button_Event.onClick.RemoveAllListeners();
        Event_buttons[2].Button_Event.onClick.AddListener(delegate { OnEvent_Selected(localIndex_2); });

        Debug.Log($"[Event LoaderSave] Loaded event '{Event_1.Event_name}' for button 1" +
                  $"[Event LoaderSave] Loaded event '{Event_2.Event_name}' for button 2" +
                  $"[Event LoaderSave] Loaded event '{Event_3.Event_name}' for button 3");
    }

    /*При нажатии на кнопку с ивентом*/
    private void OnEvent_Selected(int index)
    {
        Debug.Log($"Selected event: {Events_list[index].Event_name}");
        if (Text_Reference != null)
        {
            Text_Reference.StartTyping(Events_list[index].Event_description);
        }
        else
        {
            Debug.LogError("[OnEvent_Selected] textTyper is not assigned!");
        }
        if (Events_list[index].Event_tag == "combat")
        {
             EnemyLoader Loader = FindAnyObjectByType<EnemyLoader>();
             if (Loader != null)
             {
                 int Enemy_index = Loader.Search_Enemy(Events_list[index].Enemy_type);
                 Loader.SpawnEnemy(Enemy_index);
             }
            Combat_Manager.SetCurrent_EventTier(Events_list[index].Tier);
            Debug.Log($"Enemy tier: {Events_list[index].Tier}");
            Combat_Manager.StartBattle();
            Combat_Window.SetActive(true);
            Text_Reference.StartTyping(Events_list[index].Event_description, Text_Typer.Dialogue_Mode.Combat);
        }
        else if (Events_list[index].Event_tag == "trap")
        {
            Trap_Events Trap = FindAnyObjectByType<Trap_Events>();
            if (Trap != null)
            {
                Trap.SetCurrent_EventTier(Events_list[index].Tier);
                Debug.Log($"Trap tier: {Events_list[index].Tier}");
                Trap.Initiate_Trap(Events_list[index].Trap_type);
                Text_Reference.StartTyping(Events_list[index].Event_description, Text_Typer.Dialogue_Mode.Game);
                Game_Management Manager = FindAnyObjectByType<Game_Management>();
                if (Manager != null)
                    Manager.Update_UI_Stats();
            }
        }
        else if (Events_list[index].Event_tag == "encounter")
        {
            Encounters_Events Event = FindAnyObjectByType<Encounters_Events>();
            if (Event != null)
            {
                Event.Initiate_Encounter(Events_list[index].Event_name);
                switch (Event.Encounter_SelectType())
                {
                    case "combat":
                        Debug.Log("Combat encounter selected");
                        EnemyLoader enemies = FindAnyObjectByType<EnemyLoader>();
                        enemies.SpawnEnemy(enemies.Get_RandomEnemy());
                        Combat_Manager.StartBattle();
                        Combat_Window.SetActive(true);
                        Text_Reference.StartTyping(Events_list[index].Event_description, Text_Typer.Dialogue_Mode.Combat);
                        break;
                    case "trap":
                        Debug.Log("Trap encounter selected");
                        Trap_Events traps = FindAnyObjectByType<Trap_Events>();
                        traps.Initiate_Trap(traps.Get_RandomTrap());
                        break;
                    case "restore":
                        Debug.Log("Restore encounter selected");
                        Debug.Log($"{Player_Hero.hero_name} restored 20 health and 20 mana");
                        Player_Hero.Heal(20);
                        Player_Hero.Restore_mana(20);
                        Text_Reference.StartTyping($"{Player_Hero.hero_name} restored 20 health and 20 mana", Text_Typer.Dialogue_Mode.Game);
                        Manager.Update_UI_Stats();
                        RollNewFloor_Events();
                        break;
                    case "reward":
                        Debug.Log("Reward event selected");
                        Debug.Log($"{Player_Hero.hero_name} found 50 gold");
                        Player_Hero.gold += 50;
                        Text_Reference.StartTyping($"{Player_Hero.hero_name} found 50 gold", Text_Typer.Dialogue_Mode.Game);
                        Manager.Update_UI_Stats();
                        RollNewFloor_Events();
                        break;
                }
            }
        }
        else
        {
            Text_Reference.StartTyping(Events_list[index].Event_description, Text_Typer.Dialogue_Mode.Game);
        }
    }

    /*Получение изображения кнопки*/
    private Sprite GetEvent_Icon(Event Event_data)
    {
        switch (Event_data.Event_tag)
        {
            case "trap":
                return Resources.Load<Sprite>("Icons/trap_icon");
            case "combat":
                return Resources.Load<Sprite>("Icons/combat_icon");
            case "npc":
                return Resources.Load<Sprite>("Icons/npc_icon");
            case "loot":
                return Resources.Load<Sprite>("Icons/loot_icon");
            case "encounter":
                return Resources.Load<Sprite>("Icons/encounter_icon");
            case "camp":
                return Resources.Load<Sprite>("Icons/camp_icon");
            default:
                return Resources.Load<Sprite>("Icons/default_icon");
        }
    }
    /*Рерол ивентов при переходе на новый этаж*/
    public void RollNewFloor_Events()
    {
        Debug.Log("Rolling new events");
        Roll_events();
    }
}
