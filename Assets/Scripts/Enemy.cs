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

    [Header("COMMON")]
    [SerializeField] private GameController gameController;

    private List<Button> currentActiveButtonsIcons2D;
    private Button3D[] buttonsStatus;
    private List<GameObject> currentActiveButtons3D;
    private GameObject lastGameSymbol;
    private List<int> concreteNumbersGridButtons;

    private bool myTurn = false;
    private int currentSymbol;
    private int currentMode;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1, gameMode2D = 0, gameMode3D = 1;
    public GameObject LastGameSymbol { get => lastGameSymbol; }
    public bool EnemyTurn { get=> myTurn; set=> myTurn=value; }

    private void Awake()
    {
        currentMode = Scenes.CurrentGameMode;
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
        if (myTurn == false || gameController.IsCurrentGameEnded)
        {
            return;
        }

        if (currentMode == gameMode2D)
        {
            currentActiveButtonsIcons2D = new List<Button>();
        }
        else
        {
            currentActiveButtons3D = new List<GameObject>();
        }

        concreteNumbersGridButtons = new List<int>();
        myTurn = false;

        SetCurrentSymbol();
    }

    private void SetCurrentSymbol()
    {
        int playerSymbol = gameController.GetCurrentPlayerSymbol();

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
                    currentActiveButtonsIcons2D.Add(item);
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
                    currentActiveButtons3D.Add(buttons[counter]);
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
        currentActiveButtonsIcons2D = null;
    }

    private void MakeStep2DMode()
    {
        int random = Random.Range(0, currentActiveButtonsIcons2D.Count);
        int counter = 0;

        foreach (var item in currentActiveButtonsIcons2D)
        {
            if (counter == random)
            {
                int concreteNumber = concreteNumbersGridButtons[counter];

                gameController.SetEnemyButtonStatus(concreteNumber, currentSymbol);
                item.image.sprite = gameIcon[currentSymbol];
                item.interactable = false;
            }

            counter++;
        }
    }

    private void MakeStep3DMode()
    {
        int random = Random.Range(0, currentActiveButtons3D.Count);
        int counter = 0;

        foreach (var item in currentActiveButtons3D)
        {
            if (counter == random)
            {
                int concreteNumber = concreteNumbersGridButtons[counter];

                Vector3 symbolPos = buttons[concreteNumber].transform.position;
                symbolPos += Vector3.up;
                GameObject gameSymbol = Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);

                lastGameSymbol = gameSymbol;

                buttonsStatus[concreteNumber].GameSymbol = currentSymbol;
                buttonsStatus[concreteNumber].ButtonOff();

                Button3D.SetStepIsMaked();
            }

            counter++;
        }
    }
}
