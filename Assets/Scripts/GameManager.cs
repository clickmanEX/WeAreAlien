using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] lifeObjs;
    [SerializeField]
    AudioClip playMusic;
    [SerializeField]
    AudioClip winMusic;
    [SerializeField]
    AudioClip loseMusic;
    [SerializeField]
    GameObject buttonPanel;
    [SerializeField]
    GameObject gameEndPanel;
    [SerializeField]
    Text gameResultText;

    int lifeCount = 3;
    float playtime = 0f;
    AudioSource audioSource;

    readonly float TIME_LIMIT = 180f;
    readonly float NOTICE_TIME = 150f; //制限時間の30秒前に設定

    PHASE phase;
    enum PHASE
    {
        PLAY,
        PLAY_BEFORE_END,
        GAME_CLEAR,
        GAME_OVER
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameManager obj = GameObject.Find("GameManager").GetComponent<GameManager>();
                instance = obj;
            }
            return instance;
        }
    }

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        lifeCount = 3;
        playtime = 0f;
        SetPhase(PHASE.PLAY);
    }

    void SetPhase(PHASE phase)
    {
        this.phase = phase;

        switch (phase)
        {
            case PHASE.PLAY:
                audioSource.clip = playMusic;
                audioSource.loop = true;
                audioSource.Play();
                gameEndPanel.SetActive(false);
                break;
            case PHASE.PLAY_BEFORE_END:
                CharactorTextContoller.Instance.SetPlayBeforeEndText();
                break;
            case PHASE.GAME_CLEAR:
                audioSource.loop = false;
                audioSource.clip = winMusic;
                audioSource.PlayDelayed(0.5f);
                buttonPanel.gameObject.SetActive(false);
                gameEndPanel.SetActive(true);
                if (ScoreManager.Instance.IsUpdateHighScore())
                {
                    gameResultText.text = "Mission Complete!!" + "\n" + "Congratulations!" + "\n" + "New Record" + "\n" + "Score " + ScoreManager.Instance.GetScorePoint();
                }
                else
                {
                    gameResultText.text = "Mission Complete!!" + "\n" + "Score " + ScoreManager.Instance.GetScorePoint();
                }
                ScoreManager.Instance.SaveHighScore();
                CharactorTextContoller.Instance.SetGameClearText();
                break;
            case PHASE.GAME_OVER:
                audioSource.loop = false;
                audioSource.clip = loseMusic;
                audioSource.PlayDelayed(0.5f);
                buttonPanel.gameObject.SetActive(false);
                gameEndPanel.SetActive(true);
                gameResultText.text = "GAME OVER!!";
                CharactorTextContoller.Instance.SetGameOverText();
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (IsGameEnd())
        {
            return;
        }

        playtime += Time.deltaTime;

        if (NOTICE_TIME <= playtime && playtime < TIME_LIMIT && phase != PHASE.PLAY_BEFORE_END)
        {
            SetPhase(PHASE.PLAY_BEFORE_END);
        }
        else if (playtime >= TIME_LIMIT && phase != PHASE.GAME_CLEAR)
        {
            SetPhase(PHASE.GAME_CLEAR);
        }
    }

    public void DamageLife()
    {
        lifeCount--;

        if (lifeCount >= 0)
        {
            lifeObjs[lifeCount].SetActive(false);
            if (lifeCount == 0)
            {
                SetPhase(PHASE.GAME_OVER);
            }
        }

    }

    public bool IsGameEnd()
    {
        return phase != PHASE.PLAY && phase != PHASE.PLAY_BEFORE_END;
    }

    public bool IsGameBeforeEnd()
    {
        return phase == PHASE.PLAY_BEFORE_END;
    }

    public bool IsGameClear()
    {
        return phase == PHASE.GAME_CLEAR;
    }

    public bool IsGameOver()
    {
        return phase == PHASE.GAME_OVER;
    }
}
