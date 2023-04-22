using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����Ĳ�������
/// </summary>
public enum OpType
{
    None,
    Help,
    Pause,
    End
}
/// <summary>
/// ��Ϸ�˵�
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
    /// ��ʾ���ֵ÷�
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
    /// �̳�
    /// </summary>
    public void OnBtnContinueClicked()
    {
        root.PlayUIClick();
        if(opType == OpType.Pause)
            root.SetFightRun();

        gameObject.SetActive(false);
    }
    /// <summary>
    /// ���ش���
    /// </summary>
    public void OnBtnBackLobbyClicked()
    {
        root.PlayUIClick();
        root.ExitFight();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// �鿴��������
    /// </summary>
    public void OnBtnRuleClicked()
    {
        root.PlayUIClick();
        helpRoot.gameObject.SetActive(true);
        pauseRoot.gameObject.SetActive(false);
    }
    /// <summary>
    /// ����һ�ΰ�ť�¼�
    /// </summary>
    public void OnBtnAgainClicked()
    {
        root.PlayUIClick();
        root.ReStartFight();
        gameObject.SetActive(false);
    }

    //�����
    public void OnBtnShareClicked()
    {
        root.PlayUIClick();
        root.OpenTipsWindow("������....");
    }
}
