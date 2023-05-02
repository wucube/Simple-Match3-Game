using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动销毁
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}