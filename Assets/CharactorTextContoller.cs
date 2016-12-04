using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharactorTextContoller : MonoBehaviour
{

    public string[] scenarios;
    public string[] getScenarios;
    public static bool[] MobText = new bool[10];
    public string[] minusScenarios;

    [SerializeField]
    Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;  // 1文字の表示にかかる時間

    private int currentLine = 0;
    private int lastLine = 0;
    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数
    private float delta = 0f;
    private float seDeltaTime = 0f;
    private float nextTime = 20f;
    private float seTime = 1.1f;
    private float alertTime;
    private bool stop = false;
    private bool sebutton = false;
    private bool startText = true;
    public static int minusTextnum = 0;
    private int alertCount = 0;
    private int clearCount = 0;
    private int gameoverCount = 0;



    void Start()
    {
        for (int i = 0; i < MobText.Length; i++)
        {
            MobText[i] = false;
        }
        this.alertTime = LifeController.clearTime - 30;
        SetNextLine();
    }

    void Update()
    {
        if (LifeController.isEnd)
        {
            if (LifeController.gameTime >= LifeController.clearTime && this.clearCount < 1)
            {
                SetNextLine();
            }
            else if (LifeController.gameTime < LifeController.clearTime && this.gameoverCount < 1)
            {
                minusTextnum = 2;
                GetMinusLine();
                UFOController.minusPoint = false;
            }

            this.delta = 0;
        }
        else
        {
            this.delta += Time.deltaTime;
        }


        if (this.delta >= this.nextTime && stop == false)
        {
            for (int i = 0; i < MobText.Length; i++)
            {
                if (MobText[i])
                {
                    MobText[i] = false;
                }
            }

            SetNextLine();
            this.delta = 0;
        }

        if (sebutton)
        {
            this.seDeltaTime += Time.deltaTime;

            if (this.seDeltaTime >= this.seTime)
            {
                this.GetComponent<AudioSource>().Stop();
                this.seDeltaTime = 0;
                this.sebutton = false;
            }
        }

        if (UFOController.bunusPoint)
        {
            GetBonusLine();
            UFOController.bunusPoint = false;
        }

        if (UFOController.minusPoint)
        {
            GetMinusLine();
            UFOController.minusPoint = false;
        }

        if (LifeController.gameTime >= alertTime && LifeController.gameTime < LifeController.clearTime && alertCount < 1)
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
        if (startText)
        {
            currentText = scenarios[currentLine];
            currentLine = Random.Range(1, 8);
            startText = false;
        }
        else
        {
            if (LifeController.gameTime >= alertTime && LifeController.gameTime < LifeController.clearTime)
            {
                currentText = scenarios[8];
                nextTime = 30f;
                this.GetComponent<AudioSource>().Play();
                this.sebutton = true;
                this.alertCount++;
            }
            else if (LifeController.gameTime >= LifeController.clearTime)
            {
                currentText = scenarios[9];
                stop = true;
                this.clearCount++;
            }
            else
            {
                currentText = scenarios[currentLine];
                MobText[currentLine] = true;
                lastLine = currentLine;
                currentLine = Random.Range(1, 8);
                this.GetComponent<AudioSource>().Play();
                this.sebutton = true;
            }
        }

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }

    void GetBonusLine()
    {
        currentText = getScenarios[lastLine];

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }

    void GetMinusLine()
    {
        currentText = minusScenarios[minusTextnum];

        if (LifeController.isEnd)
        {
            this.gameoverCount++;
        }

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }


}