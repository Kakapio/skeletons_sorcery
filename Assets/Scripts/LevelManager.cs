using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    public static int fireballDamage = 8;
    public static int blueFireballDamage = (int)(fireballDamage * 1.2f); // 20% more damage than base
    public static int iceSpearDamage = 13;
    
    public Text gameText;
    public TMP_Text scoreText;
    public TMP_Text stageText;
    public TMP_Text timerText;
    public GameObject scoreInfoPrefab;

    //public AudioClip gameOverSFX;
    //public AudioClip gameWonSFX;

    public string nextLevel;

    public static bool isGameOver = false;

    static int score = 0;
    static float timer = 0f;
    List<GameObject> scoreInfoObjects = new List<GameObject>();
    GameObject scoreInfoParent;

    void Start()
    {
        SetStartText();
        SetTimerText();
        gameText.gameObject.SetActive(true);
        Invoke("RemoveGameText", 3);
        scoreInfoParent = GameObject.FindGameObjectWithTag("ScoreInfoParent");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            timer += Time.deltaTime;
            SetTimerText();
        }
    }

    void SetTimerText()
    {
        TimeSpan time = TimeSpan.FromSeconds(timer);
        if(timer >= 3600)
        {
            timerText.text = time.ToString(@"%h\:mm\:ss");
        } 
        else
        {
            timerText.text = time.ToString(@"%m\:ss");
        }
    }

    void SetStartText()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if(levelName == "StageEnd")
        {
            stageText.text = "Stage: Boss";
        }
        else
        {
            stageText.text = "Stage: " + SceneManager.GetActiveScene().buildIndex.ToString();
        }
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
    }

    void RemoveGameText()
    {
        gameText.gameObject.SetActive(false);
    }

    public void UpdateScore(int value, string reason)
    {
        score += value;
        scoreText.text = "Score: " + score;
        string message = "+" + value + " " + reason;
        

        GameObject info = Instantiate(scoreInfoPrefab, transform.position, transform.rotation);
        info.transform.SetParent(scoreInfoParent.transform);
        info.transform.localScale = new Vector3(1, 1, 1);
        info.transform.localPosition = new Vector3(0, -40 * scoreInfoObjects.Count, 0);
        info.GetComponentInChildren<TMP_Text>().text = message;
        scoreInfoObjects.Add(info);
        
        Invoke("RemoveScoreInfo", 2);
    }

    public void RemoveScoreInfo()
    {
        GameObject info = scoreInfoObjects[0];
        scoreInfoObjects.Remove(info);
        Destroy(info);
        
        foreach(GameObject element in scoreInfoObjects)
        {
            element.transform.localPosition = element.transform.localPosition + new Vector3(0, 40, 0);
        }
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
            gameText.text = "You beat the game!";
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
