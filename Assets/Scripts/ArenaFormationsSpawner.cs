using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaFormationsSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private int enemyCount;
    [SerializeField] private UnityEvent OnWaveOver;

    private void Start()
    {
        enemyCount = enemies.Count;
    }

    public void BeginWave()
    {
        StartCoroutine(SpawnEnemies());
        enemyCount = enemies.Count;
    }
    public void EnemyDefeated()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            OnWaveOver?.Invoke();
        }
    }
    public void WaveDefeated()
    {
        OnWaveOver?.Invoke();
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            enemies[i].SetActive(true);            
        }
    }
}
