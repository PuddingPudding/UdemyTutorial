using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabScript : MonoBehaviour
{
    public delegate void CallBack(); //定義某個函式規範 (回傳值，傳入的參數等等)
    [SerializeField] private GameObject m_goCrabEffect;
    [SerializeField] private int m_iMaxHP = 1;
    [SerializeField] private SpriteRenderer m_face;
    [SerializeField] private Sprite m_sprFacingUp;
    [SerializeField] private Sprite m_sprFacingDown;
    [SerializeField] private Sprite m_sprFacingLeft;
    [SerializeField] private Sprite m_sprFacingRight;
    [SerializeField] private float m_fDirChangeTime = 1.5f;
    [SerializeField] private float m_fSpeed = 3;
    //[SerializeField] private float m_fDirChangeTimeForObs = 0.75f; //給碰撞用時改向用的計時器
    private float m_fDirChangeTimeCounter = 0;
    private int m_iCurrentHP;
    private CallBack m_callBack = null;
    private int m_iCurrentDir;
    private Vector3 m_v3MoveDir = Vector3.up;
    //private float m_fDirChangeTimeForObsCtr = 0;
    //private bool m_bChangeOnObs = true; //一般狀況下撞到牆壁就會轉向，但是不要讓他一直轉，所以用bool控制
    // Use this for initialization
    void Start()
    {
        this.m_iCurrentHP = this.m_iMaxHP;
        this.m_fDirChangeTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.m_fDirChangeTimeCounter -= Time.deltaTime;
        if (this.m_fDirChangeTimeCounter <= 0)
        {
            //Debug.Log("原本螃蟹速度方向: " + this.m_v3MoveDir);
            this.m_iCurrentDir += Random.Range(1, 4); //移動方位從另外三個方向中隨機挑選
            this.m_iCurrentDir %= 4;
            this.m_fDirChangeTimeCounter += this.m_fDirChangeTime;
            this.SwitchDir(this.m_iCurrentDir);
            //Debug.Log("現在螃蟹速度方向: " + this.m_v3MoveDir);
        }
        //if (!this.m_bChangeOnObs)//撞過後一小段時間內就算再次撞牆也不要換方向
        //{
        //    this.m_fDirChangeTimeForObsCtr += Time.deltaTime;
        //    if (this.m_fDirChangeTimeForObsCtr >= this.m_fDirChangeTimeForObs)
        //    {
        //        this.m_fDirChangeTimeForObsCtr = 0;
        //        this.m_bChangeOnObs = true;
        //    }
        //}

        this.Movement((Vector2)this.m_v3MoveDir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            this.Hit(1);
        }
    }

    public void Hit(int _iDmg)
    {
        this.m_iCurrentHP -= _iDmg;
        if (this.m_iCurrentHP <= 0)
        {
            this.gameObject.SetActive(false);
            GameObject goCrabEffect = ObjectPool.Instance.GetPrefab(ePrefabType.CRAB_EFFECT , this.transform.position, this.transform.rotation);
            goCrabEffect.GetComponent<EffectScript>().Init();
            if (this.m_callBack != null)
            {
                this.m_callBack.Invoke();
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.Hit(1);
            collision.gameObject.GetComponent<PlayerScript>().Hit(1);
        }
        //else if(collision.gameObject.tag == "Obstacle" && this.m_bChangeOnObs)
        //{
        //    Debug.Log("幹林娘撞到了");
        //    this.m_fDirChangeTimeCounter = 0;
        //    this.m_bChangeOnObs = false;
        //}
    }

    void Movement(Vector2 _v2Dir)
    {
        this.transform.Translate(_v2Dir.normalized * Time.deltaTime * this.m_fSpeed);
    }

    private void SwitchDir(int _iDir)
    {
        this.m_iCurrentDir = _iDir;
        //Debug.Log("幹林爹呼叫了");
        switch (this.m_iCurrentDir) 
        {
            case 0:
                this.m_v3MoveDir = Vector3.up;
                this.m_face.sprite = this.m_sprFacingUp;
                break;
            case 1:
                this.m_v3MoveDir = Vector3.down;
                this.m_face.sprite = this.m_sprFacingDown;
                break;
            case 2:
                this.m_v3MoveDir = Vector3.left;
                this.m_face.sprite = this.m_sprFacingLeft;
                break;
            case 3:
                this.m_v3MoveDir = Vector3.right;
                this.m_face.sprite = this.m_sprFacingRight;
                break;
        }
    }

    public void SetCallBack(CallBack _callBack)
    {
        this.m_callBack = _callBack;
    }
}
