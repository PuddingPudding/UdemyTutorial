using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float m_fSpeed = 5;
    [SerializeField] private Animator m_animPlayerAnimator;
    [SerializeField] private Sprite m_sprHeart;
    [SerializeField] private Sprite m_sprHeartBroken;
    [SerializeField] private RectTransform m_heartPos;
    [SerializeField] private int m_iMaxHP = 4;
    [SerializeField] private int m_iMaxUpgradeHP = 8; //可升級的最大上限
    [SerializeField] private GameObject m_goSword;
    [SerializeField] private float m_fSwordForce = 200;
    [SerializeField] private float m_fImiTime = 1;
    [SerializeField] private SpriteRenderer m_srPlayeFace;
    private int m_iCurrentHP = 0;
    private bool m_bCanMove = true;
    private List<Image> m_imgArrHP = new List<Image>();
    private float m_fImiTimeCounter = 0;

    // Use this for initialization
    void Start()
    {
        this.m_iCurrentHP = this.m_iMaxHP;
        this.SetHPUI();
    }

    // Update is called once per frame
    void Update()
    {
        this.PlayerControl();
    }

    private void PlayerControl()
    {
        Vector2 v2MoveDir = Vector2.zero;
        float fAnimSpeed = 1; //動畫播放速度，預設為1
        if (this.m_bCanMove)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                v2MoveDir = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                v2MoveDir = Vector2.down;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                v2MoveDir = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                v2MoveDir = Vector2.right;
            }
            else //若什麼都不按，則把動畫播放速度調為0
            {
                fAnimSpeed = 0;
            }
        }
        else
        {
            fAnimSpeed = 0;
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.Attack();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            this.m_iCurrentHP++;
            this.SetHPUI();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            this.m_iCurrentHP--;
            this.SetHPUI();
        }

        if(this.m_fImiTimeCounter > 0) //若還有無敵時間
        {
            this.m_fImiTimeCounter -= Time.deltaTime;
            int iRanNum = Random.Range(0, 2);
            if(iRanNum == 1)
            {
                this.m_srPlayeFace.enabled = true;
            }
            else
            {
                this.m_srPlayeFace.enabled = false;
            }

            if(m_fImiTimeCounter <= 0) //在扣完時間假如無敵時間結束
            {
                this.m_srPlayeFace.enabled = true;
            }
        }

        this.Movement(v2MoveDir);
        this.m_animPlayerAnimator.SetFloat("SpeedX", v2MoveDir.x);
        this.m_animPlayerAnimator.SetFloat("SpeedY", v2MoveDir.y);
        this.m_animPlayerAnimator.speed = fAnimSpeed;
    }

    private void Movement(Vector2 _v2Dir)
    {
        this.transform.Translate(_v2Dir.normalized * Time.deltaTime * this.m_fSpeed);
    }

    private void SetHPUI()
    {
        int iHeartNum = this.m_iCurrentHP / 2;
        for (int i = 0; i < iHeartNum; i++)
        {
            if (i + 1 > this.m_imgArrHP.Count) //如果新生成的血量UI超出列表上限，那就額外生成
            {
                GameObject goTemp = new GameObject();
                goTemp.AddComponent<Image>().sprite = this.m_sprHeart;
                goTemp.GetComponent<RectTransform>().SetParent(this.m_heartPos);
                goTemp.GetComponent<RectTransform>().localPosition = new Vector2(150 * (i), 0);
                goTemp.transform.localScale = new Vector3(1, 1, 1);
                this.m_imgArrHP.Add(goTemp.GetComponent<Image>());
            }
            else
            {
                this.m_imgArrHP[i].sprite = this.m_sprHeart;
                this.m_imgArrHP[i].gameObject.SetActive(true);
            }
        }
        if (this.m_iCurrentHP % 2 == 1) //每兩滴血算一顆愛心，要是血量為奇數，最後會多一滴血
        {
            iHeartNum++; //愛心數量會多一，不過這一顆將會是半顆愛心
            if (iHeartNum > this.m_imgArrHP.Count) //若多出來的那一滴血超出列表範圍
            {
                GameObject goTemp = new GameObject();
                goTemp.AddComponent<Image>().sprite = this.m_sprHeartBroken;
                goTemp.GetComponent<RectTransform>().SetParent(this.m_heartPos);
                goTemp.GetComponent<RectTransform>().localPosition = new Vector2(150 * (iHeartNum - 1), 0);
                goTemp.transform.localScale = new Vector3(1, 1, 1);
                this.m_imgArrHP.Add(goTemp.GetComponent<Image>());
            }
            else
            {
                this.m_imgArrHP[iHeartNum - 1].sprite = this.m_sprHeartBroken;
                this.m_imgArrHP[iHeartNum - 1].gameObject.SetActive(true);
            }
        }

        for (int i = iHeartNum; i < this.m_imgArrHP.Count; i++) //若原本的愛心圖示列表比現在需要顯示的愛心數量多，那就把他們全部關掉
        {
            this.m_imgArrHP[i].gameObject.SetActive(false);
        }
    }

    private void Attack()
    {
        if(this.m_bCanMove)
        {
            this.m_bCanMove = false;
            //GameObject goSwordTemp = Instantiate(this.m_goSword, this.transform.position, this.m_goSword.transform.rotation);
            GameObject goSwordTemp = ObjectPool.Instance.GetPrefab(ePrefabType.SWORD);
            goSwordTemp.transform.position = this.transform.position;
            goSwordTemp.GetComponent<SwordScript>().InitAndStrike();
            #region //讓武器在生成時轉向
            if (this.m_animPlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("WalkingDown"))
            {
                goSwordTemp.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (this.m_animPlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("WalkingUp"))
            {
                goSwordTemp.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (this.m_animPlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("WalkingLeft"))
            {
                goSwordTemp.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            else if (this.m_animPlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("WalkingRight"))
            {
                goSwordTemp.transform.localEulerAngles = new Vector3(0, 0, 270);
            }
            #endregion
            goSwordTemp.GetComponent<Rigidbody2D>().AddForce(goSwordTemp.transform.up * this.m_fSwordForce);
            goSwordTemp.GetComponent<SwordScript>().SetCallBack(this.EndAttack);
            this.m_animPlayerAnimator.SetBool("Attacking", true);
        }        
    }

    private void EndAttack()
    {
        this.m_bCanMove = true;
        this.m_animPlayerAnimator.SetBool("Attacking", false);
    }

    public void Hit(int _iDmg)
    {
        if(this.m_fImiTimeCounter <= 0)
        {
            this.m_iCurrentHP -= _iDmg;
            this.m_fImiTimeCounter = this.m_fImiTime;
            this.SetHPUI();
        }        
    }

    public void HPUpgrade(int _iUpNum)
    {
        this.m_iMaxHP += _iUpNum;
        if(this.m_iMaxHP >= this.m_iMaxUpgradeHP)
        {
            this.m_iMaxHP = this.m_iMaxUpgradeHP;
        }
        this.m_iCurrentHP = this.m_iMaxHP;
        this.SetHPUI();
    }
}
