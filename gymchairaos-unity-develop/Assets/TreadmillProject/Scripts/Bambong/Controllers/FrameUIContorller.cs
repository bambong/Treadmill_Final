using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrameUIContorller : MonoBehaviour
{
    [Range(10, 150)]
    public int fontSize = 30;
    public Color color = new Color(.0f, .0f, .0f, 1.0f);
    public float width, height;

    [SerializeField]
    private TextMeshProUGUI termText;
    private float curTerm;
    void OnGUI()
    {
        Rect position = new Rect(width, height, Screen.width, Screen.height);

        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;
        string text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = color;

        Rect uiPosition = new Rect(width, height +fontSize, Screen.width, Screen.height);
      
        GUI.Label(position, text, style);

        GUI.Label(uiPosition, SceneManager.GetActiveScene().name, style);
        uiPosition.y += fontSize;
        GUI.Label(uiPosition, $"L RPM : {TokenInputManager.Instance.Save_left_rpm}", style);
        uiPosition.y += fontSize;
        GUI.Label(uiPosition, $"R RPM : {TokenInputManager.Instance.Save_right_rpm}", style);
    }

    private void Start()
    {
        TokenInputManager.Instance.ReceivedEvent += ClearTerm;
    }
    public void ClearTerm() 
    {
        curTerm = 0;
        UpdateTermTextUi();
    }
    private void Update()
    {
        curTerm += Time.deltaTime;
        UpdateTermTextUi();
    }

    private void UpdateTermTextUi() 
    {
        termText.text = curTerm.ToString("f2");
    } 
}