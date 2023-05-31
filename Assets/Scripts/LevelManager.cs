using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;

    //public AudioClip gameOverSFX;
    //public AudioClip gameWonSFX;

    public string nextLevel;

    void Start()
    {
        gameText.text = "Reach the portal\nto continue";
        gameText.gameObject.SetActive(true);
        Invoke("RemoveGameText", 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RemoveGameText()
    {
        gameText.gameObject.SetActive(false);
    }

    public void ChestFound()
    {
        gameText.text = "You found a chest!\nIn the future these will\nhave loot to help you";
        gameText.gameObject.SetActive(true);
        Invoke("RemoveGameText", 3);
    }

    public void LevelLost()
    {
        gameText.text = "YOU DIED.";
        gameText.gameObject.SetActive(true);

        //Camera.main.GetComponent<AudioSource>().pitch = 1;
        //AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);

        Invoke("LoadCurrentLevel", 2);
    }

    public void LevelBeat()
    {
        gameText.text = "You beat the stage!\nTeleporting you...";
        gameText.gameObject.SetActive(true);

        //Camera.main.GetComponent<AudioSource>().pitch = 2;
        //AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

        if(!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
        else
        {
            gameText.text = "You beat the stage!\n(More and better stages\ncoming soon)";
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
