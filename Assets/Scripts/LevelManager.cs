using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    public static int fireballDamage = 20;
    public static int blueFireballDamage = (int)(fireballDamage * 1.2f); // 20% more damage than base
    public static int iceSpearDamage = 40;
    
    public GameObject gameInfo;
    public TMP_Text scoreText;
    public TMP_Text stageText;
    public TMP_Text timerText;
    public GameObject scoreInfoPrefab;

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;

    public string nextLevel;

    public static float timer = 0f;
    public static int score = 0;

    static int savedScore;
    List<GameObject> scoreInfoObjects = new List<GameObject>();
    GameObject scoreInfoParent;
    TMP_Text gameText;
    float gameTextTimer = 0;
    

    void Start()
    {
        gameText = gameInfo.GetComponentInChildren<TMP_Text>();
        gameInfo.SetActive(false);
        SetStartText();
        SetTimerText();
        scoreInfoParent = GameObject.FindGameObjectWithTag("ScoreInfoParent");
        Time.timeScale = 1f;
        savedScore = score;
        PortalBehavior.teleporting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerHealth.isPlayerDead)
        {
            timer += Time.deltaTime;
            SetTimerText();
        }

        if(gameTextTimer < 0)
            gameInfo.SetActive(false);
        else
            gameTextTimer -= Time.deltaTime;
    }

    void SetTimerText()
    {
        timerText.text = ConvertTime(timer);
    }

    string ConvertTime(float timer)
    {
        TimeSpan time = TimeSpan.FromSeconds(timer);
        if(timer >= 3600)
        {
           return time.ToString(@"%h\:mm\:ss");
        } 
        else
        {
            return time.ToString(@"%m\:ss");
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
            UpdateGameInfo("Reach the portal to progress", Color.white);
        }

        scoreText.text = "Score: " + score;
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
        if(PlayerPrefs.GetInt("difficulty", 0) == 2)
        {
            UpdateGameInfo("Game Over", Color.red);
            Invoke("LoadMainMenu", 3);
        }
        else
        {
            UpdateGameInfo("You died... respawning", Color.red);
            Invoke("LoadCurrentLevel", 1.5f);
        }

        gameText.gameObject.SetActive(true);

        AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);
    }

    public void LevelBeat()
    {
        UpdateGameInfo("Teleporting...", Color.cyan);

        PlayerHealth.storedHealth = PlayerHealth.currentHealth;

        if(!string.IsNullOrEmpty(nextLevel))
        {
            Invoke("LoadNextLevel", 2);
        }
        else
        {
            UpdateGameInfo("YOU WIN!\nScore: " + score + "\tTime: " + ConvertTime(timer), Color.white, 5f);

            Camera.main.GetComponent<AudioSource>().volume = 0;
            AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

            if(score > PlayerPrefs.GetInt("highScore", -1))
            {
                PlayerPrefs.SetInt("highScore", score);
            }
            if(timer < PlayerPrefs.GetFloat("fastestTime", Mathf.Infinity))
            {
                PlayerPrefs.SetFloat("fastestTime", timer);
            }
            
            Invoke("LoadMainMenu", 5);
        }
    }

    public void UpdateGameInfo(string text, Color color, float duration = 3f)
    {
        gameInfo.SetActive(true);
        gameText.text = text;
        gameText.color = color;
        gameTextTimer = duration;
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        PlayerItems.items = new List<LootDropData>(PlayerItems.savedItems);
        score = savedScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadMainMenu()
    {
        PlayerPrefs.SetFloat("timePlaying", PlayerPrefs.GetFloat("timePlaying", 0f) + timer);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
