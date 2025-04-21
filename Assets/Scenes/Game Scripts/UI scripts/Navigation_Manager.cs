using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation_Manager : MonoBehaviour
{
    public enum Navigation_mode { Save, Load }
    public static Navigation_mode cur_mode;
    /*�������� ����� ����*/
    public void Load_Game()
    {
        SceneManager.LoadScene("Game");
    }
    /*�������� ����� � ���� �������� ���� ����� �������� ���� ����������*/
    public void Load_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.SetInt("First_Launch", 0);
        PlayerPrefs.SetInt("Game_Proper_Exit", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Menu");
    }
    /*�������� ����� �������� ���� � ���� ���������� ��� �������� ����������*/
    public void Open_Load_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Load;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*�������� ����� �������� ���� � ���� ���������� ��� ���������� ������*/
    public void Open_Save_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Save;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);
    }
    /*�������� ���� ���������� � �������� ����� ����*/
    public void Close_SaveMenu()
    {
        PlayerPrefs.SetInt("Open_Save", 0);
        PlayerPrefs.Save();
        SceneManager.UnloadSceneAsync("Main Menu");
    }
    /*�������� ���� �������� ���������� ��� �������� �� �������� ����*/
    public void LoadFrom_Menu()
    {
        PlayerPrefs.SetInt("Open_Save", 1);
        PlayerPrefs.Save();

        Navigation_Manager.cur_mode = Navigation_Manager.Navigation_mode.Load;
        Hero_Loader.To_Load = true;
    }

}
