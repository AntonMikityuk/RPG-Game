using System;
using UnityEngine;

[Serializable]
public class Save_Data
{
    public string heroname;
    public int level;
    public int cur_exp;
    public int required_exp;

    public int maxhealth;
    public int curhealth;
    public int maxmana;
    public int curmana;

    public int attack;
    public int defence;

    public int str;
    public int Int;
    public int dex;
    public int luck;

    public int gold;
    public int floor;

    public string Event_1ID;
    public string Event_2ID;
    public string Event_3ID;

    public Save_Data(Hero hero, Event_Buttons_Changer changer)
    {
        heroname = hero.hero_name;
        level = hero.level;
        cur_exp = hero.cur_exp;
        required_exp = hero.required_exp;

        maxhealth = hero.max_health;
        curhealth = hero.cur_health;
        maxmana = hero.max_mana;
        curmana = hero.cur_mana;

        attack = hero.attack;
        defence = hero.defense;

        str = hero.strength;
        Int = hero.intelligence;
        dex = hero.dexterity;
        luck = hero.luck;

        gold = hero.gold;
        floor = hero.floors;

        Event_1ID = changer.Event_1.Event_name;
        Event_2ID = changer.Event_2.Event_name;
        Event_3ID = changer.Event_3.Event_name;
    }
}
