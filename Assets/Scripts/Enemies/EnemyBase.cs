using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected enum sideComingFrom { Forward, Left, Behind, Right }

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected GameObject shotPrefab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float moveTime;
    [SerializeField] protected float moveVarRange;
    [SerializeField] protected bool inPosition = false;
    protected bool canTakeDamage = false;
    protected float wanderTimer = 0f;
    protected Vector3 moveDir;
    [SerializeField] protected bool attacking = false;
    [SerializeField] protected sideComingFrom whereFrom = sideComingFrom.Forward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void Attack()
    {

    }

    protected virtual void Relocate()
    {

    }

    protected virtual void Death()
    {

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
}
