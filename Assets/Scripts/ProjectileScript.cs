using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public delegate void CallBack(); //定義某個函式規範 (回傳值，傳入的參數等等)
    [SerializeField] private float m_fLifeTimeMax = 1;
    private float m_fLifeTime = 0;
    private CallBack m_callBack = null;

    public void InitAndShoot()
    {
        this.gameObject.SetActive(true);
        m_fLifeTime = this.m_fLifeTimeMax;
    }

    // Update is called once per frame
    void Update()
    {
        this.SelfUpdate();
    }

    public void SelfUpdate()
    {
        this.m_fLifeTime -= Time.deltaTime;
        if (this.m_fLifeTime <= 0)
        {
            this.ProjectileDisappear();
        }
    }

    public void ProjectileDisappear()
    {
        ObjectPool.Instance.BackToPool(ePrefabType.ENEMY_PROJECTILE, this.gameObject);
        GameObject effectTemp = ObjectPool.Instance.GetPrefab(ePrefabType.ENEMY_PROJECTILE_EFFECT, this.transform.position, this.transform.rotation);
        effectTemp.GetComponent<EffectScript>().Init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerScript>().Hit(1);
            this.ProjectileDisappear();
        }
    }
}
