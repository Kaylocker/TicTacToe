using UnityEngine;

public class Button3D : MonoBehaviour
{
    private static bool isMakeStep = false;
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

    private void OnMouseDown()
    {
        if (GameController3D.CheckEnemyTurn() == true || GameController3D.IsCurrentGameEnded)
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

    public static void SetStepMaked()
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
}
