using UnityEngine;

public class UiMenusManager : MonoBehaviour
{
    public Transform mainMenu;
    public Transform pauseMenu;
    public Transform levelLoading;
    public Transform levelComplete;
    public Transform tophighscores;

    public bool isClear()
    {
        bool isCleared = !mainMenu.gameObject.activeSelf && !levelComplete.gameObject.activeSelf && !tophighscores.gameObject.activeSelf && !levelLoading.gameObject.activeSelf;
        return isCleared;
    }
    public void Clear()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        levelComplete.gameObject.SetActive(false);
        tophighscores.gameObject.SetActive(false);
        levelLoading.gameObject.SetActive(false);
    }
    public bool isActive(string screenName)
    {
        bool result = false;
        if (screenName == "mainMenu" && mainMenu.gameObject.activeSelf)
            result = true;
        else if (screenName == "pauseMenu" && pauseMenu.gameObject.activeSelf)
            result = true;
        else if (screenName == "levelComplete" && levelComplete.gameObject.activeSelf)
            result = true;
        else if (screenName == "tophighscores" && tophighscores.gameObject.activeSelf)
            result = true;
        else if (screenName == "levelLoading" && levelLoading.gameObject.activeSelf)
            result = true;

        return result;
    }
    public void Activate(string screenName)
    {
        Clear();
        if (screenName == "mainMenu")
            mainMenu.gameObject.SetActive(true);
        else if (screenName == "pauseMenu")
            pauseMenu.gameObject.SetActive(true);
        else if (screenName == "levelComplete")
            levelComplete.gameObject.SetActive(true);
        else if (screenName == "tophighscores")
            tophighscores.gameObject.SetActive(true);
        else if (screenName == "levelLoading")
            levelLoading.gameObject.SetActive(true);
    }
}
