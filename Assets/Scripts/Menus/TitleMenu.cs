using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private int firstLevel;
    [SerializeField] private GameObject savesScreen;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject modesScreen;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void OpenSaves()
    {
        titleScreen.SetActive(false);
        savesScreen.SetActive(true);
    }
    public void ReturnToTitle(GameObject currentScreen)
    {
        titleScreen.SetActive(true);
        currentScreen.SetActive(false);
    }
    public void OpenModesScreen()
    {
        modesScreen.SetActive(true);
        savesScreen.SetActive(false);
    }
}
