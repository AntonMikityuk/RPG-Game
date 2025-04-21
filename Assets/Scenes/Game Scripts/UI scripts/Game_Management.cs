using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Game_Management : MonoBehaviour
{
    public Hero Player_hero;

    void Start()
    {
        if (Player_hero != null)
        {
            Update_UI_Stats();
        }
        else
        {
            Debug.LogError("Player_hero is not assigned!");
        }
    }
    /*�������� ����� ����*/
    public void Load_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.SetInt("First_Launch", 0);
        PlayerPrefs.SetInt("Game_Proper_Exit", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
    }
    /*�������� ����� �������� ���� � �������� ���� ���������� ��� �������� ����������*/
    public void Load_Save()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Load;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*�������� ����� �������� ���� � �������� ���� ���������� ��� ���������� ����*/
    public void Save_Game()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Save;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*������ ����*/
    public GameObject Menu_panel;

    /*������ Canvas ��� ����������*/
    public CanvasGroup Game_UI;

    /*���������� �������� ���� ��� �������� ����*/
    public void Pause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        Menu_panel.SetActive(isPaused);

        // ��������� �������������� � ������� UI
        Game_UI.interactable = !isPaused;
        Game_UI.blocksRaycasts = !isPaused;
    }
    /*���� ��� ����������� ������ ��������� � ���� ���������*/
    public TMP_Text name_text;
    public TMP_Text level_text;
    public TMP_Text strength_text;
    public TMP_Text dexterity_text;
    public TMP_Text intelligence_text;
    public TMP_Text luck_text;

    /*���� ��� ������ ���������� � ���� ����*/
    public TMP_Text health_text;
    public TMP_Text mana_text;
    public TMP_Text gold_text;
    public TMP_Text floor_text;

    /*���������� �����*/
    public void Update_UI_Stats()
    {
        name_text.text = "" + Player_hero.hero_name;
        level_text.text = "" + Player_hero.level;
        strength_text.text = "" + Player_hero.strength;
        dexterity_text.text = "" + Player_hero.dexterity;
        intelligence_text.text = "" + Player_hero.intelligence;
        luck_text.text = "" + Player_hero.luck;

        health_text.text = "" + Player_hero.cur_health + "/" + Player_hero.max_health;
        mana_text.text = "" + Player_hero.cur_mana + "/" + Player_hero.max_mana;
        gold_text.text = "" + Player_hero.gold;
        floor_text.text = "Floor: " + Player_hero.floors;
    }
}
