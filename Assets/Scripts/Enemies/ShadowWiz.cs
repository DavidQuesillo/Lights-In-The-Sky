using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWiz : EnemyBase
{
    [SerializeField] private GameObject spellAtk;
    [SerializeField] private bool trackingPlayer;
    private bool exploding;
    [SerializeField] private float explodeDelay = 0.8f;
    private float explodeTimer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplosionDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AttackTrack()
    {
        spellAtk.transform.position = GameManager.instance.transform.position;
    }

    protected override void Attack()
    {
        //base.Attack();
        trackingPlayer = false;
        explodeTimer = explodeDelay;
        exploding = true;
    }
    private void Detonate()
    {
        //testing
        spellAtk.SetActive(false);
        Debug.Log("BOOM!!!!!!");
    }

    

    private IEnumerator ExplosionDelay()
    {
        while (true)
        {
            if (exploding && explodeTimer > 0f)
            {
                explodeTimer -= Time.deltaTime;
                if (explodeTimer <= 0f)
                {
                    Detonate();
                    exploding = false;
                }
                yield return null;
            }
            yield return null;
        }
    }

    private IEnumerator AttackTimer()
    {
        while (true)
        {
            if (attacking != false)
            {
                wanderTimer = moveTime;
                while (wanderTimer > 0f)
                {
                    wanderTimer -= Time.deltaTime;
                    yield return null;
                }
                attacking = true;
            }
        }
    }
}
