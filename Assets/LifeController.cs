using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeController : MonoBehaviour {

    private GameObject life1;
    private GameObject life2;
    private GameObject life3;
    private GameObject gameResultText;
    public static int lifeCount =3;
    public static float gameTime = 0f;
    public static float clearTime = 300f;
    public static bool isEnd = false;

    // Use this for initialization
    void Start () {

        this.life1 = GameObject.Find("Life1");
        this.life2 = GameObject.Find("Life2");
        this.life3 = GameObject.Find("Life3");
        this.gameResultText = GameObject.Find("GameResultText");
        
    }
	
	// Update is called once per frame
	void Update () {

        gameTime += Time.deltaTime;

        if (2 <= lifeCount && lifeCount < 3)
        {
            life3.gameObject.SetActive(false);
        }
        if (1 <= lifeCount && lifeCount < 2)
        {
            life2.gameObject.SetActive(false);
        }
        if (lifeCount < 1)
        {
            isEnd = true;
            life1.gameObject.SetActive(false);
            this.gameResultText.GetComponent<Text>().text = "GAME OVER!!";
        }
        if(gameTime > clearTime)
        {
            isEnd = true;
            this.gameResultText.GetComponent<Text>().text = "Mission Complete!!" + "\n" + "スコア "+ Mathf.Floor(ScoreText.scorePt); 
        }

    }   
}
