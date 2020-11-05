using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject SettingsUI;

    private void Start()
    {
        MainMenuUI.SetActive(true);
        SettingsUI.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Warning!!!! cannot recover!
    public void ResetPlayer()
    {
        SaveSystem.ResetPlayer();
    }

    public void SwitchToSettings()
    {
        SettingsUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }

    public void SwitchToMenu()
    {
        SettingsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }
}
