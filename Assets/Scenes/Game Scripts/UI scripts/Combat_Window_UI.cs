using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Combat_Window_UI : MonoBehaviour
{
    public GameObject Actions_Panel, Dialogue_Panel, Heroes_Panel, Enemies_Panel;
    public GameObject Hide_Button;
    public TMP_Text HeroName_Text, HeroAttack_Value, HeroDefense_Value, HeroHealth_Value, HeroMana_Value;
    public Slider HeroHealth_Slider, HeroMana_Slider;

    public TMP_Text EnemyName_Text, EnemyAttack_Value, EnemyDefense_Value, EnemyHealth_Value, DialoguePanel_Text;
    public Slider EnemyHealth_Slider;


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

    public void UpdateHeroUI(Hero hero)
    {
        HeroHealth_Slider.maxValue = hero.max_health;
        HeroHealth_Slider.value = hero.cur_health;
        HeroHealth_Value.text = $"{hero.cur_health}/{hero.max_health}";

        HeroMana_Slider.maxValue = hero.max_mana;
        HeroMana_Slider.value = hero.cur_mana;
        HeroMana_Value.text = $"{hero.cur_mana}/{hero.max_mana}";

        HeroAttack_Value.text = hero.attack.ToString();
        HeroDefense_Value.text = hero.defense.ToString();
    }

    public void UpdateEnemyUI(Enemy_Unit enemy)
    {
        EnemyHealth_Slider.maxValue = enemy.max_health;
        EnemyHealth_Slider.value = enemy.cur_health;
        EnemyHealth_Value.text = $"{enemy.cur_health}/{enemy.max_health}";

        EnemyAttack_Value.text = enemy.attack.ToString();
        EnemyDefense_Value.text = enemy.defense.ToString();
    }

    public void Show_Actions_Panel()
    {
        Actions_Panel.SetActive(true);
    }
    public void Hide_Actions_Panel()
    {
        Actions_Panel.SetActive(false);
    }

    public void Show_Heroes_Panel()
    {
        Heroes_Panel.SetActive(true);
    }
    public void Hide_Heroes_Panel()
    {
        Heroes_Panel.SetActive(false);
    }

    public void Show_Enemies_Panel()
    {
        Enemies_Panel.SetActive(true);
    }
    public void Hide_Enemies_Panel()
    {
        Enemies_Panel.SetActive(false);
    }
    public void OnStartButtonClick()
    {
        FindObjectOfType<Battle_System>().OnBattleStartClick();
        Hide_DialoguePanel(); // Прячем диалог
    }

    public void Show_DialoguePanel()
    {
        Dialogue_Panel.SetActive(true); // Скрываем диалог
    }

    public void Hide_DialoguePanel()
    {
        Dialogue_Panel.SetActive(false); // Скрываем диалог
    }

    public void Clear_DialoguePanel()
    {
        DialoguePanel_Text.text = "";
    }

    public void Activate_HideButton()
    {
        Hide_Button.SetActive(true);
    }

    public void Deactivate_HideButton()
    {
        Hide_Button.SetActive(false);
    }

    public void End_Turn()
    {
        Show_DialoguePanel();
        Hide_Enemies_Panel();
        Hide_Actions_Panel();
        Hide_Heroes_Panel();
    }
}