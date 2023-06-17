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
    [SerializeField] protected Color crosshairColor;
    [SerializeField] protected int animIndex; //the number of the "weapon" var in the animator for this weapon
    [SerializeField] protected string weaponName;
    [SerializeField] protected Sprite uiIcon;
    [SerializeField] protected Sprite crosshair;
    [SerializeField] protected Animator anim;
    protected float currentCooldown;
    protected float timeOfSwap; //implementing a mechanic so swapping doesnt remove cooldown
    public bool canAttack = true;
    [SerializeField] protected bool fireHeld = false;
    protected Coroutine firingCoroutine; //this is necessary to stop and restart it properly

    [Header("Special VFX")]
    [SerializeField] protected pool hitFxPool;
    [SerializeField] protected pool killFxPool;
    /*[SerializeField] protected Transform hitPoolParent;
    [SerializeField] protected Transform killPoolParent;*/

    private void Awake()
    {
        /*if (hitFxPool != null)
        {
            hitFxPool.assignedParent = hitPoolParent;
        }
        if (killFxPool != null)
        {
            killFxPool.assignedParent = killPoolParent;
        }*/        
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    public string GetWeaponName()
    {
        return weaponName;
    }
    public Sprite GetUiIcon()
    {
        return uiIcon;
    }
    public Sprite GetCrosshair()
    {
        return crosshair;
    }
    public Color GetReticleColor()
    {
        return crosshairColor;
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
        print("cooldown checked");
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
                        print("parent firing");
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
