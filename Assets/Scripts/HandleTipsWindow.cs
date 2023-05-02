using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示窗口
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
    /// 设置提示弹窗的显示内容
    /// </summary>
    /// <param name="tips"></param>
    private void SetTips(string tips)
    {
        //根据内容长度调整bg的大小显示
        int len = tips.Length;
        txtTips.text = tips;
        tipsBg.gameObject.SetActive(true);
        tipsBg.GetComponent<RectTransform>().sizeDelta = new Vector2(50 * len + 50, 100);

        //动画播放控制
        ani.Play("TipsWindowAni", 0, 0);

        RuntimeAnimatorController tor = ani.runtimeAnimatorController;

        AnimationClip[] clips = tor.animationClips;

        StartCoroutine(AniPlayDone(clips[0].length)); //传入运行时动画的时间长度，单位：秒
    }

    /// <summary>
    /// 弹窗动画播完后的行为
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
