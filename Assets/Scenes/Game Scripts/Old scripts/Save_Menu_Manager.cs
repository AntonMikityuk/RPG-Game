using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Save_Menu_Manager : MonoBehaviour
{
/*
    public Save_manager manager;
      
    public void SaveTo_Slot(int slot)
    {
        manager = FindObjectOfType<Save_manager>();
        if (manager == null)
        {
            Debug.LogError("Save_manager is null!");
            return;
        }

        manager.Save_Game(Hero.Instance, slot);
        Debug.Log($"Game saved to slot {slot}");
    }

    public void LoadFrom_Slot(int slot)
    {
        manager = FindObjectOfType<Save_manager>();
        Save_Data data = manager.Load_Game(slot);
        if (manager == null)
        {
            Debug.LogError("Save_manager is null!");
            return;
        }
        Debug.Log($"Here is data - {data.curhealth}, {data.heroname}");

        if (data != null)
        {
            Debug.Log("Save data loaded, checking Hero.Instance...");

            if (Hero.Instance != null)
            {
                Debug.Log("Hero.Instance найден, загружаем данные...");
                Hero.Instance.ApplySaveData(data);
                Debug.Log($"Hero after loading: Name={Hero.Instance.hero_name}, Level={Hero.Instance.level}, HP={Hero.Instance.cur_health}/{Hero.Instance.max_health}");
            }
            else
            {
                Debug.LogError("Hero.Instance == null! ¬озможно, Hero не был загружен.");
            }
        }
        else
        {
            Debug.Log("Save slot empty");
        }
    }
    public void LoadFrom_Menu(int slot)
    {
        manager = FindObjectOfType<Save_manager>();
        Save_Data data = manager.Load_Game(slot);
        if (manager == null)
        {
            Debug.LogError("Save_manager is null!");
            return;
        }
        Debug.Log($"Here is data - {data.curhealth}, {data.heroname}");

        if (data != null)
        {
            Debug.Log("Save data loaded, checking Hero.Instance...");

            if (Hero_Loader.Instance != null)
            {
                Debug.Log($"Hero after loading: Name={Hero_Loader.Instance.Tmp_data.heroname}, Level={Hero_Loader.Instance.Tmp_data.level}, HP={Hero_Loader.Instance.Tmp_data.curhealth}/{Hero_Loader.Instance.Tmp_data.maxhealth}");
            }
            else
            {
                Debug.LogError("Hero_Loader.Instance == null! ¬озможно, Hero_Loader не был загружен.");
            }
        }
        else
        {
            Debug.Log("Save slot empty");
        }
    }
*/
}
