using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;
    public TMP_Text difficultySettingText;
    public Button[] difficultyButtons;
    public Color selectedColor;
    public Color defaultColor;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text fastestText;
    public TMP_Text difficultyText;

    private void Awake() {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivitySetting", 1f);
        sensitivityText.text = sensitivitySlider.value.ToString("f2");
        UpdateDifficultyUI();
        UpdatePlayerInfo();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PlayerItems.items = new List<LootDropData>();
        LevelManager.score = 0;
        LevelManager.timer = 0;
    }

    public void UpdateSensitivitySetting()
    {
        PlayerPrefs.SetFloat("sensitivitySetting", sensitivitySlider.value);
        sensitivityText.text = sensitivitySlider.value.ToString("f2");
    }

    public void UpdateDifficulty(int difficulty)
    {  
        PlayerPrefs.SetInt("difficulty", difficulty);
        UpdateDifficultyUI();
        UpdatePlayerInfo();
    }

    void UpdateDifficultyUI()
    {
        int difficulty = PlayerPrefs.GetInt("difficulty", 0);
        if(difficulty == 0)
        {
            difficultySettingText.text = "Enemies have less health. Spawn with full health each level.";
        }
        else if(difficulty == 1)
        {
            difficultySettingText.text = "Health is carried between levels. Respawn on death.";
        }
        else if(difficulty == 2)
        {
            difficultySettingText.text = "Enemies have more health. Game ends on death.";
        }

        int i = 0;
        foreach(Button button in difficultyButtons)
        {
            if(i == difficulty)
            {
                button.image.color = selectedColor;
            }
            else
            {
                button.image.color = defaultColor;
            }
            i++;
        }
    }

    void UpdatePlayerInfo()
    {
        timeText.text = ConvertTime(PlayerPrefs.GetFloat("timePlaying", 0f));

        var highScore = PlayerPrefs.GetInt("highScore", -1);
        if(highScore < 0)
            scoreText.text = "N/a";
        else
            scoreText.text = highScore.ToString();

        fastestText.text = ConvertTime(PlayerPrefs.GetFloat("fastestTime", -1f));

        var difficultyNames = new [] {"Easy", "Fair", "Hard"};
        difficultyText.text = difficultyNames.GetValue(PlayerPrefs.GetInt("difficulty", 0)) as string;
    }

    public string ConvertTime(float time)
    {
        if(time < 0)
        {
            return "N/a";
        }
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        if(time >= 3600)
        {
            return timeSpan.ToString(@"%h\:mm\:ss");
        } 
        else
        {
            return timeSpan.ToString(@"%m\:ss");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
