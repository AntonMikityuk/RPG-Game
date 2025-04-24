using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Saves_Manager : MonoBehaviour
{
    [Header("Save Manager")]
    public static Saves_Manager Instance { get; private set; }

    /*�������� �������������� ��������� ����������*/
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
    /*��������� ���� � �����*/
    private string Get_Path(int slot)
    {
        return Application.persistentDataPath + $"save_slot_{slot}.dat";
    }
    /*���������� ������ � ����*/
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
    /*�������� ������ �� �����*/
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
                    Debug.LogError($"������ ��������: ���� {path} ��������, �� `Save_Data` == null");
                }
                else
                {
                    Debug.Log($"�������� �������: {data.heroname}, {data.level}");
                }

                return data;
            }
        }
        else
        {
            //Debug.LogWarning($"���� ���������� �����������: {path}");
            return null;
        }
    }
    /*���������� ������ � ���� ����������*/
    public void SaveTo_Slot(int slot)
    {
        Save_Data(Hero.Instance, slot);
        Debug.Log($"Game saved to slot {slot}");
    }
    /*�������� ������ �� ����� ����������*/
    public void LoadFrom_Slot(int slot)
    {
        Save_Data data = Load_Data(slot);
        if (data != null)
        {
            Debug.Log("Save data loaded, checking Hero.Instance...");

            if (Hero.Instance != null)
            {
                Debug.Log("Hero.Instance ������, ��������� ������...");
                Hero.Instance.ApplySaveData(data);
                Game_Management Stats_updater = FindAnyObjectByType<Game_Management>();
                Stats_updater.Update_UI_Stats();
                Debug.Log($"Hero after loading: Name={Hero.Instance.hero_name}, Level={Hero.Instance.level}, HP={Hero.Instance.cur_health}/{Hero.Instance.max_health}");
            }
            else
            {
                Debug.LogError("Hero.Instance == null! ��������, Hero �� ��� ��������.");
            }
        }
        else
        {
            Debug.Log("Save slot empty");
        }
    }
}
