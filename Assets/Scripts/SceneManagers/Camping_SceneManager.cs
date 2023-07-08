using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Camping_SceneManager : MonoBehaviour
{
    public void clickSettingsButton()
    {
        SceneManager.LoadScene(1);
    }
    public void clickFireButton()
    {
        SceneManager.LoadScene(2);
    }
    public void clickTentButton()
    {
        SceneManager.LoadScene(3);
    }
    public void clickQuitButton()
    {
        Application.Quit();
    }
}
