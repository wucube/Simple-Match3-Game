using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������ű�
/// </summary>
public class CubeItem : MonoBehaviour
{
    public RectTransform rectTrans;
    public Image imgIcon;

    //������������
    public int xIndex;
    public int yIndex;
    //ͼƬ��Դ����
    public int iconIndex;

    /// <summary>
    /// �������ͼƬԴ
    /// </summary>
    /// <param name="index">ͼƬ��Դ������</param>
    public void SetIconData(int index)
    {
        iconIndex = index;
        Sprite sp = Resources.Load<Sprite>("ResImages/Cubes/cube_" + index);
        imgIcon.sprite = sp;
    }

    /// <summary>
    /// �����Ƿ���ͬ�����ݷ���ʹ�õ�ͼƬԴ�ж�
    /// </summary>
    /// <param name="item">�������</param>
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

    #region �����������
    /// <summary>
    /// �Ƿ��ƶ�
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// �ƶ�ʱ��
    /// </summary>
    private float moveTime = 0;
    /// <summary>
    /// �ƶ��ٶ�����
    /// </summary>
    private Vector3 moveVel = Vector3.zero;

    /// <summary>
    /// Ŀ��λ��
    /// </summary>
    private Vector3 targetPos = Vector3.zero;

    /// <summary>
    /// �ƶ�ʱ�����
    /// </summary>
    private float countTime = 0;
    /// <summary>
    /// ��ָ��ʱ�����ƶ�
    /// </summary>
    /// <param name="time">�ƶ�ʱ��</param>
    /// <param name="from">���λ��</param>
    /// <param name="to">Ŀ��λ��</param>
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
