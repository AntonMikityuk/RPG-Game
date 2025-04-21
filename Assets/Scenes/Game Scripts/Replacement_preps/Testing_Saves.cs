using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class Testing_Saves : MonoBehaviour
{
    public static Testing_Saves Instance { get; private set; }

    private Hero hero = FindObjectOfType<Hero>();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (hero != null)
        {
            Debug.Log("hero not found");
            return;
        }
    }
    /*Получение пути к файлу*/
    private string Get_Path(int slot)
    {
        return Application.persistentDataPath + $"save_slot_{slot}.dat";
    }
    /*Сохранение данных в файл*/
    public void Save_Data(Hero hero, int slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Get_Path(slot);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            Save_Data data = new Save_Data(hero);
            formatter.Serialize(stream, data);
        }
        Debug.Log($"Save successible, slot - {slot}, path: {path}");
    }
    /*Загрузка данных из файла*/
    public Save_Data Load_Data(int slot)
    {
        string path = Get_Path(slot);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                Save_Data data = formatter.Deserialize(stream) as Save_Data;

                if (data == null)
                {
                    Debug.LogError($"Ошибка загрузки: файл {path} прочитан, но `Save_Data` == null");
                }
                else
                {
                    Debug.Log($"Загрузка успешна: {data.heroname}, {data.level}");
                }

                return data;
            }
        }
        else
        {
            Debug.LogError($"Файл сохранения отсутствует: {path}");
            return null;
        }
    }
    /*Сохранение данных в слот сохранения*/
    public void SaveTo_Slot(Hero hero, int slot)
    {
        Save_Data(hero, slot);
        Debug.Log($"Game saved to slot {slot}");
    }
    /*Загрузка данных из слота сохранения*/
    public void LoadFrom_Slot(Hero hero, int slot)
    {
        Save_Data data = Load_Data(slot);
        if (data != null)
        {
            Debug.Log("Save data loaded, checking Hero.Instance...");

            if (hero != null)
            {
                Debug.Log("Hero.Instance найден, загружаем данные...");
                hero.ApplySaveData(data);
                Debug.Log($"Hero after loading: Name={hero.hero_name}, Level={hero.level}, HP={hero.cur_health}/{hero.max_health}");
            }
            else
            {
                Debug.LogError("Hero.Instance == null! Возможно, Hero не был загружен.");
            }
        }
        else
        {
            Debug.Log("Save slot empty");
        }
    }
}
