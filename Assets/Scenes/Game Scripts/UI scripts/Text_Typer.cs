using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Text_Typer;

public class Text_Typer : MonoBehaviour
{
    [Header("Game Window Dialogue panel")]
    public TMP_Text GameWindowDialoguePanel_text;
    [Header("Combat Window Dialogue panel")]
    public TMP_Text CombatWindowDialoguePanel_text;
    [Header("Text speed")]
    public float Text_speed = 0.06f;

    /*�������� ��� ������ ������*/
    private Coroutine Text_typer;

    /*������ ��� ������ ������ (��� ������, � ����� ���������� ���� �������� �����)*/
    public enum Dialogue_Mode {Game, Combat}

    // ����� ��� ������� �������� ������ ������
    public void StartTyping(string text, Dialogue_Mode mode = Dialogue_Mode.Game)
    {
        if (Text_typer != null)
        {
            StopCoroutine(Text_typer);
        }

        Text_typer = StartCoroutine(Type_Text(text, mode));
    }

    public void Clear_DialoguePanel(Dialogue_Mode mode)
    {
        TMP_Text targetText = mode == Dialogue_Mode.Game ? GameWindowDialoguePanel_text : CombatWindowDialoguePanel_text;

        if (targetText == null)
        {
            Debug.LogError("[Text_Typer] ������� ������ ������� �� ���������!");
            return;
        }

        targetText.text = "";
    }

    private IEnumerator Type_Text(string text, Dialogue_Mode mode)
    {
        TMP_Text targetText = mode == Dialogue_Mode.Game ? GameWindowDialoguePanel_text : CombatWindowDialoguePanel_text;

        if (targetText == null)
        {
            Debug.LogError("[Text_Typer] ������� ������ ������� �� ���������!");
            yield break;
        }

        targetText.text = "";

        foreach (char letter in text)
        {
            targetText.text += letter;
            yield return new WaitForSeconds(Text_speed);
        }

        Text_typer = null;
    }
}
