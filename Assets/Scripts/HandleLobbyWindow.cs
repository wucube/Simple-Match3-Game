using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ��Ϸ��������Ϸ���˵���
/// </summary>
public class HandleLobbyWindow : MonoBehaviour
{
    public Text txtCoin;
    public Text txtHistory;
    private GameRoot root;
    public void Init()
    {
        root = GameRoot.Instance;
        RefreshCoinData();
        RefreshHistoryData();
    }

    #region ClickEvts
    /// <summary>
    /// ������ݰ�ť����¼�
    /// </summary>
    public void OnBtnResetClicked()
    {
        PlayerPrefs.DeleteAll();
        root.OpenTipsWindow("����������");
    }
    /// <summary>
    /// ��ϰ��ť����¼�
    /// </summary>
    public void OnBtnPracticeClicked()
    {
        root.PlayUIClick();
        if (root.MCoin>= 500)
        {
            root.UpdateCoinData(root.MCoin-500);
            root.OpenFightWindow();
        }
        else
        {
            root.OpenTipsWindow("��Ҳ���");
        }
    }

    public void OnBtnMenuClicked()
    {
        root.PlayUIClick();
        root.OpenMenuWindow(OpType.Help);
    }
    #endregion

    #region ToolFunctions
    /// <summary>
    /// ˢ�½������
    /// </summary>
    public void RefreshCoinData()
    {
        txtCoin.text = root.MCoin.ToString();
    }
    /// <summary>
    /// ˢ����ʷ��¼����
    /// </summary>
    public void RefreshHistoryData()
    {
        string mTime = root.MTime;
        if (mTime != "")
        {
            int mScore = root.MScore;
            txtHistory.text = "��߻��֣�" + mScore + "�� \n\n��¼ʱ�䣺" + mTime;
            txtHistory.alignment = TextAnchor.MiddleLeft;
        }
        else
        {
            txtHistory.text = "��������";
        }
    }
    #endregion

}
