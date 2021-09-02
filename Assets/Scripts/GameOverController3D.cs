using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController3D : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] gameSymbols;
    [SerializeField] private GameObject[] verticalGameOverLines;
    [SerializeField] private GameObject[] horizontalGameOverLines;
    [SerializeField] private GameObject[] diagonalGameOverLines;

    private const int COUNT_DIAGONAL_COMBINATION = 2;
    private const int GAMESYMBOL_O = 0, GAMESYMBOL_X = 1;
    private int?[] gridSymbolsStatus;
    private int numberOfCompletedGameOverLine;
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
}
