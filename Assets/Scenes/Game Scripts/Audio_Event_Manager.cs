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
        // Используем FindObjectsByType для поиска ВСЕХ объектов, включая неактивные
        // FindObjectsInactive.Include соответствует старому параметру 'true'
        // FindObjectsSortMode.None означает, что порядок не важен (обычно самый быстрый вариант)
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        AudioListener[] audioListeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // Получаем список всех активных сцен
        List<Scene> activeScenes = new List<Scene>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            activeScenes.Add(SceneManager.GetSceneAt(i));
        }

        // Определяем, какая сцена сейчас "главная" (влияет на интерфейс)
        // Предполагаем, что сцена "Save Menu" загружается аддитивно
        bool isSaveMenuOpen = activeScenes.Exists(s => s.name == "Save Menu" && s.isLoaded); // Добавил проверку isLoaded для надежности
        
        Scene targetScene; // Сцена, компоненты которой должны быть активны

        // Если открыто меню сохранения, ищем сцену "Main Menu"
        if (isSaveMenuOpen)
        {
            targetScene = SceneManager.GetSceneByName("Main Menu");
        }
        else // Иначе используем текущую активную сцену
        {
             targetScene = SceneManager.GetActiveScene();
        }

        // Переключаем Event System
        foreach (EventSystem es in eventSystems)
        {
            // Проверяем, что gameObject не null и сцена, к которой он принадлежит, валидна
            if (es != null && es.gameObject != null && es.gameObject.scene.IsValid())
            {
                // Активируем EventSystem только если он принадлежит целевой сцене
                es.gameObject.SetActive(es.gameObject.scene == targetScene);
            }
        }

        // Переключаем Audio Listener
        foreach (AudioListener al in audioListeners)
        {
             // Проверяем, что gameObject не null и сцена, к которой он принадлежит, валидна
            if (al != null && al.gameObject != null && al.gameObject.scene.IsValid())
            {
                // Включаем AudioListener только если он принадлежит целевой сцене
                // Используем al.enabled вместо SetActive, так как AudioListener - это компонент
                al.enabled = (al.gameObject.scene == targetScene);
            }
        }
    }
}