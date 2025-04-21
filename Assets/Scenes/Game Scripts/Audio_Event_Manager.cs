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
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>(true);
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>(true);

        // �������� ������ ���� �������� ����
        List<Scene> activeScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            activeScenes.Add(SceneManager.GetSceneAt(i));
        }

        // ����������, ����� ����� ������ "�������" (������ �� ���������)
        bool isSaveMenuOpen = activeScenes.Exists(s => s.name == "Save Menu");
        Scene mainScene = isSaveMenuOpen ? SceneManager.GetSceneByName("Main Menu") : SceneManager.GetActiveScene();

        // ����������� Event System
        foreach (EventSystem es in eventSystems)
        {
            es.gameObject.SetActive(es.gameObject.scene == mainScene);
        }

        // ����������� Audio Listener
        foreach (AudioListener al in audioListeners)
        {
            al.enabled = al.gameObject.scene == mainScene;
        }
    }
}