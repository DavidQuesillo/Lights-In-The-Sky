using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource aus;
    [Header("Weapons")]
    [SerializeField] private GameObject[] weapons = new GameObject[0];
    [SerializeField] private Sprite[] reticles = new Sprite[2];
    [SerializeField] private Color[] reticleColor = new Color[2];
    [SerializeField] private Image reticleHUD;
    private int currentWeapon;
    public bool canLookAround = true;
    public bool canShoot = true;
    [Header("Shield")]
    [SerializeField] private GameObject shield;
    [SerializeField] private Image shieldUI;
    [SerializeField] private float shieldMeter = 100f;
    [SerializeField] private float shieldDrain = 1f;
    [SerializeField] private float shieldGain = 1f;
    private bool shieldDepleted = false;
    [SerializeField] private float minShieldRecharge = 20f;
    [Header("Health")]
    [SerializeField] private float health = 3;
    /// <summary>
    /// //
    /// </summary>

    public bool canTakeDamage = true;
    [Header("Movement")]
    public bool canMove = true;
    [SerializeField] private float hSpeed;
    [SerializeField] private float vSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTimer = 0f;
    [SerializeField] private float InvulTime;
    private float invulTimer;
    private Vector3 movingV;

    /*[SerializeField]
    private float shotRange = 1000f;
    [SerializeField]
    private float shootDamage = 2f;*/

    public Transform cam;
    Vector2 rotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        currentWeapon = 0;
        weapons[0].SetActive(true);
        anim.SetInteger("Weapon", 1);
        reticleHUD.sprite = reticles[currentWeapon];
        reticleHUD.color = reticleColor[currentWeapon];
    }

    // Update is called once per frame
    void Update()
    {
        weapons[currentWeapon].GetComponent<Weapon>().canAttack = canShoot;
        if (canShoot)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetBool("Shooting", true);
            }
            else
            {
                anim.SetBool("Shooting", false);
            }
        }

        if (canMove)
        {
            Flight();
        }
        if (canLookAround)
        {
            //MouseLook();
        }
        if (!GameManager.instance.paused)
        {
            WeaponSwitch();
            HoldShield();
            Dash();
        }

        //Shoot();

        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainHealth();
        }*/
    }

    #region input messages
    private void OnMouseLook()
    {
        Vector2 targetMouseDelta = Mouse.current.delta.ReadValue() * Time.deltaTime * mouseSensitivity;
        cam = Camera.main.transform;

        //rotation.y += Input.GetAxis("Mouse X");
        //rotation.x += -Input.GetAxis("Mouse Y");

        rotation.y += targetMouseDelta.x;
        rotation.x -= targetMouseDelta.y;

        //cam.SetPositionAndRotation(cam.position, new Quaternion(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), cam.rotation.y, cam.rotation.z));

        //Mathf.Clamp(rotation.x, -90f, 90f);
        if (rotation.x <= -89.8f)
        {
            rotation.x = -89.7f;
        }
        if (rotation.x >= 89.8f)
        {
            rotation.x = 89.7f;
        }
        transform.eulerAngles = rotation;
    }

    private void OnStrafing(InputValue value)
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movement = value.Get<Vector2>();

        Debug.Log(movement.ToString());
        float up = 0f;
        float down = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            up = 1f;
        }
        else
        {
            up = 0f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            down = -1f;
        }
        else
        {
            down = 0f;
        }

        //rb.velocity = new Vector3(x * hSpeed, (up + down)* vSpeed, y * hSpeed);

        Vector3 direction = cam.forward * y * hSpeed + cam.right * x * hSpeed + cam.up * (up + down) * vSpeed;
        Vector3 finalvelocity = direction; //* hSpeed;
        //finalvelocity.y += (up + down) * vSpeed;

        rb.velocity = finalvelocity;
        movingV = finalvelocity;
    }
    #endregion

    private void Flight()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float up = 0f;
        float down = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            up = 1f;
        }
        else
        {
            up = 0f;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            down = -1f;
        }
        else
        {
            down = 0f;
        }

        //rb.velocity = new Vector3(x * hSpeed, (up + down)* vSpeed, y * hSpeed);

        Vector3 direction = cam.forward * y * hSpeed + cam.right * x * hSpeed + cam.up * (up + down) * vSpeed;
        Vector3 finalvelocity = direction; //* hSpeed;
        //finalvelocity.y += (up + down) * vSpeed;

        rb.velocity = finalvelocity;
        movingV = finalvelocity;
    }

    /*private void MouseLook()
    {
        cam = Camera.main.transform;

        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");

        //cam.SetPositionAndRotation(cam.position, new Quaternion(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), cam.rotation.y, cam.rotation.z));

        //Mathf.Clamp(rotation.x, -90f, 90f);
        if (rotation.x <= -89.8f)
        {
            rotation.x = -89.7f;
        }
        if (rotation.x >= 89.8f)
        {
            rotation.x = 89.7f;
        }
        transform.eulerAngles = rotation;
    }*/

    /*private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shotRange))
            {
                Debug.DrawLine(transform.position, hit.point, debugColor, 1f);
            }
        }
    }*/

    public void TakeDamage()
    {
        if (canTakeDamage)
        {
            if (health > 0)
            {
                health -= 1f;
                GameManager.instance.UiScript.HpDamage((int)health);
                aus.Play();
                Debug.Log(((int)health % 3) * 1);
            }

            if (health <= 0)
            {
                Death();
            }
        }
        

        
    }
    public void GainHealth()
    {
        if (health >= 3f)
        {
            health = 3f;
            return;
        }
        if (health > 0)
        {
            GameManager.instance.UiScript.HpHeal((int)health);
            health += 1f;

            Debug.Log(((int)health % 3) * 1);
        }
    }
    public void Death()
    {
        GameManager.instance.GameOver();
    }

    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentWeapon != 0)
            {
                weapons[currentWeapon].SetActive(false);
                currentWeapon = 0;
                weapons[currentWeapon].SetActive(true);
                reticleHUD.sprite = reticles[currentWeapon];
                reticleHUD.color = reticleColor[currentWeapon];
                anim.SetInteger("Weapon", 1);
                GameManager.instance.UiScript.SwitchActiveWeapon(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentWeapon != 1)
            {
                weapons[currentWeapon].SetActive(false);
                currentWeapon = 1;
                weapons[currentWeapon].SetActive(true);
                reticleHUD.sprite = reticles[currentWeapon];
                reticleHUD.color = reticleColor[currentWeapon];
                anim.SetInteger("Weapon", 2);
                GameManager.instance.UiScript.SwitchActiveWeapon(1);
            }
            
        }
    }

    private void HoldShield()
    {
        shieldUI.fillAmount = shieldMeter * 0.01f;

        if (Input.GetKey(KeyCode.Mouse1) && shieldMeter > 0f && !shieldDepleted)
        {
            canShoot = false;
            shield.SetActive(true);
            weapons[currentWeapon].SetActive(false);
            anim.SetBool("Shielding", true);
            shieldMeter -= Time.deltaTime * shieldDrain;
            GameManager.instance.UiScript.UsingShield();
        }
        else
        {
            weapons[currentWeapon].SetActive(true);
            shield.SetActive(false);
            anim.SetBool("Shielding", false);
            canShoot = true;
            if (shieldMeter <= 0f)
            {
                shieldDepleted = true;
            }
            if (shieldMeter < 100f)
            {
                GameManager.instance.UiScript.ChargingShield();
                shieldMeter += Time.deltaTime * shieldGain;
                if (shieldMeter >= minShieldRecharge)
                {
                    shieldDepleted = false;
                }
            }
            else
            {
                shieldMeter = 100f;
                GameManager.instance.UiScript.HideShield();
            }
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTimer = dashDuration;
            invulTimer = InvulTime;
        }

        if (invulTimer > 0f)
        {
            canTakeDamage = false;
            invulTimer -= Time.deltaTime;
        }
        else
        {
            canTakeDamage = true;
        }

        if (dashTimer > 0f)
        {
            rb.velocity = movingV * dashSpeed;
            dashTimer -= Time.deltaTime;
            //Debug.Log("Dashing");
        }
        else
        {
            //Debug.Log("Dash end");
        }
        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            TakeDamage();
        }
    }

    public int GetCurrentWeapon()
    {
        return currentWeapon;
    }
}

