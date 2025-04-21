using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class EnemyData
    {
        public string name;
        public int level;
        public int max_health;
        public int attack;
        public int defense;
        public string sprite_name;
    }

    [System.Serializable]
    public class EnemyListWrapper
    {
        public List<EnemyData> enemies;
    }
