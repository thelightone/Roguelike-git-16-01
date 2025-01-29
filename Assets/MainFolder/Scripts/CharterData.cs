using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "CharterData", menuName = "Custom/CharterData")]
[System.Serializable]
public class CharterData : ScriptableObject
{
    [SerializeField] public int sceneIndex;
    [SerializeField] public List<LevelData> levelData;
}


[System.Serializable]
public record LevelData
{
    [SerializeField] public int maxReward;
    [SerializeField] public List<WaveData> waveData;
    [SerializeField] public WeaponTypeParent weapUnlocked;
}

[System.Serializable]
public record WaveData
{
    [SerializeField] public int duration;
    [SerializeField] public int maxEnemies;
    [SerializeField] public float enemiesHealthMultiplier;
    [SerializeField] public float enemiesDamageMultiplier;
    [SerializeField] public float enemiesSpeedMultiplier;
    [SerializeField] public bool bossWave;
    [SerializeField] public EnemySO bossData;
    [SerializeField] public List<EnemySO> enemyData;
}
