using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Button[] gameGridButton;
    [SerializeField] private Sprite[] gameIcon;

    private bool myTurn = false;
    private int currentSymbol;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private List<Button> currentActiveButtons;
    private List<int> concreteNumbersGridButtons;

    private void Update()
    {
        myTurn = GameController.CheckEnemyTurn();

        if (myTurn == false || GameController.IsCurrentGameEnded)
        {
            return;
        }

        currentActiveButtons = new List<Button>();
        concreteNumbersGridButtons = new List<int>();

        GameController.SetEnemyTurn(false);

        SetCurrentSymbol();
    }

    private void SetCurrentSymbol()
    {
        int playerSymbol = GameController.GetCurrentPlayerSymbol();

        if (playerSymbol == GAMESYMBOL_O)
        {
            currentSymbol = GAMESYMBOL_X;
        }
        else
        {
            currentSymbol = GAMESYMBOL_O;
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
                concreteNumbersGridButtons.Add(counter);
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
                int concreteNumber = concreteNumbersGridButtons[counter];

                GameController.SetEnemyButtonStatus(concreteNumber, currentSymbol);
                item.image.sprite = gameIcon[currentSymbol];
                item.interactable = false;
            }

            counter++;
        }

        concreteNumbersGridButtons = null;
        currentActiveButtons = null;
    }
}
