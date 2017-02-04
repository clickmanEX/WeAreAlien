using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharactorTextContoller : MonoBehaviour
{

    public string[] scenarios;      //上官ウインドウに出すテキストの配列
    public string[] getScenarios;   //ボーナス条件に当てはまったモブキャラを吸い込んだときに上官ウインドウに出すテキストの配列
    public static bool[] MobText = new bool[10];    //ボーナスポイント判別用ブール変数。UFOContollerに使用。
    public string[] minusScenarios; //マイナス条件に当てはまったモブキャラを吸い込んだときに上官ウインドウに出すテキストの配列

    [SerializeField]
    Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;  // 1文字の表示にかかる時間

    private int currentLine = 0;    //表示するテキストの番号。int型整数
    private int lastLine = 0;       //1個前に表示したテキストの番号。int型整数
    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数
    private float delta = 0f;               //テキスト表示時間計測用float型変数
    private float seDeltaTime = 0f;         //文字表示中に流すSEの時間計測用float型変数
    private float nextTime = 20f;           //現在のテキストを表示させる時間。float型変数
    private float seTime = 1.1f;            //文字表示中に流すSEの再生時間。float型変数
    private float alertTime;                //ゲーム終了間近に表示するテキストの時間。 
    private bool stop = false;              //上官ウインドウに表示するテキストを更新させないようにするブール変数          
    private bool sebutton = false;          //文字表示中に流すSEのON/OFFを担うブール変数
    private bool startText = true;          //ゲーム開始時のみに表示させるテキストのON/OFFを担うブール変数
    public static int minusTextnum = 0;     //マイナス条件に当てはまったモブキャラを吸い込んだときに上官ウインドウに出すテキストの番号
    private int alertCount = 0;             //ゲーム終了間近に表示するテキストを何回も流さないようにするカウント
    private int clearCount = 0;             //ゲームクリア時に表示するテキストを何回も流さないようにするカウント
    private int gameoverCount = 0;          //ゲームオーバー時に表示するテキストを何回も流さないようにするカウント



    void Start()
    {
        //ボーナスポイント判別用ブール変数をすべてfalseにリセット。
        for (int i = 0; i < MobText.Length; i++)
        {
            MobText[i] = false;
        }
        this.alertTime = LifeController.clearTime - 30;
        SetNextLine();
    }

    void Update()
    {
        //ゲーム終了時の処理。テキスト表示が終わったらテキスト表示時間を0にリセット。
        if (LifeController.isEnd)
        {
            //ゲームクリアしたときのテキストを表示
            if (LifeController.gameTime >= LifeController.clearTime && this.clearCount < 1)
            {
                SetNextLine();
            }
            else if (LifeController.gameTime < LifeController.clearTime && this.gameoverCount < 1)     //ゲームオーバーしたときのテキストを表示
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

        //テキスト表示時間がnextTimeを超えたらすべての次のテキストに更新。
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
                //テキストを表示した後、currentLineは次のテキスト番号に移るため、lastLineでテキスト番号を保存。
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
        //保存したテキスト番号lastLineを参照してボーナステキストを表示。
        currentText = getScenarios[lastLine];

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }

    void GetMinusLine()
    {
        //ゲームオーバー時、UFOControllerからもらったminusTextnumからテキスト表示
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