using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Sprite[] imageObject;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private List<Card> currentCard;
    public Card card1;
    public Card card2;

    [Header("Game Setting")]
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private float maxTime;
    public float delayStartTime;
    public float currentTime;
    private int score;
    public int _score;
    private int combo;
    private bool timerRunning = false;
    private bool isStart;

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
        }
        
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
            
        }
        else
        {
            card1.CardReset();
            card2.CardReset();
            combo = 0;
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

#endregion

#region GameScore
    public void GetScore(GameObject _scoreText)
    {
        combo++;
        score += 1000 * combo;
        _scoreText.GetComponentInChildren<TextMeshProUGUI>().text = "+" + (1000 * combo).ToString();
    }
#endregion
}
