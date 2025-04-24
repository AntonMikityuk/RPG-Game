using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class EnemyLoader : MonoBehaviour
{
    [Header("Enemy prefab")]
    public GameObject enemyPrefab;
    [Header("Enemy List")]
    public List<EnemyData> enemyList;
    [Header("Enemy Spawn Point")]
    public Transform EnemySpawn_Point;
    /*Загрузка врагов при старте*/
    private void Start()
    {
        LoadEnemies();
    }
    /*Загрузка противников из файла*/
    private void LoadEnemies()
    {
        Debug.Log("[EnemyLoader] LoadEnemies() called");
        string path = Path.Combine(Application.streamingAssetsPath, "Enemies.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            EnemyListWrapper data = JsonUtility.FromJson<EnemyListWrapper>(json);

            if (data != null && data.enemies != null)
            {
                enemyList = data.enemies;
                Debug.Log("Enemies loaded successfully!");
                Debug.Log($"Enemies count: {enemyList.Count}");

                PrintEnemyList(enemyList);
            }
            else
            {
                Debug.LogError("Failed to parse Enemies.json or list is empty.");
            }
        }
        else
        {
            Debug.LogError("Enemy file not found: " + path);
        }
    }

    // Метод для вывода списка загруженных врагов
    private void PrintEnemyList(List<EnemyData> enemies)
    {
        Debug.Log("[EnemyLoader] Loaded enemies:");
        foreach (var enemy in enemies)
        {
            Debug.Log($"- {enemy.name} | Level: {enemy.level} | HP: {enemy.max_health} | ATK: {enemy.attack} | DEF: {enemy.defense}");
        }
    }
    /*Спавн противника*/
    public void SpawnEnemy(int index)
    {
        if (EnemySpawn_Point == null)
        {
            Debug.LogError("Enemy Spawn Point not assigned in EnemyLoader!");
            return;
        }
        if (index < 0 || index >= enemyList.Count)
        {
            Debug.LogWarning($"[SpawnEnemy] Attempted spawn with wrong index: {index}. Available: {enemyList.Count} enemies.");
            return;
        }
        EnemyData data = enemyList[index];
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab not assigned in EnemyLoader!");
            return;
        }
        GameObject newEnemy = Instantiate(enemyPrefab, EnemySpawn_Point.position, Quaternion.identity); // Используем enemySpawnPoint.position
        newEnemy.transform.localScale = new Vector3(40f, 40f, 20f);

        Enemy_Unit enemyUnit = newEnemy.GetComponent<Enemy_Unit>();
        if (enemyUnit == null)
        {
            Debug.LogError($"Enemy prefab '{enemyPrefab.name}' does not contain Enemy_Unit!", enemyPrefab);
            Destroy(newEnemy);
            return;
        }

        enemyUnit.unitName = data.name;
        enemyUnit.level = data.level;
        enemyUnit.max_health = data.max_health;
        enemyUnit.cur_health = data.max_health;
        enemyUnit.attack = data.attack;
        enemyUnit.defense = data.defense;

        Sprite enemySprite = Resources.Load<Sprite>($"Sprites/{data.sprite_name}");
        if (enemySprite != null)
        {
            enemyUnit.SetSprite(enemySprite);
        }
        else
        {
            Debug.LogWarning($"Sprite not found by path: Sprites/{data.sprite_name}");
        }
        Debug.Log($"[SpawnEnemy] Enemy '{enemyUnit.unitName}' spawned on: {newEnemy.transform.position} (from spawn point: {EnemySpawn_Point.name}), sprite name: {data.sprite_name}");
    }

    /*Поиск протиника*/
    public int Search_Enemy(string name)
    {
        foreach (var  enemy in enemyList)
        {
            if (enemy.name == name)
            {
                Debug.Log($"Enemy found, name: {enemy.name}");
                return enemyList.IndexOf(enemy);
            }
        }
        Debug.Log("Enemy not found");
        return -1;
    }
}