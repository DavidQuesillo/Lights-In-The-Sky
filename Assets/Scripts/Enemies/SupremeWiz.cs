using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class SupremeWiz : EnemyBase
{
    [SerializeField] private Animator anim;
    [SerializeField] private Material livingMat; //the non-transparent material
    [SerializeField] private Material deadMat; //the transparent material for fading on death
    [SerializeField] private Renderer mr;
    [Header("Explosion Spell Behavior")]
    [SerializeField] private GameObject[] spellAtks; //the designated explosion object. Since each of these enemies only has one, its easier to have them each carry their own. It might be more optimal to have them shared one pooled explosion in the future but this is simpler
    [SerializeField] private GameObject[] spellEffects; //the object that indicates where the attack is being casted
    [SerializeField] private Vector3 minSpellBounds;
    [SerializeField] private Vector3 maxSpellBounds;
    [SerializeField] private bool castingSpell; //whether or not the enemy is currently casting the spell and tracking the player's position on it
    [SerializeField] private float castingTime; //the amount of time the attack will track the player before detonating
    private float trackingTimer; //the timer that will count down
    private bool exploding; //the boolean that will pause functioning while the explosion is going off and the enemy is holding the pose
    [SerializeField] private float explodeDelay = 0.8f; //the delay between the attack locking in and actually becoming damaging
    private float explodeTimer; //the timer that will count down based on explode delay
    [SerializeField] private float explosionLifetime; //the amount of time the explosion will stay active
    private float boomLifeTimer; //the timer that will count down the explosion lifetime
                                 // private float flyTimer; //the timer that will count down how long the enemy flies in one direction
    [Header("Visual Explosion Components")]
    [SerializeField] private List<VisualEffect> castVFX;
    [SerializeField] private List<VisualEffect> rings;
    [SerializeField] private List<GameObject> boomSpheres; //list bc additive alpha required several to be visible
    [SerializeField] private float finalBoomScale; //the magnitude of the scale of the sphere at the end of the explosion
    [SerializeField] private AnimationCurve boomCurve;


    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    //[SerializeField] private float sameDirTimer = 1f;
    [SerializeField] private LayerMask boundaryMask; //the layers that this enemy should count as walls to not fly into

    #region considered movement system
    /*[SerializeField] private int flightSteps = 5; //the amount of times the enemy will go on a new direction after reaching their current destination. Attacks when it reaches 0
    [SerializeField] private float stepsDistance; //how far each step goes forward
    private Vector3 stepVector; //the vector to which the next step will go
    private int flightsCurrent; //the "timer" for the functions
    private bool stepCompleted; //indicates when a step has been completed and stepvector was reached
    */
    #endregion

    private float flyTimer;
    [SerializeField] private float sameDirTimer = 1f;

    /*private Coroutine explosiontimer;
    private Coroutine atktimer;
    private Coroutine flyvector;
    private Coroutine attackloop;*/


    // Start is called before the first frame update
    void Start()
    {
        canTakeDamage = true;
        health = baseHealth;
    }
    private void OnEnable()
    { 
        StartCoroutine(ExplosionDelay());
        StartCoroutine(AttackTimer());
        StartCoroutine(UpdateFlyVector());
        StartCoroutine(RepeatAttack());

        GameObject spawnVfx = ShadMageSpawnVFXPool.Instance.RequestPoolObject(); //replace with proper own one
        spawnVfx.transform.position = transform.position;
        spawnVfx.GetComponent<VisualEffect>().Play();
        aus.PlayOneShot(spawnSoundFX);

        mr.material = livingMat;
        
        health = baseHealth;
        rb.velocity = Vector3.zero;
        anim.SetBool("Dead", false);
        anim.SetBool("Detonating", false);
        anim.SetBool("Casting", false);
        canTakeDamage = true;
        canAttack = true;
        GetComponent<Collider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (!attacking && !exploding)
        {
            rb.AddForce(moveDir * speed * Time.deltaTime, ForceMode.Acceleration);
            //Debug.Log((moveDir * speed * Time.deltaTime).ToString());
            //Debug.Log(moveDir);
            
            rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        }
        if (!exploding)
        {
            rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        }

        /*else if (attacking)
        {
            rb.velocity = Vector3.zero;
            trackingEffect.SetActive(true);
            trackingEffect.transform.position = GameManager.instance.player.transform.position;
            AttackTrack();
        }*/
    }

    private void PlaceAttacks()
    {
        for (int i = 0; i < spellAtks.Length; i++)
        {
            Vector3 pos = new Vector3(Random.Range(minSpellBounds.x, maxSpellBounds.x),
                Random.Range(minSpellBounds.y, maxSpellBounds.y), Random.Range(minSpellBounds.z, maxSpellBounds.z));

            spellAtks[i].transform.position = pos;
            spellEffects[i].transform.position = pos;
            spellEffects[i].SetActive(true);
        }
        anim.SetBool("Casting", true);
        
        /*foreach (var spell in spellAtks)
        {
            

            spellAtk.transform.position = GameManager.instance.player.transform.position;
            trackingEffect.SetActive(true);
            trackingEffect.transform.position = GameManager.instance.player.transform.position;
            print("tracking player");
            anim.SetBool("Casting", true);
        }*/
    }

    protected override void Attack()
    {
        //base.Attack();
        castingSpell = false;
        explodeTimer = explodeDelay;
        //exploding = true;
        //testing
        foreach (var fx in castVFX)
        {
            fx.gameObject.SetActive(false);
        }
        for (int i = 0; i < rings.Count; i++)
        {
            if (i % 2 == 0 || i == 0)
            {
                rings[i].transform.rotation = new Quaternion(-0.382683426f, 0, 0, 0.923879564f);
                rings[i].transform.DOLocalRotate(Vector3.forward * 1080f, explosionLifetime, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            }
            else
            {
                rings[i].transform.rotation = new Quaternion(0.382683426f, 0, 0, 0.923879564f);
                rings[i].transform.DOLocalRotate(-Vector3.forward * 1080f, explosionLifetime, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            }
            rings[i].Play();
        }
        boomSpheres[0].transform.localScale = Vector3.one * 0.2f;
        boomSpheres[0].transform.DOScale(finalBoomScale, 0.2f).SetEase(Ease.OutExpo);
        foreach (var sphere in boomSpheres)
        {
            sphere.GetComponent<Renderer>().material.DOFade(1f, 0f);
            sphere.GetComponent<Renderer>().material.DOFade(0f, explosionLifetime).SetEase(boomCurve);
        }

        for (int i = 0; i < spellAtks.Length; i++)
        {
            spellAtks[i].SetActive(true);
            spellEffects[i].SetActive(false);
        }        
        anim.SetBool("Detonating", true);
        Debug.Log("BOOM!!!!!!");
    }
    public override void TakeDamage(float dmg)
    {
        if (canTakeDamage)
        {
            base.TakeDamage(dmg);
            if (health <= 0f)
            {
                //gameObject.SetActive(false);
                Death();
            }
        }
    }
    protected override void Death()
    {
        base.Death();
        anim.SetBool("Dead", true);
        castingSpell = false;
        exploding = false;
        attacking = false;
        for (int i = 0; i < spellAtks.Length; i++)
        {
            spellAtks[i].SetActive(false);
            spellEffects[i].SetActive(false);
        }
        mr.material = deadMat;
        GetComponent<Collider>().enabled = false;

        /*StopCoroutine(explosiontimer);
        StopCoroutine(atktimer);
        StopCoroutine(flyvector);
        StopCoroutine(attackloop);*/
    }
    /*private void Detonate()
    {
        //testing
        spellAtk.SetActive(true);
        Debug.Log("BOOM!!!!!!");
    }*/



    private IEnumerator ExplosionDelay()
    {
        while (true)
        {
            if (exploding && explodeTimer > 0f)
            {
                explodeTimer -= Time.deltaTime;
                if (explodeTimer <= 0f)
                {
                    //Detonate();
                    Attack();
                    yield return new WaitForSeconds(explosionLifetime);
                    exploding = false;
                    attacking = false;
                    anim.SetBool("Detonating", false);
                    for (int i = 0; i < spellAtks.Length; i++)
                    {
                        spellAtks[i].SetActive(false);
                    }
                    //spellAtk.SetActive(false);
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
                yield return null;
            }
            yield return null;
        }
    }
    private IEnumerator RepeatAttack()
    {
        while (true)
        {
            if (attacking)
            {
                //anim.SetBool("Attacking", true);
                
                /*barrageProg = barrageAmount;
                while (barrageProg > 0)
                {
                    ShootProjectile();
                    barrageProg -= 1;
                    yield return new WaitForSeconds(0.05f);
                }*/
                //Attack();
                rb.velocity = Vector3.zero;                                
                castingSpell = true;    
                trackingTimer = 0f;
                PlaceAttacks();
                foreach (var fx in castVFX)
                {
                    fx.gameObject.SetActive(true);
                    fx.transform.DORotate(Vector3.up * 720f + Vector3.up * Random.Range(-30f, 10f), castingTime + explodeDelay, RotateMode.FastBeyond360).SetEase(Ease.InExpo);
                }
                while (trackingTimer < castingTime)
                {
                    //PlaceAttacks();
                    trackingTimer += Time.deltaTime;
                    yield return null;
                }
                print("casting time complete");
                //attacking = false;
                explodeTimer = explodeDelay;
                anim.SetBool("Casting", false);
                anim.SetBool("Detonating", true);
                exploding = true;
                yield return new WaitUntil(() => !exploding);
                //anim.SetBool("Attacking", false);
            }
            else
            {
                wanderTimer = moveTime;
                while (wanderTimer > 0f)
                {
                    if (canAttack)
                    {
                        wanderTimer -= Time.deltaTime;
                    }
                    //wanderTimer -= Time.deltaTime;
                    yield return null;
                }
                attacking = true;
            }
            yield return null;
        }
    }
    private IEnumerator UpdateFlyVector()
    {
        while (true)
        {
            flyTimer = sameDirTimer; 
            while (!Physics.Raycast(transform.position, moveDir.normalized, 5f, boundaryMask))
            {
                flyTimer -= Time.deltaTime;
                //Debug.Log("moving successfully");
                if (flyTimer <= 0f) { break;}
                yield return null;
            }
            if (flyTimer > 0) {rb.velocity = Vector3.zero; }
            moveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            yield return null;
        }
        /*while (true)
        {
            if (!trackingPlayer && !exploding && !GameManager.instance.paused)
            {
                if (flightsCurrent > 0)
                {
                    CalculateNextStep();                    
                    yield return new WaitUntil(() => stepCompleted);
                    
                }
            }
            yield return null;
        }*/
    }
    /*private IEnumerator MoveEnemy()
    {
        while (true)
        {
            if (!trackingPlayer && !exploding && rb.position != stepVector && !GameManager.instance.paused)
            {
                if (rb.position != stepVector)
                {
                    rb.MovePosition(Vector3.MoveTowards(rb.position, stepVector, speed));
                }                
            }
            if (rb.position == stepVector)
            {

            }
        }
    }

    private void CalculateNextStep()
    {
        RaycastHit hit;
        Vector3 stepCandidate = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        if (!Physics.Raycast(rb.position, stepCandidate, out hit, stepsDistance + 1f, boundaryMask))
        {
            stepVector = rb.position + (stepCandidate.normalized * stepsDistance);
        }
        else
        {
            //stepVector = rb.position + (-stepCandidate.normalized * stepsDistance);
            stepVector = rb.position + (hit.normal.normalized * stepsDistance);
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            canTakeDamage = true;
            transform.parent = null;
            //mr.enabled = true;
            inPosition = true;
        }
        if (other.CompareTag("PlayerShot") || other.CompareTag("Explosion"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
        /*if (other.CompareTag("Shredder"))
        {
            //LockAttack();
            canAttack = false;
        }*/
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            Debug.Log("Exited the zone");
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Shredder"))
        {
            canAttack = true;
        }
    }
}
