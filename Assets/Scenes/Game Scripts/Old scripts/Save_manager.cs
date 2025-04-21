using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Save_manager : MonoBehaviour
{
    public static Save_manager Instance { get; private set; }

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
    }

    private string Get_Path(int slot)
    {
        return Application.persistentDataPath + $"save_slot_{slot}.dat";
    }

    public void Save_Game(Hero hero, int slot)
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

    public Save_Data Load_Game(int slot)
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
}
