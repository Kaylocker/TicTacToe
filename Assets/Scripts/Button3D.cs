using UnityEngine;

public class Button3D : MonoBehaviour
{
    private static bool isMakeStep = false;
    private bool isButtonActivated = false;
    private bool isWorking = true;
    private int? gameSymbol = null;

    public bool IsButtonActivated { get => isButtonActivated; }
    public int? GameSymbol { get => gameSymbol; }
    public bool IsWorking { get => isWorking; }

    private void OnMouseDown()
    {
        if (GameController3D.CheckEnemyTurn() == true)
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
        if(isWorking)
        {
            isWorking = false ;
        }
    }
}
