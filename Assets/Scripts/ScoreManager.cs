using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Text scoreValueText;
    [SerializeField]
    Text highScoreValueText;

    int score;
    int comboBonus;
    float time;
    MobInformation.MobParamator nowBonusMobParam;

    int highScore;

    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                ScoreManager obj = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                instance = obj;
            }
            return instance;
        }
    }

    readonly float BONUS_UPDATE_TIME = 20f;
    readonly int IMPACT_OBJ_SCORE_POINT = -10000;
    readonly int REDUCE_SCORE_PER_SECOND = 333; //吸い込み中に減るスコア量(毎秒)

    readonly string HIGH_SCORE_KEY = "High_Score";

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        comboBonus = 1;
        time = 0f;
        nowBonusMobParam = null;
        scoreValueText.text = score.ToString();
        highScoreValueText.text = highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameEnd())
        {
            return;
        }

        time += Time.deltaTime;
        if (time >= BONUS_UPDATE_TIME)
        {
            SetBonusMobParam();
            CharactorTextContoller.Instance.SetCharactorText();
            time = 0f;
        }
    }

    void SetBonusMobParam()
    {
        nowBonusMobParam = MobInformation.Bonus_MobParamators[Random.Range(0, MobInformation.Bonus_MobParamators.Length)];
    }

    public MobInformation.MobParamator GetNowBonusMobParam()
    {
        return nowBonusMobParam;
    }

    public int GetScorePoint()
    {
        return score;
    }

    public bool CalcScorePointInCaptureMob(string tagName)
    {
        bool isCaptureSuccess = false;
        var captureMobParam = MobInformation.SelectMobParamator(tagName);

        if (!captureMobParam.isDamageObj)
        {
            if (nowBonusMobParam != null && string.Equals(captureMobParam.tagName, nowBonusMobParam.tagName))
            {
                score += captureMobParam.bonusPoint * comboBonus;
                CharactorTextContoller.Instance.SetBonusCaptureText();
            }
            else
            {
                score += captureMobParam.basePoint * comboBonus;
            }
            comboBonus++;
            isCaptureSuccess = true;
        }
        else
        {
            if (nowBonusMobParam != null && string.Equals(captureMobParam.tagName, nowBonusMobParam.tagName))
            {
                score += captureMobParam.bonusPoint * comboBonus;
                comboBonus++;
                isCaptureSuccess = true;
                CharactorTextContoller.Instance.SetBonusCaptureText();
            }
            else
            {
                score += captureMobParam.basePoint;
                CharactorTextContoller.Instance.SetCaputureFailText();
            }
        }
        scoreValueText.text = score.ToString();
        return isCaptureSuccess;
    }

    public void CalcScorePointInImpactObject()
    {
        score += IMPACT_OBJ_SCORE_POINT;
        scoreValueText.text = score.ToString();
    }

    //モブキャラを吸い込む操作の中にいれること
    public void CalcScorePointInSuction()
    {
        score -= (int)(REDUCE_SCORE_PER_SECOND * Time.deltaTime);
        scoreValueText.text = score.ToString();
    }

    //吸い込みボタンを離したらリセットするようにこの処理を入れること
    public void ResetComboBonus()
    {
        comboBonus = 1;
    }

    public bool IsUpdateHighScore()
    {
        return score > highScore;
    }

    public void SaveHighScore()
    {
        if(IsUpdateHighScore())
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
        }
    }
}
