using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour , IReusable
{
    public delegate void CallBack(); //定義某個函式規範 (回傳值，傳入的參數等等)
    [SerializeField] private float m_fLifeTimeMax = 0.5f;
    [SerializeField] private GameObject m_goSwordEffect;
    private float m_fLifeTime = 0;
    private CallBack m_callBack = null;
    private BackToPool m_backToPool = null;

    // Use this for initialization
    void Start()
    {
        Debug.Log("劍劍生成");
    }

    public void InitAndStrike()
    {
        this.gameObject.SetActive(true);
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
            this.SwordDisappear();
        }
    }

    public void SwordDisappear()
    {
        this.gameObject.SetActive(false);
        //Instantiate(this.m_goSwordEffect, this.transform.position, this.transform.rotation);
        GameObject goSwordEffectTemp = ObjectPool.Instance.GetPrefab(ePrefabType.SWORD_EFFECT, this.transform.position, this.transform.rotation);
        goSwordEffectTemp.GetComponent<EffectScript>().Init();
        if (this.m_callBack != null)
        {
            this.m_callBack.Invoke();
        }
        ObjectPool.Instance.BackToPool(ePrefabType.SWORD, this.gameObject);
    }

    public void SetCallBack(CallBack _callBack)
    {
        this.m_callBack = _callBack;
    }

    public void SetReuse(BackToPool _backToPool)
    {
        this.m_backToPool = _backToPool;
    }
}
