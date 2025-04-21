using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Unit : MonoBehaviour
{
    public string unitName;
    public int level;

    public int max_health;
    public int cur_health;

    public int attack;
    public int defense;

    public SpriteRenderer Sprite_Renderer; // Поле для отображения спрайта

    public bool Take_Damage(int damage)
    {
        cur_health -= Mathf.Max(0, damage - defense);
        if (cur_health <= 0)
        {
            Debug.Log($"{unitName} is dead!");
            return true;
        }
        else
        {
            Debug.Log($"{unitName} took {damage} damage!");
            return false;
        }
    }

    public int Calculate_Damage(int damage)
    { 
        return damage - defense;
    }

    public void Heal(int healAmount)
    {
        cur_health = Mathf.Min(cur_health + healAmount, max_health);
    }

    public void SetSprite(Sprite sprite)
    {
        if (Sprite_Renderer != null)
        {
            Sprite_Renderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Sprite wasn't set");
        }
    }
}
