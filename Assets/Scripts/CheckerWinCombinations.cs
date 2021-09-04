using UnityEngine;

public abstract class CheckerWinCombinations : MonoBehaviour
{
    protected int COUNT_DIAGONAL_COMBINATION = 2;
    protected int completedGameOverLine;

    protected bool CheckVerticalCombinationEndGame(int?[] gridSymbolsStatus)
    {
        int sizeGrid = gridSymbolsStatus.Length;
        int sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);

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

    protected bool CheckHorizontalCombinationEndGame(int?[] gridSymbolsStatus)
    {
        int sizeGrid = gridSymbolsStatus.Length;
        int sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);

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

    protected bool CheckDiagonalCombinationEndGame(int?[] gridSymbolsStatus)
    {
        int sizeGrid = gridSymbolsStatus.Length;
        int sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);
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

    protected bool CheckCurrentCombination(int?[,] gridCombination)
    {
        int sizeLineGrid = gridCombination.GetLength(1);
        int? currentCombinationLine = 0;

        for (int i = 0; i < gridCombination.GetLength(0); i++)
        {
            for (int k = 0; k < gridCombination.GetLength(1); k++)
            {
                currentCombinationLine += gridCombination[i, k];
            }

            if (currentCombinationLine == 0)
            {
                completedGameOverLine = i;
                return true;
            }

            currentCombinationLine = 0;

            for (int j = 0; j < gridCombination.GetLength(1); j++)
            {
                currentCombinationLine += gridCombination[i, j];
            }

            if (currentCombinationLine == sizeLineGrid)
            {
                completedGameOverLine = i;
                return true;
            }

            currentCombinationLine = 0;
        }

        return false;
    }

}
