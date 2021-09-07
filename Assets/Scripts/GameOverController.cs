using UnityEngine;
using UnityEngine.UI;

public class GameOverController : CheckerWinCombinations
{
    [Header("2D MODE")]
    [SerializeField] private Button[] gameGridButton;
    [SerializeField] private Sprite[] gameIcon;

    [Header("3D MODE")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;

    [Header("COMMON")]
    [SerializeField] private GameController gameController;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject[] verticalGameOverLines;
    [SerializeField] private GameObject[] horizontalGameOverLines;
    [SerializeField] private GameObject[] diagonalGameOverLines;

    private Vector3[] verticalGameOverLinesStartPosition;
    private Vector3[] horizontalGameOverLinesStartPosition;
    private Vector3[] diagonalGameOverLinesStartPosition;

    private Button3D[] buttonsStatus;

    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1, gameMode2D = 0, gameMode3D = 1;
    private int?[] gridSymbolsStatus;
    private int sizeGrid, sizeLineGrid;
    private int currentGameMode;
    private bool isSomeOneDoStep;
    private bool checkThisStepOnWin = false;

    private void Awake()
    {
        currentGameMode = Scenes.CurrentGameMode;
    }

    private void Start()
    {
        StartSettings();
    }

    private void Update()
    {
        isSomeOneDoStep = enemy.EnemyTurn;

        if (checkThisStepOnWin != isSomeOneDoStep)
        {
            if (currentGameMode == gameMode3D)
            {
                if (gameController.ActiveSymbols.Count==0)
                {
                    return;
                }
            }

            checkThisStepOnWin = isSomeOneDoStep;

            bool isGameOver = CatchGameOver();

            if (isGameOver)
            {
                gameController.IsCurrentGameEnded = true;
            }
        }
    }

    private void StartSettings()
    {
        checkThisStepOnWin = false;

        if (currentGameMode == gameMode2D)
        {
            sizeGrid = gameGridButton.Length;
            sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);
            gridSymbolsStatus = new int?[gameGridButton.Length];
            ResetGameOverLines2D();
        }
        else
        {
            sizeGrid = buttons.Length;
            sizeLineGrid = (int)Mathf.Sqrt(sizeGrid);
            gridSymbolsStatus = new int?[buttons.Length];
            GetGameOverLinesStartPosition();
            ResetGameOverLines3D();
            GetButtonsStatusArray();
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

    public void ResetGame()
    {
        gameController.ResetGame = true;

        if (currentGameMode == gameMode2D)
        {
            ResetGameOverLines2D();
        }
        else
        {
            ResetGameOverLines3D();
        }
    }

    private void ResetGameOverLines2D()
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

    private void ResetGameOverLines3D()
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

    private void SetGameSymbolsGridContain2D()
    {
        int counter = 0;

        foreach (var item in gameGridButton)
        {
            if (item.interactable == false && gameController.GetConcreteGridButtonStatus(counter) == GAMESYMBOL_O)
            {
                gridSymbolsStatus[counter] = GAMESYMBOL_O;
            }
            else if (item.interactable == false && gameController.GetConcreteGridButtonStatus(counter) == GAMESYMBOL_X)
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

    private void SetGameSymbolsGridContain3D()
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
        if (currentGameMode == gameMode2D)
        {
            SetGameSymbolsGridContain2D();
        }
        else
        {
            SetGameSymbolsGridContain3D();

        }

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

        if (currentGameMode == gameMode2D)
        {
            foreach (var item in gameGridButton)
            {
                if (item.interactable == true)
                {
                    counterActiveButtons++;
                }
            }
        }
        else
        {
            foreach (var item in buttonsStatus)
            {
                if (item.GameSymbol == null)
                {
                    counterActiveButtons++;
                }
            }
        }

        return counterActiveButtons == 0;
    }
}
