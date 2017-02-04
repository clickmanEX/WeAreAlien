using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    private Text scoreText;             //スコアを表示するテキスト
    public static float scorePt = 0;    //スコアを表示する数字部分。HighScoreText・UFOController・GameResultController・LifeControllerに使用

    // Use this for initialization
    void Start () {

        this.scoreText = GetComponent<Text>();
        
    }
	
	// Update is called once per frame
	void Update () {

        
        this.scoreText.text = "Score " + Mathf.Floor(scorePt);


    }
}
