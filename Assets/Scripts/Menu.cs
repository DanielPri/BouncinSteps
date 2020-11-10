using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject SettingsUI;
    public TextMeshProUGUI sensDisplay;
    public TextMeshProUGUI levelDisplay;

    private PlayerData playerData;
    private float sensitivity = 0.3f;
    private Slider slider;

    private void Start()
    {
        MainMenuUI.SetActive(true);
        SettingsUI.SetActive(false);

        slider = SettingsUI.transform.GetComponentInChildren<Slider>();

        // if redirected from pause menu
        Time.timeScale = 1;

        // if a save exists load it, otherwise set one up
        playerData = SaveSystem.LoadPlayer();
        if(playerData == null)
        {
            playerData = new PlayerData(0,sensitivity);
        }
        slider.value = playerData.rotationSensitivity;
        levelDisplay.text = playerData.level.ToString();
    }

    public void StartGame()
    {
        playerData.rotationSensitivity = sensitivity;
        SaveSystem.SavePlayer(playerData);
        SceneManager.LoadScene("Game");
    }

    // Warning!!!! cannot recover!
    public void ResetPlayer()
    {
        SaveSystem.ResetPlayer();
        playerData = new PlayerData(0, sensitivity);
        levelDisplay.text = "0";
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

    public void setRotationSpeed(float value)
    {
        sensitivity = value;
        // remaps the value to display 0-100
        float display = (value - slider.minValue) / (slider.maxValue - slider.minValue) * (100f - 0f) + 0f;
        sensDisplay.text = Mathf.Round(display).ToString();
    }
}
