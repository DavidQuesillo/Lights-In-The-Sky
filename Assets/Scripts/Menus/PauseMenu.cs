using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private int titleScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosePause()
    {
        GameManager.instance.ClosePauseMenu();
    }

    public void OpenOptions()
    {

    }

    public void ReturnToTitle()
    {
        GameManager.instance.ChangeScene(titleScene);
    }
}
