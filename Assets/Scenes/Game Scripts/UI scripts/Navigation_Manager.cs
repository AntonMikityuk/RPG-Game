using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation_Manager : MonoBehaviour
{
    public enum Navigation_mode { Save, Load }
    public static Navigation_mode cur_mode;
    /*Загрузка сцены игры*/
    public void Load_Game()
    {
        SceneManager.LoadScene("Game");
    }
    /*Загрузка сцены и окна главного меню после закрытия меню сохранений*/
    public void Load_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.SetInt("First_Launch", 0);
        PlayerPrefs.SetInt("Game_Proper_Exit", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
    }
    /*Открытие сцены главного меню и окна сохранений для загрузки сохранения*/
    public void Open_Load_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Load;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*Открытие сцены главного меню и меню сохранений для сохранения данных*/
    public void Open_Save_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Save;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*Закрытие меню сохранений и закрытие сцены меню*/
    public void Close_SaveMenu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.Save();
        SceneManager.UnloadSceneAsync("Main Menu");
    }
    /*Открытие окна загрузки сохранений для загрузки из главного меню*/
    public void LoadFrom_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Load;
        Hero_Loader.To_Load = true;
    }

}
