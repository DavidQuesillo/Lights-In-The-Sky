using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<GameObject> arenaObjects;
    [SerializeField] private ArenaFormationsSpawner waveSpawner;
    [SerializeField] private Vector3 placementWhileAway;
    [SerializeField] private Rigidbody rb;
    /*private bool arenaExiting;
    private bool arenaEntering;
    private bool arenaReady;*/
    
    /*[SerializeField] private float removalTime;
    [SerializeField] private float enterTime;*/

    // Start is called before the first frame update
    void Start()
    {
        //SwitchArenas();        
    }

    public void RemoveArena(float duration)
    {
        //arenaExiting = true;
        for (int i = 0; i < arenaObjects.Count; i++)
        {
            arenaObjects[i].GetComponent<Renderer>().material.DOColor(Color.clear, duration / 3f);
            arenaObjects[i].GetComponent<Collider>().enabled = false;
        }
        //transform.DOMove(placementWhileAway, duration);
        rb.DOMove(placementWhileAway, duration).SetEase(Ease.InCirc);
    }
    public void EnterNewArena(float duration)
    {
        for (int i = 0; i < arenaObjects.Count; i++)
        {
            arenaObjects[i].GetComponent<Renderer>().material.color = Color.white;
            arenaObjects[i].GetComponent<Collider>().enabled = true;
            //transform.DOMove(Vector3.zero, duration);
            //transform.DOLocalMove(Vector3.zero, duration);            
        }
        rb.DOMove(Vector3.zero, duration);
        Debug.Log("Current arena: " + gameObject.name);
    }
    public void SpawnEnemies()
    {
        waveSpawner.BeginWave();
    }

    /*private IEnumerator SwitchArenas()
    {
        while (true)
        {
            if (arenaExiting)
            {
                //RemoveArena();
                //yield return new WaitWhile(()=> arenaExiting);
                //yield return new WaitForSeconds(removalTime);
                //arenaEntering = true;


                for (int i = 0; i < arenaObjects.Count; i++)
                {
                    arenaObjects[i].GetComponent<Renderer>().material.color = Color.white;
                    arenaObjects[i].GetComponent<Collider>().enabled = true;
                    transform.DOMove(Vector3.zero, enterTime);
                }
                arenaExiting = false;
            }
            if (arenaEntering)
            {
                //EnterNewArena();
                yield return new WaitForSeconds(enterTime);
                arenaEntering = false;
                arenaReady = true;
            }
            if (arenaReady)
            {
                waveSpawner.BeginWave();
                arenaReady = false;
            }
        }
    }*/
}
