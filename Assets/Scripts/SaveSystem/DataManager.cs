using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Awake()
    {
        if (Data.GetInitialized()) return;

        Data.Load();
        Data.Save();
        
        Data.LoadScoreboard();
        Data.SaveScore();
    }
}
