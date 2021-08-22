using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Button[] gameGridButton;
    public Sprite[] gameIcon;

    private bool myTurn = false;
    private int? currentSymbol;
    private List<Button> currentActiveButtons;

    private void Update()
    {
        myTurn = GameController.CheckEnemyTurn();

        if (myTurn == false || GameController.IsCurrentGameEnded)
        {
            return;
        }

        currentActiveButtons = new List<Button>();
        GameController.SetEnemyTurn(false);
        SetCurrentSymbol();
    }
    private void SetCurrentSymbol()
    {
        int playerSymbol = GameController.GetCurrentPlayerSymbol();

        if (playerSymbol == (int)GameSymbol.O)
        {
            currentSymbol = (int)GameSymbol.X;
        }
        else
        {
            currentSymbol = (int)GameSymbol.O;
        }

        GetCurrentActiveButtons();
    }
    private void GetCurrentActiveButtons()
    {
        int counter = 0;
        
        foreach (var item in gameGridButton)
        {
            if (gameGridButton[counter].interactable)
            {
                currentActiveButtons.Add(item);
            }

            counter++;
        }

        MakeStep();
    }
    private void MakeStep()
    {
        int random = Random.Range(0, currentActiveButtons.Count);
        int counter = 0;

        foreach (var item in currentActiveButtons)
        {
            if (counter == random)
            {
                item.image.sprite = gameIcon[(int)currentSymbol];
                item.interactable = false;
            }

            counter++;
        }

        currentActiveButtons = null;
    }
}
