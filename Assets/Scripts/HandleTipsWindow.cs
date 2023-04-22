using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ʾ����
/// </summary>
public class HandleTipsWindow : MonoBehaviour
{
    public Image tipsBg;
    public Text txtTips;
    public Animator ani;
    
    private Queue<string> tipsQue = new Queue<string> ();

    private bool isTipsShow = false;
    public void AddTips(string tips)
    {
        tipsQue.Enqueue (tips);
    }
    /// <summary>
    /// ������ʾ��������ʾ����
    /// </summary>
    /// <param name="tips"></param>
    private void SetTips(string tips)
    {
        //�������ݳ��ȵ���bg�Ĵ�С��ʾ
        int len = tips.Length;
        txtTips.text = tips;
        tipsBg.gameObject.SetActive(true);
        tipsBg.GetComponent<RectTransform>().sizeDelta = new Vector2(50 * len + 50, 100);

        //�������ſ���
        ani.Play("TipsWindowAni", 0, 0);

        RuntimeAnimatorController tor = ani.runtimeAnimatorController;

        AnimationClip[] clips = tor.animationClips;

        StartCoroutine(AniPlayDone(clips[0].length)); //��������ʱ������ʱ�䳤�ȣ���λ����
    }

    /// <summary>
    /// ����������������Ϊ
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator AniPlayDone(float sec)
    {
        yield return new WaitForSeconds(sec);
        tipsBg.gameObject.SetActive (false);
        //ani.StartPlayback();
        isTipsShow = false;
    }

    private void Update()
    {
        if(tipsQue.Count > 0 && isTipsShow == false)
        {
            string tips = tipsQue.Dequeue();

            isTipsShow = true;

            SetTips(tips);
        }
        
    }
}
