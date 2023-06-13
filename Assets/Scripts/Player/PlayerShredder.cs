using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerShredder : Weapon
{
    [SerializeField] private PlayerBullet shredder;
    [SerializeField] private Collider shredCol;
    [SerializeField] private List<Transform> enemiesHit; //list that stores enemies in range to display feedback
    //[SerializeField] private VisualEffect //here we will address the blizzard fx        
    private List<Transform> hitsUpdate = new List<Transform>();
    private List<Transform> hitHistory = new List<Transform>(); //the list from the last check, to skip updating list if possible
    private bool attackActive;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = 0f;
        StartCoroutine(ContinuousFire());
        //StartCoroutine(ConstantDamage());
        shredder.RemoveLifetime();
        shredder.SetDamage(damage);

    }

    // Update is called once per frame
    void Update()
    {
        //print(currentCooldown.ToString());
        if (fireHeld)
        {
            anim.SetBool("Shooting", true);
        }
        else
        {
            anim.SetBool("Shooting", false);
        }
    }

    protected override void Fire()
    {
        base.Fire();
        //attackActive = true;
        //print("firing");
        DamageBlizzard();
    }
    private void DamageBlizzard()
    {        
        shredCol.enabled = true;

        /*if (enemiesHit != hitHistory) //checks if the enemy list is the same as last hit. If so, theres no need to update
        {                       //this should save much work since in many hits this will be the case.
            
            hitsUpdate = enemiesHit;  //empty the list to fill it with the current list of active enemies
            foreach (var item in enemiesHit)
            {
                print("On " + item.name);
                if (!item.gameObject.activeInHierarchy  || !item.gameObject.activeSelf)
                {
                    print("added inactive " + item.name);
                    hitsUpdate.Remove(item);
                }
            }
            foreach (var item in hitsUpdate) //pass through the list and remove the inactives from this list
            {
                if (enemiesHit.Contains(item))
                {
                    enemiesHit.Remove(item);
                }
            }            
        }*/
        //hitsUpdate = enemiesHit;
        if (hitsUpdate.Count > 0)
        {
            hitsUpdate.Clear();
        }
        foreach (var item in enemiesHit)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                hitsUpdate.Add(item);
            }
            print(item.name + "added to hitsupdates");
        }
        foreach (var item in hitsUpdate)
        {
            if (enemiesHit.Contains(item))
            {
                enemiesHit.Remove(item);
            }
        }

        foreach (var enemy in enemiesHit)
        {
            GameObject blizzardHit = hitFxPool.RequestPoolObject();
            blizzardHit.transform.SetPositionAndRotation(enemy.position, Quaternion.LookRotation(-transform.forward));
            enemy.GetComponent<EnemyBase>().TakeDamage(damage);
            //blizzardHit.GetComponent<AudioSource>()?.Play(); //it will be too annoying before I find a better sound

        }
        //hitHistory = enemiesHit; //save the list to use it on next hit
        foreach (var item in enemiesHit)
        {
            if (hitHistory.Count > 0)
            {
                hitHistory.Clear();
                hitHistory.Add(item);
            }
        }
        //shredCol.enabled = false;
        currentCooldown = fireRate;
    }

    public override void StartFiring()
    {
        base.StartFiring();
        shredCol.enabled = true;
    }
    public override void StopFiring()
    {
        base.StopFiring();
        shredCol.enabled = false;
        enemiesHit.Clear();
        //shredder.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.CompareTag("Enemy"))
        {
            if (currentCooldown <= 0f)
            {
                GameObject blizzardHit = hitFxPool.RequestPoolObject();                
                blizzardHit.transform.SetPositionAndRotation(other.transform.position, Quaternion.LookRotation(-transform.forward));
                blizzardHit.GetComponent<AudioSource>()?.Play();
            }                       
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesHit.Contains(other.transform))
            {
                enemiesHit.Add(other.transform);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesHit.Contains(other.transform))
            {
                enemiesHit.Remove(other.transform);
                //hitHistory.Remove(other.transform);  //maybe
            }
        }
    }

    /*private IEnumerator ConstantDamage()
    {
        return;
    }*/
}
