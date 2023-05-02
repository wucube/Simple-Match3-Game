using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 方块对象脚本
/// </summary>
public class CubeItem : MonoBehaviour
{
    public RectTransform rectTrans;
    public Image imgIcon;

    //方块阵列索引
    public int xIndex;
    public int yIndex;
    //图片资源索引
    public int iconIndex;

    /// <summary>
    /// 方块加载图片源
    /// </summary>
    /// <param name="index">图片资源的索引</param>
    public void SetIconData(int index)
    {
        iconIndex = index;
        Sprite sp = Resources.Load<Sprite>("ResImages/Cubes/cube_" + index);
        imgIcon.sprite = sp;
    }

    /// <summary>
    /// 方块是否相同，根据方块使用的图片源判断
    /// </summary>
    /// <param name="item">方块对象</param>
    /// <returns></returns>
    public bool IsSameType(CubeItem item)
    {
        if(item.iconIndex== iconIndex) return true;

        else return false;
    }

    public bool Equals(CubeItem item)
    {
        if (item.xIndex == xIndex && item.yIndex == yIndex)
            return true;
        else return false;
    }

    #region 动画控制相关
    /// <summary>
    /// 是否移动
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// 移动时间
    /// </summary>
    private float moveTime = 0;
    /// <summary>
    /// 移动速度向量
    /// </summary>
    private Vector3 moveVel = Vector3.zero;

    /// <summary>
    /// 目标位置
    /// </summary>
    private Vector3 targetPos = Vector3.zero;

    /// <summary>
    /// 移动时间计数
    /// </summary>
    private float countTime = 0;
    /// <summary>
    /// 在指定时间内移动的速度
    /// </summary>
    /// <param name="time">移动时间</param>
    /// <param name="from">起点位置</param>
    /// <param name="to">目标位置</param>
    public void MovePosInTime(float time, Vector3 from, Vector3 to)
    {
        moveTime = time;
        targetPos = to;
        rectTrans.localPosition = from;

        float speedX = (to.x - from.x) / moveTime;
        float speedY = (to.y - from.y) / moveTime;
        float speedZ = (to.z - from.z) / moveTime;

        moveVel = new Vector3(speedX, speedY, speedZ);

        isMove = true;
    }
    private void Update()
    {
        if (isMove)
        {
            float delt = Time.deltaTime;

            rectTrans.localPosition += delt * moveVel;

            countTime += delt;
            
            if (countTime >= moveTime)
            {
                rectTrans.localPosition = targetPos;

                //reset Data
                moveTime = 0;
                countTime = 0;
                targetPos = Vector3.zero;
                moveVel = Vector3.zero;
                isMove = false;
            }
        }
    }
    #endregion
}
