using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("2D MODE")]
    [SerializeField] private Button[] gameGridButton;
    [SerializeField] private Sprite[] gameIcon;

    [Header("3D MODE")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;

    private List<Button> currentActiveButtonsIcons;

    private Button3D[] buttonsStatus;
    private List<GameObject> currentActiveButtons;
    private List<int> concreteNumbersGridButtons;

    private bool myTurn = false;
    private int currentSymbol;
    private int currentMode;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1, gameMode2D = 0, gameMode3D = 1;

    private void Awake()
    {
        currentMode = Scenes.GetCurrentGameMode();
    }

    private void Start()
    {
        if (currentMode == gameMode3D)
        {
            SetButtonsStatusArray();
        }
    }

    private void Update()
    {
        myTurn = GameController.CheckEnemyTurn();

        if (myTurn == false || GameController.IsCurrentGameEnded)
        {
            return;
        }

        if (currentMode == gameMode2D)
        {
            currentActiveButtonsIcons = new List<Button>();
        }
        else
        {
            currentActiveButtons = new List<GameObject>();
        }

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

    private void GetCurrentActiveButtons()
    {
        int counter = 0;

        if (currentMode == gameMode2D)
        {
            foreach (var item in gameGridButton)
            {
                if (gameGridButton[counter].interactable)
                {
                    currentActiveButtonsIcons.Add(item);
                    concreteNumbersGridButtons.Add(counter);
                }

                counter++;
            }
        }
        else
        {
            foreach (var item in buttonsStatus)
            {
                if (buttonsStatus[counter].IsWorking)
                {
                    currentActiveButtons.Add(buttons[counter]);
                    concreteNumbersGridButtons.Add(counter);
                }

                counter++;
            }
        }

        MakeStep();
    }

    private void MakeStep()
    {
        if (currentMode == gameMode2D)
        {
            MakeStep2DMode();
        }
        else
        {
            MakeStep3DMode();
        }

        concreteNumbersGridButtons = null;
        currentActiveButtonsIcons = null;
    }

    private void MakeStep3DMode()
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
                GameObject gameSymbol = Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);

                GameController.AddGameSymbol(gameSymbol);

                buttonsStatus[concreteNumber].GameSymbol = currentSymbol;
                buttonsStatus[concreteNumber].ButtonOff();

                Button3D.SetStep();
            }

            counter++;
        }
    }

    private void MakeStep2DMode()
    {
        int random = Random.Range(0, currentActiveButtonsIcons.Count);
        int counter = 0;

        foreach (var item in currentActiveButtonsIcons)
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
    }
}
