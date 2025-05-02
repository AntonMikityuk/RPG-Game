using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//
[System.Serializable]
public class Hero : MonoBehaviour
{
   /*Персонаж*/
    public static Hero Instance { get; private set; }
    /*Создание персонажа при запуске скрипта*/
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Duplicate Hero detected, destroying!");
            Destroy(gameObject);
        }
        if (Hero_Loader.Is_Loaded)
        {
            ApplySaveData(Hero_Loader.Tmp_data);
            Hero_Loader.Clear();
        }
    }

    [Header("Hero Name")]
    public string hero_name = "Adventurer";

    [Header("Exp and Level")]
    public int level;
    public int cur_exp;
    public int required_exp;

    [Header("HP and Mana")]
    public int max_health;
    public int cur_health;
    public int max_mana;
    public int cur_mana;

    [Header("Attack and Defense")]
    public int attack;
    public int defense;

    [Header("Stats")]
    public int strength;
    public int dexterity;
    public int intelligence;
    public int luck;

    [Header("Gold and Floors")]
    public int gold;
    public int floors;

    /*Получение урона*/
    public void Take_Damage(int damage)
    {
        cur_health -= Mathf.Max(0, damage - defense);
        if (cur_health <= 0)
        {
            Debug.Log("You are dead!");
        }
        else
            Debug.Log($"You have taken {damage} damage from enemy hit");
    }

    public void Take_TrapDamage(int damage)
    {
        cur_health -= Mathf.Max(0, damage);
        if (cur_health <= 0)
        {
            Debug.Log("You are dead!");
        }
        else
            Debug.Log($"You have taken {damage} damage from enemy hit");
    }

    /*Лечение*/
    public void Heal(int heal_amount)
    {
        cur_health = Mathf.Min(cur_health + heal_amount, max_health);
        Debug.Log($"You have healed {heal_amount} hp");
    }

    /*Восстановление маны*/
    public void Restore_mana(int mana_amount)
    {
        cur_mana += mana_amount;
        if (cur_mana > max_mana)
            cur_mana = max_mana;
        Debug.Log($"You have restored {mana_amount} mana");
    }

    /*Подсчет получаемого урона*/
    public int Calculate_Damage(int damage)
    {
        return damage - defense;
    }
    /*Подсчет получаемого лечения*/
    public int Calculate_Heal(int intelligence)
    {
        return intelligence * 5;
    }
    /*Подсчет восстанавливаемой маны*/
    public int Calculate_ManaRestoretaion(int intelligence)
    {
        return intelligence * 3;
    }

    /*Повышение уровня*/
    public void Level_up()
    {
        while (cur_exp >= required_exp)
        {
            level++;
            cur_exp -= required_exp;
            required_exp += 30;
            Increase_strength(2);
            Increase_dexterity(1);
            Increase_intelligence(3);
            Increase_luck(5);
            Update_stats();
            Debug.Log("Level up!");
        }

        Game_Management manager = FindAnyObjectByType<Game_Management>();
        if (manager != null)
        { 
            manager.Update_UI_Stats();
        }
        else
        {
            Debug.Log("No UI Manager found!");
        }
    }

    /*Обновление характеристик*/
    public void Update_stats()
    {
        // Пропорциональное обновление здоровья
        max_health = strength * 10;
        float healthRatio = (float)cur_health / max_health;
        cur_health = Mathf.RoundToInt(max_health * healthRatio);

        // Пропорциональное обновление маны
        max_mana = intelligence * 5;
        float manaRatio = (float)cur_mana / max_mana;
        cur_mana = Mathf.RoundToInt(max_mana * manaRatio);

        // Обновление атаки и защиты
        attack = strength * 2;
        defense = dexterity * 2;
    }

    /*Повышение силы*/
    public void Increase_strength(int str)
    {
        strength += str;
    }

    /*Повышение ловкости*/
    public void Increase_dexterity(int dex)
    {
        dexterity += dex;
    }

    /*Повышение интеллекта*/
    public void Increase_intelligence(int intel)
    {
        intelligence += intel;
    }

    /*Повышение удачи*/
    public void Increase_luck(int lck)
    {
        luck += lck;
    }

    /*Применение данных из сохранения*/
    public void ApplySaveData(Save_Data data)
    {
        Debug.Log($"Applying save data: Name={data.heroname}, Level={data.level}, HP={data.curhealth}/{data.maxhealth}");

        hero_name = data.heroname;
        level = data.level;
        cur_exp = data.cur_exp;
        required_exp = data.required_exp;

        max_health = data.maxhealth;
        cur_health = data.curhealth;
        max_mana = data.maxmana;
        cur_mana = data.curmana;

        attack = data.attack;
        defense = data.defence;

        strength = data.str;
        intelligence = data.Int;
        dexterity = data.dex;
        luck = data.luck;

        gold = data.gold;
        floors = data.floor;

        Debug.Log($"Hero updated: Name={hero_name}, Level={level}, HP={cur_health}/{max_health}");
    }

    public bool Check_Stat(string Stat, int Requirement)
    {
        int Stat_Value;

        switch (Stat)
        {
            case "strength":
                Stat_Value = this.strength;
                break;
            case "dexterity":
                Stat_Value = this.dexterity;
                break;
            case "intelligence":
                Stat_Value = this.intelligence;
                break;
            case "luck":
                Stat_Value = this.luck;
                break;
            case "attack":
                Stat_Value = this.attack;
                break;
            case "defense":
                Stat_Value = this.defense;
                break;
            default:
                Debug.LogWarning($"[Hero.CheckStat] Unidentified stat: '{Stat}'");
                return false;
        }


        bool Pass = Stat_Value >= Requirement;

        Debug.Log($"[Hero.CheckStat] Check: {Stat} ({Stat_Value}) > {Requirement} = {Pass}");
        return Pass;
    }
}
