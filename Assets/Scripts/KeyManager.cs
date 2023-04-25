using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    [SerializeField] private bool redKey;
    [SerializeField] private bool blueKey;
    [SerializeField] private bool yellowKey;
    [SerializeField] private bool greenKey;

    private void Awake()
    {
        instance = this;
    }

    public void PickUpKey(Keys keyColor)
    {
        switch (keyColor)
        {
            case Keys.Red:
                redKey = true;
                break;
            case Keys.Blue:
                blueKey = true;
                break;
            case Keys.Yellow:
                yellowKey = true;
                break;
            case Keys.Green:
                greenKey = true;
                break;
            default:
                break;
        }
    }

    public bool CheckForKey(Keys keyColor)
    {
        switch (keyColor)
        {
            case Keys.Red:
                return redKey;
            case Keys.Blue:
                return blueKey;
            case Keys.Yellow:
                return yellowKey;
            case Keys.Green:
                return greenKey;
            default:
                return false;
        }
    }
}
