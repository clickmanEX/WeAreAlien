using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UFOController : MonoBehaviour {

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


    // Use this for initialization
    void Start () {

        this.myRigidbody = GetComponent<Rigidbody>();
        Debug.Log(LifeController.lifeCount);

}
	
	// Update is called once per frame
	void Update () {

        if (LifeController.isEnd)
        {
            this.turnForce *= coefficient;   //すぐに止まらないように慣性をつけた動きにするため。
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("GameScene");
                LifeController.isEnd = false;
                ScoreText.scorePt = 0;
                LifeController.lifeCount = 3;
            }

        }
        this.myRigidbody.velocity *= coefficient;   //すぐに止まらないように慣性をつけた動きにするため。

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
                if (Input.GetKey(KeyCode.UpArrow) || isForwardButtonDown)
                {
                    this.myRigidbody.AddForce(0, 0, this.turnForce);
                }
                if (Input.GetKey(KeyCode.DownArrow) || isBackButtonDown)
                {
                    this.myRigidbody.AddForce(0, 0, -this.turnForce);
                }

            }

            

        }
        else{           //スペースキー押してる間はモブを吸い込むため、UFOは停止する。
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

                    if (touch.phase == TouchPhase.Stationary && worldPos.x > 0 )
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
            ScoreText.scorePt += 1000 * comboBonus;
            this.comboBonus++;
            Debug.Log(mob.gameObject.tag);
        }

        if (mob.gameObject.tag == "Car" || mob.gameObject.tag == "Ambulance" || mob.gameObject.tag == "Bear")
        {
            ScoreText.scorePt -= 10000;
            LifeController.lifeCount -= 1;
            Debug.Log(mob.gameObject.tag);
            Debug.Log(LifeController.lifeCount);
        }

        if(mob.gameObject.tag == "BillRight")
        {
            ScoreText.scorePt -= 10000;
            LifeController.lifeCount -= 1;
            this.myRigidbody.AddForce(-this.billForce, 0, 0);
            Debug.Log(mob.gameObject.tag);
            Debug.Log(LifeController.lifeCount);
        }
        if (mob.gameObject.tag == "BillLeft")
        {
            ScoreText.scorePt -= 10000;
            LifeController.lifeCount -= 1;
            this.myRigidbody.AddForce(this.billForce, 0, 0);
            Debug.Log(mob.gameObject.tag);
            Debug.Log(LifeController.lifeCount);
        }

    }

    public void GetMyCatchButtonDown()
    {
        isCatchButtonDown = true;
    }
    public void GetMyCatchButtonUp()
    {
        isCatchButtonDown = false;
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
