using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SceneAudioEventManager : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        UpdateActiveComponents();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateActiveComponents();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        UpdateActiveComponents();
    }

    public void UpdateActiveComponents()
    {
        // ���������� FindObjectsByType ��� ������ ���� ��������, ������� ����������
        // FindObjectsInactive.Include ������������� ������� ��������� 'true'
        // FindObjectsSortMode.None ��������, ��� ������� �� ����� (������ ����� ������� �������)
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        AudioListener[] audioListeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // �������� ������ ���� �������� ����
        List<Scene> activeScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            activeScenes.Add(SceneManager.GetSceneAt(i));
        }

        // ����������, ����� ����� ������ "�������" (������ �� ���������)
        // ������������, ��� ����� "Save Menu" ����������� ���������
        bool isSaveMenuOpen = activeScenes.Exists(s => s.name == "Save Menu" && s.isLoaded); // ������� �������� isLoaded ��� ����������
        
        Scene targetScene; // �����, ���������� ������� ������ ���� �������

        // ���� ������� ���� ����������, ���� ����� "Main Menu"
        if (isSaveMenuOpen)
        {
            targetScene = SceneManager.GetSceneByName("Main Menu");
        }
        else // ����� ���������� ������� �������� �����
        {
             targetScene = SceneManager.GetActiveScene();
        }

        // ����������� Event System
        foreach (EventSystem es in eventSystems)
        {
            // ���������, ��� gameObject �� null � �����, � ������� �� �����������, �������
            if (es != null && es.gameObject != null && es.gameObject.scene.IsValid())
            {
                // ���������� EventSystem ������ ���� �� ����������� ������� �����
                es.gameObject.SetActive(es.gameObject.scene == targetScene);
            }
        }

        // ����������� Audio Listener
        foreach (AudioListener al in audioListeners)
        {
             // ���������, ��� gameObject �� null � �����, � ������� �� �����������, �������
            if (al != null && al.gameObject != null && al.gameObject.scene.IsValid())
            {
                // �������� AudioListener ������ ���� �� ����������� ������� �����
                // ���������� al.enabled ������ SetActive, ��� ��� AudioListener - ��� ���������
                al.enabled = (al.gameObject.scene == targetScene);
            }
        }
    }
}