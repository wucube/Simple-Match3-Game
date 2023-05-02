using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 游戏大厅（游戏主菜单）
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
    /// 清空数据按钮点击事件
    /// </summary>
    public void OnBtnResetClicked()
    {
        PlayerPrefs.DeleteAll();
        root.OpenTipsWindow("数据清除完成");
    }
    /// <summary>
    /// 练习按钮点击事件
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
            root.OpenTipsWindow("金币不足");
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
    /// 刷新金币数据
    /// </summary>
    public void RefreshCoinData()
    {
        txtCoin.text = root.MCoin.ToString();
    }
    /// <summary>
    /// 刷新历史记录数据
    /// </summary>
    public void RefreshHistoryData()
    {
        string mTime = root.MTime;
        if (mTime != "")
        {
            int mScore = root.MScore;
            txtHistory.text = "最高积分：" + mScore + "分 \n\n记录时间：" + mTime;
            txtHistory.alignment = TextAnchor.MiddleLeft;
        }
        else
        {
            txtHistory.text = "暂无数据";
        }
    }
    #endregion

}
