using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{

    private Text highScoreText;     //ハイスコアを表示するテキスト
    private float highScorePt;      //ハイスコアを表示する数字部分
    private string highScorekey = "High Score"; //ハイスコアを保存するキー
    public bool highScoreUpdate = false;        //ハイスコアを保存するかどうか判断するブール変数

    // Use this for initialization
    void Start()
    {
        this.highScoreText = GetComponent<Text>();
        this.highScorePt = PlayerPrefs.GetFloat(highScorekey, 0);
        this.highScoreText.text = "HighScore " + Mathf.Floor(highScorePt);

    }

    // Update is called once per frame
    void Update()
    {
        //現在のスコアがハイスコアを上回り、かつゲームをクリアしたときのみハイスコアを更新して保存し、表示する。
        if (ScoreText.scorePt > highScorePt)
        {
            highScoreUpdate = true;

            if (LifeController.gameTime > LifeController.clearTime)
            {
                PlayerPrefs.SetFloat(highScorekey, ScoreText.scorePt);
                this.highScoreText.text = "HighScore " + Mathf.Floor(ScoreText.scorePt);
            }
               
        }else if(ScoreText.scorePt <= highScorePt)
        {
            highScoreUpdate = false;
        }

    }
}