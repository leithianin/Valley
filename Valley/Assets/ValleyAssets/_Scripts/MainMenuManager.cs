using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    bool isSceneLoading;

    bool isMenuOpen = false;

    [SerializeField] private CanvasGroup mainMenu;

    public void Play()
    {
        if (!isSceneLoading)
        {
            isSceneLoading = true;
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenMenu()
    {
        isMenuOpen = !isMenuOpen;

        mainMenu.gameObject.SetActive(isMenuOpen);
        /*Debug.Log(isMenuOpen);
        if (isMenuOpen)
        {
            mainMenu.alpha = 1;
        }
        else
        {
            mainMenu.alpha = 0;
        }
        mainMenu.blocksRaycasts = isMenuOpen;
        mainMenu.interactable = isMenuOpen;*/
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isSceneLoading = false;
    }
}
