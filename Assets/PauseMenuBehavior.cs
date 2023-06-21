using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuBehavior : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;
    
    public static bool isGamePaused = false;
    public GameObject pauseMenu;

    private void Awake() {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivitySetting", 1f);
        sensitivityText.text = sensitivitySlider.value.ToString("f2");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        GameObject confirm = GameObject.FindGameObjectWithTag("Confirm");
        if(confirm != null)
        {
            confirm.SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateSensitivitySetting()
    {
        PlayerPrefs.SetFloat("sensitivitySetting", sensitivitySlider.value);
        sensitivityText.text = sensitivitySlider.value.ToString("f2");
        FindObjectOfType<StarterAssets.ThirdPersonController>().UpdateSensitivity();
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetFloat("timePlaying", PlayerPrefs.GetFloat("timePlaying", 0f) + LevelManager.timer);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ExitGame()
    {
        PlayerPrefs.SetFloat("timePlaying", PlayerPrefs.GetFloat("timePlaying", 0f) + LevelManager.timer);
        Application.Quit();
    }
}
