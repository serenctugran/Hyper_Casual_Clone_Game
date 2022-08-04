using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;

    public bool gameActive = false;

    public GameObject startMenu, gameMenu, gameOverMenu, finishMenu;
    public Text scoreText, finishScoreText, currentLevelText, nextLevelText,startingMenuMoneyText,gameOverMenuMoneyText,finishGameMenuMoneyText;
    public Slider levelProgressBar;
    public float maxDistance;
    public GameObject finishLine;
    public Button RequestAdButton;
    int currentLevel;

    public int score;

    public AudioSource gameMusicAudioSource;
    public AudioClip victoryAudioClip, gameOverAudioClip;

    public DailyReward dailyReward;
    

    private void Start()
    {
       
        Current = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        PlayerController.Current = GameObject.FindObjectOfType<PlayerController>();
        GameObject.FindObjectOfType<MarketController>().InitializeMarketController();
        dailyReward.InitializedDailyReward();
        currentLevelText.text = (currentLevel + 1).ToString();
        nextLevelText.text = (currentLevel + 2).ToString();
        UpdateMoneyText();
        gameMusicAudioSource = Camera.main.GetComponent<AudioSource>();
        if (AddController.Current.IsReadyInterstitalAd())
        {
            AddController.Current.interstitial.Show();
        }
    }
    private void Update()
    {
        if (gameActive)
        {
            PlayerController player = PlayerController.Current;
            float distance = finishLine.transform.position.z - PlayerController.Current.transform.position.z;
            levelProgressBar.value = 1 - (distance / maxDistance);
        }
    }

    public void ShowRewardAdd()
    {
        if (AddController.Current.rewardedAd.IsLoaded())
        {
            AddController.Current.rewardedAd.Show();
        }
    }

    public void StartLevel()
    {
        AddController.Current.bannerView.Hide();
        maxDistance = finishLine.transform.position.z - PlayerController.Current.transform.position.z;
        PlayerController.Current.ChangeSpeed(PlayerController.Current.runningSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        PlayerController.Current.animator.SetBool("running",true);
        gameActive = true;

    }

    public void RestartLevel()
    {
        LevelLoader.Current.ChanceLevel(this.gameObject.scene.name);
    }

    public void LoadNextLevel()
    {
        LevelLoader.Current.ChanceLevel("Level " + (currentLevel + 1));
    }
    public void GameOver()
    {
        if (AddController.Current.IsReadyInterstitalAd())
        {
            AddController.Current.interstitial.Show();
        }
        AddController.Current.bannerView.Show();
        UpdateMoneyText();
        gameMusicAudioSource.Stop();
        gameMusicAudioSource.PlayOneShot(gameOverAudioClip);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gameActive = false;

    }

    public void FinishGame()
    {
        if (AddController.Current.rewardedAd.IsLoaded())
        {
            RequestAdButton.gameObject.SetActive(true);
        }
        else
        {
            RequestAdButton.gameObject.SetActive(false);
        }
        AddController.Current.bannerView.Show();
        GiveMoneyToPlayer(score);
        gameMusicAudioSource.Stop();
        gameMusicAudioSource.PlayOneShot(victoryAudioClip);
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        finishScoreText.text = score.ToString();
        gameMenu.SetActive(false);
        finishMenu.SetActive(true);
        gameActive = false;

    }
    public void ChangeScore(int increment)
    {
        score += increment;
        scoreText.text = score.ToString();
    }

    public void UpdateMoneyText()
    {
        int money = PlayerPrefs.GetInt("money");
        startingMenuMoneyText.text = money.ToString();
        gameOverMenuMoneyText.text = money.ToString();
        finishGameMenuMoneyText.text = money.ToString();
    }

    public void GiveMoneyToPlayer(int increment)
    {
        int money = PlayerPrefs.GetInt("money");
        money = Mathf.Max(0, money + increment);
        PlayerPrefs.SetInt("money", money);
        UpdateMoneyText();
    }
}
