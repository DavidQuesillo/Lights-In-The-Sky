using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform cam;
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float shotRange;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected Color debugColor;
    [SerializeField] protected int animIndex; //the number of the "weapon" var in the animator for this weapon
    [SerializeField] protected Animator anim;
    protected float currentCooldown;
    protected float timeOfSwap; //implementing a mechanic so swapping doesnt remove cooldown
    public bool canAttack = true;
    [SerializeField] protected bool fireHeld = false;

    [Header("Special VFX")]
    [SerializeField] private pool hitFxPool;
    [SerializeField] private pool killFxPool;

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    public void SetAnim(Animator animSet)
    {
        anim = animSet;
    }
    protected virtual void Fire()
    {

    }

    protected virtual void SecondAction()
    {

    }

    public virtual void StartFiring()
    {
        fireHeld = true;
    }
    public virtual void StopFiring()
    {
        fireHeld = false;
    }

    /*protected virtual void OnFire(InputValue value)
    {
        fireHeld = !fireHeld;
    }
    protected virtual void OnFire(CallbackContext ctx)
    {
        if ()
        fireHeld = !fireHeld;
    }*/
    public float GetDamage()
    {
        return damage;
    }

    protected bool GetIfCooldownPassedWhileSwapped()
    {
        if (Time.time - timeOfSwap < currentCooldown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected IEnumerator ContinuousFire()
    {
        while (true)
        {
            if (GameManager.instance.currentState == EGameStates.Gameplay && !GameManager.instance.paused)
            {
                currentCooldown -= Time.deltaTime;
                //Debug.Log("pillar cd: " + currentCooldown.ToString());
                if (fireHeld && canAttack)
                {
                    if (currentCooldown <= 0f)
                    {
                        Fire();
                        currentCooldown = fireRate;
                    }
                }
            }
            yield return null;
        }
    }

    public int GetAnimIndex()
    {
        return animIndex;
    }
}
