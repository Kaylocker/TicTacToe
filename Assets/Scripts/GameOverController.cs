using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button[] gameGridButton;
    [SerializeField] private Sprite[] gameIcon;
    [SerializeField] private GameObject[] verticalGameOverLines;
    [SerializeField] private GameObject[] horizontalGameOverLines;
    [SerializeField] private GameObject[] diagonalGameOverLines;

    private const int COUNT_DIAGONAL_COMBINATION = 2;
    private int?[] gridSymbolsStatus;
    private int numberOfCompletedGameOverLine;
    private int sizeGrid, sizeLineGrid;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
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
            if (item.interactable == false && GameController.GetConcreteGridButtonStatus(counter)==GAMESYMBOL_O)
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

    private bool CheckCurrentCombination(int?[,] gridCombination)
    {
        int? currentCombinationLine = 0;

        for (int i = 0; i < gridCombination.GetLength(0); i++)
        {
            for (int k = 0; k < gridCombination.GetLength(1); k++)
            {
                currentCombinationLine += gridCombination[i, k];
            }

            if (currentCombinationLine == 0)
            {
                numberOfCompletedGameOverLine = i;
                return true;
            }

            currentCombinationLine = 0;

            for (int j = 0; j < gridCombination.GetLength(1); j++)
            {
                currentCombinationLine += gridCombination[i, j];
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

        return counterActiveButtons == 0;
    }
}
