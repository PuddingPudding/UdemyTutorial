using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public delegate void CallBack(); //定義某個函式規範 (回傳值，傳入的參數等等)
    [SerializeField] private float m_fLifeTimeMax = 0.5f;
    private float m_fLifeTime = 0;
    private CallBack m_callBack = null;

    public void InitAndStrike()
    {
        m_fLifeTime = this.m_fLifeTimeMax;
    }

    private void Update()
    {
        this.SelfUpdate();
    }

    public void SelfUpdate()
    {
        this.m_fLifeTime -= Time.deltaTime;
        if(this.m_fLifeTime <= 0)
        {
            this.gameObject.SetActive(false);
            if(this.m_callBack != null)
            {
                Debug.Log("WTF?");
                this.m_callBack.Invoke();
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    public void SetCallBack(CallBack _callBack)
    {
        this.m_callBack = _callBack;
    }
}
