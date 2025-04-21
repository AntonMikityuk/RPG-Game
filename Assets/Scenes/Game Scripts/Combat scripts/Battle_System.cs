using System.Collections;
using UnityEngine;
using static Battle_System;

public class Battle_System : MonoBehaviour
{
    private Hero Player_Hero;
    private Enemy_Unit Enemy;
    public GameObject Combat_Window;
    public Combat_Window_UI CombatUI;
    public Text_Typer CombatWindow_Typer;
    
    public enum Battle_States { WAIT_FOR_CLICK, PLAYER_TURN, SELECT_ACTION, SELECT_HERO, SELECT_ENEMY, ENEMY_TURN, WIN, LOSS }
    public Battle_States State;
    
    private bool battleStartClicked = false;
    private string selectedAction;

    public enum Event_Tier {Common, Rare, Epic, Special }
    private Event_Tier Current_Tier;
    public void SetCurrent_EventTier(Event_Tier tier)
    {
        Current_Tier = tier;
    }

    public void StartBattle()
    {
        Player_Hero = FindObjectOfType<Hero>();
        Enemy = FindObjectOfType<Enemy_Unit>();

        if (Player_Hero == null || Enemy == null)
        {
            Debug.LogError("Ошибка: Не найден герой или противник!");
            return;
        }

        StartCoroutine(Setup_Battle());
    }

    private IEnumerator Setup_Battle()
    {
        Debug.Log($"Battle is starting! Enemy: {Enemy.unitName}");
        State = Battle_States.WAIT_FOR_CLICK;
        battleStartClicked = false;

        CombatUI.Activate_HideButton();
        yield return new WaitUntil(() => battleStartClicked);

        CombatUI.Deactivate_HideButton();
        CombatUI.InitializeUI(Player_Hero, Enemy);
        State = Battle_States.PLAYER_TURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        Debug.Log("Player turn, choose action");
        State = Battle_States.SELECT_ACTION;
        CombatUI.Show_Actions_Panel();
        CombatUI.Show_Heroes_Panel();
    }

    public void OnPlayerChooseAction(string action)
    {
        if (State != Battle_States.SELECT_ACTION) return;

        selectedAction = action;
        Debug.Log($"Player selected action: {action}");
        
        State = Battle_States.SELECT_HERO;
    }

    public void OnHeroSelected()
    {
        if (State != Battle_States.SELECT_HERO) return;
        
        Debug.Log("Hero selected");
        if (selectedAction == "Attack")
        {
            State = Battle_States.SELECT_ENEMY;
            CombatUI.Hide_Heroes_Panel();
            CombatUI.Show_Enemies_Panel();
        }
        else if (selectedAction == "Heal" || selectedAction == "RestoreMana")
        {
            CombatUI.Hide_Actions_Panel();
            CombatUI.Hide_Heroes_Panel();
            StartCoroutine(ExecutePlayerAction());
        }
        else
        {
            State = Battle_States.SELECT_ENEMY;
            CombatUI.Hide_Heroes_Panel();
            CombatUI.Show_Enemies_Panel();
        }
    }

    public void OnEnemySelected()
    {
        if (State != Battle_States.SELECT_ENEMY) return;

        Debug.Log("Enemy selected");
        CombatUI.Hide_Actions_Panel();
        CombatUI.Hide_Enemies_Panel();

        StartCoroutine(ExecutePlayerAction());
    }

    private IEnumerator ExecutePlayerAction()
    {
        CombatUI.Clear_DialoguePanel();
        CombatUI.Show_DialoguePanel();
       // yield return new WaitForSeconds(1f);
        if (selectedAction == "Attack")
        {
            yield return StartCoroutine(Player_Attack());
        }
        else if (selectedAction == "Heal")
        {
            yield return StartCoroutine(Player_Heal());
        }
        else if (selectedAction == "RestoreMana")
        {
            yield return StartCoroutine(Player_RestoreMana());
        }
    }

    private IEnumerator Player_Attack()
    {
        Debug.Log("Player attacks!");
        bool isDead = Enemy.Take_Damage(Player_Hero.attack);
        int Enemy_DamageTaken = Enemy.Calculate_Damage(Player_Hero.attack); 
        CombatUI.UpdateEnemyUI(Enemy);
        CombatWindow_Typer.StartTyping($"{Player_Hero.hero_name} attacks! {Enemy.unitName} took {Enemy_DamageTaken} damage!", Text_Typer.Dialogue_Mode.Combat);
        yield return new WaitForSeconds(5f);
        if (isDead)
        {
            State = Battle_States.WIN;
            StartCoroutine(EndBattle());
        }
        else
        {
            State = Battle_States.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator Player_Heal()
    {
        Debug.Log("Player heals");
        int Heal_amount = Player_Hero.Calculate_Heal(Player_Hero.intelligence);
        Player_Hero.Heal(Heal_amount);
        CombatWindow_Typer.StartTyping($"{Player_Hero.hero_name} restores {Heal_amount} hp.", Text_Typer.Dialogue_Mode.Combat);
        CombatUI.UpdateHeroUI(Player_Hero);
        yield return new WaitForSeconds(5f);
        State = Battle_States.ENEMY_TURN;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator Player_RestoreMana()
    {
        Debug.Log("Player restores mana");
        int Mana_amount = Player_Hero.Calculate_ManaRestoretaion(Player_Hero.intelligence);
        Player_Hero.Restore_mana(Mana_amount);
        CombatWindow_Typer.StartTyping($"{Player_Hero.hero_name} restores {Mana_amount} mana.", Text_Typer.Dialogue_Mode.Combat);
        CombatUI.UpdateHeroUI(Player_Hero);
        yield return new WaitForSeconds(5f);
        State = Battle_States.ENEMY_TURN;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy turn...");
      //  yield return new WaitForSeconds(1f);
        CombatUI.Clear_DialoguePanel();
        int Player_DamageTaken = Player_Hero.Calculate_Damage(Enemy.attack);
        Player_Hero.Take_Damage(Enemy.attack);
        CombatWindow_Typer.StartTyping($"{Enemy.unitName} attacks! {Player_Hero.hero_name} took {Player_DamageTaken} damage!", Text_Typer.Dialogue_Mode.Combat);
        CombatUI.UpdateHeroUI(Player_Hero);
        yield return new WaitForSeconds(5f);

        if (Player_Hero.cur_health <= 0)
        {
            State = Battle_States.LOSS;
            StartCoroutine(EndBattle());
        }
        else
        {
            State = Battle_States.PLAYER_TURN;
            PlayerTurn();
        }
    }

    private IEnumerator EndBattle()
    {
        if (State == Battle_States.WIN)
        {
            Debug.Log("You won!");
            CombatUI.Clear_DialoguePanel();
            //CombatUI.Show_DialoguePanel();
            //CombatUI.Hide_Actions_Panel();
            CombatWindow_Typer.StartTyping($"You won!", Text_Typer.Dialogue_Mode.Combat);
            yield return new WaitForSeconds(5f);
            if (Enemy != null)
                Destroy(Enemy.gameObject);
            Game_Management manager = FindObjectOfType <Game_Management>();
            Reward();
            manager.Update_UI_Stats();
            CombatWindow_Typer.GameWindowDialoguePanel_text.text = "";
        }
        else
        {
            Debug.Log("You lost!");
            CombatUI.Clear_DialoguePanel();
            CombatWindow_Typer.StartTyping($"You lost!", Text_Typer.Dialogue_Mode.Combat);
            yield return new WaitForSeconds(5f);
        }
        Combat_Window.SetActive(false);
    }

    public void OnBattleStartClick()
    {
        if (State == Battle_States.WAIT_FOR_CLICK)
        {
            battleStartClicked = true;
        }
    }

    public void Reward()
    {
        switch (Current_Tier)
        {
            case Event_Tier.Common:
                Debug.Log("Common tier reward: 10 gold");
                Player_Hero.gold += 10;
                break;
            case Event_Tier.Rare:
                Debug.Log("Rare tier reward: 30 gold");
                Player_Hero.gold += 30;
                break;
            case Event_Tier.Epic:
                Debug.Log("Epic tier reward: 50 gold");
                Player_Hero.gold += 50;
                break;
        }
    }
}
