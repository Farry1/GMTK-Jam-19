using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    public static UIController Instance { get { return _instance; } }

    public GameObject overlayRendererObj;
    private Image overlayRenderer;
    public GameObject turnIndicator;

    public GameObject preLevelContainer;
    public GameObject winContainer;
    public GameObject gameOverContainer;
    public GameObject nextTurnButton;
    public GameObject inGameMenu;
    public GameObject instructions;
    public GameObject menuButton;
    public Text sceneHeadline;
    public Text sceneDescription;
    public Text sceneLocation;
    public Text sceneTipsText;
    public Text sceneDoneText;
    public GameObject nextTurnTooltip;

    private string[] tipsContainer =
        { "Use Middle Mouse Button to rotate",
        "If a Shadow Fairy is petrified it can be resurrected if another one is nearby.",
        "Sometimes the Raycast of the light is bad, sorry :("
        };

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


    // Start is called before the first frame update
    void Start()
    {
        overlayRenderer = overlayRendererObj.GetComponent<Image>();
        overlayRenderer.gameObject.SetActive(true);

        sceneHeadline.text = GameManager.Instance.levelNumber;
        sceneDescription.text = GameManager.Instance.levelDescription;
        sceneLocation.text = GameManager.Instance.levelName;
        //sceneTipsText.text = GameManager.Instance.additionalGameOverText;
        sceneTipsText.text = tipsContainer[Random.Range(0, tipsContainer.Length)];
        sceneDoneText.text = GameManager.Instance.levelNumber + " complete";

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOutOverlay(float fadeSpeed)
    {
        preLevelContainer.SetActive(false);
        StartCoroutine(FadeOut(fadeSpeed));
    }

    public void FadeInOverlay(float fadeSpeed)
    {
        overlayRenderer.gameObject.SetActive(true);
        //continueButton.SetActive(true);
        StartCoroutine(FadeIn(fadeSpeed));
    }

    public void ShowWinOverlay()
    {
        turnIndicator.SetActive(false);
        FadeInOverlay(0.15f);
        winContainer.SetActive(true);
    }

    public void ShowGameOverContainer()
    {
        turnIndicator.SetActive(false);
        gameOverContainer.SetActive(true);
    }

    public void ShowInGameMenu()
    {
        nextTurnButton.SetActive(false);
        inGameMenu.SetActive(true);
        menuButton.SetActive(false);
    }

    public void HideInGameMenu()
    {
        nextTurnButton.SetActive(true);
        inGameMenu.SetActive(false);
        instructions.SetActive(false);
        menuButton.SetActive(true);
    }

    IEnumerator FadeOut(float fadeSpeed)
    {
        Color newColor = overlayRenderer.color;
        for (float f = 1f; f >= 0; f -= fadeSpeed)
        {
            newColor.a = f;
            overlayRenderer.color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
        overlayRenderer.gameObject.SetActive(false);

    }

    IEnumerator FadeIn(float fadeSpeed)
    {
        Color newColor = overlayRenderer.color;
        for (float f = 0f; f <= 2f; f += fadeSpeed)
        {
            newColor.a = f;
            overlayRenderer.color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }
}

