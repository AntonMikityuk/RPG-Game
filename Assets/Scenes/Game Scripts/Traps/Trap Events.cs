using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using static Battle_System;

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

    private Event_Tier Current_Tier;

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
        Typer.StartTyping($"{Player_hero.hero_name} activated trap: {CurrentTrap_Data.trap_name}", Text_Typer.Dialogue_Mode.Game);
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
                    Typer.Clear_DialoguePanel(Text_Typer.Dialogue_Mode.Game);
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
    public void SetCurrent_EventTier(Event_Tier tier)
    {
        Current_Tier = tier;
    }
    public void Reward()
    {
        switch (Current_Tier)
        {
            case Event_Tier.Common:
                Debug.Log($"{Player_hero.hero_name} gained 10 exp");
                Player_hero.cur_exp += 10;
                if (Player_hero.cur_exp >= Player_hero.required_exp)
                    Player_hero.Level_up();
                break;
            case Event_Tier.Rare:
                Debug.Log($"{Player_hero.hero_name} gained 25 exp");
                Player_hero.cur_exp += 25;
                if (Player_hero.cur_exp >= Player_hero.required_exp)
                    Player_hero.Level_up();
                break;
            case Event_Tier.Epic:
                Debug.Log($"{Player_hero.hero_name} gained 50 exp");
                Player_hero.cur_exp += 50;
                if (Player_hero.cur_exp >= Player_hero.required_exp)
                    Player_hero.Level_up();
                break;
        }
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
    public string Get_RandomTrap()
    {
        Debug.Log("Random Trap search in progress");
        int random = Random.Range(0, Traps.Trap_List.Count);
        Debug.Log($"Found trap: name: {Traps.Trap_List[random].trap_name}");
        return Traps.Trap_List[random].trap_name;
    }
}
