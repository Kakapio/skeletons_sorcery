using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;
    
    private void Awake() {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivitySetting", 1f);
        sensitivityText.text = sensitivitySlider.value.ToString("f2");
    }

    private void Update() {
        print(PlayerPrefs.GetFloat("sensitivitySetting", 1f));
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateSensitivitySetting()
    {
        PlayerPrefs.SetFloat("sensitivitySetting", sensitivitySlider.value);
        sensitivityText.text = sensitivitySlider.value.ToString("f2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
