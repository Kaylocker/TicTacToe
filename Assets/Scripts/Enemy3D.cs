using System.Collections.Generic;
using UnityEngine;

public class Enemy3D : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;

    private Button3D[] buttonsStatus;
    private List<GameObject> currentActiveButtons;
    private List<int> concreteNumbersGridButtons;
    private bool myTurn = false;
    private int currentSymbol;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;

    private void Start()
    {
        SetButtonsStatusArray();
    }

    private void Update()
    {
        myTurn = GameController3D.CheckEnemyTurn();

        if (myTurn == false || GameController3D.IsCurrentGameEnded)
        {
            return;
        }

        currentActiveButtons = new List<GameObject>();
        concreteNumbersGridButtons = new List<int>();

        GameController3D.SetEnemyTurn(false);

        SetCurrentSymbol();
    }

    private void SetButtonsStatusArray()
    {
        buttonsStatus = new Button3D[buttons.Length];

        int counter = 0;

        foreach (var item in buttons)
        {
            buttonsStatus[counter] = item.GetComponent<Button3D>();
            counter++;
        }
    }

    private void SetCurrentSymbol()
    {
        int playerSymbol = GameController3D.GetCurrentPlayerSymbol();

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

        foreach (var item in buttonsStatus)
        {
            if (buttonsStatus[counter].IsWorking)
            {
                currentActiveButtons.Add(buttons[counter]);
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

                Vector3 symbolPos = buttons[concreteNumber].transform.position;
                symbolPos += Vector3.up;
                Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);

                buttonsStatus[concreteNumber].ButtonOff();

                Button3D.SetStepMaked();
                GameController3D.SetEnemyButtonStatus(concreteNumber, currentSymbol);
            }

            counter++;
        }

        concreteNumbersGridButtons = null;
        currentActiveButtons = null;
    }
}
