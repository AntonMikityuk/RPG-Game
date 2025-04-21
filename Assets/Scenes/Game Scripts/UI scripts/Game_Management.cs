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
    /*Загрузка сцены меню*/
    public void Load_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.SetInt("First_Launch", 0);
        PlayerPrefs.SetInt("Game_Proper_Exit", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
    }
    /*Загрузка сцены главного меню и открытие окна сохранений для загрузки сохранения*/
    public void Load_Save()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Load;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*Загрузка сцены главного меню и открытие окна сохранений для сохранения игры*/
    public void Save_Game()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Save;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*Панель меню*/
    public GameObject Menu_panel;

    /*Группа Canvas для блокировки*/
    public CanvasGroup Game_UI;

    /*Блокировка игрового окна при открытии меню*/
    public void Pause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        Menu_panel.SetActive(isPaused);

        // Блокируем взаимодействие с игровым UI
        Game_UI.interactable = !isPaused;
        Game_UI.blocksRaycasts = !isPaused;
    }
    /*Поля для отображения статов персонажа в меню персонажа*/
    public TMP_Text name_text;
    public TMP_Text level_text;
    public TMP_Text strength_text;
    public TMP_Text dexterity_text;
    public TMP_Text intelligence_text;
    public TMP_Text luck_text;

    /*Поля для вывода информации в окно игры*/
    public TMP_Text health_text;
    public TMP_Text mana_text;
    public TMP_Text gold_text;
    public TMP_Text floor_text;

    /*Обновление полей*/
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
