using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{   
    #region Card Value
    public GameManager gameManager;
    [SerializeField] private Image imageObject;
    [SerializeField] private GameObject doorClose;
    [SerializeField] private GameObject cardPivot;
    private int cardIndex;
    private bool isOpen;
    #endregion

    #region Referrent
    public bool _isOpen;
    public int _cardIndex;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = true;
        StartCoroutine(ShowCard());
    }
    public void SetObject(Sprite _object, int _index)
    {
        imageObject.sprite = _object;
        cardIndex = _index;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen)
        {
            doorClose.SetActive(false);
        }
        else
        {
            doorClose.SetActive(true);
        }

        _isOpen = isOpen;
        _cardIndex = cardIndex;

    }

    public void OnClick()
    {
        if(!isOpen)
        {
            gameManager.GetCard(this);
            isOpen = true;
        }
    }

    public void CardReset()
    {
        StartCoroutine(DelayReset());
        
    }

    public void HideCard()
    {
        StartCoroutine(DelayHide());
    }

    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(0.5f);
        isOpen = false;
        
    }
    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(0.5f);
        cardPivot.SetActive(false);
        
    }
    IEnumerator ShowCard()
    {
        yield return new WaitForSeconds(gameManager.delayStartTime);
        isOpen = false;
    }
}
