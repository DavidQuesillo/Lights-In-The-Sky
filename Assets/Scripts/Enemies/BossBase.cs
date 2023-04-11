using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossBase : MonoBehaviour
{
    [SerializeField] protected string BossName;
    [SerializeField] protected TextMeshProUGUI nameSlot;
    [SerializeField] protected Image hpFill;
    [SerializeField] protected float Health = 600f;
    [SerializeField] protected float totalHealth = 0f;

    protected virtual void BeginBossFight()
    {
        GameManager.instance.DisplayBossUi();
        transform.parent = null;
    }

    public virtual void TakeDamage(float dmg)
    {
        Health -= dmg;
        hpFill.fillAmount = Health / totalHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShot"))
        {
            TakeDamage(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }
}
