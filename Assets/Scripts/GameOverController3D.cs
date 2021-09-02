using UnityEngine;

public class GameOverController3D : CheckerWinCombinations
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] verticalGameOverLines;
    [SerializeField] private GameObject[] horizontalGameOverLines;
    [SerializeField] private GameObject[] diagonalGameOverLines;

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
        HideGameOverLines();
        GetButtonsStatusArray();
    }

    private void Update()
    {
        isSomeOneDoStep = GameController3D.CheckEnemyTurn();

        if (checkThisStepOnWin != isSomeOneDoStep)
        {
            checkThisStepOnWin = isSomeOneDoStep;

            bool isGameOver = CatchGameOver();

            if (isGameOver)
            {
                GameController3D.IsCurrentGameEnded = true;
            }
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

        bool verticalWin = CheckVerticalCombinationEndGame(gridSymbolsStatus);

        if (verticalWin)
        {
            verticalGameOverLines[numberOfCompletedGameOverLine].SetActive(true);
            return true;
        }

        bool horizontalWin = CheckHorizontalCombinationEndGame(gridSymbolsStatus);

        if (horizontalWin)
        {
            horizontalGameOverLines[numberOfCompletedGameOverLine].SetActive(true);
            return true;
        }

        bool diagonalWin = CheckDiagonalCombinationEndGame(gridSymbolsStatus);

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
