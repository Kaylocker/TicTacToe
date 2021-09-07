using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("2D MODE")]
    [SerializeField] private Sprite[] gameIcon;
    [SerializeField] private Button[] gameGridButtons;
    [SerializeField] private GameObject[] currentGameSymbolIcon;

    [Header("3D MODE")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] currentGameSymbols;

    [SerializeField] private Button changerPlayerSymbol;

    private Button3D[] buttonsStatus;
    private static List<GameObject> activeSymbols;

    private static bool isEnemyTurn;
    private static bool isGameActive;
    private static bool isCurrentGameEnded = false;
    private static int currentSymbol = GAMESYMBOL_O;
    private static int?[] gameGridButtonsStatus;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1, gameMode2D = 0, gameMode3D = 1;
    private int currentGameMode;

    public static List<GameObject> ActiveSymbols { get => activeSymbols; }
    public static bool IsGameActive { get => isGameActive; set => isGameActive = value; }
    public static bool IsCurrentGameEnded
    {
        get => isCurrentGameEnded;

        set
        {
            isCurrentGameEnded = value;

            if (isCurrentGameEnded)
            {
                isGameActive = false;
            }
        }
    }
    public static bool ResetGame { get; set; }

    private void Awake()
    {
        currentGameMode = Scenes.GetCurrentGameMode();
    }

    private void Start()
    {
        SetStartSettings();
        SetStartGameSymbol();
    }

    private void Update()
    {
        if (ResetGame)
        {
            ResetGame = false;
            changerPlayerSymbol.interactable = true;
            isCurrentGameEnded = false;

            if (currentGameMode == gameMode3D)
            {
                DestroyGameSymbols();
                ResetButtonsProperties();
            }

            SetStartSettings();
        }

        if (Button3D.CheckStep() == true && !isEnemyTurn)
        {
            isGameActive = true;
            isEnemyTurn = true;
            changerPlayerSymbol.interactable = false;

            ButtonAction3D();
        }
    }

    private void SetStartSettings()
    {
        isGameActive = false;
        isEnemyTurn = false;

        if (currentGameMode == gameMode2D)
        {
            gameGridButtonsStatus = new int?[gameGridButtons.Length];

            for (int i = 0; i < gameGridButtons.Length; i++)
            {
                gameGridButtons[i].interactable = true;
                gameGridButtons[i].image.sprite = null;
                gameGridButtonsStatus[i] = null;
            }
        }
        else
        {
            activeSymbols = new List<GameObject>();
            GetButtonsStatusArray();
        }

    }

    private void GetButtonsStatusArray()
    {
        buttonsStatus = new Button3D[buttons.Length];

        int counter = 0;

        foreach (var item in buttons)
        {
            buttonsStatus[counter] = item.GetComponent<Button3D>();
            counter++;
        }
    }

    public void ButtonAction(int numberGridButton)
    {
        isGameActive = true;

        if (isEnemyTurn || isCurrentGameEnded)
        {
            return;
        }

        changerPlayerSymbol.interactable = false;

        gameGridButtons[numberGridButton].image.sprite = gameIcon[currentSymbol];
        gameGridButtonsStatus[numberGridButton] = currentSymbol;
        gameGridButtons[numberGridButton].interactable = false;

        isEnemyTurn = true;
    }

    public void ButtonAction3D()
    {
        int counter = 0;

        foreach (var item in buttonsStatus)
        {
            if (buttonsStatus[counter].IsButtonActivated && buttonsStatus[counter].IsWorking)
            {
                Vector3 symbolPos = buttons[counter].transform.position;
                symbolPos += Vector3.up;
                GameObject gameSymbol = Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);
                Button3D.SetStep();
                activeSymbols.Add(gameSymbol);

                buttonsStatus[counter].GameSymbol = currentSymbol;
                buttonsStatus[counter].ButtonOff();
            }

            counter++;
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
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(false);
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(true);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_O].SetActive(false);
                currentGameSymbols[GAMESYMBOL_X].SetActive(true);
            }

            currentSymbol = GAMESYMBOL_X;
        }
        else
        {
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(false);
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(true);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_X].SetActive(false);
                currentGameSymbols[GAMESYMBOL_O].SetActive(true);
            }

            currentSymbol = GAMESYMBOL_O;
        }
    }

    private void SetStartGameSymbol()
    {
        if (currentSymbol == GAMESYMBOL_O)
        {
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(true);
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(false);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_O].SetActive(true);
                currentGameSymbols[GAMESYMBOL_X].SetActive(false);
            }
        }
        else
        {
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(false);
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(true);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_O].SetActive(false);
                currentGameSymbols[GAMESYMBOL_X].SetActive(true);
            }
               
        }
    }

    public static bool CheckEnemyTurn()
    {
        return isEnemyTurn;
    }

    public static void SetEnemyTurn(bool turn)
    {
        isEnemyTurn = turn;
    }

    public static int GetCurrentPlayerSymbol()
    {
        return currentSymbol;
    }

    public static void SetEnemyButtonStatus(int numberButton, int symbolNumber)
    {
        if (gameGridButtonsStatus[numberButton] == null)
        {
            gameGridButtonsStatus[numberButton] = symbolNumber;
        }
    }

    public static int? GetConcreteGridButtonStatus(int numberButton)
    {
        return gameGridButtonsStatus[numberButton];
    }

    public static void AddGameSymbol(GameObject gameSymbol)
    {
        activeSymbols.Add(gameSymbol);
    }

    private void ResetButtonsProperties()
    {
        foreach (var item in buttonsStatus)
        {
            item.ResetButton();
        }
    }

    private void DestroyGameSymbols()
    {
        foreach (var item in activeSymbols)
        {
            Destroy(item);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}


