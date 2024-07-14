using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject mainMenu;
    public GameObject menu;
    public GameObject DifficultySelect;
    public GameObject MainGame;
    public void StartEasyGame()
    {
        gameManager.StartGame(4,3);
        gameManager.difficultMode = 1;
        HideMenu();
    }
    public void StartHardGame()
    {
        gameManager.StartGame(5,4);
        gameManager.difficultMode = 2;
        HideMenu();
    }
    public void StartChallengeGame()
    {
        gameManager.StartGame(5,6);
        gameManager.difficultMode = 3;
        HideMenu();
    }

    public void HideMenu()
    {
        mainMenu.SetActive(false);
        MainGame.SetActive(true);
    }
}
