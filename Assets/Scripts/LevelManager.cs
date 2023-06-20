using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int fireballDamage = 8;
    public static int blueFireballDamage = (int)(fireballDamage * 1.2f); // 20% more damage than base
    public static int iceSpearDamage = 13;
    
    public Text gameText;
    public Text scoreText;
    public Text scoreInfoText;

    //public AudioClip gameOverSFX;
    //public AudioClip gameWonSFX;

    public string nextLevel;

    public static bool isGameOver = false;

    static int score = 0;
    string scoreInfo;

    void Start()
    {
        SetStartText();
        gameText.gameObject.SetActive(true);
        Invoke("RemoveGameText", 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetStartText()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if(levelName == "Stage0")
        {
            gameText.text = "Enter the portal\nto start the game";
        }
        else if (levelName == "StageEnd")
        {
            gameText.text = "You have completed the game in it's\ncurrent state\nenter the portal to restart the game";
        }
        else
        {
            gameText.text = "Reach the portal\nto continue";
        }

        scoreText.text = "Score: " + score;
        scoreInfo = "";
        scoreInfoText.text = scoreInfo;
    }
    void RemoveGameText()
    {
        gameText.gameObject.SetActive(false);
    }

    public void ChestFound()
    {
        gameText.text = "";
        gameText.gameObject.SetActive(true);
        UpdateScore(5, "Chest Found");
        Invoke("RemoveGameText", 3);
    }

    public void UpdateScore(int value, string reason)
    {
        score += value;
        scoreText.text = "Score: " + score;
        string info = "+" + value + " " + reason + "\n";
        scoreInfo += info;
        scoreInfoText.text = scoreInfo;
        Invoke("UpdateScoreInfo", 2);
    }

    public void UpdateScoreInfo()
    {
        int index = scoreInfo.IndexOf("\n");
        
        if(index >= scoreInfo.Length)
        {
            scoreInfo = "";
        }
        else
        {
            scoreInfo = scoreInfo[(index + 1)..];
        }
        scoreInfoText.text = scoreInfo;
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
