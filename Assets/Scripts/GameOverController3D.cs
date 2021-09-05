using UnityEngine;
using System.Collections.Generic;

public class GameOverController3D : CheckerWinCombinations
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] verticalGameOverLines;
    [SerializeField] private GameObject[] horizontalGameOverLines;
    [SerializeField] private GameObject[] diagonalGameOverLines;

    private Vector3[] verticalGameOverLinesStartPosition;
    private Vector3[] horizontalGameOverLinesStartPosition;
    private Vector3[] diagonalGameOverLinesStartPosition;

    private Button3D[] buttonsStatus;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private int?[] gridSymbolsStatus;
    private int sizeGrid, sizeLineGrid;
    private bool isSomeOneDoStep;
    private bool checkThisStepOnWin = false;

    private void Start()
    {
        sizeGrid = buttons.Length;
        sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);
        gridSymbolsStatus = new int?[buttons.Length];
        checkThisStepOnWin = false;

        GetGameOverLinesStartPosition();
        ResetGameOverLines();
        GetButtonsStatusArray();
    }

    private void Update()
    {
        isSomeOneDoStep = GameController3D.CheckEnemyTurn();

        if (checkThisStepOnWin != isSomeOneDoStep )
        {
            if (GameController3D.ActiveSymbols.Count == 0)
            {
                return;
            }

            checkThisStepOnWin = isSomeOneDoStep;

            bool isGameOver = CatchGameOver();

            if (isGameOver)
            {
                GameController3D.IsCurrentGameEnded = true;
            }
        }
    }

    private void GetGameOverLinesStartPosition()
    {
        verticalGameOverLinesStartPosition = new Vector3[verticalGameOverLines.Length];
        horizontalGameOverLinesStartPosition = new Vector3[horizontalGameOverLines.Length];
        diagonalGameOverLinesStartPosition = new Vector3[diagonalGameOverLines.Length];

        for (int i = 0; i < verticalGameOverLines.Length; i++)
        {
            verticalGameOverLinesStartPosition[i] = verticalGameOverLines[i].transform.position;
        }

        for (int i = 0; i < horizontalGameOverLines.Length; i++)
        {
            horizontalGameOverLinesStartPosition[i] = horizontalGameOverLines[i].transform.position;
        }

        for (int i = 0; i < diagonalGameOverLines.Length; i++)
        {
            diagonalGameOverLinesStartPosition[i] = diagonalGameOverLines[i].transform.position;
        }
    }

    private void ResetGameOverLines()
    {
        for (int i = 0; i < verticalGameOverLines.Length; i++)
        {
            verticalGameOverLines[i].SetActive(false);
            verticalGameOverLines[i].transform.position = verticalGameOverLinesStartPosition[i];
        }

        for (int i = 0; i < horizontalGameOverLines.Length; i++)
        {
            horizontalGameOverLines[i].SetActive(false);
            horizontalGameOverLines[i].transform.position = horizontalGameOverLinesStartPosition[i];
        }

        for (int i = 0; i < diagonalGameOverLines.Length; i++)
        {
            diagonalGameOverLines[i].SetActive(false);
            diagonalGameOverLines[i].transform.position = diagonalGameOverLinesStartPosition[i];
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

    public void ResetGame()
    {
        GameController3D.ResetGame = true;
        ResetGameOverLines();
    }

    private void SetGameSymbolsGridContain()
    {
        int counter = 0;

        foreach (var item in buttonsStatus)
        {
            if (item.GameSymbol == GAMESYMBOL_O)
            {
                gridSymbolsStatus[counter] = GAMESYMBOL_O;
            }
            else if (item.GameSymbol == GAMESYMBOL_X)
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

        foreach (var item in buttonsStatus)
        {
            if (item.GameSymbol == null)
            {
                counterActiveButtons++;
            }
        }

        return counterActiveButtons == 0;
    }

}
