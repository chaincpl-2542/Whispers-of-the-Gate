using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Sprite[] imageObject;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private List<Card> currentCard;
    public Card card1;
    public Card card2;
    public SoundManager soundManager;
    [Header("Game Setting")]
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private float maxTime;
    public float delayStartTime;
    public float currentTime;
    public int difficultMode;
    private int score;
    public int _score;
    private int combo;
    private bool timerRunning = false;
    private bool isStart;
    
    public TextMeshProUGUI textFinalScore;
    public GameObject finalScore;

    public void Update()
    {
        if(isStart)
        {
            
            if(card1 && card2)
            {
                CardCheck();
            }

            if(currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
            }
            
            if(currentCard.Count == 0)
            {
                EndGame();
            }

            if(currentTime <= 0)
            {
                TimeUp();
            }
        }
        GetComponent<UIManager>().LoadScore();
        _score = score;

        
    }

#region GenerateCard
    public void GenerateCard(int cardTotal)
    {
        for(int i = 0; i < cardTotal; i++)
        {
            GameObject _card = Instantiate(cardPrefab,container.transform.position,Quaternion.identity);
            _card.GetComponent<Card>().gameManager = this;
            _card.transform.SetParent(container.transform);
            _card.transform.localScale = new Vector3(1, 1, 1);
            _card.name = ("Card " + (i + 1));
        }

        SetCardObject();
    }

    public void SetCardObject()
    {
        List<GameObject> cardRandomList = new List<GameObject>();
        foreach(Card child in container.transform.GetComponentsInChildren<Card>())
        {
            currentCard.Add(child);
            cardRandomList.Add(child.gameObject);
        }
    
        List<Sprite> remainingSprites = new List<Sprite>(imageObject);
        for (int i = 0; i < currentCard.Count; i++)
        {
            if (cardRandomList.Count == 0)
            {
                break;
            }

            int randomSpriteNum = Random.Range(0, remainingSprites.Count);
            
            for (int j = 0; j < 2; j++)
            {
                int randomCardNum = Random.Range(0, cardRandomList.Count);
                
                cardRandomList[randomCardNum].GetComponent<Card>().SetObject(remainingSprites[randomSpriteNum],i);
                cardRandomList.RemoveAt(randomCardNum);
            }
            remainingSprites.RemoveAt(randomSpriteNum);
        }
    }
#endregion

#region GetCard & Check
    public void GetCard(Card selectedCard)
    {
        if(!card1)
        {
            GetFirstCard(selectedCard);
        }
        else
        {
            GetSecondCard(selectedCard);
        }
        soundManager.PlayDoorSound();
    }
    private void GetFirstCard(Card selectedCard)
    {
        card1 = selectedCard;
    }
    private void GetSecondCard(Card selectedCard)
    {
        card2 = selectedCard;
    }

    public void CardCheck()
    {
        if(card1._cardIndex == card2._cardIndex)
        {
            card1.HideCard();
            card2.HideCard();
            GameObject scoreText = Instantiate(scorePrefab, card1.transform.position,Quaternion.identity);
            scoreText.transform.SetParent(card1.transform, false);
            Destroy(scoreText,1);

            currentCard.Remove(card1.GetComponent<Card>());
            currentCard.Remove(card2.GetComponent<Card>());

            GetScore(scoreText);
            soundManager.PlayCorrectSound();
            
        }
        else
        {
            card1.CardReset();
            card2.CardReset();
            combo = 0;
            soundManager.PlayWrongSound();
        }

        card1 = null;
        card2 = null;
    }
#endregion
    
#region Game condition
    public void StartGame(int _row,int _col)
    {
        GenerateCard(_row * _col);
        container.GetComponent<GridLayoutGroup>().constraintCount = _col;
        StartCoroutine(DelayStartGame());
        currentTime = maxTime; // Reset timer to starting value
    }

    public void EndGame()
    {
        isStart = false;
        timerRunning = false;
        textFinalScore.text = "Score : " + score.ToString();
        SaveScore(score,difficultMode);
        finalScore.SetActive(true);
    }

    public void TimeUp()
    {
        isStart = false;
        timerRunning = false;
        soundManager.PlayGameOverSound();
        textFinalScore.text = "Score : " + score.ToString();
        finalScore.SetActive(true);
        SaveScore(score,difficultMode);
    }

    IEnumerator DelayStartGame()
    {
        yield return new WaitForSeconds(delayStartTime);
        isStart = true;
        timerRunning = true;
    }

    public void ResetGame()
    {
        for (int i = container.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

#endregion

#region GameScore
    public void GetScore(GameObject _scoreText)
    {
        combo++;
        score += 1000 * combo;
        
        _scoreText.GetComponentInChildren<TextMeshProUGUI>().text = "+" + (1000 * combo).ToString();
    }

    public void SaveScore(int currentScore, int mode)
    {
        if(mode == 1)
        {
            int highScore = PlayerPrefs.GetInt("Easy");
            if(currentScore >= highScore)
            {
                PlayerPrefs.SetInt("Easy", currentScore);
            }
        }
        else if(mode == 2)
        {
            int highScore = PlayerPrefs.GetInt("Hard");
            if(currentScore >= highScore)
            {
                PlayerPrefs.SetInt("Hard", currentScore);
            }
        }
        else if(mode == 3)
        {
            int highScore = PlayerPrefs.GetInt("Challenge");
            if(currentScore >= highScore)
            {
                PlayerPrefs.SetInt("Challenge", currentScore);
            }
        }
    }
#endregion
}
