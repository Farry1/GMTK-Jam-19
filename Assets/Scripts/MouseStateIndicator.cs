using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateIndicator : MonoBehaviour
{
    [HideInInspector] public enum MouseState { Danger, Default };
    [HideInInspector] public MouseState mouseState;
    public Vector3 offset = new Vector3(0, 0.5f, 0);

    private SpriteRenderer spriteRenderer;

    private static MouseStateIndicator _instance;
    public static MouseStateIndicator Instance { get { return _instance; } }

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UnsetMouseState();        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit mousePosition;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mousePosition, Mathf.Infinity))
        {
            transform.position = mousePosition.point + offset;

        }
    }

    public void SetMouseStateDanger()
    {
        spriteRenderer.gameObject.SetActive(true);
        mouseState = MouseState.Danger;
    }

    public void UnsetMouseState()
    {
        spriteRenderer.gameObject.SetActive(false);
        mouseState = MouseState.Default;
    }
}
