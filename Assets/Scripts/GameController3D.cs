using UnityEngine;
using UnityEngine.UI;

public class GameController3D : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] currentGameSymbols;
    [SerializeField] private Button changerGameSymbol;

    private Button3D[] buttonsStatus;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private static int?[] gameGridButtonsStatus;
    private static int currentSymbol = GAMESYMBOL_O;
    private static bool isEnemyTurn;
    private static bool isGameActive;
    private static bool isCurrentGameEnded = false;

    public static int CurrentSymbol { get => currentSymbol; }
    public static bool IsCurrentGameEnded { get => isCurrentGameEnded; }

    private void Start()
    {
        SetStartGameSymbol();
        StartSettings();
    }

    private void Update()
    {
        if (Button3D.CheckStep() == true && !isEnemyTurn)
        {
            isGameActive = true;
            isEnemyTurn = true;
            changerGameSymbol.interactable = false;

            Button3D.SetStepMaked();
            InstantiateGameSymbol();
        }
    }

    private void StartSettings()
    {
        isEnemyTurn = false;
        isGameActive = false;

        GetButtonsStatusArray();

        gameGridButtonsStatus = new int?[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            gameGridButtonsStatus[i] = null;
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

    public void InstantiateGameSymbol()
    {
        int counter = 0;

        foreach (var item in buttonsStatus)
        {
            if (buttonsStatus[counter].IsButtonActivated && buttonsStatus[counter].IsWorking)
            {
                Vector3 symbolPos = buttons[counter].transform.position;
                symbolPos += Vector3.up;
                Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);

                gameGridButtonsStatus[counter] = currentSymbol;
                buttonsStatus[counter].ButtonOff();
            }

            counter++;
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

}
