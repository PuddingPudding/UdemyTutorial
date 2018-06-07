﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject m_goSwordPrefab;
    [SerializeField] private GameObject m_goSwordEffect;
    [SerializeField] private GameObject m_goCrabEffect;
    [SerializeField] private List<GameObject> m_listSwordPrefab;
    [SerializeField] private List<GameObject> m_listSwordEffect;
    [SerializeField] private List<GameObject> m_listCrabEffect;
    private static ObjectPool g_instance = null;

    private void Awake()
    {
        ObjectPool.g_instance = this;
        DontDestroyOnLoad(this);
    }
    private ObjectPool()//先將建構子宣告私有化，避免人家亂生孩子
    {
    }
    public static ObjectPool Instance //唯一存在的getter
    {
        get
        {
            return ObjectPool.g_instance;
        }
    }

    public GameObject GetPrefab(ePrefabType _prefabType)
    {
        List<GameObject> listPrefabChose = null;
        GameObject goPrefabChose = null;
        GameObject goPrefabTemp = null;
        switch (_prefabType)
        {
            case ePrefabType.SWORD:
                listPrefabChose = this.m_listSwordPrefab;
                goPrefabChose = this.m_goSwordPrefab;
                break;
            case ePrefabType.SWORD_EFFECT:
                listPrefabChose = this.m_listSwordEffect;
                goPrefabChose = this.m_goSwordEffect;
                break;
            case ePrefabType.CRAB_EFFECT:
                listPrefabChose = this.m_listCrabEffect;
                goPrefabChose = this.m_goCrabEffect;
                break;
        }

        if (listPrefabChose.Count > 0)
        {
            Debug.Log("原本就有物件，試圖排出");
            goPrefabTemp = listPrefabChose[0];
            listPrefabChose.RemoveAt(0);
        }
        else
        {
            goPrefabTemp = Instantiate(goPrefabChose);
        }
        goPrefabTemp.SetActive(true);
        return goPrefabTemp;
    }

    public GameObject GetPrefab(ePrefabType _prefabType, Vector3 _pos, Quaternion _rotation)
    {
        GameObject goTemp = this.GetPrefab(_prefabType);
        goTemp.transform.position = _pos;
        goTemp.transform.rotation = _rotation;
        return goTemp;
    }

    public void BackToPool(ePrefabType _prefabType, GameObject _goPrefab)
    {
        switch (_prefabType)
        {
            case ePrefabType.SWORD:
                this.m_listSwordPrefab.Add(_goPrefab);
                _goPrefab.SetActive(false);
                break;
            case ePrefabType.SWORD_EFFECT:
                this.m_listSwordEffect.Add(_goPrefab);
                _goPrefab.SetActive(false);
               break;
            case ePrefabType.CRAB_EFFECT:
                this.m_listCrabEffect.Add(_goPrefab);
                _goPrefab.SetActive(false);
                break;
        }
    }
}
