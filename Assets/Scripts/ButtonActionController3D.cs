using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionController3D : MonoBehaviour
{
    private bool isButtonActivated = false;
    private bool isWorking = true;
    private static bool isMakeStep = false;
    private int? gameSymbol = null;

    public bool IsButtonActivated { get => isButtonActivated; }
    public int? GameSymbol { get => gameSymbol; }
    public bool IsWorking { get => isWorking; }

    private void OnMouseDown()
    {

        if (Input.GetMouseButtonDown(0) && !isButtonActivated)
        {
            isButtonActivated = true;
            isMakeStep = true;
        }
    }

    public static bool CheckIsMakeStep()
    {
        return isMakeStep;
    }

    public static void SetIsStepMaked(bool statusStep)
    {
        isMakeStep = statusStep;
    }

    public void ButtonOff()
    {
        if(isWorking)
        {
            isWorking = false ;
        }
    }
}
