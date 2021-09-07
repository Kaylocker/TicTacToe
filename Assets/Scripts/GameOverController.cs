using UnityEngine;
using UnityEngine.UI;

public class GameOverController : CheckerWinCombinations
{
    [SerializeField] private Button[] gameGridButton;
    [SerializeField] private Sprite[] gameIcon;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;

    [SerializeField] private GameObject[] verticalGameOverLines;
    [SerializeField] private GameObject[] horizontalGameOverLines;
    [SerializeField] private GameObject[] diagonalGameOverLines;

    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private int?[] gridSymbolsStatus;
    private int sizeGrid, sizeLineGrid;
    private bool isSomeOneDoStep;
    private bool checkThisStepOnWin = false;

    private void Start()
    {
        sizeGrid = gameGridButton.Length;
        sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);
        gridSymbolsStatus = new int?[gameGridButton.Length];
        checkThisStepOnWin = false;
        HideGameOverLines();
    }

    private void Update()
    {
        isSomeOneDoStep = GameController.CheckEnemyTurn();

        if (checkThisStepOnWin != isSomeOneDoStep)
        {
            checkThisStepOnWin = isSomeOneDoStep;

            bool isGameOver = CatchGameOver();

            if (isGameOver)
            {
                GameController.IsCurrentGameEnded = true;
            }
        }
    }

    private void HideGameOverLines()
    {
        for (int i = 0; i < sizeLineGrid; i++)
        {
            verticalGameOverLines[i].SetActive(false);
            horizontalGameOverLines[i].SetActive(false);
        }

        for (int i = 0; i < diagonalGameOverLines.Length; i++)
        {
            diagonalGameOverLines[i].SetActive(false);
        }
    }

    public void ResetGame()
    {
        GameController.ResetGame = true;
        HideGameOverLines();
    }

    private void SetGameSymbolsGridContain()
    {
        int counter = 0;

        foreach (var item in gameGridButton)
        {
            if (item.interactable == false && GameController.GetConcreteGridButtonStatus(counter) == GAMESYMBOL_O)
            {
                gridSymbolsStatus[counter] = GAMESYMBOL_O;
            }
            else if (item.interactable == false && GameController.GetConcreteGridButtonStatus(counter) == GAMESYMBOL_X)
            {
                gridSymbolsStatus[counter] = GAMESYMBOL_X;
            }
            else
            {
                gridSymbolsStatus[counter] = null;
            }

            counter++;
        }
    }

    private bool CatchGameOver()
    {
        SetGameSymbolsGridContain();

        bool verticalWin = CheckVerticalCombinationEndGame(gridSymbolsStatus);

        if (verticalWin)
        {
            verticalGameOverLines[completedGameOverLine].SetActive(true);
            return true;
        }

        bool horizontalWin = CheckHorizontalCombinationEndGame(gridSymbolsStatus);

        if (horizontalWin)
        {
            horizontalGameOverLines[completedGameOverLine].SetActive(true);
            return true;
        }

        bool diagonalWin = CheckDiagonalCombinationEndGame(gridSymbolsStatus);

        if (diagonalWin)
        {
            diagonalGameOverLines[completedGameOverLine].SetActive(true);
            return true;
        }

        bool standOff = CheckStandOffCombinationEndGame();

        if (standOff)
        {
            return true;
        }

        return false;
    }

    private bool CheckStandOffCombinationEndGame()
    {
        int counterActiveButtons = 0;

        foreach (var item in gameGridButton)
        {
            if (item.interactable == true)
            {
                counterActiveButtons++;
            }
        }

        return counterActiveButtons == 0;
    }
}
