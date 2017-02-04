using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UFOController : MonoBehaviour {

    private Rigidbody myRigidbody;
    private float maxSpeed = 50.0f;     //UFOの最高速度 
    private float coefficient = 0.95f;  //ゲーム終了後の減衰係数 
    private float turnForce = 50.0f;    //UFOを動かす力 
    private float billForce = 1000.0f;  //ビルにぶつかったときの反発力 
    private float minusTime = 1.5f;     //スコアを減少させるのに使う値 
    private int comboBonus = 1;         //コンボボーナスをリセットするための変数 
    private bool stop = false;          //機体をストップさせるブール変数 
    public static bool isCatchButtonDown = false;  //キャッチボタン用ブール変数 
    public static bool isBoostButtonDown = false;  //ブーストボタン用ブール変数 
    private int isCatchButtonTrueCount = 0;     //SE再生用のブール変数 
    private int isBoostButtonTrueCount = 0;     //SE再生用のブール変数 
    private AudioSource[] ufoSE;                //インスペクタに入れたオーディオソースを入れる配列 
    public static bool bunusPoint = false;      //ボーナスポイント用テキストを呼び出すブール変数。CharactorTextContollerに使われている。 
    public static bool minusPoint = false;      //マイナスポイント用テキストを呼び出すブール変数。CharactorTextContollerに使われている。 
    public VirtualJoystick virtualJoystick;

    // Use this for initialization
    void Start () {

        this.myRigidbody = GetComponent<Rigidbody>();
        ufoSE = GetComponents<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

        //VirtualJoystickクラスのInputDirectionに水平・垂直情報があるため、抽出 
        Vector3 UFOmove = Vector3.zero;
        UFOmove.x = Input.GetAxis("Horizontal");
        UFOmove.z = Input.GetAxis("Vertical");

        if (virtualJoystick.InputDirection != Vector3.zero)
        {
            UFOmove = virtualJoystick.InputDirection;
        }

        if (LifeController.isEnd)
        {
            this.turnForce *= coefficient;   //ゲーム終了後、すぐに止まらないように慣性をつけた動きにするため。 
            UFOmove = Vector3.zero;
        }

        if (this.stop == false)     //52~81行目まで、上下左右の動きをつける。スペースキーを押していない間は動けるように条件をつけた。 
        {
            if (this.myRigidbody.velocity.x <= maxSpeed)
            {
                this.myRigidbody.AddForce(UFOmove.x * this.turnForce, 0, 0);

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
                this.myRigidbody.AddForce(0, 0, UFOmove.z * this.turnForce);

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    this.myRigidbody.AddForce(0, 0, this.turnForce);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    this.myRigidbody.AddForce(0, 0, -this.turnForce);
                }

            }

        }
        else         //スペースキー押してる間はモブを吸い込み、UFOを停止させる。
        {           
                this.myRigidbody.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space) || isCatchButtonDown)    //スペースキー押してる間は停止、スペースキーを離すと動けるようにし、吸い込む音を停止。
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

        if (isCatchButtonDown)    //CatchButton押したとき、一回だけ吸い込み音を再生
        {
            isCatchButtonTrueCount++;

            if(isCatchButtonTrueCount <= 1)
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

    }

    //UFOと衝突したモブキャラについているタグで判別してスコアを計算。キャッチボタンを押している間にモブキャラが衝突するとコンボボーナスが+1加算され、コンボボーナス値とプラスポイントを乗算したものがスコアに入る。
    void OnCollisionEnter(Collision mob)
    {
        //人間系モブキャラについて計算する。
        if (mob.gameObject.tag == "Human" || mob.gameObject.tag == "Army" || mob.gameObject.tag == "Chef"
            || mob.gameObject.tag == "Scientist" || mob.gameObject.tag == "Alien"
            || mob.gameObject.tag == "Cat" || mob.gameObject.tag == "Dog")
        {
            //上官ウインドウのテキストと特定のモブキャラタグが一致した場合、ボーナスポイントで計算。それ以外は通常ポイントで計算。
            //CharactorTextContollerのbunusPointをtureにすることでボーナス用テキストを表示させる。
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

        //乗り物系モブキャラについて計算する。
        if (mob.gameObject.tag == "Car" || mob.gameObject.tag == "Ambulance" || mob.gameObject.tag == "Bear")
        {
            //上官ウインドウのテキストと特定のモブキャラタグが一致した場合、ボーナスポイントで計算。それ以外はマイナス
            //CharactorTextContollerのbunusPointをtureにすることでボーナス用テキストを表示させる。
            if (CharactorTextContoller.MobText[5] && mob.gameObject.tag == "Bear" || CharactorTextContoller.MobText[6] && mob.gameObject.tag == "Ambulance")
            {
                ScoreText.scorePt += 5000 * comboBonus;
                bunusPoint = true;
                this.comboBonus++;
                return;
            }

            //CharactorTextContollerのminusPointをtureにすることでマイナス用テキストを表示させる。
            ScoreText.scorePt -= 10000;
            minusPoint = true;
            CharactorTextContoller.minusTextnum = 0;
            LifeController.lifeCount -= 1;
            ufoSE[1].Play();
        }

        //178～195行目は左右の壁に当たった際、マイナスポイント・マイナス用テキスト表示・ライフ-1の計算をし、連続で当たらないようにAddForceで反発力を与える。
        if(mob.gameObject.tag == "BillRight")
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

    //ゲーム終了時は押しても反応しないように処理
    public void GetMyCatchButtonDown()
    {
        if (LifeController.isEnd == false)
        {
            isCatchButtonDown = true;
        }
    }

    //ボタンを離した時、SE再生用ブール(isCatchButtonTrueCount)を0にリセット
    public void GetMyCatchButtonUp()
    {
        isCatchButtonDown = false;
        isCatchButtonTrueCount = 0;
    }

    //ゲーム終了時は押しても反応しないように処理
    public void GetBoostButtonDown()
    {
        if (LifeController.isEnd == false)
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
