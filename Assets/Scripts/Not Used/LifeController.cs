using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#if false
public class LifeController : MonoBehaviour
{

    private GameObject life1;
    private GameObject life2;
    private GameObject life3;
    private GameObject gameResultText;
    private GameObject restart;
    private GameObject title;
    private GameObject bgm;
    private AudioSource[] GameBGM;
    public static int lifeCount = 3;
    public static float gameTime = 0f;
    public static float clearTime = 180f;
    public static bool isEnd = false;
    private int gameoverCount = 0;

    // Use this for initialization
    void Start()
    {

        this.life1 = GameObject.Find("Life1");
        this.life2 = GameObject.Find("Life2");
        this.life3 = GameObject.Find("Life3");
        this.restart = GameObject.Find("ReStartButton");
        this.title = GameObject.Find("BackToTitleButton");
        this.gameResultText = GameObject.Find("GameResultText");
        this.bgm = GameObject.Find("BGM");
        this.restart.gameObject.SetActive(false);
        this.title.gameObject.SetActive(false);
        GameBGM = GetComponents<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        gameTime += Time.deltaTime;

        if (2 <= lifeCount && lifeCount < 3)
        {
            life3.gameObject.SetActive(false);
        }
        if (1 <= lifeCount && lifeCount < 2)
        {
            life2.gameObject.SetActive(false);
        }
        if (lifeCount < 1 && this.gameoverCount < 1)
        {
            isEnd = true;
            life1.gameObject.SetActive(false);
            bgm.GetComponent<AudioSource>().Stop();
            GameBGM[0].Play(22050);
            this.gameResultText.GetComponent<Text>().text = "GAME OVER!!";
            this.restart.gameObject.SetActive(true);
            this.title.gameObject.SetActive(true);
            this.gameoverCount++;

        }
        if (gameTime > clearTime && this.gameoverCount < 1)
        {
            isEnd = true;
            bgm.GetComponent<AudioSource>().Stop();
            GameBGM[1].Play(22050);
            this.gameoverCount++;
            this.gameResultText.GetComponent<Text>().text = "Mission Complete!!" + "\n" + "スコア " + Mathf.Floor(ScoreText.scorePt);
            this.restart.gameObject.SetActive(true);
            this.title.gameObject.SetActive(true);
        }

    }
}
#endif
