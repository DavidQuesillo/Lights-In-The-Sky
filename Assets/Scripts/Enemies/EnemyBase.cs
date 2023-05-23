using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected enum sideComingFrom { Forward, Left, Behind, Right }

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected GameObject shotPrefab;
    [SerializeField] protected AudioClip spawnSoundFX;
    [SerializeField] protected AudioClip deathSoundFX;
    [SerializeField] protected AudioSource aus;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] private bool isPartOfWave;
    [SerializeField] protected Vector3 initialPosition;
    [SerializeField] protected float health;
    [SerializeField] protected float baseHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float moveTime;
    [SerializeField] protected float moveVarRange;
    [SerializeField] protected bool inPosition = false;
    protected bool canTakeDamage = true;
    [SerializeField] protected float wanderTimer = 0f;
    protected Vector3 moveDir;
    [SerializeField] protected bool attacking = false;
    [SerializeField] protected sideComingFrom whereFrom = sideComingFrom.Forward;

    /*protected void Awake()
    {
        initialPosition = transform.position;
        Debug.Log("Ran from parent");
    }*/

    public void SetStartPos()
    {
        initialPosition = transform.position;
    }

    public Vector3 GetStartPos()
    {
        return initialPosition;
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Relocate()
    {

    }

    protected virtual void Death()
    {
        if (isPartOfWave)
        {
            SendMessageUpwards("EnemyDefeated");
        }
        canTakeDamage = false;
    }
    public virtual void TurnInactive()
    {
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;
    }

    public virtual void CriticalHit()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.gameObject.GetComponent<PlayerBullet>().GetDamage());
            Debug.Log("Damage taken");
        }
    }

    public virtual float GetHP()
    {
        return health;
    }
}
