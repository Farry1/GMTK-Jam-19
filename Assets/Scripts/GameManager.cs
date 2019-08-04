using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { PreLevel, Intro, EnemyTurn, PlayerTurn, GameOver, Outro, Win }
    public GameState gameState = GameState.Intro;

    public Levelmessage levelmessage;
    public Text turnText;
    private bool enemyTurnStarted = false;

    GameObject[] allFairiesObjects;
    List<Fairy> allFairies = new List<Fairy>();

    public Sprite playerTurnIndicatorSprite;
    public Sprite enemyTurnIndicatorSprite;
    Image turnIndicatorImage;

    public string nextSceneName;

    bool isPlayingOutro = false;
    bool isPlayingIntro = false;
    bool isHandlingGameOver = false;

    private FMODUnity.StudioEventEmitter ambienceEmitter;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        ambienceEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
        turnIndicatorImage = transform.Find("Canvas").Find("TurnIndicatorImage").GetComponent<Image>();
        allFairiesObjects = GameObject.FindGameObjectsWithTag("Player");
        SetAllFairies();
        GhostController.Instance.HighlightTargetWaypoint();
    }

    void SetAllFairies()
    {
        foreach (GameObject go in allFairiesObjects)
        {
            allFairies.Add(go.GetComponent<Fairy>());
        }
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.PreLevel:
                break;

            case GameState.Intro:
                turnText.text = levelmessage.levelName;
                if (!isPlayingIntro)
                    StartCoroutine(IntroAnimation());
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.PlayerTurn:
                turnText.text = "Player Turn";
                CheckWinCondition();
                break;

            case GameState.Outro:
                if (!isPlayingOutro)
                    StartCoroutine(OutroAnimations());
                break;
            case GameState.GameOver:
                if (!isHandlingGameOver)
                    StartCoroutine(HandleGameOver());

                turnText.text = "GameOver";
                break;
        }
    }

    public void CheckForGameOver()
    {
        if (FairyMovementController.Instance.AllFairiesPetrified())
            gameState = GameState.GameOver;

    }

    void CheckWinCondition()
    {
        if (FairyMovementController.Instance.AllFairiesInTeamRange() && ! FairyMovementController.Instance.NoFairyPetrified())
        {
            gameState = GameState.Outro;
            turnText.text = "Won";


            //UIController.Instance.ShowWinOverlay();
        }
    }

    public void SwitchToIntro()
    {
        gameState = GameState.Intro;
    }

    public void SwitchToPlayerTurn()
    {
        gameState = GameState.PlayerTurn;
        turnText.text = "Player Turn";
        UIController.Instance.turnButton.SetActive(true);
        GhostController.Instance.HighlightTargetWaypoint();
        FairyMovementController.Instance.ResetAllFairies();
        turnIndicatorImage.sprite = playerTurnIndicatorSprite;
    }

    public void SwitchToEnemyTurn()
    {
        GhostController.Instance.ResetHighlightColor();
        gameState = GameState.EnemyTurn;
        turnText.text = "Enemy Turn";
        UIController.Instance.turnButton.SetActive(false);
        turnIndicatorImage.sprite = enemyTurnIndicatorSprite;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator HandleGameOver()
    {
        isHandlingGameOver = true;
        UIController.Instance.turnButton.SetActive(false);
        yield return new WaitForSeconds(2f);
        UIController.Instance.ShowGameOverContainer();
    }


    IEnumerator IntroAnimation()
    {
        ambienceEmitter.Play();
        UIController.Instance.turnButton.SetActive(false);
        isPlayingIntro = true;
        foreach (Fairy fairy in FairyMovementController.Instance.allFairies)
        {
            fairy.PlayIntroAnimation();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);
        SwitchToPlayerTurn();
    }

    IEnumerator OutroAnimations()
    {
        isPlayingOutro = true;

        foreach (Fairy fairy in FairyMovementController.Instance.allFairies)
        {
            fairy.PlayOutroAnimation();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        FairyMovementController.Instance.DeactivateAllFairies();
        gameState = GameState.Win;
        UIController.Instance.ShowWinOverlay();
    }
}
