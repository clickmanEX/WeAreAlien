using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{
    [SerializeField] VirtualJoystick virtualJoystick;
    private Rigidbody myRigidbody;
    private float speed = 50.0f;
    private float billForce = 1000.0f;
    private bool stop = false;
    public static bool isCatchButtonDown = false;
    public static bool isBoostButtonDown = false;
    private int isCatchButtonTrueCount = 0;
    private int isBoostButtonTrueCount = 0;
    private AudioSource[] ufoSE;

    readonly float MOVE_MAX_SPEED = 50f;
    readonly float DAMPING_COEF = 0.95f;

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
        ufoSE = GetComponents<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //VirtualJoystickクラスのInputDirectionに水平・垂直情報があるため、抽出 
        Vector3 UFOmove = Vector3.zero;
        UFOmove.x = Input.GetAxis("Horizontal");
        UFOmove.z = Input.GetAxis("Vertical");

        if (virtualJoystick.InputDirection != Vector3.zero)
        {
            UFOmove = virtualJoystick.InputDirection;
        }

        if (GameManager.Instance.IsGameEnd())
        {
            this.speed *= DAMPING_COEF;   //すぐに止まらないように慣性をつけた動きにするため。
        }


        if (this.stop == false)     //28~52行目まで、上下左右の動きをつける。スペースキーを押していない間は動けるように条件をつけた。
        {
            if (this.myRigidbody.velocity.x <= MOVE_MAX_SPEED)
            {
                this.myRigidbody.AddForce(UFOmove.x * speed, 0, 0);

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    this.myRigidbody.AddForce(-speed, 0, 0);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    this.myRigidbody.AddForce(speed, 0, 0);
                }
            }

            if (this.myRigidbody.velocity.z <= MOVE_MAX_SPEED)
            {
                this.myRigidbody.AddForce(0, 0, UFOmove.z * speed);

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    this.myRigidbody.AddForce(0, 0, this.speed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    this.myRigidbody.AddForce(0, 0, -this.speed);
                }
            }
        }
        else         //スペースキー押してる間はモブを吸い込むため、UFOは停止する。
        {
            this.myRigidbody.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space) || isCatchButtonDown)    //スペースキー押してる間は停止、スペースキーを離すと動けるように。
        {
            this.stop = true;
            ScoreManager.Instance.CalcScorePointInSuction();
        }
        else
        {
            this.stop = false;
            ScoreManager.Instance.ResetComboBonus();
            ufoSE[0].Stop();
        }

        if (isCatchButtonDown)    //CatchButton押したとき吸い込み音を再生
        {
            isCatchButtonTrueCount++;

            if (isCatchButtonTrueCount <= 1)
            {
                ufoSE[0].Play();
            }

        }

        if (isBoostButtonDown)    //BoostButton押したとき、一回だけ吸い込み音を再生
        {
            isBoostButtonTrueCount++;

            if (isBoostButtonTrueCount <= 1)
            {
                ufoSE[2].Play();
            }

        }
        else       //BoostButton離したとき吸い込み音を停止
        {
            ufoSE[2].Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ufoSE[0].Play();
        }


        ///タッチ座標をスクリーン座標からワールド座標に変換し、x座標で画面を左右2分割にしてフリッパーを制御
        foreach (Touch touch in Input.touches)
        {
            ///タッチ座標がx,y座標しかないのでz座標を追加し、ワールド座標に変換
            Vector3 pos = touch.position;
            pos.z = 100f;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);

            if (this.stop == false)
            {
                if (this.myRigidbody.velocity.x <= MOVE_MAX_SPEED)
                {
                    if (touch.phase == TouchPhase.Stationary && worldPos.x <= 0)
                    {
                        this.myRigidbody.AddForce(-this.speed, 0, 0);
                    }

                    if (touch.phase == TouchPhase.Stationary && worldPos.x > 0)
                    {
                        this.myRigidbody.AddForce(this.speed, 0, 0);
                    }

                }

            }

        }
    }


    void OnCollisionEnter(Collision mob)
    {
        if (mob.gameObject.tag == "Bill")
        {
            GameManager.Instance.DamageLife();
            ScoreManager.Instance.CalcScorePointInImpactObject();
            CharactorTextContoller.Instance.SetImpactBillText();
            float Reflectivity = billForce * Mathf.Sign(myRigidbody.velocity.x) * -1f;
            myRigidbody.AddForce(Reflectivity, 0, 0);
            ufoSE[1].Play();
        }
        else
        {
            bool isSuccessCaputure = ScoreManager.Instance.CalcScorePointInCaptureMob(mob.gameObject.tag);
            if (!isSuccessCaputure)
            {
                GameManager.Instance.DamageLife();
                ufoSE[1].Play();
            }
        }
    }

    public void GetMyCatchButtonDown()
    {
        isCatchButtonDown = true;

    }
    public void GetMyCatchButtonUp()
    {
        isCatchButtonDown = false;
        isCatchButtonTrueCount = 0;
    }

    //ゲーム終了時は押しても反応しないように処理
    public void GetBoostButtonDown()
    {
        if (!GameManager.Instance.IsGameEnd())
        {
            isBoostButtonDown = true;
        }
    }

    //ボタンを離した時、SE再生用ブール(isCatchButtonTrueCount)を0にリセット
    public void GetBoostButtonUp()
    {
        isBoostButtonDown = false;
        isBoostButtonTrueCount = 0;
    }
}
