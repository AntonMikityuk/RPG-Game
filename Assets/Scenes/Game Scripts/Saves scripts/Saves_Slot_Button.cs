using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saves_Slot_Button : MonoBehaviour
{
    /*Персонаж игрока*/
    private Hero hero;
    [Header("Slots")]
    public int Slot;
    /*Поиск персонажа при старте скрипта*/
    private void Awake()
    {
        hero = Hero.Instance;
    }
    /*Обработчик нажатия на слот сохранения*/
    public void OnSlot_clicked()
    {
        Saves_Manager menuManager = FindAnyObjectByType<Saves_Manager>();
        if (menuManager == null)
        {
            Debug.LogError("Save_Menu_Manager не найден!");
            return;
        }

        if (Navigation_Manager.cur_mode == Navigation_Manager.Navigation_mode.Save)
        {
            menuManager.SaveTo_Slot(Slot);
        }
        else if (Navigation_Manager.cur_mode == Navigation_Manager.Navigation_mode.Load)
        {
            Save_Data data = menuManager.Load_Data(Slot); // Проверяем наличие сохранения

            if (data == null) // Если сохранения нет, прерываем выполнение
            {
                Debug.LogWarning($"Слот {Slot} пуст! Загрузка невозможна.");
                return;
            }
            if (Hero_Loader.To_Load) // Проверяем, нужно ли загрузить в Hero_Loader
            {
                Hero_Loader.LoadGameData(Slot);
                SceneManager.LoadScene("Game");
            }
            else
            {
                menuManager.LoadFrom_Slot(Slot);
                SceneManager.UnloadSceneAsync("Main menu");
            }
        }
    }
}
