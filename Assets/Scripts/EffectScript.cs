using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField] private float m_fLifeTime = 2;
    [SerializeField] private ePrefabType m_effectType = ePrefabType.SWORD_EFFECT;
    private float m_fLifeTimeCounter = 0;

    private void Start()
    {
        this.Init();
    }

    public void Init()
    {
        this.m_fLifeTimeCounter = this.m_fLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        m_fLifeTimeCounter -= Time.deltaTime;
        if(this.m_fLifeTimeCounter <= 0 && this.isActiveAndEnabled)
        {
            ObjectPool.Instance.BackToPool(m_effectType,this.gameObject);
        }
    }
}
