using UnityEngine;

/// <summary>
/// ��Ϸ���ڵ�
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
    /// ���ӵ�н������
    /// </summary>
    private int mCoin = 0;
    public int MCoin => mCoin;
    /// <summary>
    /// �����ʷ��߷�
    /// </summary>
    private int mScore = 0;
    public int MScore => mScore;
    /// <summary>
    /// ��߷ִ�����ʱ��
    /// </summary>
    private string mTime = "";
    public string MTime => mTime;
    /// <summary>
    /// ��ǰ�ֻ�÷���
    /// </summary>
    private int mCurScore = 0;
    public int MCurScore => mCurScore;
    /// <summary>
    /// �Ƿ�Ϊ�¼�¼
    /// </summary>
    private bool isNewRecord = false;
    public bool IsNewRecord => isNewRecord;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        OpenTipsWindow("��Ϸ�����ɹ�");
        InitGameData();
        OpenLobbyWindow();

    }
    /// <summary>
    /// �򿪴�������(��Ϸ�˵�����)
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
    /// ����������
    /// </summary>
    public void OpenFightWindow()
    {
        fightWin.gameObject.SetActive(true);
        fightWin.Init();
        PlayFightBG();
    }
    /// <summary>
    /// �򿪲˵�����
    /// </summary>
    /// <param name="tp"></param>
    public void OpenMenuWindow(OpType tp)
    {
        menuWin.gameObject.SetActive(true);
        menuWin.Init(tp);
    }
    /// <summary>
    /// ��ʼ����Ϸ����
    /// </summary>
    private void InitGameData()
    {
        //�Ƿ��״ε�¼
        if (PlayerPrefs.HasKey("isFirstLogin"))
        {
            //��ȡ�洢������
            //���
            mCoin = PlayerPrefs.GetInt("coin");
            //��ʷ�ɼ�
            mScore = PlayerPrefs.GetInt("score");
            //��ʷ�ɼ�����ʱ��
            mTime = PlayerPrefs.GetString("time");
        }
        else
        {
            //�����״ε�¼Key
            PlayerPrefs.SetString("isFirstLogin", "Yes");
            //����Ĭ������
            mCoin = 8888;
            mScore = 0;
            mTime = "";
            PlayerPrefs.SetInt("coin", mCoin);
            PlayerPrefs.SetInt("score", mScore);
            PlayerPrefs.SetString("time", mTime);
            OpenTipsWindow("�״ε�¼����8888���");
        }
    }
    /// <summary>
    /// ������Ҽ�¼����
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

    #region Audio���
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
