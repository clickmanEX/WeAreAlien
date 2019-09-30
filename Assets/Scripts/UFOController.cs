using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{
    [SerializeField] VirtualJoystick virtualJoystick;
    [SerializeField] AudioClip boostClip;
    [SerializeField] AudioClip captureClip;
    [SerializeField] AudioClip damageClip;

    Rigidbody myRigidbody;
    float speed = 50.0f;
    float billForce = 1000.0f;
    AudioSource audioSource;

    float damageTime;

    readonly float MOVE_MAX_SPEED = 50f;
    readonly float DAMPING_COEF = 0.95f;
    readonly float MOVE_RANGE_MIN = -40f;
    readonly float MOVE_RANGE_MAX = 50f;
    readonly float DAMAGE_PHASE_END_TIME = 1f;

    PHASE phase;
    enum PHASE
    {
        MOVE,
        BOOST,
        CAPTURE,
        DAMAGE,
        NONE
    }

    private static UFOController instance;
    public static UFOController Instance
    {
        get
        {
            if (instance == null)
            {
                UFOController obj = GameObject.Find("UFO").GetComponent<UFOController>();
                instance = obj;
            }
            return instance;
        }
    }

    void Awake()
    {
        this.myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        SetPhase(PHASE.MOVE);
    }

    void SetPhase(PHASE phase)
    {
        this.phase = phase;
        switch (phase)
        {
            case PHASE.MOVE:
                audioSource.Stop();
                ScoreManager.Instance.ResetComboBonus();
                break;
            case PHASE.BOOST:
                audioSource.clip = boostClip;
                audioSource.loop = false;
                audioSource.Play();
                ScoreManager.Instance.ResetComboBonus();
                break;
            case PHASE.CAPTURE:
                audioSource.clip = captureClip;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case PHASE.DAMAGE:
                audioSource.clip = damageClip;
                audioSource.loop = false;
                audioSource.Play();
                damageTime = 0f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case PHASE.MOVE:
            case PHASE.BOOST:
                ControlUFO();
                break;
            case PHASE.CAPTURE:
                myRigidbody.velocity = new Vector3(0, 0, 0);
                ScoreManager.Instance.CalcScorePointInSuction();
#if UNITY_EDITOR
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    GetMyCatchButtonUp();
                }
#endif
                break;
            case PHASE.DAMAGE:
                damageTime += Time.deltaTime;
                if (damageTime > DAMAGE_PHASE_END_TIME)
                {
                    SetPhase(PHASE.MOVE);
                }
                break;
        }

        float z = Mathf.Clamp(transform.localPosition.z, MOVE_RANGE_MIN, MOVE_RANGE_MAX);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }

    void ControlUFO()
    {
        //VirtualJoystickクラスのInputDirectionに水平・垂直情報があるため、抽出 
        Vector3 moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");

        if (virtualJoystick.InputDirection != Vector3.zero)
        {
            moveVector = virtualJoystick.InputDirection;
        }

        if (GameManager.Instance.IsGameEnd())
        {
            speed *= DAMPING_COEF;   //すぐに止まらないように慣性をつけた動きにするため。
            return;
        }


        if (this.myRigidbody.velocity.x <= MOVE_MAX_SPEED)
        {
            this.myRigidbody.AddForce(moveVector.x * speed, 0, 0);
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.myRigidbody.AddForce(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.myRigidbody.AddForce(speed, 0, 0);
            }
#endif
        }

        if (this.myRigidbody.velocity.z <= MOVE_MAX_SPEED)
        {
            this.myRigidbody.AddForce(0, 0, moveVector.z * speed);
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.myRigidbody.AddForce(0, 0, this.speed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.myRigidbody.AddForce(0, 0, -this.speed);
            }
#endif
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetMyCatchButtonDown();
        }
#endif
    }


    void OnCollisionEnter(Collision mob)
    {
        if (phase == PHASE.DAMAGE)
        {
            return; //ダメージ受けている最中は判定させないようにする。
        }

        if (mob.gameObject.tag == "Bill")
        {
            CharactorTextContoller.Instance.SetImpactBillText();
            GameManager.Instance.DamageLife();
            ScoreManager.Instance.CalcScorePointInImpactObject();
            float Reflectivity = billForce * Mathf.Sign(myRigidbody.velocity.x) * -1f;
            myRigidbody.AddForce(Reflectivity, 0, 0);
            SetPhase(PHASE.DAMAGE);
        }
        else
        {
            bool isSuccessCaputure = ScoreManager.Instance.CalcScorePointInCaptureMob(mob.gameObject.tag);
            if (!isSuccessCaputure)
            {
                GameManager.Instance.DamageLife();
                SetPhase(PHASE.DAMAGE);
            }
        }
    }

    public void GetMyCatchButtonDown()
    {
        SetPhase(PHASE.CAPTURE);
    }

    public void GetMyCatchButtonUp()
    {
        SetPhase(PHASE.MOVE);
    }

    //ゲーム終了時は押しても反応しないように処理
    public void GetBoostButtonDown()
    {
        SetPhase(PHASE.BOOST);
    }

    public void GetBoostButtonUp()
    {
        SetPhase(PHASE.MOVE);
    }

    public bool IsBoost()
    {
        return phase == PHASE.BOOST;
    }

    public bool IsCaputuring()
    {
        return phase == PHASE.CAPTURE;
    }
}
