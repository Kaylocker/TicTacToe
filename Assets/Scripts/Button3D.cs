using UnityEngine;

public class Button3D : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private static bool isMakeStep = false;
    private bool isGameReset = false;
    private bool isButtonActivated = false;
    private bool isWorking = true;
    private int? gameSymbol = null;

    public bool IsButtonActivated
    {
        get => isButtonActivated;

        set
        {
            if (isButtonActivated == false && value != false)
            {
                isButtonActivated = value;
            }
            else
            {
                return;
            }

        }

    }
    public bool IsWorking { get => isWorking; }
    public int? GameSymbol
    {
        get => gameSymbol;

        set
        {
            if (gameSymbol == null)
            {
                gameSymbol = value;
            }
            else
            {
                return;
            }
        }
    }

    private void Update()
    {
        if (isGameReset)
        {
            isGameReset = false;
            isMakeStep = false;
            isButtonActivated = false;
            isWorking = true;
            gameSymbol = null;
        }
    }

    private void OnMouseDown()
    {
        if (enemy.EnemyTurn == true || isGameReset)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !isButtonActivated)
        {
            isButtonActivated = true;
            isMakeStep = true;
        }
    }

    public static bool CheckStep()
    {
        return isMakeStep;
    }

    public static void SetStepIsMaked()
    {
        isMakeStep = false;
    }

    public void ButtonOff()
    {
        if (isWorking)
        {
            isWorking = false;
        }
    }

    public void ResetButton()
    {
        isGameReset = true;
    }
}
