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
    private static int currentSymbol = gameSymbol_O;
    private static int[] gameGridButtonsStatus;
    private const int gameSymbol_O = 0, gameSymbol_X = 1;

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
        gameGridButtonsStatus = new int[gameGridButtons.Length];

        for (int i = 0; i < gameGridButtons.Length; i++)
        {
            gameGridButtons[i].interactable = true;
            gameGridButtons[i].image.sprite = null;
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

        if (currentSymbol == gameSymbol_O)
        {
            currentGameSymbolIcon[gameSymbol_O].SetActive(false);
            currentGameSymbolIcon[gameSymbol_X].SetActive(true);
            currentSymbol = gameSymbol_X;
        }
        else
        {
            currentGameSymbolIcon[gameSymbol_X].SetActive(false);
            currentGameSymbolIcon[gameSymbol_O].SetActive(true);
            currentSymbol = gameSymbol_O;
        }
    }

    private void SetStartGameSymbol()
    {
        if (currentSymbol == gameSymbol_O)
        {
            currentGameSymbolIcon[gameSymbol_O].SetActive(true);
            currentGameSymbolIcon[gameSymbol_X].SetActive(false);
        }
        else
        {
            currentGameSymbolIcon[gameSymbol_O].SetActive(false);
            currentGameSymbolIcon[gameSymbol_X].SetActive(true);
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

    public static int GetConcreteGridButtonStatus(int numberButton)
    {
        return gameGridButtonsStatus[numberButton];
    }

    public void Quit()
    {
        Application.Quit();
    }
}


