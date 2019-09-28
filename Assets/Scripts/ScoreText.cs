using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{

    private Text scoreText;
    public static float scorePt = 0;

    // Use this for initialization
    void Start()
    {

        this.scoreText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {


        this.scoreText.text = "スコア " + Mathf.Floor(scorePt);


    }
}
