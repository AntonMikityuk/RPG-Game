using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Management : MonoBehaviour
{
    /*
    public void Load_Game()
    {
        SceneManager.LoadScene("Game");
    }

    public GameObject Main_menu;
    public GameObject Save_menu;

    public enum SaveMenu_mode {Save, Load}
    public static SaveMenu_mode cur_mode;
    private void Start()
    {
        if (PlayerPrefs.GetInt("Game_Proper_Exit", 0) == 0)
        {
            PlayerPrefs.SetInt("First_Launch", 1);
            PlayerPrefs.SetInt("Open_Save", 0);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetInt("Game_Proper_Exit", 0);
        PlayerPrefs.Save();

        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            Main_menu.SetActive(false);
            Save_menu.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.GetInt("Open_Save", 0) == 1)
            {
                Main_menu.SetActive(false);
                Save_menu.SetActive(true);
            }
            else
            {
                Main_menu.SetActive(true);
                Save_menu.SetActive(false);
            }
        }
    }

    public void Close_SaveMenu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.Save();
        SceneManager.UnloadSceneAsync("Main Menu");
    }
    public void LoadFrom_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Menu_Management.cur_mode = Menu_Management.SaveMenu_mode.Load;
        Hero_Loader.Instance.Is_Loaded = true;
    }
    */
}
