using UnityEngine;

/// <summary>
/// 游戏根节点
/// </summary>
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance = null;
    #region UIDefine
    public HandleLobbyWindow lobbyWin;
    public HandleTipsWindow tipsWin;
    public HandleFightWindow fightWin;
    public HandleMenuWindow menuWin;

    public AudioSource bgAudio;
    public AudioSource uiClickAudio;
    public AudioSource effectAudio;

    public AudioClip lobbyBgClip;
    public AudioClip fightBgClip;
    public AudioClip bombClip;
    public AudioClip lightingClip;
    public AudioClip waveClip;
    public AudioClip[] cleanClips;


    #endregion

    #region DataArea
    /// <summary>
    /// 玩家拥有金币数量
    /// </summary>
    private int mCoin = 0;
    public int MCoin => mCoin;
    /// <summary>
    /// 玩家历史最高分
    /// </summary>
    private int mScore = 0;
    public int MScore => mScore;
    /// <summary>
    /// 最高分创建的时间
    /// </summary>
    private string mTime = "";
    public string MTime => mTime;
    /// <summary>
    /// 当前局获得分类
    /// </summary>
    private int mCurScore = 0;
    public int MCurScore => mCurScore;
    /// <summary>
    /// 是否为新纪录
    /// </summary>
    private bool isNewRecord = false;
    public bool IsNewRecord => isNewRecord;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        OpenTipsWindow("游戏启动成功");
        InitGameData();
        OpenLobbyWindow();

    }
    /// <summary>
    /// 打开大厅界面(游戏菜单界面)
    /// </summary>
    private void OpenLobbyWindow()
    {
        lobbyWin.gameObject.SetActive(true);
        lobbyWin.Init();
        PlayLobbyBG();

        fightWin.gameObject.SetActive(false);
        menuWin.gameObject.SetActive(false);
        tipsWin.gameObject.SetActive(true);
    }
    /// <summary>
    /// 打开消除界面
    /// </summary>
    public void OpenFightWindow()
    {
        fightWin.gameObject.SetActive(true);
        fightWin.Init();
        PlayFightBG();
    }
    /// <summary>
    /// 打开菜单界面
    /// </summary>
    /// <param name="tp"></param>
    public void OpenMenuWindow(OpType tp)
    {
        menuWin.gameObject.SetActive(true);
        menuWin.Init(tp);
    }
    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData()
    {
        //是否首次登录
        if (PlayerPrefs.HasKey("isFirstLogin"))
        {
            //获取存储的数据
            //金币
            mCoin = PlayerPrefs.GetInt("coin");
            //历史成绩
            mScore = PlayerPrefs.GetInt("score");
            //历史成绩创建时间
            mTime = PlayerPrefs.GetString("time");
        }
        else
        {
            //设置首次登录Key
            PlayerPrefs.SetString("isFirstLogin", "Yes");
            //设置默认数据
            mCoin = 8888;
            mScore = 0;
            mTime = "";
            PlayerPrefs.SetInt("coin", mCoin);
            PlayerPrefs.SetInt("score", mScore);
            PlayerPrefs.SetString("time", mTime);
            OpenTipsWindow("首次登录赠送8888金币");
        }
    }
    /// <summary>
    /// 更新玩家记录数据
    /// </summary>
    /// <param name="score"></param>
    public void UpdateRecordData(int score)
    {
        mCurScore = score;
        if (mCurScore >= mScore)
        {
            isNewRecord = true;
            mScore = score;
            var dt = System.DateTime.Now;
            string str = dt.Year+"-"+dt.Month+"-"+dt.Day+" "+dt.ToLongTimeString();
            mTime = str;
            PlayerPrefs.SetInt("score", mScore);
            PlayerPrefs.SetString("time",mTime);
            lobbyWin.RefreshHistoryData();
        }
        else isNewRecord = false;
    }
    public void OpenTipsWindow(string tips)
    {
        tipsWin.AddTips(tips);
    }

    public void UpdateCoinData(int coin)
    {
        mCoin = coin;
        PlayerPrefs.SetInt("coin",mCoin);
        lobbyWin.RefreshCoinData();
    }

    #region Audio相关
    public void PlayLobbyBG()
    {
        if (bgAudio.clip == null || bgAudio.clip.name != lobbyBgClip.name)
        {
            bgAudio.clip = lobbyBgClip;
            bgAudio.loop = true;
            bgAudio.Play();
        }
    }
    public void PlayFightBG()
    {
        if (bgAudio.clip == null || bgAudio.clip.name != fightBgClip.name)
        {
            bgAudio.clip = fightBgClip;
            bgAudio.loop = true;
            bgAudio.Play();
        }
    }

    public void PlayUIClick()
    {
        uiClickAudio.Play();
    }

    public void PlayEffectAudio(string name)
    {
        for (int i = 0; i < cleanClips.Length; i++)
        {
            string clipName = cleanClips[i].name;
            if (clipName == name)
            {
                effectAudio.clip = cleanClips[i];
                effectAudio.Play();
                return;
            }
        }
        switch (name)
        {
            case "bomb":
                effectAudio.clip = bombClip;
                break;
            case "lighting":
                effectAudio.clip = lightingClip;
                break;
            case "wave":
                effectAudio.clip = waveClip;
                break;
        }
        effectAudio.Play();

    }
    #endregion

    public void SetFightRun()
    {
        fightWin.SetFightRun();
    }

    public void ExitFight()
    {
        fightWin.ClearFight();
        fightWin.gameObject.SetActive(false);
        PlayLobbyBG();
    }

    public void ReStartFight()
    {
        fightWin.ClearFight();
        OpenFightWindow();
    }
}
