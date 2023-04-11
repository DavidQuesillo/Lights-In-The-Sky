using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss1 : BossBase
{
    [Header("Function connections")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject model;
    [SerializeField] private Transform shootPoint;
    [Header("Functionality")]
    [SerializeField] private bool defeated = false;
    [Header("Movement")]
    [SerializeField] private Vector3 moveAreaMin;
    [SerializeField] private Vector3 moveAreaMax;
    [SerializeField] protected float speed;
    private bool inPosition = false;
    private Vector3 moveDir = Vector3.zero;
    [Header("Attacks")]
    [SerializeField] private float slashSpeed;
    [SerializeField] private float fireballSpeed;
    [SerializeField] private float slashRate;
    [SerializeField] private float fireballRate;
    private WaitForSeconds slashWait;
    private WaitForSeconds fireballWait;
    [SerializeField] private GameObject vSlash;
    [SerializeField] private GameObject hSlash;
    [SerializeField] private GameObject fireBall;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        nameSlot.text = BossName;
        totalHealth = Health;
        slashWait = new WaitForSeconds(slashRate);
        fireballWait = new WaitForSeconds(fireballRate);
    }

    // Update is called once per frame
    void Update()
    {
        rb.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - rb.position, Vector3.up);
        if (Health <= 0f)
        {
            GameManager.instance.ChangeScene(0);
        }
    }

    private void Relocate()
    {
            //Attack();
            FindNewPosition();
            rb.DOMove(moveDir, speed);
    }

    private void VerticalSlash()
    {
        GameObject vShot = Instantiate(vSlash, shootPoint.position, Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position));
        anim.SetTrigger("vSlash");
        vShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * slashSpeed, ForceMode.VelocityChange);
    }
    private void HorizontalSlash()
    {
        GameObject hShot = Instantiate(hSlash, shootPoint.position, Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position));
        anim.SetTrigger("hSlash");
        hShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * slashSpeed, ForceMode.VelocityChange);
    }
    private void ShootFireBall()
    {
        GameObject fShot = Instantiate(fireBall, shootPoint.position, Quaternion.identity);
        anim.SetBool("Fireball", true);
        fShot.GetComponent<Rigidbody>().AddForce((GameManager.instance.player.transform.position - shootPoint.position).normalized * fireballSpeed, ForceMode.VelocityChange);
    }

    private void FindNewPosition()
    {
        moveDir = new Vector3(Random.Range(moveAreaMin.x, moveAreaMax.x), Random.Range(moveAreaMin.y, moveAreaMax.y), Random.Range(moveAreaMin.z, moveAreaMax.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyActivate"))
        {
            BeginBossFight();
        }
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }
    protected override void BeginBossFight()
    {
        base.BeginBossFight();
        model.SetActive(true);
        GameManager.instance.PlayBossSong();

        Debug.Log("Fight Started");
        StartCoroutine(MoveAround());
    }

    private void CheckReachedDir()
    {
        if (rb.position == moveDir)
        {
            inPosition = true;
            anim.SetBool("Fireball", false);
        }
    }

    private IEnumerator MoveAround()
    {
        Relocate();
        Debug.Log("Initial move");
        while (!defeated)
        {
            CheckReachedDir();
            Debug.Log("Checking location");
            if (inPosition)
            {
                Debug.Log("again move");
                //Attack();
                FindNewPosition();
                rb.DOMove(moveDir, speed);
                inPosition = false;
                StartCoroutine(Attack());
            }
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
       int choice = Random.Range(0, 2);
           
       if (choice == 0)
       {
           VerticalSlash();
           yield return slashWait;
           HorizontalSlash();
           yield return slashWait;
           VerticalSlash();
       }
       else
       {
           ShootFireBall();
           yield return fireballWait;
           ShootFireBall();
           yield return fireballWait;
           ShootFireBall();
           yield return fireballWait;
           ShootFireBall();
           yield return fireballWait;
           ShootFireBall();
       }
       Debug.Log("attack ended");
    }
}