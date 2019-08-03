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


    public GameObject continueButton;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOutOverlay(float fadeSpeed)
    {
        continueButton.SetActive(false);
        StartCoroutine(FadeOut(fadeSpeed));
    }

    IEnumerator FadeOut(float fadeSpeed)
    {
        Debug.Log("Fading");
        Color newColor = overlayRenderer.color; 
        for (float f = 1f; f >= 0; f -= fadeSpeed)
        {
            newColor.a = f;
            overlayRenderer.color = newColor;
            yield return new WaitForSeconds(fadeSpeed);
        }
        Debug.Log("Fade Done");
    }
}

