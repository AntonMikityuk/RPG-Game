using UnityEngine;

public static class Hero_Loader
{
    public static Save_Data Tmp_data { get; private set; }
    public static bool Is_Loaded { get; private set; } = false;

    public static bool To_Load { get; set; } = false;

    /*Загрузка информации в буфер*/
    public static void LoadGameData(int slot)
    {
        Save_Data data = Saves_Manager.Instance.Load_Data(slot);
        if (data != null)
        {
            Tmp_data = data;
            Is_Loaded = true;
            Debug.Log("Save data stored in Hero_Loader.");
        }
        else
        {
            Debug.LogError("Failed to load save data!");
        }
    }

    /*Очистка буфера*/
    public static void Clear()
    {
        Tmp_data = null;
        Is_Loaded = false;
    }
}