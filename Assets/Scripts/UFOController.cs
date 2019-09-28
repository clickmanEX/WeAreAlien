using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{

    private Rigidbody myRigidbody;
    private float maxSpeed = 50.0f;
    private float coefficient = 0.95f;  //減衰用変数
    private float turnForce = 50.0f;
    private float billForce = 1000.0f;
    private float minusTime = 1.5f;
    private int comboBonus = 1;
    private bool stop = false;
    public static bool isCatchButtonDown = false;
    private bool isForwardButtonDown = false;
    private bool isBackButtonDown = false;
    private int isCatchButtonTrueCount = 0;
    private AudioSource[] ufoSE;
    public static bool bunusPoint = false;
    public static bool minusPoint = false;

    // Use this for initialization
    void Start()
    {

        this.myRigidbody = GetComponent<Rigidbody>();
        ufoSE = GetComponents<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        if (LifeController.isEnd)
        {
            this.turnForce *= coefficient;   //すぐに止まらないように慣性をつけた動きにするため。
        }


        if (this.stop == false)     //28~52行目まで、上下左右の動きをつける。スペースキーを押していない間は動けるように条件をつけた。
        {
            if (this.myRigidbody.velocity.x <= maxSpeed)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    this.myRigidbody.AddForce(-this.turnForce, 0, 0);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    this.myRigidbody.AddForce(this.turnForce, 0, 0);
                }
            }

            if (this.myRigidbody.velocity.z <= maxSpeed)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    this.myRigidbody.AddForce(0, 0, this.turnForce);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    this.myRigidbody.AddForce(0, 0, -this.turnForce);
                }
                if (isForwardButtonDown)
                {
                    this.myRigidbody.AddForce(-this.turnForce, 0, this.turnForce);
                }
                if (isBackButtonDown)
                {
                    this.myRigidbody.AddForce(this.turnForce, 0, -this.turnForce);
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
            ScoreText.scorePt -= 500 / minusTime * Time.deltaTime;

        }
        else
        {
            this.stop = false;
            this.comboBonus = 1;
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
                if (this.myRigidbody.velocity.x <= maxSpeed)
                {
                    if (touch.phase == TouchPhase.Stationary && worldPos.x <= 0)
                    {
                        this.myRigidbody.AddForce(-this.turnForce, 0, 0);
                    }

                    if (touch.phase == TouchPhase.Stationary && worldPos.x > 0)
                    {
                        this.myRigidbody.AddForce(this.turnForce, 0, 0);
                    }

                }

            }

        }
    }


    void OnCollisionEnter(Collision mob)
    {

        if (mob.gameObject.tag == "Human" || mob.gameObject.tag == "Army" || mob.gameObject.tag == "Chef"
            || mob.gameObject.tag == "Scientist" || mob.gameObject.tag == "Alien"
            || mob.gameObject.tag == "Cat" || mob.gameObject.tag == "Dog")
        {

            if (CharactorTextContoller.MobText[1] && mob.gameObject.tag == "Army" || CharactorTextContoller.MobText[2] && mob.gameObject.tag == "Scientist"
               || CharactorTextContoller.MobText[3] && mob.gameObject.tag == "Chef" || CharactorTextContoller.MobText[4] && mob.gameObject.tag == "Cat"
               || CharactorTextContoller.MobText[4] && mob.gameObject.tag == "Dog" || CharactorTextContoller.MobText[7] && mob.gameObject.tag == "Alien")
            {
                ScoreText.scorePt += 2000 * comboBonus;
                bunusPoint = true;
                this.comboBonus++;
                return;
            }

            ScoreText.scorePt += 1000 * comboBonus;
            this.comboBonus++;
        }

        if (mob.gameObject.tag == "Car" || mob.gameObject.tag == "Ambulance" || mob.gameObject.tag == "Bear")
        {
            if (CharactorTextContoller.MobText[5] && mob.gameObject.tag == "Bear" || CharactorTextContoller.MobText[6] && mob.gameObject.tag == "Ambulance")
            {
                ScoreText.scorePt += 5000 * comboBonus;
                bunusPoint = true;
                this.comboBonus++;
                return;
            }

            ScoreText.scorePt -= 10000;
            minusPoint = true;
            CharactorTextContoller.minusTextnum = 0;
            LifeController.lifeCount -= 1;
            ufoSE[1].Play();
        }

        if (mob.gameObject.tag == "BillRight")
        {
            ScoreText.scorePt -= 10000;
            LifeController.lifeCount -= 1;
            minusPoint = true;
            CharactorTextContoller.minusTextnum = 1;
            this.myRigidbody.AddForce(-this.billForce, 0, 0);
            ufoSE[1].Play();
        }
        if (mob.gameObject.tag == "BillLeft")
        {
            ScoreText.scorePt -= 10000;
            LifeController.lifeCount -= 1;
            minusPoint = true;
            CharactorTextContoller.minusTextnum = 1;
            this.myRigidbody.AddForce(this.billForce, 0, 0);
            ufoSE[1].Play();
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

    public void GetMyForwardButtonDown()
    {
        this.isForwardButtonDown = true;
    }

    public void GetMyForwardButtonUp()
    {
        this.isForwardButtonDown = false;
    }


    public void GetMyBackButtonDown()
    {
        this.isBackButtonDown = true;
    }

    public void GetMyBackButtonUp()
    {
        this.isBackButtonDown = false;
    }

}
