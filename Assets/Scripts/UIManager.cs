using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Timer : " + gameManager.currentTime.ToString("0");
        scoreText.text = "Score : " + gameManager._score.ToString();
    }

    public void LoadScore()
    {
        if(gameManager.difficultMode == 1)
        {
            bestScoreText.text = "Best score : " + PlayerPrefs.GetInt("Easy").ToString();
        }
        else if(gameManager.difficultMode == 2)
        {
            bestScoreText.text = "Best score : " + PlayerPrefs.GetInt("Hard").ToString();
        }
        else if(gameManager.difficultMode == 3)
        {
            bestScoreText.text = "Best score : " + PlayerPrefs.GetInt("Challenge").ToString();
        }
    }
}
