using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

public class Player : MonoBehaviour
{
    #region vars
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource aus;
    [SerializeField] private Transform axisRef;
    //[SerializeField] private Transform cam;
    [SerializeField] private Transform hands;
    [SerializeField] private Renderer HandsModel;
    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    [SerializeField] private Sprite[] reticles = new Sprite[2];
    [SerializeField] private Color[] reticleColor = new Color[2];
    [SerializeField] private Image reticleHUD;
    private int currentWeapon;
    public bool canLookAround = true;
    public bool canShoot = true;
    [SerializeField] private bool fireHeld;
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
    [SerializeField] private float hSlow;
    [SerializeField] private float vSlow;
    [SerializeField] private float smoothingSpeed = 0.1f;
    private Vector2 smoothedVector;
    [SerializeField] private bool movingSlow;
    [SerializeField] private Vector3 moveVector;
    [SerializeField] private float verticalValue;
    private float smoothedVertical;
    private Vector2 smoothInputVelocity;
    private float smoothVertVelocity;
    //old
    [SerializeField] private float dashSpeed;
    /*[SerializeField] private float dashDuration;
    private float dashTimer = 0f;*/
    [SerializeField] private float invulTime;
    private bool iFramesEnabled;
    [SerializeField] private float invulTimer;
    private Vector3 movingV;

    /*[SerializeField]
    private float shotRange = 1000f;
    [SerializeField]
    private float shootDamage = 2f;*/

    public Transform cam;
    Vector2 rotation = Vector2.zero;
    #endregion

    #region debug functions
    public void AssignNewArsenal(List<Weapon> newArsenal)
    {
        weapons[currentWeapon].StopFiring();
        //currentWeapon = 0;
        WeaponSwitch(0);
        foreach (var item in weapons)
        {
            item.gameObject.SetActive(false);
        }
        weapons  = newArsenal;
        /*foreach (var item in weapons)
        {
            //item.gameObject.SetActive(true);
            //GameManager.instance.UiScript.InitWeaponInUi(, weapons[i].GetWeaponName(), weapons[i].GetUiIcon());
        }*/
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(true);
            GameManager.instance.UiScript.InitWeaponInUi(i, weapons[i].GetWeaponName(), weapons[i].GetUiIcon());
        }
        

        
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        for (int i = 0; i < weapons.Count; i++)
        {
            //weapons[i].gameObject.SetActive(false);
            weapons[i].gameObject.SetActive(true);
            weapons[i].SetAnim(anim);
            GameManager.instance.UiScript.InitWeaponInUi(i, weapons[i].GetWeaponName(), weapons[i].GetUiIcon());
        }
        currentWeapon = 0;
        weapons[0].gameObject.SetActive(true);
        anim.SetInteger("Weapon", 1);
        reticleHUD.sprite = reticles[currentWeapon];
        reticleHUD.color = reticleColor[currentWeapon];
        canTakeDamage = true;
        StartCoroutine(InvulFrames());
    }

    private void FixedUpdate()
    {
        if (smoothedVertical != verticalValue)
        {
            smoothedVertical = Mathf.SmoothDamp(smoothedVertical, verticalValue, ref smoothVertVelocity, 0.1f);
        }
        if(smoothedVector != (Vector2)moveVector)
        {
            smoothedVector = Vector2.SmoothDamp(smoothedVector, moveVector, ref smoothInputVelocity, 0.1f);
        }
        
        
        if (!movingSlow)
        {
            //rb.velocity = (axisRef.forward * moveVector.y + axisRef.right * moveVector.x) * hSpeed + new Vector3(0f, verticalValue * vSpeed, 0f);            
            rb.velocity = (axisRef.forward * smoothedVector.y + axisRef.right * smoothedVector.x) * hSpeed + new Vector3(0f, smoothedVertical * vSpeed, 0f);

        }
        else
        {
            //rb.velocity = (axisRef.forward * moveVector.y + axisRef.right * moveVector.x) * hSlow + new Vector3(0f, verticalValue * vSlow, 0f);
            rb.velocity = (axisRef.forward * smoothedVector.y + axisRef.right * smoothedVector.x) * hSlow + new Vector3(0f, smoothedVertical * vSlow, 0f);
        }
        
        //rb.velocity = moveVector;

        //Debug.Log(cam.forward);
    }

    // Update is called once per frame
    void Update()
    {
        weapons[currentWeapon].GetComponent<Weapon>().canAttack = canShoot;
        if (canShoot)
        {
            //if (Input.GetKey(KeyCode.Mouse0))
            if (fireHeld)
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
            //Flight();
        }
        if (canLookAround)
        {
            //MouseLook();
        }
        if (!GameManager.instance.paused)
        {
            //WeaponSwitch();
            HoldShield();
            //Dash();
        }

        //Shoot();

        //debug health commands
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
        //Debug.Log("using mouselook new");
        if (!canLookAround) { return;}
        Vector2 targetMouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;
        

        //rotation.y += Input.GetAxis("Mouse X");
        //rotation.x += -Input.GetAxis("Mouse Y");

        rotation.y += targetMouseDelta.x;
        rotation.x -= targetMouseDelta.y;

        //cam.SetPositionAndRotation(cam.position, new Quaternion(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), cam.rotation.y, cam.rotation.z));

        //Mathf.Clamp(rotation.x, -90f, 90f);

        //////////////////////////////////////////////////////////////
        //version that rotates player, the old one that 'worked'
        /*if (rotation.x <= -89.8f)
        {
            rotation.x = -89.7f;
        }
        if (rotation.x >= 89.8f)
        {
            rotation.x = 89.7f;
        }
        transform.eulerAngles = rotation;*/
        //////////////////////////////////////////////////////////////

        if (rotation.x <= -89.8f)
        {
            rotation.x = -89.7f;
        }
        if (rotation.x >= 89.8f)
        {
            rotation.x = 89.7f;
        }
        hands.eulerAngles = rotation;
    }

    private void OnStrafing(InputValue value)
    {
        if (!canMove) { return;}

        /*float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");*/

        Vector2 movement = value.Get<Vector2>();
        
        //Debug.Log(movement.ToString());
        //float up = 0f;
        //float down = 0f;

        /*if (Input.GetKey(KeyCode.Space))
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
        }*/

        //rb.velocity = new Vector3(x * hSpeed, (up + down)* vSpeed, y * hSpeed);

        //Vector3 direction = cam.forward * y * hSpeed + cam.right * x * hSpeed + cam.up * (up + down) * vSpeed;
        //Vector3 finalvelocity = direction; //* hSpeed;

        //moveVector =  new Vector3(movement.x, cam.forward.normalized.y * movement.y);



        //moveVector = (cam.forward * movement.y + cam.right * movement.x) * hSpeed;
        moveVector = new Vector2(movement.x, movement.y);

        //moveVector.x = cam.forward.normalized.x * movement.x * hSpeed;
        //moveVector.z = cam.forward.normalized.y * movement.y * hSpeed;

        ///////////finalvelocity.y += (up + down) * vSpeed;

        //rb.velocity = finalvelocity;
        //movingV = finalvelocity;
    }

    private void OnElevation(InputValue value)
    {
        //rb.velocity = new Vector3(rb.velocity.x, 1f * vSpeed, rb.velocity.z);
        verticalValue = value.Get<float>();
    }

    private void OnSlow(InputValue value)
    {
        if (value.isPressed)
        {
            movingSlow = true;
        }
        else
        {
            movingSlow = false;
        }
    }

    private void OnFire(InputValue value)
    {
        /*if (ctx.performed)
        {
            fireHeld = true;
            weapons[currentWeapon].StartFiring();
        }
        if (ctx.canceled)
        {
            fireHeld = false;
            weapons[currentWeapon].StopFiring();
        }        */

        if (value.isPressed)
        {
            fireHeld = true;
            weapons[currentWeapon].StartFiring();
        }
        else
        {
            fireHeld = false;
            weapons[currentWeapon].StopFiring();
        }
    }

    private void OnWeapon1() { WeaponSwitch(0);}
    private void OnWeapon2() { WeaponSwitch(1);}
    private void OnWeapon3() { WeaponSwitch(2);}
    private void OnWeapon4() { WeaponSwitch(3);}
    private void OnWeapon5() { WeaponSwitch(4);}
    private void OnWeapon6() { WeaponSwitch(5);}
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
                iFramesEnabled = true;
                //Debug.Log(((int)health % 3) * 1);
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

    private void WeaponSwitch(int weaponIndex)
    {
        if (weaponIndex >= weapons.Count)
        {

            Debug.Log("weapon missing in list");
            return;
        }
        if (currentWeapon != weaponIndex)
        {            
            weapons[currentWeapon].StopFiring();
            //weapons[currentWeapon].gameObject.SetActive(false);
            weapons[currentWeapon].enabled = false;
            currentWeapon = weaponIndex;
            //weapons[currentWeapon].gameObject.SetActive(true);
            weapons[currentWeapon].enabled = true;
            if (fireHeld)
            {
                weapons[currentWeapon].StartFiring();
            }
            reticleHUD.sprite = reticles[currentWeapon];
            reticleHUD.color = reticleColor[currentWeapon];
            anim.SetInteger("Weapon", weapons[currentWeapon].GetAnimIndex());
            GameManager.instance.UiScript.SwitchActiveWeapon(weaponIndex);
        }

        /*switch (weaponIndex)
        {
            case 0:
                if (currentWeapon != 0)
                {
                    weapons[currentWeapon].gameObject.SetActive(false);
                    currentWeapon = 0;
                    weapons[currentWeapon].gameObject.SetActive(true);
                    reticleHUD.sprite = reticles[currentWeapon];
                    reticleHUD.color = reticleColor[currentWeapon];
                    anim.SetInteger("Weapon", weapons[currentWeapon].GetAnimIndex());
                    GameManager.instance.UiScript.SwitchActiveWeapon(0);
                }
                break;
            case 1:
                if (currentWeapon != 1)
                {
                    weapons[currentWeapon].gameObject.SetActive(false);
                    currentWeapon = 1;
                    weapons[currentWeapon].gameObject.SetActive(true);
                    reticleHUD.sprite = reticles[currentWeapon];
                    reticleHUD.color = reticleColor[currentWeapon];
                    anim.SetInteger("Weapon", weapons[currentWeapon].GetAnimIndex());
                    GameManager.instance.UiScript.SwitchActiveWeapon(1);
                }
                break;
            case 2:
                if (currentWeapon != 2)
                {
                    weapons[currentWeapon].gameObject.SetActive(false);
                    currentWeapon = 2;
                    weapons[currentWeapon].gameObject.SetActive(true);
                    reticleHUD.sprite = reticles[currentWeapon];
                    reticleHUD.color = reticleColor[currentWeapon];
                    anim.SetInteger("Weapon", weapons[currentWeapon].GetAnimIndex());
                    GameManager.instance.UiScript.SwitchActiveWeapon(2);
                }
                break;
            default:
                break;
        }*/

        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentWeapon != 0)
            {
                weapons[currentWeapon].gameObject.SetActive(false);
                currentWeapon = 0;
                weapons[currentWeapon].gameObject.SetActive(true);
                reticleHUD.sprite = reticles[currentWeapon];
                reticleHUD.color = reticleColor[currentWeapon];
                anim.SetInteger("Weapon", weapons[currentWeapon].GetAnimIndex());
                GameManager.instance.UiScript.SwitchActiveWeapon(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentWeapon != 1)
            {
                weapons[currentWeapon].gameObject.SetActive(false);
                currentWeapon = 1;
                weapons[currentWeapon].gameObject.SetActive(true);
                reticleHUD.sprite = reticles[currentWeapon];
                reticleHUD.color = reticleColor[currentWeapon];
                anim.SetInteger("Weapon", weapons[currentWeapon].GetAnimIndex());
                GameManager.instance.UiScript.SwitchActiveWeapon(1);
            }
            
        }*/
    }

    private void HoldShield()
    {
        //shieldUI.fillAmount = shieldMeter * 0.01f;

        if (Input.GetKey(KeyCode.Mouse1) && shieldMeter > 0f && !shieldDepleted)
        {
            canShoot = false;
            shield.SetActive(true);
            weapons[currentWeapon].gameObject.SetActive(false);
            anim.SetBool("Shielding", true);
            shieldMeter -= Time.deltaTime * shieldDrain;
            GameManager.instance.UiScript.UsingShield(shieldMeter);
        }
        else
        {
            weapons[currentWeapon].gameObject.SetActive(true);
            shield.SetActive(false);
            anim.SetBool("Shielding", false);
            canShoot = true;
            if (shieldMeter <= 0f)
            {
                shieldDepleted = true;
            }
            if (shieldMeter < 100f)
            {
                GameManager.instance.UiScript.ChargingShield(shieldMeter);
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

    public void DamageShield(float dmg)
    {
        shieldMeter -= dmg;
        if (shieldMeter <= 0f)
        {
            shieldDepleted = true;
        }
    }

    /*private void Dash()
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
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            TakeDamage();
        }
    }

    private IEnumerator InvulFrames()
    {
        while (true)
        {
            if (iFramesEnabled)
            {
                canTakeDamage = false;
                invulTimer = invulTime;
                while (invulTimer > 0f)
                {
                    HandsModel.enabled = false;
                    yield return new WaitForSeconds(0.01f);
                    HandsModel.enabled = true;
                    invulTimer -= Time.deltaTime + 0.01f;
                    yield return null;
                }
                canTakeDamage = true;
                HandsModel.enabled = true;
                iFramesEnabled = false;
            }
            yield return null;
        }
    }


    public int GetCurrentWeapon()
    {
        return currentWeapon;
    }
}

