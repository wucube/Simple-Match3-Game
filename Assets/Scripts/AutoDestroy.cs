using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ�����
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
