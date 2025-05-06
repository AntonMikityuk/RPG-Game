using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Trap_Events : MonoBehaviour
{
    [Header("Text Typer")]
    public Text_Typer Typer;

    [Header("Game Manager")]
    public Game_Management Game_Manager;

    [Header("Event_System")]
    public Event_Buttons_Changer Event_Changer;

    [Header("Button")]
    public Button Continue_Button;

    [Header("Trap Loader")]
    public Trap_Loader Traps;

    private Hero Player_hero;

    private Trap_Data CurrentTrap_Data;

    private void Awake()
    {
        Player_hero = FindAnyObjectByType<Hero>();
        if (Player_hero == null)
        {
            Debug.LogError("Hero not found!");
            return;
        }
        Hide_ContinueButton();
    }

    public void Initiate_Trap(string Trap_Name)
    {
        if (Traps == null || Traps.Trap_List == null)
        {
            Debug.LogError("Trap Loader or Trap List not initialised");
            return;
        }
        int Trap_Index = Traps.Search_Trap(Trap_Name);
        if (Trap_Index == -1)
        {
            Debug.LogError("Trap not found");
            return;
        }
        CurrentTrap_Data = Traps.Trap_List[Trap_Index];
        Show_ContinueButton();
    }

    private IEnumerator Disarment()
    {
        if (CurrentTrap_Data == null)
        {
            Debug.LogError("Trap data not found");
            Hide_ContinueButton();
        }
        if (Player_hero.Check_Stat(CurrentTrap_Data.Stat_ToChek, CurrentTrap_Data.Requirement))
        {
            Debug.Log("Trap successfully disarmed");
            Typer.StartTyping($"{Player_hero.hero_name} successfully disarmed {CurrentTrap_Data.trap_name}!", Text_Typer.Dialogue_Mode.Game);
            yield return new WaitForSeconds(5f);
            Player_hero.floors++;
            Game_Manager.Update_UI_Stats();
            Event_Changer.RollNewFloor_Events();
        }
        else
        {
            if (CurrentTrap_Data.Damage > 0)
            {
                Debug.Log($"Failed to desarm trap, {Player_hero.hero_name} took {CurrentTrap_Data.Damage}");
                Player_hero.Take_TrapDamage(CurrentTrap_Data.Damage);
                Typer.StartTyping($"{Player_hero.hero_name} failed to disarm {CurrentTrap_Data.trap_name} and took {CurrentTrap_Data.Damage} damage", Text_Typer.Dialogue_Mode.Game);
                Game_Manager.Update_UI_Stats();
                yield return new WaitForSeconds(5f);
                if (Player_hero.cur_health <= 0)
                {
                    Typer.StartTyping($"You are dead!", Text_Typer.Dialogue_Mode.Game);
                    yield return new WaitForSeconds(3f);
                    Game_Manager.ShowGameOver_Panel();
                }
            }
        }
        Hide_ContinueButton();
        CurrentTrap_Data = null;
    }

    public void Disarm_Attempt()
    {
        if (CurrentTrap_Data == null)
        {
            Debug.LogError("No trap data to disarm. Initiate_Trap might not have been called correctly.");
            Hide_ContinueButton();
            return;
        }
        // Запускаем корутину
        StartCoroutine(Disarment());
        Hide_ContinueButton();
    }

    private void Hide_ContinueButton()
    {
        if (Continue_Button != null)
        {
            Continue_Button.gameObject.SetActive(false);
        }
    }
    private void Show_ContinueButton()
    {
        if (Continue_Button != null)
        {
            Continue_Button.gameObject.SetActive(true);
        }
    }
}
