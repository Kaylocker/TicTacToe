using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite[] gameIcon;
    [SerializeField] private Button[] gameGridButtons;
    [SerializeField] private Button changerPlayerSymbol;
    [SerializeField] private GameObject[] currentGameSymbolIcon;

    private static bool isEnemyTurn;
    private static bool isGameActive;
    private static bool isCurrentGameEnded = false;
    private static int currentSymbol = GAMESYMBOL_O;
    private static int?[] gameGridButtonsStatus;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;

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
            SetStartSettings();
            changerPlayerSymbol.interactable = true;
            isCurrentGameEnded = false;
        }
    }

    private void SetStartSettings()
    {
        isGameActive = false;
        isEnemyTurn = false;
        gameGridButtonsStatus = new int?[gameGridButtons.Length];

        for (int i = 0; i < gameGridButtons.Length; i++)
        {
            gameGridButtons[i].interactable = true;
            gameGridButtons[i].image.sprite = null;
            gameGridButtonsStatus[i] = null;
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

    public void ChangeCurrentGameSymbol()
    {
        if (isGameActive)
        {
            return;
        }

        if (currentSymbol == GAMESYMBOL_O)
        {
            currentGameSymbolIcon[GAMESYMBOL_O].SetActive(false);
            currentGameSymbolIcon[GAMESYMBOL_X].SetActive(true);
            currentSymbol = GAMESYMBOL_X;
        }
        else
        {
            currentGameSymbolIcon[GAMESYMBOL_X].SetActive(false);
            currentGameSymbolIcon[GAMESYMBOL_O].SetActive(true);
            currentSymbol = GAMESYMBOL_O;
        }
    }

    private void SetStartGameSymbol()
    {
        if (currentSymbol == GAMESYMBOL_O)
        {
            currentGameSymbolIcon[GAMESYMBOL_O].SetActive(true);
            currentGameSymbolIcon[GAMESYMBOL_X].SetActive(false);
        }
        else
        {
            currentGameSymbolIcon[GAMESYMBOL_O].SetActive(false);
            currentGameSymbolIcon[GAMESYMBOL_X].SetActive(true);
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

    public void Quit()
    {
        Application.Quit();
    }
}


