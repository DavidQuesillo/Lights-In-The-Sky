using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArenaRotation : MonoBehaviour
{
    [SerializeField] private List<Arena> arenasList;
    private int currentArena;
    private bool arenaExiting;
    private bool arenaEntering;
    private bool arenaReady;
    [SerializeField] private float removalTime;
    [SerializeField] private float enterTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchArenas());
        //Debug.Log("Arena list count: " + arenasList.Count);
    }
    public void BeginArenaRotation()
    {
        arenaEntering = true;
    }
    private int GetNextArena()
    {
        int nextArena = Random.Range(0, arenasList.Count - 1);
        if (nextArena != currentArena)
        {
            return nextArena;
        }
        else
        {
            if (nextArena == 0)
            {
                nextArena += Random.Range(1, arenasList.Count - 1);
                return nextArena;
            }
            else if (nextArena == arenasList.Count - 1)
            {
                nextArena -= Random.Range(1, arenasList.Count - 1);
                return nextArena;
            }
            else
            {
                nextArena += Random.Range(-1, 2);
                return nextArena;
            }
        }
    }


    public void ArenaCompleted()
    {
        arenaExiting = true;
    }

    private IEnumerator SwitchArenas()
    {
        while (true)
        {
            if (arenaExiting)
            {
                arenasList[currentArena].RemoveArena(removalTime);
                //RemoveArena();
                //yield return new WaitWhile(()=> arenaExiting);
                yield return new WaitForSeconds(removalTime);
                //arenaEntering = true;                
                arenaEntering = true;
                arenaExiting = false;
            }
            if (arenaEntering)
            {
                //EnterNewArena();
                currentArena = GetNextArena();
                arenasList[currentArena].EnterNewArena(enterTime);
                yield return new WaitForSeconds(enterTime);                
                arenasList[currentArena].SpawnEnemies();
                arenaEntering = false;
            }
            yield return null;
        }
    }
}
