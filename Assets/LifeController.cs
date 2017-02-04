using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeController : MonoBehaviour {

    private GameObject life1;   //ライフバー画像
    private GameObject life2;   //ライフバー画像
    private GameObject life3;   //ライフバー画像
    private GameObject gameResultText;      //ゲーム終了後に表示するテキスト
    private GameObject restart;         //ゲーム終了後に表示するリスタートボタン
    private GameObject title;           //ゲーム終了後に表示するタイトルボタン
    private GameObject bgm;             //ゲーム中に再生するBGM
    private AudioSource[] GameEndBGM;      //ゲーム終了後に再生するBGMを入れる配列
    public static int lifeCount =3;     //ライフを管理する変数。GameResultController・UFOControllerにも使用。
    public static float gameTime = 0f;  //ゲームの経過時間測定用float型変数。 CharactorTextContollerにも使用。
    public static float clearTime = 180f;   //ゲームの制限時間用float型変数。CharactorTextContollerにも使用。
    public static bool isEnd = false;       //ゲーム終了を管理するブール変数。UFOController・FloorController・MobController・CharactorTextContoller・GameResultControllerにも使用。
    private int gameoverCount = 0;      //何回もゲームオーバーにならないように管理するint型変数
    public HighScoreText highScoreText; 

    // Use this for initialization
    void Start () {

        this.life1 = GameObject.Find("Life1");
        this.life2 = GameObject.Find("Life2");
        this.life3 = GameObject.Find("Life3");
        this.restart = GameObject.Find("ReStartButton");
        this.title = GameObject.Find("BackToTitleButton");
        this.gameResultText = GameObject.Find("GameResultText");
        this.bgm = GameObject.Find("BGM");
        this.restart.gameObject.SetActive(false);
        this.title.gameObject.SetActive(false);
        GameEndBGM = GetComponents<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

        gameTime += Time.deltaTime;

        //lifeCountの数に応じてライフバーの表示を減らす処理
        if (lifeCount < 3)
        {
            life3.gameObject.SetActive(false);
        }
        if (lifeCount < 2)
        {
            life2.gameObject.SetActive(false);
        }

        //lifeCountが0になったらゲームオーバーテキスト・リスタートボタン・タイトルボタンを表示する。
        if (lifeCount < 1 && this.gameoverCount < 1)
        {
            isEnd = true;
            life1.gameObject.SetActive(false);
            bgm.GetComponent<AudioSource>().Stop();
            GameEndBGM[0].PlayDelayed(0.7f);
            this.gameResultText.GetComponent<Text>().text = "GAME OVER!!";
            this.restart.gameObject.SetActive(true);
            this.title.gameObject.SetActive(true);
            this.gameoverCount++;

        }

        //制限時間を超えたらゲームクリアテキスト・リスタートボタン・タイトルボタンを表示する。
        if (gameTime > clearTime && this.gameoverCount < 1)
        {
            isEnd = true;
            bgm.GetComponent<AudioSource>().Stop();
            GameEndBGM[1].Play();
            this.gameoverCount++;

            //ハイスコアを更新したらハイスコア更新専用メッセージを表示する。
            if (highScoreText.highScoreUpdate)
            {
                this.gameResultText.GetComponent<Text>().text = "Mission Complete!!" + "\n" + "Congratulations!" + "\n" + "New Record" + "\n" + "Score " + Mathf.Floor(ScoreText.scorePt);

            }else
            {
                this.gameResultText.GetComponent<Text>().text = "Mission Complete!!" + "\n" + "Score " + Mathf.Floor(ScoreText.scorePt);

            }
            this.restart.gameObject.SetActive(true);
            this.title.gameObject.SetActive(true);
        }

    }   
}
