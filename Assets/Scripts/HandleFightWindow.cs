using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleFightWindow : MonoBehaviour
{
    public GameObject cubeItem;
    public Transform cubeRootTrans;
    /// <summary>
    /// �������еĵ�λ������
    /// </summary>
    public float xSpace;
    /// <summary>
    /// �������еĵ�λ������
    /// </summary>
    public float ySpace;
    public float moveTime;
    public float waveSpace;

    public Text txtTimer;
    public Image bottomBarImg;
    /// <summary>
    /// ��Ϸ�Ծ�ʱ��
    /// </summary>
    public int fightTime;

    public int cubeScore;
    public Transform numRootTrans;
    public Animator numAni; 
    public GameObject cleanEffect;
    public Transform effectRootTrans;

    /// <summary>
    /// ���ܲ۵���ֵ
    /// </summary>
    public int skillPointOp;
    public Image topBarImg;
    public Image topBarPoint;

    public GameObject bombEffect;
    public GameObject lightingHEffect;
    public GameObject lightingVEffect;

    #region DataArea
    /// <summary>
    /// ���п������ķ������
    /// </summary>
    private List<CubeItem> destroyList = new List<CubeItem>();

    /// <summary>
    /// ����Ķ�ά����
    /// </summary>
    private CubeItem[,] itemArr = new CubeItem[6, 6];
    private System.Random rd = null;
    private GameRoot root;
    /// <summary>
    /// �����Ƿ���������
    /// </summary>
    private bool IsProcess = false;
    /// <summary>
    /// ��Ϸ�Ƿ���������
    /// </summary>
    private bool isRun = false;
    private float deltaCount = 0;
    /// <summary>
    /// ����ʱ��
    /// </summary>
    private int mCount;
    private int mScore;
    /// <summary>
    /// ������������
    /// </summary>
    public int mSkillPoint;
    #endregion

    public void Init()
    {
        rd = new System.Random();
        root = GameRoot.Instance;
        InitCubeData();
        mScore = 0;
        SetScoreNum(mScore,false);
        mCount = fightTime;
        SetTimerBarVal(mCount);
        mSkillPoint = 0;
        SetSkillBarVal(mSkillPoint);
        isRun = true;
    }
    /// <summary>
    /// ���ü���״̬�����ۻ�ֵ
    /// </summary>
    /// <param name="point"></param>
    private void SetSkillBarVal(int point)
    {
        float val = point * 1.0f / skillPointOp;
        topBarImg.fillAmount = val;
        float dis = val * 570 + 37;
        topBarPoint.rectTransform.localPosition = new Vector3(dis, -67, 0);
    }
    /// <summary>
    /// ��ʾ�����仯
    /// </summary>
    /// <param name="mScore"></param>
    /// <param name="isJump"></param>
    private void SetScoreNum(int mScore,bool isJump = true)
    {
        if (isJump)
        {
            numAni.Play("NumRootAni", 0, 0);
            RuntimeAnimatorController tor = numAni.runtimeAnimatorController;
            var clips = tor.animationClips;
            StartCoroutine(AniPlayDone(clips[0].length / 3, mScore));
        }
        else
        {
            SetPicNum(mScore);
        }
    }
    /// <summary>
    /// ����������ɺ����Ϊ
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    IEnumerator AniPlayDone(float sec,int num)
    {
        yield return new WaitForSeconds(sec);
        SetPicNum(num);
    }
    /// <summary>
    /// �������ּ��ض�Ӧ������ͼƬ
    /// </summary>
    /// <param name="num">���������</param>
    private void SetPicNum(int num)
    {
        //��ȡȫ���Ӷ����ͼƬ���
        Image[] images = new Image[5];
        for(int i = 0; i < numRootTrans.childCount; i++)
        {
            Transform trans = numRootTrans.GetChild(i);
            images[i] = trans.GetComponent<Image>();
        }
        //�����������תΪ�ַ�������
        string numStr = num.ToString();
        int len = numStr.Length;
        string[] numArr = new string[len];
        for(int i = 0; i < len; i++)
        {
            numArr[i] = numStr.Substring(i, 1);//���ݴ������ֵĳ��ȣ���ȡ�ַ���
        }
        //�����ַ������ֵ�����ض�Ӧ����ͼƬ������ʾ������(�༭��ʹ��layout����Զ��Ű�)
        for(int i = 0; i < images.Length; i++)
        {
            if (i < numArr.Length)
            {
                images[i].gameObject.SetActive(true);
                images[i].sprite = Resources.Load<Sprite>("ResImages/Fight/num_" + numArr[i]);
            }
            else
                images[i].gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        //ʱ������
        if (isRun)
        {
            deltaCount += Time.deltaTime;
            if (deltaCount >= 1)
            {
                deltaCount -= 1;
                mCount -= 1;
                if (mCount <= 0)
                {
                    isRun = false;
                    mCount = 0;
                    GameOver();
                }
                SetTimerBarVal(mCount);
            }
        }
    }
    /// <summary>
    /// ������Ϸʱ������ֵ
    /// </summary>
    /// <param name="time"></param>
    private void SetTimerBarVal(int time)
    {
        float val = time * 1.0f / fightTime;
        bottomBarImg.fillAmount = val;
        txtTimer.text = time + "s";
    }
    private void GameOver()
    {
        //�����������
        root.UpdateRecordData(mScore);
        //������Ϸ�����˵�
        root.OpenMenuWindow(OpType.End);
    }
    /// <summary>
    /// ����һ��6x6��С��������
    /// </summary>
    private void InitCubeData()
    {
        int[,] iconIndexArr = null;
        while (true)
        {
            iconIndexArr = GetRandomArr(6, 6);

            if (IsValidData(iconIndexArr))
                break;
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                //ʵ��������Ԥ���壬���÷����λ�����길����Ϊ����ű���ӵ���¼�

                GameObject cube = Instantiate(cubeItem);

                CubeItem item = cube.GetComponent<CubeItem>();
                item.xIndex = i;
                item.yIndex = j;
                item.SetIconData(iconIndexArr[i, j]);
                item.name = "item_" + i + "_" + j;

                RectTransform rectTrans = item.GetComponent<RectTransform>();
                rectTrans.SetParent(cubeRootTrans);
                rectTrans.localPosition = new Vector2(i * xSpace, j * ySpace);

                Button btn = item.GetComponent<Button>();
                btn.onClick.AddListener(()=> OnItemClicked(item));

                itemArr[i,j] = item;
            }
        }
    }
    /// <summary>
    /// �Ƿ�Ϊ��Ч����
    /// </summary>
    /// <param name="indexArr"></param>
    /// <returns></returns>
    private bool IsValidData(int[,] indexArr)
    {
        for(int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                int val = indexArr[x, y];

                int count = 0;
                //һ����������������Ƿ�������Լ���ͬ������

                //��
                if (y <= 4 && val == indexArr[x, y + 1])
                    count += 1;
                //��
                if (y >= 1 && val == indexArr[x, y - 1])
                    count += 1;
                //��
                if (x >= 1 && val == indexArr[x - 1, y])
                    count += 1;
                //��
                if (x <= 4 && val == indexArr[x + 1, y])
                    count += 1;
                //����һ���������������Ч����
                if (count >= 2) return true;
            }
        }
        return false;
    }
    /// <summary>
    /// �������ĵ���¼�
    /// </summary>
    /// <param name="clickItem"></param>
    public void OnItemClicked(CubeItem clickItem)
    {
        if (IsProcess || !isRun) return;

        IsProcess = true;
        //��⵱ǰ�����С���Ƿ��������
        bool canDestroy = FindDestroyItems(clickItem);

        if (canDestroy)
        {
            //ͳ��ÿһ�������ٵĸ���
            int[] createArr = new int[6];
            //���Խ�������
            for (int i = 0; i < destroyList.Count; i++)
            {
                CubeItem ci = destroyList[i];
                itemArr[ci.xIndex, ci.yIndex] = null;
                createArr[ci.xIndex] += 1;
                Destroy(ci.gameObject);
            }

            //���ֱ仯
            mScore += cubeScore * destroyList.Count;
            SetScoreNum(mScore);

            //С������Ǩ��
            MoveResetCubeItem();
            //���������µ�CubeItem
            CreateNewCubeItem(createArr);
            //���ܵ���Ч���
            if (mSkillPoint < skillPointOp)
            {
                mSkillPoint += 1;
                if (mSkillPoint >= skillPointOp)
                {
                    mSkillPoint = 0;
                    CreateRandomSkill();
                }
                SetSkillBarVal(mSkillPoint);
            }
            //������ݵ���Ч��
            int[,] iconIndexArr = new int[6, 6];
            foreach (var item in itemArr)
                iconIndexArr[item.xIndex, item.yIndex] = item.iconIndex;
            if (!IsValidData(iconIndexArr))
            {
                isRun = false;
                while (true)
                {
                    //���������������
                    iconIndexArr = GetRandomArr(6, 6);
                    if (IsValidData(iconIndexArr))
                        break;
                }
                //д������������ݲ������������
                StartCoroutine(PlayWaveAni(iconIndexArr));
            }
        }
        else
        {
            IsProcess = false;
            //���У����ټ��ܵ�
            mSkillPoint -= 2;
            if (mSkillPoint < 0)
                mSkillPoint = 0;
            SetSkillBarVal(mSkillPoint);
        }
    }
    /// <summary>
    /// ����������ܷ���
    /// </summary>
    private void CreateRandomSkill()
    {
        int xPos = rd.Next(0, 6);
        int yPos = rd.Next(0, 6);
        int rdSkillIndex = rd.Next(5, 7);
        itemArr[xPos,yPos].SetIconData(rdSkillIndex);
    }
    /// <summary>
    /// ���з����������ɵĶ���
    /// </summary>
    /// <param name="iconIndexArr">����ͼƬ����������</param>
    /// <returns></returns>
    private IEnumerator PlayWaveAni(int[,] iconIndexArr)
    {
        yield return new WaitForSeconds(moveTime+0.1f);
        root.PlayEffectAudio("wave");
        for (int x = 0; x < 6; x++)
        {
            yield return new WaitForSeconds(waveSpace);
            for (int y = 0; y < 6; y++)
            {
                CubeItem item = itemArr[x, y];
                Animator ani = item.GetComponent<Animator>();
                ani.Play("CubeItemAni");
                RuntimeAnimatorController tor = ani.runtimeAnimatorController;
                var Clips = tor.animationClips;
                if (x == 5 && y == 5)
                    StartCoroutine(DelayIconSet(item, iconIndexArr, Clips[0].length / 2,true));
                else 
                    StartCoroutine(DelayIconSet(item, iconIndexArr, Clips[0].length / 2, false));
            }
        }
    }
    /// <summary>
    /// �ӳ�ͼƬIcon�ļ���
    /// </summary>
    /// <param name="item"></param>
    /// <param name="iconIndexArr"></param>
    /// <param name="delay"></param>
    /// <param name="isLast">�Ƿ�Ϊ������ɵķ���</param>
    /// <returns></returns>
    private IEnumerator DelayIconSet(CubeItem item,int[,] iconIndexArr,float delay,bool isLast)
    {
        yield return new WaitForSeconds(delay);
        item.SetIconData(iconIndexArr[item.xIndex, item.yIndex]);
        if (isLast)
        {
            root.OpenTipsWindow("��������������");
            isRun = true;
        }
    }
    
    /// <summary>
    /// ���������µķ���
    /// </summary>
    /// <param name="createArr"></param>
    private void CreateNewCubeItem(int[] createArr)
    {
        for(int i = 0; i < createArr.Length; i++)
        {
            //��������ÿ����Ҫ�����ĸ���
            int count = createArr[i];
            //�ӵ�һ����ʼ����
            for (int j = 1; j <= count; j++)
            {
                GameObject cube = Instantiate(cubeItem);
                CubeItem item = cube.GetComponent<CubeItem>();
                RectTransform rectTrans = item.GetComponent<RectTransform>();
                item.xIndex = i;
                int yIndex = 5 - count + j; //�½����������λ�õ�Yֵ������ʣ�෽��ƫ�ƺ�Yֵ+1
                item.yIndex = yIndex;
                int iconIndex = rd.Next(0, 5);
                item.SetIconData(iconIndex);
                item.name = "item_" + i + "_" + yIndex;
                rectTrans.SetParent(cubeRootTrans);
                Vector3 from = new Vector3(i * xSpace, (5 + j) * ySpace, 0);
                Vector3 to = new Vector3(i * xSpace, yIndex * ySpace, 0);
                //rectTrans.localPosition = new Vector3(i * xSpace, yIndex * ySpace, 0);
                item.MovePosInTime(moveTime,from,to);

                //����¼�����
                Button btn = item.GetComponent<Button>();
                btn.onClick.AddListener(() =>OnItemClicked(item));

                //����itemArr����
                itemArr[i, yIndex] = item;
            }
        }
        StartCoroutine(CallAniDoneAction());
    }
    private IEnumerator CallAniDoneAction()
    {
        yield return new WaitForSeconds(moveTime);
        IsProcess = false;
    }
    
    /// <summary>
    /// �ƶ�����
    /// </summary>
    private void MoveResetCubeItem()
    {
        int[,] offsetArr = new int[6, 6];

        //ͳ�Ʒ���Ҫ����ƫ�Ƶ���
        for(int i = 0; i < destroyList.Count; i++)
        {
            CubeItem item = destroyList[i];
            for (int y = 0; y < 6; y++)
            {
                //�ڿ���������֮�ϵķ��鶼Ҫ����ƫ��
                if (y > item.yIndex)
                {
                    offsetArr[item.xIndex, y] += 1;
                }
            }
        }

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                CubeItem item = itemArr[x, y];

                if (item != null && offsetArr[x, y] != 0)
                {
                    //�仯λ������
                    RectTransform rectTrans = item.GetComponent<RectTransform>();
                    Vector3 pos = rectTrans.localPosition;
                    float posY = pos.y - offsetArr[x, y] * ySpace;
                    //rectTrans.localPosition = new Vector3(pos.x, offsetY, 0);
                    item.MovePosInTime(moveTime,pos, new Vector3(pos.x, posY, 0));

                    //��������
                    item.yIndex -= offsetArr[x, y];

                    //����itemArr�������
                    itemArr[x, y] = null;
                    itemArr[item.xIndex, item.yIndex] = item;  
                }
            }
        }
    }

    /// <summary>
    /// �������ڷ����ܷ�����
    /// </summary>
    /// <param name="rootItem">���ҵĸ��ڵ㷽��</param>
    /// <returns></returns>
    private bool FindDestroyItems( CubeItem rootItem)
    {
        destroyList.Clear();
        destroyList.Add(rootItem);

        //�Ƿ���������ItemCube
        if(rootItem.iconIndex==5||rootItem.iconIndex == 6)
        {
            SelectBySkill(rootItem);
            return true;
        }

        //���ÿ�ֲ��ҵĸ��ڵ�
        List<CubeItem> rootList = new List<CubeItem> ();
        rootList.Add(rootItem);

        while(rootList.Count > 0)
        {
            //���ҵ��Ŀ���������
            List<CubeItem> findList = new List<CubeItem>();
            for (int i = 0; i < rootList.Count; i++)
            {
                CubeItem item = rootList[i];

                //��
                if (item.yIndex <= 4)
                {
                    CubeItem findItem = itemArr[item.xIndex, item.yIndex + 1];
                    //�ж��Ƿ�Ϊͬһ���͵�CubeItem
                    if (findItem.IsSameType(item)&&!IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    }
                }
                //��
                if (item.yIndex >= 1)
                {
                    CubeItem findItem = itemArr[item.xIndex, item.yIndex - 1];
                    if (findItem.IsSameType(item) && !IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    }
                }
                //��
                if (item.xIndex >= 1)
                {
                    CubeItem findItem = itemArr[item.xIndex - 1, item.yIndex];
                    if (findItem.IsSameType(item) && !IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    }
                }
                //��
                if (item.xIndex <= 4)
                {
                    CubeItem findItem = itemArr[item.xIndex + 1, item.yIndex];
                    if (findItem.IsSameType(item) && !IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    } 
                }
            }
            //���²��Ҹ��ڵ�
            rootList = findList;
        }
        if (destroyList.Count >= 3)
        {
            //��Ч����
            for (int i = 0; i < destroyList.Count; i++)
            {
                //CubeItemλ����Claen������ͬ
                CubeItem ci = destroyList[i];
                Vector3 pos = ci.GetComponent<RectTransform>().localPosition;
                GameObject clean = Instantiate(cleanEffect);
                clean.transform.SetParent(effectRootTrans);
                clean.GetComponent<RectTransform>().localPosition = pos;

            }
            //��Ч����
            int index = rd.Next(1, 9);
            root.PlayEffectAudio("s_" + index);

            return true;
        }
            
        else return false;
    }
    /// <summary>
    /// �����Ƿ�Ϸ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsLegal(int x,int y)
    {
        if (x < 0 || x > 5 || y < 0 || y > 5)
            return false;
        return true;
    }

    /// <summary>
    /// ������ܷ���
    /// </summary>
    /// <param name="item"></param>
    private void SelectBySkill(CubeItem item)
    {
        int x = item.xIndex;
        int y = item.yIndex;
        switch (item.iconIndex)
        {
            case 5:
                //ը������
                //��
                if (IsLegal(x, y + 1))
                    destroyList.Add(itemArr[x, y + 1]);
                //��
                if (IsLegal(x, y -1))
                    destroyList.Add(itemArr[x, y - 1]);
                //��
                if (IsLegal(x-1, y))
                    destroyList.Add(itemArr[x - 1, y]);
                //��
                if (IsLegal(x+1, y))
                    destroyList.Add(itemArr[x + 1, y]);
                //����
                if (IsLegal(x-1, y + 1))
                    destroyList.Add(itemArr[x - 1, y + 1]);
                //����
                if (IsLegal(x+1, y + 1))
                    destroyList.Add(itemArr[x + 1, y + 1]);
                //����
                if (IsLegal(x-1, y - 1))
                    destroyList.Add(itemArr[x - 1, y - 1]);
                //����
                if (IsLegal(x+1, y - 1))
                    destroyList.Add(itemArr[x + 1, y - 1]);

                //effect ����
                for (int i = 0; i < destroyList.Count; i++)
                {
                    CubeItem ci = destroyList[i];
                    Vector3 pos = ci.GetComponent<RectTransform>().localPosition;

                    //�ڷ���λ�ô�����ը��Ч
                    GameObject bomb = Instantiate(bombEffect);
                    bomb.transform.SetParent(effectRootTrans);
                    bomb.GetComponent<RectTransform>().localPosition = pos;
                }
                //audio ����
                root.PlayEffectAudio("bomb");
                break;
            case 6:
                //��������
                for (int i = 0; i < 6; i++)
                {
                    //����
                    if(i!=y) destroyList.Add(itemArr[x, i]);

                    //����
                    if(i!=x) destroyList.Add(itemArr[i, y]);

                }
                //��������������Ч
                GameObject lightH = Instantiate(lightingHEffect);
                lightH.transform.SetParent(effectRootTrans);
                lightH.GetComponent<RectTransform>().localPosition = new Vector3(447,ySpace*y,0);

                //�������������Ч
                GameObject lightV = Instantiate(lightingVEffect);
                lightV.transform.SetParent(effectRootTrans);
                lightV.GetComponent<RectTransform>().localPosition = new Vector3(xSpace * x, 456, 0);
                root.PlayEffectAudio("lighting");
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// �Ƿ�Ϊ��ѡ�еķ���
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool IsSelected(CubeItem item)
    {
        for (int i = 0; i < destroyList.Count; i++)
        {
            if (item.Equals(destroyList[i]))
                return true;
        }
        return false;
    }

    /// <summary>
    /// ��������Ķ�ά���飬����Ԫ��ֵ��0~5֮��
    /// </summary>
    /// <param name="width">�������</param>
    /// <param name="height">�������</param>
    /// <returns></returns>
    private int[,] GetRandomArr(int width,int height)
    {
        int[,] indexArr = new int[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                indexArr[x,y] = rd.Next(0,5);
            }
        }
        return indexArr;

    }
    public void OnBtnMenuClicked()
    {
        root.PlayUIClick();
        isRun = false;
        root.OpenMenuWindow(OpType.Pause);
    }
    public void SetFightRun()
    {
        isRun = true;
    }
    public void ClearFight()
    {
        rd = null;
        root = null;
        IsProcess = false;
        isRun = false;
        deltaCount = 0;
        mCount = 0;
        mScore = 0;
        mSkillPoint = 0;
        destroyList.Clear();

        for (int i = 0; i < cubeRootTrans.childCount; i++)
            Destroy(cubeRootTrans.GetChild(i).gameObject);
    }
}
