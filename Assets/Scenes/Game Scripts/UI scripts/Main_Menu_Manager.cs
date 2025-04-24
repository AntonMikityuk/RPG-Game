using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_Manager : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject Main_menu;
    [Header("Saves Menu")]
    public GameObject Save_menu;
    /*Для открытия правильного окна (главного меню) при запуске игры*/
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
}
