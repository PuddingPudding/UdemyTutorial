﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScript : MonoBehaviour
{
    public delegate void CallBack(); //定義某個函式規範 (回傳值，傳入的參數等等)
    [SerializeField] private float m_fSpeed = 3;
    [SerializeField] private float m_fDirTime = 0.7f; //每次方向轉變所需的時間
    [SerializeField] private Animator m_animator;
    [SerializeField] private int m_iMaxHP = 4;
    [SerializeField] private float m_fAtkTime = 2;
    [SerializeField] private float m_fShootingForce = 500;
    private int m_iCurrentHP;
    private float m_fDirTimer = 0;
    private int m_iCurrentDir = 0;
    private Vector2 m_v2MoveDir = Vector2.zero;
    private float m_fAtkTimer = 0;
    private CallBack m_callBack = null;

    // Use this for initialization
    void Start()
    {
        this.m_iCurrentHP = this.m_iMaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        this.SelfUpdate();
    }

    public void SelfUpdate()
    {
        this.m_fDirTimer += Time.deltaTime;
        if (this.m_fDirTimer >= this.m_fDirTime)
        {
            this.m_fDirTimer -= this.m_fDirTime;
            this.m_iCurrentDir = Random.Range(0, 4);
            switch (this.m_iCurrentDir)
            {
                case 0:
                    this.m_v2MoveDir = Vector2.up;
                    break;
                case 1:
                    this.m_v2MoveDir = Vector2.down;
                    break;
                case 2:
                    this.m_v2MoveDir = Vector2.left;
                    break;
                case 3:
                    this.m_v2MoveDir = Vector2.right;
                    break;
            }
        }
        this.m_fAtkTimer += Time.deltaTime;
        if(this.m_fAtkTimer >= this.m_fAtkTime)
        {
            this.Attack();
            this.m_fAtkTimer -= this.m_fAtkTime;
        }

        this.Movement(this.m_v2MoveDir);
        this.m_animator.SetFloat("SpeedX", this.m_v2MoveDir.x);
        this.m_animator.SetFloat("SpeedY", this.m_v2MoveDir.y);
    }

    private void Movement(Vector2 _v2Dir)
    {
        this.transform.Translate(_v2Dir.normalized * Time.deltaTime * this.m_fSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            collision.GetComponent<SwordScript>().SwordDisappear();
            this.Hit(1);
        }
    }

    private void Attack()
    {
        GameObject goProjectileTemp = ObjectPool.Instance.GetPrefab(ePrefabType.ENEMY_PROJECTILE , this.transform.position , this.transform.rotation);
        if(this.m_animator.GetCurrentAnimatorStateInfo(0).IsName("WalkingDown") )
        {
            goProjectileTemp.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else if (this.m_animator.GetCurrentAnimatorStateInfo(0).IsName("WalkingUp"))
        {
            goProjectileTemp.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (this.m_animator.GetCurrentAnimatorStateInfo(0).IsName("WalkingLeft"))
        {
            goProjectileTemp.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (this.m_animator.GetCurrentAnimatorStateInfo(0).IsName("WalkingRight"))
        {
            goProjectileTemp.transform.localEulerAngles = new Vector3(0, 0, 270);
        }
        goProjectileTemp.GetComponent<Rigidbody2D>().AddForce(goProjectileTemp.transform.up * this.m_fShootingForce);
        goProjectileTemp.GetComponent<ProjectileScript>().InitAndShoot();
    }

    public void Hit(int _iDmg)
    {
        this.m_iCurrentHP -= _iDmg;
        if (this.m_iCurrentHP <= 0)
        {
            this.gameObject.SetActive(false);
            ObjectPool.Instance.GetPrefab(ePrefabType.DRAGON_EFFECT, this.transform.position, this.transform.rotation);
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
}
