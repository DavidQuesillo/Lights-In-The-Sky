using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWiz : EnemyBase
{
    [SerializeField] private Animator anim;
    [Header("Explosion Spell Behavior")]
    [SerializeField] private GameObject spellAtk; //the designated explosion object. Since each of these enemies only has one, its easier to have them each carry their own. It might be more optimal to have them shared one pooled explosion in the future but this is simpler
    [SerializeField] private GameObject trackingEffect; //the object that indicates where the attack is being casted
    [SerializeField] private bool trackingPlayer; //whether or not the enemy is currently casting the spell and tracking the player's position on it
    [SerializeField] private float castingTime; //the amount of time the attack will track the player before detonating
    private float trackingTimer; //the timer that will count down
    private bool exploding; //the boolean that will pause functioning while the explosion is going off and the enemy is holding the pose
    [SerializeField] private float explodeDelay = 0.8f; //the delay between the attack locking in and actually becoming damaging
    private float explodeTimer; //the timer that will count down based on explode delay
    [SerializeField] private float explosionLifetime; //the amount of time the explosion will stay active
    private float boomLifeTimer; //the timer that will count down the explosion lifetime
    // private float flyTimer; //the timer that will count down how long the enemy flies in one direction

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


    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    { 
        StartCoroutine(ExplosionDelay());
        StartCoroutine(AttackTimer());
        StartCoroutine(UpdateFlyVector());
        StartCoroutine(RepeatAttack());
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
        /*else if (attacking)
        {
            rb.velocity = Vector3.zero;
            trackingEffect.SetActive(true);
            trackingEffect.transform.position = GameManager.instance.player.transform.position;
            AttackTrack();
        }*/
    }

    private void AttackTrack()
    {
        spellAtk.transform.position = GameManager.instance.player.transform.position;
        trackingEffect.SetActive(true);
        trackingEffect.transform.position = GameManager.instance.player.transform.position;
        print("tracking player");
    }

    protected override void Attack()
    {
        //base.Attack();
        trackingPlayer = false;
        explodeTimer = explodeDelay;
        //exploding = true;
        //testing
        spellAtk.SetActive(true);
        trackingEffect.SetActive(false);
        Debug.Log("BOOM!!!!!!");
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
                    spellAtk.SetActive(false);
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
                trackingPlayer = true;    
                trackingTimer = 0f;
                while (trackingTimer < castingTime)
                {
                    AttackTrack();
                    trackingTimer += Time.deltaTime;
                    yield return null;
                }
                print("casting time complete");
                //attacking = false;
                explodeTimer = explodeDelay;
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
}
