using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharactorTextContoller : MonoBehaviour
{
    [SerializeField]
    Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;  // 1文字の表示にかかる時間

    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数
    private float seDeltaTime = 0f;
    private float seTime = 1.1f;
    private bool sebutton = false;

    readonly string GAME_START_TEXT = "我々ノ任務ハ地球人ヲ捕獲スルコトダ。作戦時間ハ3分。健闘ヲ祈ル！";
    readonly string PLAY_BEFORE_END_TEXT = "作戦終了30秒前ダ！帰還ニ備エロ！";
    readonly string GAME_CLEAR_TEXT = "見事ナ働キデアッタ！作戦成功ダ！";
    readonly string GAME_OVER_TEXT = "作戦は失敗した・・・。今スグ帰還セヨ・・・。";
    readonly string IMPACT_BILL_TEXT = "機体ガ損傷シタ！コレ以上機体ヲブツケルナ！";
    readonly string CAPTURE_FAIL_TEXT = "バカモノ！コンナ大キナモノヲ回収シタラ重サデ墜落シテシマウダロウガ！";

    private static CharactorTextContoller instance;
    public static CharactorTextContoller Instance
    {
        get
        {
            if (instance == null)
            {
                CharactorTextContoller obj = GameObject.Find("CharactorTextContoller").GetComponent<CharactorTextContoller>();
                instance = obj;
            }
            return instance;
        }
    }

    void Start()
    {
        currentText = GAME_START_TEXT;
        CalcTextDisplayTime();
    }

    void Update()
    {
        if (sebutton)
        {
            seDeltaTime += Time.deltaTime;

            if (seDeltaTime >= seTime)
            {
                this.GetComponent<AudioSource>().Stop();
                seDeltaTime = 0;
                sebutton = false;
            }
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

    public void SetCharactorText()
    {
        var mobParam = ScoreManager.Instance.GetNowBonusMobParam();
        currentText = mobParam.instructionsText;
        this.GetComponent<AudioSource>().Play();
        this.sebutton = true;
        CalcTextDisplayTime();
    }

    public void SetBonusCaptureText()
    {
        var mobParam = ScoreManager.Instance.GetNowBonusMobParam();
        currentText = mobParam.successCaptuteText;
        CalcTextDisplayTime();
    }

    public void SetCaputureFailText()
    {
        currentText = CAPTURE_FAIL_TEXT;
        CalcTextDisplayTime();
    }

    public void SetImpactBillText()
    {
        currentText = IMPACT_BILL_TEXT;
        CalcTextDisplayTime();
    }

    public void SetGameOverText()
    {
        currentText = GAME_OVER_TEXT;
        CalcTextDisplayTime();
    }

    public void SetPlayBeforeEndText()
    {
        currentText = PLAY_BEFORE_END_TEXT;
        this.GetComponent<AudioSource>().Play();
        this.sebutton = true;
        CalcTextDisplayTime();
    }

    public void SetGameClearText()
    {
        currentText = GAME_CLEAR_TEXT;
        CalcTextDisplayTime();
    }

    void CalcTextDisplayTime()
    {
        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }
}