using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 点击的操作类型
/// </summary>
public enum OpType
{
    None,
    Help,
    Pause,
    End
}
/// <summary>
/// 游戏菜单
/// </summary>
public class HandleMenuWindow : MonoBehaviour
{
    public Transform helpRoot;
    public Transform pauseRoot;
    public Transform endRoot;

    public Text txtCurrent;
    public Text txtHistory;
    public Image imgRecord;

    private OpType opType = OpType.None;
    private GameRoot root = null;

    public void Init(OpType tp)
    {
        root = GameRoot.Instance;
        opType = tp;
        switch (opType)
        {
            case OpType.Help:
                helpRoot.gameObject.SetActive(true);
                pauseRoot.gameObject.SetActive(false);
                endRoot.gameObject.SetActive(false);
                break;
            case OpType.Pause:
                helpRoot.gameObject.SetActive(false);
                pauseRoot.gameObject.SetActive(true);
                endRoot.gameObject.SetActive(false);
                break;
            case OpType.End:
                helpRoot.gameObject.SetActive(false);
                pauseRoot.gameObject.SetActive(false);
                endRoot.gameObject.SetActive(true);
                SetScoreData();
                break;
            case OpType.None:
                break;
        }
    }
    /// <summary>
    /// 显示当局得分
    /// </summary>
    private void SetScoreData()
    {
        txtCurrent.text = root.MCurScore.ToString();
        txtHistory.text = root.MScore.ToString();
        if(root.IsNewRecord)
            imgRecord.gameObject.SetActive(true);
        else 
            imgRecord.gameObject.SetActive(false);
    }
    /// <summary>
    /// 继承
    /// </summary>
    public void OnBtnContinueClicked()
    {
        root.PlayUIClick();
        if(opType == OpType.Pause)
            root.SetFightRun();

        gameObject.SetActive(false);
    }
    /// <summary>
    /// 返回大厅
    /// </summary>
    public void OnBtnBackLobbyClicked()
    {
        root.PlayUIClick();
        root.ExitFight();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 查看消除规则
    /// </summary>
    public void OnBtnRuleClicked()
    {
        root.PlayUIClick();
        helpRoot.gameObject.SetActive(true);
        pauseRoot.gameObject.SetActive(false);
    }
    /// <summary>
    /// 再来一次按钮事件
    /// </summary>
    public void OnBtnAgainClicked()
    {
        root.PlayUIClick();
        root.ReStartFight();
        gameObject.SetActive(false);
    }

    //待完成
    public void OnBtnShareClicked()
    {
        root.PlayUIClick();
        root.OpenTipsWindow("开发中....");
    }
}
