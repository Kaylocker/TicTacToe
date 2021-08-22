using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Button[] gameGridButton;
    public Sprite[] gameIcon;
    public GameObject[] verticalGameOverLines;
    public GameObject[] horizontalGameOverLines;
    public GameObject[] diagonalGameOverLines;

    private const int COUNT_DIAGONAL_COMBINATION = 2;
    private int?[] gridSymbolsStatus;
    private int numberOfCompletedGameOverLine;
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

    public void ResetGame()
    {
        GameController.ResetGame = true;
        HideGameOverLines();
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
    private bool CatchGameOver()
    {
        SetGameSymbolsGridContain();

        bool verticalWin = CheckVerticalCombinationEndGame();

        if (verticalWin)
        {
            verticalGameOverLines[numberOfCompletedGameOverLine].SetActive(true);
            return true;
        }

        bool horizontalWin = CheckHorizontalCombinationEndGame();

        if (horizontalWin)
        {
            horizontalGameOverLines[numberOfCompletedGameOverLine].SetActive(true);
            return true;
        }

        bool diagonalWin = CheckDiagonalCombinationEndGame();

        if (diagonalWin)
        {
            diagonalGameOverLines[numberOfCompletedGameOverLine].SetActive(true);
            return true;
        }

        bool standOff = CheckStandOffCombinationEndGame();

        if (standOff)
        {
            return true;
        }

        return false;
    }
    private void SetGameSymbolsGridContain()
    {
        int counter = 0;

        foreach (var item in gameGridButton)
        {
            if (item.interactable == false && item.image.sprite == gameIcon[(int)GameSymbol.O])
            {
                gridSymbolsStatus[counter] = (int)GameSymbol.O;
            }
            else if (item.interactable == false && item.image.sprite == gameIcon[(int)GameSymbol.X])
            {
                gridSymbolsStatus[counter] = (int)GameSymbol.X;
            }
            else
            {
                gridSymbolsStatus[counter] = null;
            }

            counter++;
        }
    }
    private bool CheckVerticalCombinationEndGame()
    {
        int?[,] verticalCombination = new int?[sizeLineGrid, sizeLineGrid];

        for (int i = 0; i < sizeLineGrid; i++)
        {
            int counter = i;

            for (int j = 0; j < sizeLineGrid; j++)
            {
                verticalCombination[i, j] = gridSymbolsStatus[counter];

                counter += sizeLineGrid;
            }
        }

        bool isVerticalCombinationCompleted = CheckCurrentCombination(verticalCombination);

        return isVerticalCombinationCompleted;
    }
    private bool CheckHorizontalCombinationEndGame()
    {
        int?[,] horizontalCombination = new int?[sizeLineGrid, sizeLineGrid];

        int counter = 0;

        for (int i = 0; i < sizeLineGrid; i++)
        {
            for (int j = 0; j < sizeLineGrid; j++)
            {
                horizontalCombination[i, j] = gridSymbolsStatus[counter];
                counter++;
            }
        }

        bool isHorizontalCombinationCompleted = CheckCurrentCombination(horizontalCombination);

        return isHorizontalCombinationCompleted;
    }
    private bool CheckDiagonalCombinationEndGame()
    {
        int?[,] diagonalCombination = new int?[COUNT_DIAGONAL_COMBINATION, sizeLineGrid];

        int counter = 0;

        for (int i = 0; i < COUNT_DIAGONAL_COMBINATION; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < sizeLineGrid; j++)
                {
                    diagonalCombination[i, j] = gridSymbolsStatus[counter];
                    counter += sizeLineGrid;
                    counter++;
                }
            }
            else
            {
                counter = sizeLineGrid - 1;

                for (int j = 0; j < sizeLineGrid; j++)
                {
                    diagonalCombination[i, j] = gridSymbolsStatus[counter];
                    counter += sizeLineGrid;
                    counter--;
                }
            }
        }

        bool isDiagonalCombinationCompleted = CheckCurrentCombination(diagonalCombination);

        return isDiagonalCombinationCompleted;
    }
    private bool CheckCurrentCombination(int?[,] grid)
    {
        int? currentCombinationLine = 0;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int k = 0; k < grid.GetLength(1); k++)
            {
                currentCombinationLine += grid[i, k];
            }

            if (currentCombinationLine == (int)GameSymbol.O)
            {
                numberOfCompletedGameOverLine = i;
                return true;
            }

            currentCombinationLine = 0;

            for (int j = 0; j < grid.GetLength(1); j++)
            {
                currentCombinationLine += grid[i, j];
            }

            if (currentCombinationLine == sizeLineGrid)
            {
                numberOfCompletedGameOverLine = i;
                return true;
            }

            currentCombinationLine = 0;
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

        if (counterActiveButtons == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
