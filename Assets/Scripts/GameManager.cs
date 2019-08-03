using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Intro, EnemyTurn, PlayerTurn, GameOver }
    public GameState gameState = GameState.Intro;

    public Text turnText;
    public GameObject turnButton;

    private bool enemyTurnStarted = false;

    GameObject[] allFairiesObjects;
    List<Fairy> allFairies = new List<Fairy>();


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
            case GameState.Intro:
                SwitchToPlayerTurn();
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.PlayerTurn:
                turnText.text = "Player Turn";
                break;
            case GameState.GameOver:
                turnText.text = "GameOver";
                break;
        }
    }

    public void CheckForGameOver()
    {
        if (AllFairiesPetrified())
            gameState = GameState.GameOver;

    }

    private bool AllFairiesPetrified()
    {
        foreach (Fairy fairy in allFairies)
        {
            if (fairy.fairyState == Fairy.FairyState.Alive)
                return false;
        }

        return true;
    }


    public void SwitchToPlayerTurn()
    {
        gameState = GameState.PlayerTurn;
        turnText.text = "Player Turn";
        turnButton.SetActive(true);
        GhostController.Instance.HighlightTargetWaypoint();
        FairyMovementController.Instance.ResetAllFairies();
    }

    public void SwitchToEnemyTurn()
    {
        GhostController.Instance.ResetHighlightColor();
        gameState = GameState.EnemyTurn;
        turnText.text = "Enemy Turn";
        turnButton.SetActive(false);
    }
}
