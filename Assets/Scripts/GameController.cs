using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("2D MODE")]
    [SerializeField] private Sprite[] gameIcon;
    [SerializeField] private Button[] gameGridButtons;
    [SerializeField] private GameObject[] currentGameSymbolIcon;

    [Header("3D MODE")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] currentGameSymbols;

    [Header("COMMON")]
    [SerializeField] GameOverController gameOverController;
    [SerializeField] Enemy enemy;
    [SerializeField] private Button changerPlayerSymbol;

    private Button3D[] buttonsStatus;
    private List<GameObject> activeSymbols;

    private bool isGameActive;
    private bool isCurrentGameEnded = false;
    private int currentSymbol = GAMESYMBOL_O;
    private int currentGameMode;
    private int?[] gameGridButtonsStatus;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1, gameMode2D = 0, gameMode3D = 1;

    public List<GameObject> ActiveSymbols { get => activeSymbols; }
    public bool IsGameActive { get => isGameActive; private set => isGameActive = value; }
    public bool IsCurrentGameEnded { get => isCurrentGameEnded; set => isCurrentGameEnded = value; }
    public bool ResetGame { get; set; }

    private void Awake()
    {
        currentGameMode = Scenes.CurrentGameMode;
    }

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
            changerPlayerSymbol.interactable = true;
            isCurrentGameEnded = false;

            if (currentGameMode == gameMode3D)
            {
                DestroyGameSymbols();
                ResetButtonsProperties();
            }

            SetStartSettings();
        }

        if (Button3D.CheckStep() == true && !enemy.EnemyTurn && !isCurrentGameEnded)
        {
            AddEnemyPrefabGameSymbol();
            isGameActive = true;
            enemy.EnemyTurn = true;

            changerPlayerSymbol.interactable = false;

            ButtonAction3D();
        }
    }

    private void SetStartSettings()
    {
        isGameActive = false;
        enemy.EnemyTurn = false;

        if (currentGameMode == gameMode2D)
        {
            gameGridButtonsStatus = new int?[gameGridButtons.Length];

            for (int i = 0; i < gameGridButtons.Length; i++)
            {
                gameGridButtons[i].interactable = true;
                gameGridButtons[i].image.sprite = null;
                gameGridButtonsStatus[i] = null;
            }
        }
        else
        {
            activeSymbols = new List<GameObject>();
            GetButtonsStatusArray();
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

    public void ButtonAction(int numberGridButton)
    {
        isGameActive = true;

        if (enemy.EnemyTurn || isCurrentGameEnded)
        {
            return;
        }

        changerPlayerSymbol.interactable = false;

        gameGridButtons[numberGridButton].image.sprite = gameIcon[currentSymbol];
        gameGridButtonsStatus[numberGridButton] = currentSymbol;
        gameGridButtons[numberGridButton].interactable = false;

        enemy.EnemyTurn = true;
    }

    private void ButtonAction3D()
    {
        AddEnemyPrefabGameSymbol();

        int counter = 0;

        foreach (var item in buttonsStatus)
        {
            if (buttonsStatus[counter].IsButtonActivated && buttonsStatus[counter].IsWorking)
            {
                Vector3 symbolPos = buttons[counter].transform.position;
                symbolPos += Vector3.up;
                GameObject gameSymbol = Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);
                Button3D.SetStepIsMaked();
                activeSymbols.Add(gameSymbol);

                buttonsStatus[counter].GameSymbol = currentSymbol;
                buttonsStatus[counter].ButtonOff();
            }

            counter++;
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
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(false);
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(true);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_O].SetActive(false);
                currentGameSymbols[GAMESYMBOL_X].SetActive(true);
            }

            currentSymbol = GAMESYMBOL_X;
        }
        else
        {
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(false);
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(true);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_X].SetActive(false);
                currentGameSymbols[GAMESYMBOL_O].SetActive(true);
            }

            currentSymbol = GAMESYMBOL_O;
        }
    }

    private void SetStartGameSymbol()
    {
        if (currentSymbol == GAMESYMBOL_O)
        {
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(true);
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(false);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_O].SetActive(true);
                currentGameSymbols[GAMESYMBOL_X].SetActive(false);
            }
        }
        else
        {
            if (currentGameMode == gameMode2D)
            {
                currentGameSymbolIcon[GAMESYMBOL_O].SetActive(false);
                currentGameSymbolIcon[GAMESYMBOL_X].SetActive(true);
            }
            else
            {
                currentGameSymbols[GAMESYMBOL_O].SetActive(false);
                currentGameSymbols[GAMESYMBOL_X].SetActive(true);
            }

        }
    }

    public int GetCurrentPlayerSymbol()
    {
        return currentSymbol;
    }

    public  void SetEnemyButtonStatus(int numberButton, int symbolNumber)
    {
        if (gameGridButtonsStatus[numberButton] == null)
        {
            gameGridButtonsStatus[numberButton] = symbolNumber;
        }
    }

    public int? GetConcreteGridButtonStatus(int numberButton)
    {
        return gameGridButtonsStatus[numberButton];
    }

    private void AddEnemyPrefabGameSymbol()
    {
        if (enemy.LastGameSymbol != null)
        {
            activeSymbols.Add(enemy.LastGameSymbol);
        }
    }

    private void ResetButtonsProperties()
    {
        foreach (var item in buttonsStatus)
        {
            item.ResetButton();
        }
    }

    private void DestroyGameSymbols()
    {
        foreach (var item in activeSymbols)
        {
            Destroy(item);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}


