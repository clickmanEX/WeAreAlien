using UnityEngine;
using System.Collections;
//TODO:リファクタリングすること
public class MobController : MonoBehaviour
{

    private Rigidbody mobRigidbody;
    Transform ufoTrs;

    float timer;
    private float capturingSpeed;
    [SerializeField]
    bool isBoundRun;

    public enum STATE
    {
        RUN,
        CAPTURED,
        DOWN,
        NONE
    }
    public STATE state;

    readonly float CAPTURE_TIME = 1.5f;

    private void Awake()
    {
        mobRigidbody = GetComponent<Rigidbody>();
        ufoTrs = GameObject.Find("UFO").transform;
        float upDistance = ufoTrs.transform.position.y;
        capturingSpeed = upDistance / CAPTURE_TIME;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        timer = 0f;
        transform.localEulerAngles = Vector3.zero;
        SetState(STATE.RUN);
    }

    void SetState(STATE state)
    {
        this.state = state;
        switch (state)
        {
            case STATE.RUN:
            case STATE.CAPTURED:
                mobRigidbody.isKinematic = true;
                break;
            case STATE.DOWN:
                mobRigidbody.isKinematic = false;
                break;
            case STATE.NONE:
                break;
        }
    }

    readonly float RUN_SPEED = -10f;
    readonly float BOOST_SPEED_COEF = 10f;
    readonly float BOUND_AMPLITUDE = 1f;
    readonly float BOUND_FREQUENCY = 1f;
    readonly float ROTATE_SPEED = 360f;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.RUN:
                if (UFOController.Instance.IsBoost())
                {
                    transform.Translate(0, 0, Time.deltaTime * RUN_SPEED * BOOST_SPEED_COEF);
                }
                else
                {
                    transform.Translate(0, 0, Time.deltaTime * RUN_SPEED);
                }

                if (isBoundRun)
                {
                    timer += Time.deltaTime;
                    float y = BOUND_AMPLITUDE * Mathf.Sin(2f * Mathf.PI * BOUND_FREQUENCY * timer);
                    transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Abs(y), transform.localPosition.z);
                }
                break;
            case STATE.CAPTURED:
                transform.Rotate(0, ROTATE_SPEED * Time.deltaTime, 0);
                transform.Translate(0, Time.deltaTime * capturingSpeed, 0);
                break;
            case STATE.DOWN:
                if (Mathf.Approximately(mobRigidbody.velocity.y, 0f))
                {
                    //TODO モブキャラ同士が重なった時、重力が効かないケースが発生する。要調査
                    mobRigidbody.isKinematic = true;
                    mobRigidbody.isKinematic = false;
                }
                break;
        }

        if (this.transform.position.y < -10f || this.transform.position.z < -60f)
        {
            MobGenerator.Instance.ReduceMobObjectCount();
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider triggerStay)
    {
        if (UFOController.Instance.IsCaputuring())
        {
            if (state != STATE.CAPTURED)
            {
                transform.localPosition = new Vector3(ufoTrs.localPosition.x, transform.localPosition.y, ufoTrs.localPosition.z);
                SetState(STATE.CAPTURED);
            }
        }
        else
        {
            if (state == STATE.CAPTURED)
            {
                SetState(STATE.DOWN);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "UFOBody" && state == STATE.CAPTURED)
        {
            transform.position = new Vector3(-40f, 0f, -40f);
            gameObject.SetActive(false);
            MobGenerator.Instance.ReduceMobObjectCount();
        }

        if (other.gameObject.tag == "Floor" && state == STATE.DOWN)
        {
            Init();
        }
    }
}

