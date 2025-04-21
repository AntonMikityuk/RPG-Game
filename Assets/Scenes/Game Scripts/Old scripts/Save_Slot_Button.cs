using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Save_Slot_Button : MonoBehaviour
{
   /*
    private Hero hero;
    public int Slot;
    private void Awake()
    {
        hero = Hero.Instance;
    }
    public void OnSlot_clicked()
    {
        Save_Menu_Manager menuManager = FindObjectOfType<Save_Menu_Manager>();
        if (menuManager == null)
        {
            Debug.LogError("Save_Menu_Manager не найден!");
            return;
        }

        if (Menu_Management.cur_mode == Menu_Management.SaveMenu_mode.Save)
        {
            menuManager.SaveTo_Slot(Slot);
        }
        else if (Menu_Management.cur_mode == Menu_Management.SaveMenu_mode.Load)
        {
            menuManager.LoadFrom_Slot(Slot);
        }
        else if (Menu_Management.cur_mode == Menu_Management.SaveMenu_mode.Load && Hero_Loader.Instance.Is_Loaded == true)
        {
            Save_Menu_Manager Load_manager = FindObjectOfType<Save_Menu_Manager>();
            Load_manager.LoadFrom_Menu(Slot);
           // SceneManager.LoadScene("Game");
        }

        SceneManager.UnloadSceneAsync("Main Menu");
    }
   */
}
