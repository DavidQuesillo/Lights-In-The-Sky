using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameplayUI : MonoBehaviour
{
    [Header("WeaponBar")]
    [SerializeField] private RectTransform weaponsPos;
    [SerializeField] private Image[] weaponsList = new Image[0];
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color enabledColor;
    private int currentWeapon = 0;
    private bool displayingWeapons = false;
    [SerializeField] private float wOnScreenDuration;
    private float weaponsDisplayTimer;

    [Header("Health Points")]
    [SerializeField] private RectTransform hpPos;
    [SerializeField] private Image[] hpDots = new Image[3];
    [SerializeField] private Image[] hpCorner = new Image[3];
    [SerializeField] private Color hpColor;
    [SerializeField] private Color hpLostColor;
    private bool displayingHp = false;
    [SerializeField] private float hpOnScreenDuration;
    private float hpDisplayTimer;
    //private bool hpVisible;

    [Header("Shield Meter")]
    [SerializeField] private RectTransform shieldPos;
    [SerializeField] private Image shieldCorner;
    private bool displayingShield = false;


    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        currentWeapon = GameManager.instance.player.GetComponent<Player>().GetCurrentWeapon();
        //weaponsList[currentWeapon].color = enabledColor;
        weaponsList[currentWeapon].GetComponent<RectTransform>().localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponUiTimer();
        HpUiTimer();
    }

    public void SwitchActiveWeapon(int weapon)
    {
        if (!displayingWeapons)
        {
            DisplayWeapons();
        }
        else
        {
            weaponsDisplayTimer = wOnScreenDuration;
        }
        weaponsList[currentWeapon].DOColor(disabledColor, 0.1f);
        weaponsList[currentWeapon].GetComponent<RectTransform>().DOScale(0.8f, 0.1f);
        weaponsList[currentWeapon].gameObject.GetComponentInChildren<TextMeshProUGUI>().DOColor(new Color(disabledColor.r, disabledColor.g, disabledColor.b, 0f), 0.1f);

        currentWeapon = weapon;
        weaponsList[currentWeapon].DOColor(enabledColor, 0.1f);
        weaponsList[currentWeapon].GetComponent<RectTransform>().DOScale(1f, 0.1f);
        weaponsList[currentWeapon].gameObject.GetComponentInChildren<TextMeshProUGUI>().DOColor(enabledColor, 0.1f);
    }

    public void DisplayWeapons()
    {
        displayingWeapons = true;
        weaponsDisplayTimer = wOnScreenDuration;
        //weaponsPos.DOMove(Vector3.zero, 0.1f);
        weaponsPos.DOAnchorPos(Vector2.zero, 0.1f);
        weaponsPos.gameObject.GetComponent<Image>().DOFade(0.7f, 0.1f);

        for (int i = 0; i < weaponsList.Length; i++)
        {
            if (i != currentWeapon)
            {
                weaponsList[i].DOFade(0.8f, 0.1f);
            }
            else
            {
                weaponsList[i].DOFade(0.8f, 0.1f);
            }
        }
        //Debug.Log("weapons shown");
    }
    public void HideWeapons()
    {
        displayingWeapons = false;
        //weaponsPos.DOMove(Vector3.left * 1f, 0.1f);
        weaponsPos.DOAnchorPos(Vector2.down * 15f, 0.1f);
        weaponsPos.GetComponent<Image>().DOFade(0f, 0.1f);

        for (int i = 0; i < weaponsList.Length; i++)
        {
            weaponsList[i].DOFade(0f, 0.1f);
            weaponsList[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, 0.1f);
        }
        //Debug.Log("Ws hidden");
    }

    private void WeaponUiTimer()
    {
        if (displayingWeapons)
        {
            weaponsDisplayTimer -= Time.deltaTime;
            if (weaponsDisplayTimer <= 0f)
            {
                HideWeapons();
            }
        }
    }

    public void DisplayHp()
    {
        
        hpDisplayTimer = hpOnScreenDuration;        
        //hpPos.DOMove(Vector3.zero, 0.1f);
        //hpPos.DOAnchorPos(Vector2.zero, 0.1f);
        
        if (!displayingHp)
        {
            hpDots[0].DOFade(1f, 0.1f);
            hpDots[1].DOFade(1f, 0.1f);
            hpDots[2].DOFade(1f, 0.1f);
            displayingHp = true;
        }
        
    }
    public void HideHp()
    {
        displayingHp = false;
        //hpPos.DOMove(Vector3.right * 100f , 0.1f);
        //hpPos.DOAnchorPos(Vector2.right * 100f, 0.1f);
        hpDots[0].DOFade(0f, 0.1f);
        hpDots[1].DOFade(0f, 0.1f);
        hpDots[2].DOFade(0f, 0.1f);
    }
    private void HpUiTimer()
    {
        if (displayingHp)
        {
            hpDisplayTimer -= Time.deltaTime;
            if (hpDisplayTimer <= 0f)
            {
                HideHp();
            }
        }
    }
    public void HpDamage(int hp)
    {
        //hpDots[hp % 3].color = hpLostColor;
        DisplayHp();
        hpDots[hp % 3].DOColor(hpLostColor, 0.2f);
        hpCorner[hp % 3].DOColor(hpLostColor, 0.2f);
        //DisplayHp();
        
    }
    public void HpHeal(int hp)
    {
        //hpDots[hp % 3].color = hpColor;
        DisplayHp();
        hpDots[hp % 3].DOColor(hpColor, 0.2f);
        hpCorner[hp % 3].DOColor(hpColor, 0.2f);
        //DisplayHp();
    }

    public void UsingShield(float currentShield)
    {
        shieldCorner.fillAmount = currentShield * 0.01f;
        shieldPos.GetComponent<Image>().fillAmount = currentShield * 0.01f;
        //shieldPos.DOAnchorPos(Vector2.zero, 0.1f);
        shieldPos.gameObject.GetComponent<Image>().DOFade(0.85f, 0.1f);        
        displayingShield = true;
    }
    public void ChargingShield(float currentShield)
    {
        shieldCorner.fillAmount = currentShield * 0.01f;
        shieldPos.GetComponent<Image>().fillAmount = currentShield * 0.01f;
        shieldPos.GetComponent<Image>().DOFade(0.40f, 0.1f);
    }
    public void HideShield()
    {
        if (displayingShield)
        {
            displayingShield = false;
            //shieldPos.DOAnchorPos(Vector3.down * 200f, 0.1f);
            shieldPos.gameObject.GetComponent<Image>().DOFade(0f, 0.1f);
        }
    }
}
