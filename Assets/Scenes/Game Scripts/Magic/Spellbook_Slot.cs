using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spellbook_Slot : MonoBehaviour
{
    public TMP_Text Spell_NameText;
    public TMP_Text Spell_LevelText;
    public TMP_Text Spell_ManaCostText;
    public Image Spell_IconImage;
    public Image Spell_ManaIconImage;
    public Button Slot_Button;

    [Header ("Slot info")]
    public int SpellBook_Slot;

    [System.NonSerialized]
    public Spell Current_SpellData;

    public void Clear_Slot()
    {
        if (Spell_NameText) Spell_NameText.text = "";
        if (Spell_LevelText) Spell_LevelText.text = "";
        if (Spell_ManaCostText) Spell_ManaCostText.text = "";
        if (Spell_IconImage) Spell_IconImage.sprite = null;
        if (Spell_IconImage) Spell_IconImage.enabled = false;
        if (Spell_ManaIconImage) Spell_ManaIconImage.enabled = false;

        if (Slot_Button) Slot_Button.interactable = false;

        Current_SpellData = null;
    }

    // Метод для заполнения слота данными заклинания
    public void DisplaySpell(Spell Spell_ToDisplay)
    {
        if (Spell_ToDisplay == null)
        {
            Clear_Slot();
            return;
        }

        Current_SpellData = Spell_ToDisplay;

        if (Spell_NameText) Spell_NameText.text = Spell_ToDisplay.Spell_Name;
        if (Spell_LevelText) Spell_LevelText.text = "Lvl " + Spell_ToDisplay.Level.ToString();
        if (Spell_ManaCostText) Spell_ManaCostText.text = Spell_ToDisplay.Mana_Cost.ToString();

        if (Spell_IconImage)
        {
            // Иконки должны лежать, например, в Assets/Resources/SpellIcons/
            // Имя файла иконки (без расширения) должно быть в spellToDisplay.Icon_Name
            Sprite Icon_Sprite = Resources.Load<Sprite>("Spell_Icons/" + Spell_ToDisplay.Spell_Icon);
            if (Icon_Sprite != null)
            {
                Spell_IconImage.sprite = Icon_Sprite;
                Spell_IconImage.enabled = true;
            }
            else
            {
                Debug.LogWarning($"Icon not found: SpellIcons/{Spell_ToDisplay.Spell_Icon}");
                Spell_IconImage.enabled = false;
            }
        }

        if (Spell_ManaIconImage) Spell_ManaIconImage.enabled = true; // Показываем иконку маны, если есть заклинание
        if (Slot_Button) Slot_Button.interactable = true;   // Делаем кнопку активной
    }

}
