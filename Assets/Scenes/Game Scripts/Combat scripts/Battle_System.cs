using System.Collections;
using UnityEngine;
using static Battle_System;

public class Battle_System : MonoBehaviour
{
    /*Персонаж игрока*/
    private Hero Player_Hero;
    /*Противник*/
    private Enemy_Unit Enemy;
    [Header("Combat Window")]
    public GameObject Combat_Window;
    [Header("Combat UI Manager")]
    public Combat_Window_UI CombatUI;
    [Header("Combat Text Typer")]
    public Text_Typer CombatWindow_Typer;
    /*Состояния битвы*/
    public enum Battle_States { WAIT_FOR_CLICK, PLAYER_TURN, SELECT_ACTION, SELECT_HERO, SELECT_ENEMY, ENEMY_TURN, WIN, LOSS }
    /*Текущее состояние битвы*/
    public Battle_States State;
    /*Переменная для отключения кнопки скрытия диалоговой панели*/
    private bool battleStartClicked = false;
    /*Переменная для записи выбранного действия*/
    private string selectedAction;
    /*Тиры противника*/
    public enum Event_Tier {Common, Rare, Epic, Special }
    /*Текущий тир противника*/
    private Event_Tier Current_Tier;
    /*Установка текущего тира противника*/
    public void SetCurrent_EventTier(Event_Tier tier)
    {
        Current_Tier = tier;
    }
    /*Начало битвы*/
    public void StartBattle()
    {
        Player_Hero = FindAnyObjectByType<Hero>();
        Enemy = FindAnyObjectByType<Enemy_Unit>();

        if (Player_Hero == null || Enemy == null)
        {
            Debug.LogError("Ошибка: Не найден герой или противник!");
            return;
        }

        StartCoroutine(Setup_Battle());
    }
    /*Инициализация битвы*/
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
    /*Ход игрока*/
    private void PlayerTurn()
    {
        Debug.Log("Player turn, choose action");
        State = Battle_States.SELECT_ACTION;
        CombatUI.Show_Actions_Panel();
        CombatUI.Show_Heroes_Panel();
    }
    /*После выбора действия*/
    public void OnPlayerChooseAction(string action)
    {
        if (State != Battle_States.SELECT_ACTION) return;

        selectedAction = action;
        Debug.Log($"Player selected action: {action}");
        
        State = Battle_States.SELECT_HERO;
    }
    /*После выбора героя*/
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
    /*После выбора противника*/
    public void OnEnemySelected()
    {
        if (State != Battle_States.SELECT_ENEMY) return;

        Debug.Log("Enemy selected");
        CombatUI.Hide_Actions_Panel();
        CombatUI.Hide_Enemies_Panel();

        StartCoroutine(ExecutePlayerAction());
    }
    /*Завершение хода игрока*/
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
    /*Атака игрока*/
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
    /*Лечение*/
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
    /*Восстановление маны*/
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
    /*Ход противника*/
    private IEnumerator EnemyTurn()
    {
        CombatUI.Clear_DialoguePanel();

        Debug.Log("Enemy turn...");
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
    /*Завершение битвы*/
    private IEnumerator EndBattle()
    {
        if (State == Battle_States.WIN)
        {
            CombatUI.Clear_DialoguePanel();
            Debug.Log("You won!");

            CombatWindow_Typer.StartTyping($"You won!", Text_Typer.Dialogue_Mode.Combat);
            yield return new WaitForSeconds(5f);

            if (Enemy != null)
                Destroy(Enemy.gameObject);

            Game_Management manager = FindAnyObjectByType <Game_Management>();
            Reward();
            Player_Hero.floors++;

            Event_Buttons_Changer Event_changer = FindAnyObjectByType<Event_Buttons_Changer>();
            Event_changer.RollNewFloor_Events();
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
    /*При первом клике в окне битвы*/
    public void OnBattleStartClick()
    {
        if (State == Battle_States.WAIT_FOR_CLICK)
        {
            battleStartClicked = true;
        }
    }
    /*Выдача награды за победу в битве*/
    public void Reward()
    {
        switch (Current_Tier)
        {
            case Event_Tier.Common:
                Debug.Log("Common tier reward: 10 gold, 20 exp");
                Player_Hero.gold += 10;
                Player_Hero.cur_exp += 20;
                if (Player_Hero.cur_exp >= Player_Hero.required_exp)
                    Player_Hero.Level_up();
                break;
            case Event_Tier.Rare:
                Debug.Log("Rare tier reward: 30 gold, 50 exp");
                Player_Hero.gold += 30;
                Player_Hero.cur_exp += 50;
                if (Player_Hero.cur_exp >= Player_Hero.required_exp)
                    Player_Hero.Level_up();
                break;
            case Event_Tier.Epic:
                Debug.Log("Epic tier reward: 50 gold, 100 exp");
                Player_Hero.gold += 50;
                Player_Hero.cur_exp += 100;
                if (Player_Hero.cur_exp >= Player_Hero.required_exp)
                    Player_Hero.Level_up();
                break;
        }
    }
}
