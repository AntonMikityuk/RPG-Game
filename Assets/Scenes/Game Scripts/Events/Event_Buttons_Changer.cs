using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Event_Buttons_Changer : MonoBehaviour
{
    public Event_System Event_Changer;
    public Text_Typer Text_Reference;
    public GameObject Combat_Window;

    public Battle_System Combat_Manager;

    [System.Serializable]
    public class Event_Button
    {
        public Button Button_Event;
        public TextMeshProUGUI Event_Text;
        public Image Event_Icon;
    }

    public List<Event_Button> Event_buttons = new List<Event_Button>();

    private List<Event> Events_list = new List<Event>();

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
        }
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
             EnemyLoader Loader = FindObjectOfType<EnemyLoader>();
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
}
