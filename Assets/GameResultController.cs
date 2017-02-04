using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameResultController : MonoBehaviour {

    private string highScorekey = "High Score";

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TitleButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadTitle", 0.8f);        
    }

    //ゲームに関係するstatic変数を初期値にリセット。
    public void LoadTitle()
    {
        LifeController.isEnd = false;
        ScoreText.scorePt = 0;
        LifeController.lifeCount = 3;
        LifeController.gameTime = 0f;
        UFOController.bunusPoint = false;
        UFOController.minusPoint = false;
        SceneManager.LoadScene("Title");
    }

    public void RestartButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadRestart", 0.8f);
    }

    //ゲームに関係するstatic変数を初期値にリセット
    public void LoadRestart()
    {
        LifeController.isEnd = false;
        ScoreText.scorePt = 0;
        LifeController.lifeCount = 3;
        LifeController.gameTime = 0f;
        UFOController.bunusPoint = false;
        UFOController.minusPoint = false;
        SceneManager.LoadScene("GameScene");
    }

    public void HighScoreDelete()
    {
        PlayerPrefs.DeleteKey(highScorekey);
    }

}
