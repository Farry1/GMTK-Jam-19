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
    public GameObject turnButton;
    public Text sceneHeadline;
    public Text sceneDescription;
    public Text sceneLocation;
    public Text sceneTipsText;
    public Text sceneDoneText;



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
        sceneHeadline.text = GameManager.Instance.levelmessage.levelNumber;
        sceneDescription.text = GameManager.Instance.levelmessage.levelDescription;
        sceneLocation.text = GameManager.Instance.levelmessage.levelName;
        sceneTipsText.text = GameManager.Instance.levelmessage.additionalGameOverText;
        sceneDoneText.text = GameManager.Instance.levelmessage.levelNumber + " complete";
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

    IEnumerator FadeOut(float fadeSpeed)
    {
        //Debug.Log("Fading");
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
        Debug.Log("Fade In");
        Color newColor = overlayRenderer.color;
        for (float f = 0f; f <= 2f; f += fadeSpeed)
        {
            newColor.a = f;
            overlayRenderer.color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
        Debug.Log("Fade Done");
    }
}

