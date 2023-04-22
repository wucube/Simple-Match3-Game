using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ×Ô¶¯Ïú»Ù
/// </summary>
public class AutoDestroy : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
