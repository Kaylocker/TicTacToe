using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController3D : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] currentGameSymbols;
    [SerializeField] private Button changerPlayerSymbol;

    private Button3D[] buttonsStatus;
    private static List<GameObject> activePrefabs;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private static int currentSymbol = GAMESYMBOL_O;
    private static bool isEnemyTurn;
    private static bool isGameActive;
    private static bool isCurrentGameEnded = false;

    public static bool ResetGame { get; set; }
    public static int CurrentSymbol { get => currentSymbol; }
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

    private void Start()
    {
        SetStartGameSymbol();
        SetStartSettings();
    }

    private void Update()
    {
        if (ResetGame)
        {
            ResetGame = false;
            isGameActive = false;
            isEnemyTurn = false;
            changerPlayerSymbol.interactable = true;
            isCurrentGameEnded = false;

            DestroyGameSymbols();
            ResetButtonsProperties();
            SetStartSettings();
        }

        if (Button3D.CheckStep() == true && !isEnemyTurn)
        {
            isGameActive = true;
            isEnemyTurn = true;
            changerPlayerSymbol.interactable = false;

            Button3D.SetStepMaked();
            InstantiateGameSymbol();
        }
    }

    private void DestroyGameSymbols()
    {
        foreach (var item in activePrefabs)
        {
            Destroy(item);
        }
    }

    private void ResetButtonsProperties()
    {
        foreach (var item in buttonsStatus)
        {
            item.ResetButton();
        }
    }

    private void SetStartSettings()
    {
        isEnemyTurn = false;
        isGameActive = false;
        activePrefabs = new List<GameObject>();

        GetButtonsStatusArray();
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
                GameObject gameSymbol = Instantiate(gameSymbols[currentSymbol], symbolPos, Quaternion.identity);

                activePrefabs.Add(gameSymbol);

                buttonsStatus[counter].GameSymbol = currentSymbol;
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

    public static void AddGameSymbol(GameObject gameSymbol)
    {
        activePrefabs.Add(gameSymbol);
    }

}
