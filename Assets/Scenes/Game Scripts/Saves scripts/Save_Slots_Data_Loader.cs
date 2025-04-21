using TMPro;
using UnityEngine;

public class Save_Slots_Data_Loader : MonoBehaviour
{
    public int Slot; // Номер слота
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI floorText;

    private void Awake()
    {
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        Save_Data data = Saves_Manager.Instance.Load_Data(Slot);
        
        if (data != null)
        {
            nameText.text = $""+ data.heroname;
            levelText.text = $"Level: {data.level}";
            floorText.text = $"Floor: {data.floor}";
        }
        else
        {
            nameText.text = "Empty";
            levelText.text = "Level: -";
            floorText.text = "Floor: -";
            Debug.LogWarning($"Slot {Slot} is empty");
        }
    }
}