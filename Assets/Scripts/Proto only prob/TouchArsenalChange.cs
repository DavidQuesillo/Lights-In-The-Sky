using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchArsenalChange : MonoBehaviour
{
    public List<Weapon> arsenal = new List<Weapon>();
    public void GivePlayerArsenal()
    {
        GameManager.instance.player.GetComponent<Player>().AssignNewArsenal(arsenal);
    }
}
