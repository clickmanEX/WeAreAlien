using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharactorTextContoller : MonoBehaviour {

    public string[] scenarios;
    [SerializeField]
    Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;  // 1文字の表示にかかる時間

    private int currentLine = 0;
    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数
    private float delta = 0f;
    private float nextTime = 5f;
    private float alertTime;
    private bool stop = false;




    void Start()
    {
        this.alertTime = LifeController.clearTime - 30;
        SetNextLine();

    }

    void Update()
    {
        this.delta += Time.deltaTime;

        if (this.delta >= this.nextTime && stop == false)
        {
            SetNextLine();
            this.delta = 0;
        }
            

        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

        // 表示文字数が前回の表示文字数と異なるならテキストを更新する
        if (displayCharacterCount != lastUpdateCharacter)
        {
            uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
    }


    void SetNextLine()
    {
        if(LifeController.gameTime >= alertTime &&  LifeController.gameTime < LifeController.clearTime)
        {
            currentText = scenarios[5];
            nextTime = 30f;
            Debug.Log(LifeController.clearTime);
            Debug.Log(alertTime);
        }
        else if(LifeController.gameTime >= LifeController.clearTime)
        {
            currentText = scenarios[6];
            stop = true;
        }else
        {
            currentText = scenarios[currentLine];
            currentLine = Random.Range(1, 5);
        }
        

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }
}