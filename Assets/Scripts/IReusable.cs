using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReusable
{
    void SetReuse(BackToPool _backToPool);
}

public delegate void BackToPool(GameObject _goInput); //回收到池子