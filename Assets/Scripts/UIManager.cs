using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text scoreText;
    public Canvas endGameCanvas;
    public Text endScore;
    public Button resetButton;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    //UI initial method.
    public void OnStartUI() 
    {
        resetButton.onClick.AddListener(GameManager.instance.RestartGame);
        scoreText.text = "Score: " + GameManager.instance.points.ToString();
        endGameCanvas.gameObject.SetActive(false);
    }
    //Activates the canvas that will be showed in the end of the game.
    public void ActivateEndGameUI() 
    {
        endGameCanvas.gameObject.SetActive(true);
        endScore.text = scoreText.text;
    }
}
