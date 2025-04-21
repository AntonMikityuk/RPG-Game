using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Text_Typer;

public class Text_Typer : MonoBehaviour
{
    public TMP_Text GameWindowDialoguePanel_text;
    public TMP_Text CombatWindowDialoguePanel_text;
    public float Text_speed = 0.06f;

    private Coroutine Text_typer;

    public enum Dialogue_Mode {Game, Combat}

    // Метод для запуска плавного вывода текста
    public void StartTyping(string text, Dialogue_Mode mode = Dialogue_Mode.Game)
    {
        if (Text_typer != null)
        {
            StopCoroutine(Text_typer);
        }

        Text_typer = StartCoroutine(Type_Text(text, mode));
    }

    private IEnumerator Type_Text(string text, Dialogue_Mode mode)
    {
        TMP_Text targetText = mode == Dialogue_Mode.Game ? GameWindowDialoguePanel_text : CombatWindowDialoguePanel_text;

        if (targetText == null)
        {
            Debug.LogError("[Text_Typer] Целевая панель диалога не назначена!");
            yield break;
        }

        targetText.text = ""; // Очищаем текст перед началом вывода

        foreach (char letter in text)
        {
            targetText.text += letter; // Добавляем по одной букве
            yield return new WaitForSeconds(Text_speed); // Ждем перед добавлением следующей буквы
        }

        Text_typer = null; // После завершения обнуляем переменную
    }
}
