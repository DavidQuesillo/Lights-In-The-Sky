using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform cam;
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float shotRange;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected Color debugColor;
    [SerializeField] protected int animIndex;
    protected float currentCooldown;
    public bool canAttack = true;

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    protected virtual void Fire()
    {

    }

    protected virtual void SecondAction()
    {

    }

    public float GetDamage()
    {
        return damage;
    }

    public int GetAnimIndex()
    {
        return animIndex;
    }
}
