using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerScript>().HPUpgrade(1);
            this.gameObject.SetActive(false);
            ObjectPool.Instance.BackToPool(ePrefabType.POTION_PREFAB , this.gameObject);
        }
    }
}
