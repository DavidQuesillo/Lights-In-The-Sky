using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameStates { Gameplay, Restart, GameOver, Title, CutScene }

public class GameManager : MonoBehaviour
{
    public EGameStates currentState = EGameStates.Gameplay;
    public delegate void FNotifyGameStateChange(EGameStates newGameState);
    public static event FNotifyGameStateChange OnGameStageChange;
    public static GameManager instance;
    public GameObject player;
    [Header("UI")]
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject bossUI;
    public GameplayUI UiScript;
    [Header("Music")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip stageSong;
    [SerializeField] private AudioClip bossSong;
    //public bool canPause = true;
    public bool paused = false;
    //public bool gameover = false;

    [SerializeField] private List<HiScore> scoreboard;

    private void Awake()
    {
        instance = this;
        ChangeGameState(EGameStates.Gameplay);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (Data.GetScoreData().GetFullScoreboardData() != null)
        {
            scoreboard = Data.GetScoreData().GetFullScoreboardData();
        }
        else
        {
            print("couldnt get scoreboard");
        }
    }

    public void DebugHiscoreSet() //press a key and save a hardcoded score into the board
    {

    }

    public void ChangeGameState(EGameStates newState)
    {
        currentState = newState;
        OnGameStageChange?.Invoke(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PauseMenu.activeInHierarchy && currentState == EGameStates.Gameplay)
        {
            ClosePauseMenu();
            Debug.Log("Unpaused");
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseMenu.activeInHierarchy && currentState == EGameStates.Gameplay)
        {
            OpenPauseMenu();
            Debug.Log("Paused");
        }
    }

    public void OpenPauseMenu()
    {
        PauseMenu.SetActive(true);
        gameplayUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<Player>().canLookAround = false;
        player.GetComponent<Player>().canShoot = false;
        paused = true;
    }
    public void ClosePauseMenu()
    {
        PauseMenu.SetActive(false);
        gameplayUI.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<Player>().canLookAround = true;
        player.GetComponent<Player>().canShoot = true;
        paused = false;
    }
    public void DisplayBossUi()
    {
        bossUI.SetActive(true);
    }

    public void GameOver()
    {
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<Player>().canLookAround = false;
        player.GetComponent<Player>().canShoot = false;
        Time.timeScale = 0f;
        ChangeGameState(EGameStates.GameOver);
        aus.Stop();
    }

    public IEnumerator RetryStage()
    {
        if (currentState == EGameStates.GameOver)
        {
            Debug.Log("retry press");
            ChangeGameState(EGameStates.Restart);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
            //UiManager.instance.DisplayGameOver(false);
            gameOverUI.SetActive(false);
            ChangeGameState(EGameStates.Gameplay);
            Debug.Log("retry ready");
        }
    }

    public void PlayBossSong()
    {
        aus.clip = bossSong;
        aus.volume = 0.15f;
        aus.Play();
    }
    /*public void PlayMusic() //should probably be able to add argument song input
    {
        aus.Play();
    }*/

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
