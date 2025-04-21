using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class EnemyLoader : MonoBehaviour
{
    public GameObject enemyPrefab; // Базовый префаб врага
    public List<EnemyData> enemyList;

    private void Start()
    {
        LoadEnemies();
       // SpawnEnemy(0); // Спавним первого врага для теста
    }

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

    public void SpawnEnemy(int index)
    {
        if (index < 0 || index >= enemyList.Count) return;

        EnemyData data = enemyList[index];
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        newEnemy.transform.localScale = new Vector3(30f, 30f, 10f);
        Enemy_Unit enemyUnit = newEnemy.GetComponent<Enemy_Unit>();

        // Настраиваем характеристики
        enemyUnit.unitName = data.name;
        enemyUnit.level = data.level;
        enemyUnit.max_health = data.max_health;
        enemyUnit.cur_health = data.max_health;
        enemyUnit.attack = data.attack;
        enemyUnit.defense = data.defense;

        // Загружаем спрайт
        Sprite enemySprite = Resources.Load<Sprite>($"Sprites/{data.sprite_name}");
        if (enemySprite != null)
        {
            Debug.Log($"Sprite {enemySprite} loaded");
            enemyUnit.SetSprite(enemySprite);
        }
        else
        {
            Debug.LogWarning("Sprite not loaded");
        }

        Debug.Log($"[SpawnEnemy] {enemyUnit.name} created on position: {enemyUnit.transform.position}, sprite name: {enemyUnit.Sprite_Renderer}");
    }

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