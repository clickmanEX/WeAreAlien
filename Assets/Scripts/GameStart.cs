using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GameStartButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadGameStart", 0.8f);
        
    }

    public void LoadGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }


    public void TutrialButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadTutrial", 0.8f);
          
    }

    public void LoadTutrial()
    {
        SceneManager.LoadScene("Tutrial");
    }

    public void GameEndButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadGameEnd", 0.8f);
        
    }

    public void LoadGameEnd()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
    	Application.OpenURL("http://www.yahoo.co.jp/");
        #else
    	Application.Quit();
        #endif
    }

}
