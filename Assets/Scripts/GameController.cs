using UnityEngine;
using UnityEngine.UI;

public enum GameSymbol { O, X, EMPTY = -1 }

public class GameController : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Sprite[] gameIcon;
    public Button[] gameGridButton;
    public Button changerPlayerSymbol;
    public GameObject[] currentGameSymbolIcon;

    private static bool isEnemyTurn;
    private static bool isGameActive;
    private static bool isCurrentGameEnded = false;
    private static int currentSymbol = (int)GameSymbol.O;

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

        for (int i = 0; i < gameGridButton.Length; i++)
        {
            gameGridButton[i].interactable = true;
            gameGridButton[i].image.sprite = null;
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

        gameGridButton[numberGridButton].image.sprite = gameIcon[currentSymbol];
        gameGridButton[numberGridButton].interactable = false;

        isEnemyTurn = true;
    }

    public void ChangeCurrentGameSymbol()
    {
        if (isGameActive)
        {
            return;
        }

        if (currentSymbol == (int)GameSymbol.O)
        {
            currentGameSymbolIcon[(int)GameSymbol.O].SetActive(false);
            currentGameSymbolIcon[(int)GameSymbol.X].SetActive(true);
            currentSymbol = (int)GameSymbol.X;
        }
        else
        {
            currentGameSymbolIcon[(int)GameSymbol.X].SetActive(false);
            currentGameSymbolIcon[(int)GameSymbol.O].SetActive(true);
            currentSymbol = (int)GameSymbol.O;
        }
    }

    private void SetStartGameSymbol()
    {
        if (currentSymbol == (int)GameSymbol.O)
        {
            currentGameSymbolIcon[(int)GameSymbol.O].SetActive(true);
            currentGameSymbolIcon[(int)GameSymbol.X].SetActive(false);
        }
        else
        {
            currentGameSymbolIcon[(int)GameSymbol.O].SetActive(false);
            currentGameSymbolIcon[(int)GameSymbol.X].SetActive(true);
        }
    }

    public static bool CheckEnemyTurn()
    {
        if (isEnemyTurn)
        {
            return isEnemyTurn;
        }
        else
        {
            return false;
        }
    }

    public static void SetEnemyTurn(bool value)
    {
        isEnemyTurn = value;
    }

    public static int GetCurrentPlayerSymbol()
    {
        return currentSymbol;
    }

    public void Quit()
    {
        Application.Quit();
    }
}


