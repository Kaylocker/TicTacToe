using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController3D : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] currentGameSymbols;

    private ButtonActionController3D[] buttonsStatus;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private static int currentSymbol = GAMESYMBOL_O;
    private static bool isEnemyTurn;
    private static bool isGameActive;

    public static int CurrentSymbol { get => currentSymbol; }
    private void Start()
    {
        SetStartGameSymbol();
        StartSettings();
    }

    private void Update()
    {
        if (ButtonActionController3D.CheckIsMakeStep() == true)
        {
            isGameActive = true;

            ButtonActionController3D.SetIsStepMaked(false);
            InstantinaiteGameSymbol();
        }
    }

    private void StartSettings()
    {
        isEnemyTurn = false;
        isGameActive = false;
        SetButtonsStatusArray();
    }

    private void SetStartGameSymbol()
    {
        if (currentSymbol == GAMESYMBOL_O)
        {
            currentGameSymbols[GAMESYMBOL_O].SetActive(true);
            currentGameSymbols[GAMESYMBOL_X].SetActive(false);
        }
        else
        {
            currentGameSymbols[GAMESYMBOL_O].SetActive(false);
            currentGameSymbols[GAMESYMBOL_X].SetActive(true);
        }
    }

    public void ChangeCurrentGameSymbol()
    {
        if (isGameActive)
        {
            return;
        }

        if (currentSymbol == GAMESYMBOL_O)
        {
            currentGameSymbols[GAMESYMBOL_O].SetActive(false);
            currentGameSymbols[GAMESYMBOL_X].SetActive(true);
            currentSymbol = GAMESYMBOL_X;
        }
        else
        {
            currentGameSymbols[GAMESYMBOL_X].SetActive(false);
            currentGameSymbols[GAMESYMBOL_O].SetActive(true);
            currentSymbol = GAMESYMBOL_O;
        }
    }

    private void SetButtonsStatusArray()
    {
        buttonsStatus = new ButtonActionController3D[buttons.Length];

        int counter = 0;

        foreach (var item in buttons)
        {
            buttonsStatus[counter] = item.GetComponent<ButtonActionController3D>();
            print(buttonsStatus[counter].IsButtonActivated);
            counter++;
        }
    }
    public void InstantinaiteGameSymbol()
    {
        int counter = 0;

        foreach (var item in buttonsStatus)
        {
            if (buttonsStatus[counter].IsButtonActivated && buttonsStatus[counter].IsWorking)
            {
                Vector3 symbolPos = buttons[counter].transform.position;
                symbolPos += Vector3.up;
                Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);

                buttonsStatus[counter].ButtonOff();
            }

            counter++;
        }
    }

}
