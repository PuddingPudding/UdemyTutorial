using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float m_fSpeed = 5;
    [SerializeField] private Animator m_animPlayerAnimator;
    [SerializeField] private Sprite m_sprHeart;
    [SerializeField] private Sprite m_sprHeartBroken;
    [SerializeField] private RectTransform m_heartPos;
    [SerializeField] private int m_iCurrentHP = 4;
    private List<Image> m_imgArrHP = new List<Image>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v2MoveDir = Vector2.zero;
        float fAnimSpeed = 1; //動畫播放速度，預設為1
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) )
        {
            v2MoveDir = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) )
        {
            v2MoveDir = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            v2MoveDir = Vector2.left ;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) )
        {
            v2MoveDir = Vector2.right;
        }
        else //若什麼都不按，則把動畫播放速度調為0
        {
            fAnimSpeed = 0;
        }

        this.Movement(v2MoveDir);
        this.m_animPlayerAnimator.SetFloat("SpeedX", v2MoveDir.x);
        this.m_animPlayerAnimator.SetFloat("SpeedY", v2MoveDir.y);
        this.m_animPlayerAnimator.speed = fAnimSpeed;
    }

    void Movement(Vector2 _v2Dir)
    {
        this.transform.Translate(_v2Dir.normalized * Time.deltaTime * this.m_fSpeed);
    }

    void SetHPUI()
    {
        for(int i = 1; i < this.m_iCurrentHP; i++)
        {
        }
    }
}
