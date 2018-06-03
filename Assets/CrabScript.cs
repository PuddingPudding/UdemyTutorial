using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabScript : MonoBehaviour
{
    public delegate void CallBack(); //定義某個函式規範 (回傳值，傳入的參數等等)
    [SerializeField] private GameObject m_goCrabEffect;
    [SerializeField] private int m_iMaxHP = 1;
    private int m_iCurrentHP;
    private CallBack m_callBack = null;
    // Use this for initialization
    void Start()
    {
        this.m_iCurrentHP = this.m_iMaxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Sword")
        {
            this.m_iCurrentHP--;
            if(this.m_iCurrentHP <= 0)
            {
                this.gameObject.SetActive(false);
                Instantiate(this.m_goCrabEffect, this.transform.position, this.transform.rotation);
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

    public void SetCallBack(CallBack _callBack)
    {
        this.m_callBack = _callBack;
    }
}
