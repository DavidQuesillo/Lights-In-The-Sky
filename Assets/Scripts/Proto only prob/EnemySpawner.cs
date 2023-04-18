using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnTimer = 5f;
    [SerializeField] private pool sourcePool;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoSpawner());
    }

    private void SpawnMonster()
    {
        GameObject enemy = sourcePool.RequestPoolObject();
        enemy.transform.position = transform.position;
    }

    private IEnumerator AutoSpawner()
    {
        while (true)
        {
            timer = spawnTimer;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            if (timer <= 0)
                SpawnMonster();
            yield return null;
        }

    }
}
