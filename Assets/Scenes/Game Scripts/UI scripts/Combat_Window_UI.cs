using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Combat_Window_UI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject Actions_Panel;
    public GameObject Dialogue_Panel;
    public GameObject Heroes_Panel;
    public GameObject Enemies_Panel;

    [Header("Buttons")]
    public GameObject Hide_Button;

    [Header("Hero UI")]
    public TMP_Text HeroName_Text;
    public TMP_Text HeroAttack_Value;
    public TMP_Text HeroDefense_Value;
    public TMP_Text HeroHealth_Value;
    public TMP_Text HeroMana_Value;
    public Slider HeroHealth_Slider;
    public Slider HeroMana_Slider;

    [Header("Enemy UI")]
    public TMP_Text EnemyName_Text;
    public TMP_Text EnemyAttack_Value;
    public TMP_Text EnemyDefense_Value;
    public TMP_Text EnemyHealth_Value;
    public TMP_Text DialoguePanel_Text;
    public Slider EnemyHealth_Slider;

    /*Инициализация показателей героя и противника*/
    public void InitializeUI(Hero hero, Enemy_Unit enemy)
    {
        if (hero == null || enemy == null)
        {
            Debug.LogError("Ошибка: Не найден герой или противник для обновления UI!");
            return;
        }

        UpdateHeroUI(hero);
        UpdateEnemyUI(enemy);
    }
    /*Обновления показателей героя*/
    public void UpdateHeroUI(Hero hero)
    {
        HeroName_Text.text = hero.hero_name;
        HeroHealth_Slider.maxValue = hero.max_health;
        HeroHealth_Slider.value = hero.cur_health;
        HeroHealth_Value.text = $"{hero.cur_health}/{hero.max_health}";

        HeroMana_Slider.maxValue = hero.max_mana;
        HeroMana_Slider.value = hero.cur_mana;
        HeroMana_Value.text = $"{hero.cur_mana}/{hero.max_mana}";

        HeroAttack_Value.text = hero.attack.ToString();
        HeroDefense_Value.text = hero.defense.ToString();
    }
    /*Обновления показателей противника*/
    public void UpdateEnemyUI(Enemy_Unit enemy)
    {
        EnemyHealth_Slider.maxValue = enemy.max_health;
        EnemyHealth_Slider.value = enemy.cur_health;
        EnemyHealth_Value.text = $"{enemy.cur_health}/{enemy.max_health}";

        EnemyAttack_Value.text = enemy.attack.ToString();
        EnemyDefense_Value.text = enemy.defense.ToString();
    }
    /*Показать панель действий*/
    public void Show_Actions_Panel()
    {
        Actions_Panel.SetActive(true);
    }
    /*Скрыть панель действий*/
    public void Hide_Actions_Panel()
    {
        Actions_Panel.SetActive(false);
    }
    /*Показать панель героев*/
    public void Show_Heroes_Panel()
    {
        Heroes_Panel.SetActive(true);
    }
    /*Скрыть панель героев*/
    public void Hide_Heroes_Panel()
    {
        Heroes_Panel.SetActive(false);
    }
    /*Показать панель врагов*/
    public void Show_Enemies_Panel()
    {
        Enemies_Panel.SetActive(true);
    }
    /*Скрыть панель врагов*/
    public void Hide_Enemies_Panel()
    {
        Enemies_Panel.SetActive(false);
    }
    /*Скрытие диалоговой панели при нажатии в окне битвы*/
    public void OnStartButtonClick()
    {
        FindAnyObjectByType<Battle_System>().OnBattleStartClick();
        Hide_DialoguePanel(); // Прячем диалог
    }
    /*Показать диалоговую панель*/
    public void Show_DialoguePanel()
    {
        Dialogue_Panel.SetActive(true); // Скрываем диалог
    }
    /*Скрыть диалоговую панель*/
    public void Hide_DialoguePanel()
    {
        Dialogue_Panel.SetActive(false); // Скрываем диалог
    }
    /*Очистить диалоговую панель*/
    public void Clear_DialoguePanel()
    {
        DialoguePanel_Text.text = "";
    }
    /*Активировать кнопку скрытия диалоговой панели*/
    public void Activate_HideButton()
    {
        Hide_Button.SetActive(true);
    }
    /*Отключить кнопку скрытия диалоговой панели*/
    public void Deactivate_HideButton()
    {
        Hide_Button.SetActive(false);
    }
    /*Скрытие панелей при завершении хода и открытие диалоговой панели*/
    public void End_Turn()
    {
        Show_DialoguePanel();
        Hide_Enemies_Panel();
        Hide_Actions_Panel();
        Hide_Heroes_Panel();
    }
}