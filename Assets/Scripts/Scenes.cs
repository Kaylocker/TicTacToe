using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    private const int gameMode2D = 0, gameMode3D = 1, countGameModes = 4;
    private static int currentGameMode;
    public static int CurrentGameMode { get => currentGameMode; }

    public void LoadScene(int sceneNumber)
    {
        if (currentGameMode == gameMode2D)
        {
            SceneManager.LoadScene(sceneNumber);
        }
        else
        {
            if (sceneNumber == 0)
            {
                SceneManager.LoadScene(sceneNumber);
            }
            else
            {
                sceneNumber += countGameModes;
                SceneManager.LoadScene(sceneNumber);

            }
        }
    }

    public void ChangeGameMode(Text buttonText)
    {
        if (currentGameMode == gameMode2D)
        {
            currentGameMode = gameMode3D;
            buttonText.text = "3D";
        }
        else
        {
            currentGameMode = gameMode2D;
            buttonText.text = "2D";
        }
    }

}
