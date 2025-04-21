using System;
using UnityEngine;

[Serializable]
public class Save_Data
{
    public string heroname;
    public int level;

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

    public Save_Data(Hero hero)
    {
        heroname = hero.hero_name;
        level = hero.level;

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
    }
}
