using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleFightWindow : MonoBehaviour
{
    public GameObject cubeItem;
    public Transform cubeRootTrans;
    /// <summary>
    /// 方块阵列的单位横坐标
    /// </summary>
    public float xSpace;
    /// <summary>
    /// 方块阵列的单位纵坐标
    /// </summary>
    public float ySpace;
    public float moveTime;
    public float waveSpace;

    public Text txtTimer;
    public Image bottomBarImg;
    /// <summary>
    /// 游戏对局时间
    /// </summary>
    public int fightTime;

    public int cubeScore;
    public Transform numRootTrans;
    public Animator numAni; 
    public GameObject cleanEffect;
    public Transform effectRootTrans;

    /// <summary>
    /// 技能槽的总值
    /// </summary>
    public int skillPointOp;
    public Image topBarImg;
    public Image topBarPoint;

    public GameObject bombEffect;
    public GameObject lightingHEffect;
    public GameObject lightingVEffect;

    #region DataArea
    /// <summary>
    /// 所有可消除的方块对象
    /// </summary>
    private List<CubeItem> destroyList = new List<CubeItem>();

    /// <summary>
    /// 方块的二维数组
    /// </summary>
    private CubeItem[,] itemArr = new CubeItem[6, 6];
    private System.Random rd = null;
    private GameRoot root;
    /// <summary>
    /// 方块是否正在消除
    /// </summary>
    private bool IsProcess = false;
    /// <summary>
    /// 游戏是否正在运行
    /// </summary>
    private bool isRun = false;
    private float deltaCount = 0;
    /// <summary>
    /// 游玩时间
    /// </summary>
    private int mCount;
    private int mScore;
    /// <summary>
    /// 技能增长点数
    /// </summary>
    public int mSkillPoint;
    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
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
    /// 设置技能状态条的累积值
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
    /// 显示分数变化
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
    /// 动画播放完成后的行为
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
    /// 根据数字加载对应的数字图片
    /// </summary>
    /// <param name="num">传入的数字</param>
    private void SetPicNum(int num)
    {
        //获取全部子对象的图片组件
        Image[] images = new Image[5];
        for(int i = 0; i < numRootTrans.childCount; i++)
        {
            Transform trans = numRootTrans.GetChild(i);
            images[i] = trans.GetComponent<Image>();
        }
        //将传入的数字转为字符串数组
        string numStr = num.ToString();
        int len = numStr.Length;
        string[] numArr = new string[len];
        for(int i = 0; i < len; i++)
        {
            numArr[i] = numStr.Substring(i, 1);//根据传入数字的长度，截取字符串
        }
        //根据字符数组的值，加载对应数字图片，并显示出来。(编辑器使用layout组件自动排版)
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
        //时间流逝
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
    /// 设置游戏时间条的值
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
        //保存分数数据
        root.UpdateRecordData(mScore);
        //弹出游戏结束菜单
        root.OpenMenuWindow(OpType.End);
    }
    /// <summary>
    /// 生成一个6x6的小方块阵列
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
                //实例化方块预制体，设置方块的位置坐标父对象，为方块脚本添加点击事件

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
    /// 是否为有效数组
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
                //一个方块的上下左右是否存在与自己相同的数字

                //上
                if (y <= 4 && val == indexArr[x, y + 1])
                    count += 1;
                //下
                if (y >= 1 && val == indexArr[x, y - 1])
                    count += 1;
                //左
                if (x >= 1 && val == indexArr[x - 1, y])
                    count += 1;
                //右
                if (x <= 4 && val == indexArr[x + 1, y])
                    count += 1;
                //存在一组可消除，就是有效数据
                if (count >= 2) return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 方块对象的点击事件
    /// </summary>
    /// <param name="clickItem"></param>
    public void OnItemClicked(CubeItem clickItem)
    {
        if (IsProcess || !isRun) return;

        IsProcess = true;
        //检测当前点击的小块是否可以消除
        bool canDestroy = FindDestroyItems(clickItem);

        if (canDestroy)
        {
            //统计每一竖排销毁的个数
            int[] createArr = new int[6];
            //可以进行消除
            for (int i = 0; i < destroyList.Count; i++)
            {
                CubeItem ci = destroyList[i];
                itemArr[ci.xIndex, ci.yIndex] = null;
                createArr[ci.xIndex] += 1;
                Destroy(ci.gameObject);
            }

            //积分变化
            mScore += cubeScore * destroyList.Count;
            SetScoreNum(mScore);

            //小块数据迁移
            MoveResetCubeItem();
            //顶部创建新的CubeItem
            CreateNewCubeItem(createArr);
            //技能点生效检测
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
            //检测数据的有效性
            int[,] iconIndexArr = new int[6, 6];
            foreach (var item in itemArr)
                iconIndexArr[item.xIndex, item.yIndex] = item.iconIndex;
            if (!IsValidData(iconIndexArr))
            {
                isRun = false;
                while (true)
                {
                    //重新生成随机数据
                    iconIndexArr = GetRandomArr(6, 6);
                    if (IsValidData(iconIndexArr))
                        break;
                }
                //写入重新随机数据并表现随机过程
                StartCoroutine(PlayWaveAni(iconIndexArr));
            }
        }
        else
        {
            IsProcess = false;
            //不行，减少技能点
            mSkillPoint -= 2;
            if (mSkillPoint < 0)
                mSkillPoint = 0;
            SetSkillBarVal(mSkillPoint);
        }
    }
    /// <summary>
    /// 随机创建技能方块
    /// </summary>
    private void CreateRandomSkill()
    {
        int xPos = rd.Next(0, 6);
        int yPos = rd.Next(0, 6);
        int rdSkillIndex = rd.Next(5, 7);
        itemArr[xPos,yPos].SetIconData(rdSkillIndex);
    }
    /// <summary>
    /// 所有方块重新生成的动画
    /// </summary>
    /// <param name="iconIndexArr">方块图片索引的数组</param>
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
    /// 延迟图片Icon的加载
    /// </summary>
    /// <param name="item"></param>
    /// <param name="iconIndexArr"></param>
    /// <param name="delay"></param>
    /// <param name="isLast">是否为最后生成的方块</param>
    /// <returns></returns>
    private IEnumerator DelayIconSet(CubeItem item,int[,] iconIndexArr,float delay,bool isLast)
    {
        yield return new WaitForSeconds(delay);
        item.SetIconData(iconIndexArr[item.xIndex, item.yIndex]);
        if (isLast)
        {
            root.OpenTipsWindow("重新随机方块完成");
            isRun = true;
        }
    }
    
    /// <summary>
    /// 顶部创建新的方块
    /// </summary>
    /// <param name="createArr"></param>
    private void CreateNewCubeItem(int[] createArr)
    {
        for(int i = 0; i < createArr.Length; i++)
        {
            //方块阵列每竖排要创建的个数
            int count = createArr[i];
            //从第一个开始创建
            for (int j = 1; j <= count; j++)
            {
                GameObject cube = Instantiate(cubeItem);
                CubeItem item = cube.GetComponent<CubeItem>();
                RectTransform rectTrans = item.GetComponent<RectTransform>();
                item.xIndex = i;
                int yIndex = 5 - count + j; //新建方块的最终位置的Y值，就是剩余方块偏移后Y值+1
                item.yIndex = yIndex;
                int iconIndex = rd.Next(0, 5);
                item.SetIconData(iconIndex);
                item.name = "item_" + i + "_" + yIndex;
                rectTrans.SetParent(cubeRootTrans);
                Vector3 from = new Vector3(i * xSpace, (5 + j) * ySpace, 0);
                Vector3 to = new Vector3(i * xSpace, yIndex * ySpace, 0);
                //rectTrans.localPosition = new Vector3(i * xSpace, yIndex * ySpace, 0);
                item.MovePosInTime(moveTime,from,to);

                //添加事件监听
                Button btn = item.GetComponent<Button>();
                btn.onClick.AddListener(() =>OnItemClicked(item));

                //更新itemArr数据
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
    /// 移动方块
    /// </summary>
    private void MoveResetCubeItem()
    {
        int[,] offsetArr = new int[6, 6];

        //统计方块要向下偏移的量
        for(int i = 0; i < destroyList.Count; i++)
        {
            CubeItem item = destroyList[i];
            for (int y = 0; y < 6; y++)
            {
                //在可消除方块之上的方块都要往下偏移
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
                    //变化位置数据
                    RectTransform rectTrans = item.GetComponent<RectTransform>();
                    Vector3 pos = rectTrans.localPosition;
                    float posY = pos.y - offsetArr[x, y] * ySpace;
                    //rectTrans.localPosition = new Vector3(pos.x, offsetY, 0);
                    item.MovePosInTime(moveTime,pos, new Vector3(pos.x, posY, 0));

                    //更新索引
                    item.yIndex -= offsetArr[x, y];

                    //更新itemArr里的数据
                    itemArr[x, y] = null;
                    itemArr[item.xIndex, item.yIndex] = item;  
                }
            }
        }
    }

    /// <summary>
    /// 查找相邻方块能否消除
    /// </summary>
    /// <param name="rootItem">查找的根节点方块</param>
    /// <returns></returns>
    private bool FindDestroyItems( CubeItem rootItem)
    {
        destroyList.Clear();
        destroyList.Add(rootItem);

        //是否点击到技能ItemCube
        if(rootItem.iconIndex==5||rootItem.iconIndex == 6)
        {
            SelectBySkill(rootItem);
            return true;
        }

        //存放每轮查找的根节点
        List<CubeItem> rootList = new List<CubeItem> ();
        rootList.Add(rootItem);

        while(rootList.Count > 0)
        {
            //查找到的可消除方块
            List<CubeItem> findList = new List<CubeItem>();
            for (int i = 0; i < rootList.Count; i++)
            {
                CubeItem item = rootList[i];

                //上
                if (item.yIndex <= 4)
                {
                    CubeItem findItem = itemArr[item.xIndex, item.yIndex + 1];
                    //判断是否为同一类型的CubeItem
                    if (findItem.IsSameType(item)&&!IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    }
                }
                //下
                if (item.yIndex >= 1)
                {
                    CubeItem findItem = itemArr[item.xIndex, item.yIndex - 1];
                    if (findItem.IsSameType(item) && !IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    }
                }
                //左
                if (item.xIndex >= 1)
                {
                    CubeItem findItem = itemArr[item.xIndex - 1, item.yIndex];
                    if (findItem.IsSameType(item) && !IsSelected(findItem))
                    {
                        destroyList.Add(findItem);
                        findList.Add(findItem);
                    }
                }
                //右
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
            //更新查找根节点
            rootList = findList;
        }
        if (destroyList.Count >= 3)
        {
            //特效播放
            for (int i = 0; i < destroyList.Count; i++)
            {
                //CubeItem位置与Claen物体相同
                CubeItem ci = destroyList[i];
                Vector3 pos = ci.GetComponent<RectTransform>().localPosition;
                GameObject clean = Instantiate(cleanEffect);
                clean.transform.SetParent(effectRootTrans);
                clean.GetComponent<RectTransform>().localPosition = pos;

            }
            //音效播放
            int index = rd.Next(1, 9);
            root.PlayEffectAudio("s_" + index);

            return true;
        }
            
        else return false;
    }
    /// <summary>
    /// 索引是否合法
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
    /// 点击技能方块
    /// </summary>
    /// <param name="item"></param>
    private void SelectBySkill(CubeItem item)
    {
        int x = item.xIndex;
        int y = item.yIndex;
        switch (item.iconIndex)
        {
            case 5:
                //炸弹消除
                //上
                if (IsLegal(x, y + 1))
                    destroyList.Add(itemArr[x, y + 1]);
                //下
                if (IsLegal(x, y -1))
                    destroyList.Add(itemArr[x, y - 1]);
                //左
                if (IsLegal(x-1, y))
                    destroyList.Add(itemArr[x - 1, y]);
                //右
                if (IsLegal(x+1, y))
                    destroyList.Add(itemArr[x + 1, y]);
                //左上
                if (IsLegal(x-1, y + 1))
                    destroyList.Add(itemArr[x - 1, y + 1]);
                //右上
                if (IsLegal(x+1, y + 1))
                    destroyList.Add(itemArr[x + 1, y + 1]);
                //左下
                if (IsLegal(x-1, y - 1))
                    destroyList.Add(itemArr[x - 1, y - 1]);
                //右下
                if (IsLegal(x+1, y - 1))
                    destroyList.Add(itemArr[x + 1, y - 1]);

                //effect 播放
                for (int i = 0; i < destroyList.Count; i++)
                {
                    CubeItem ci = destroyList[i];
                    Vector3 pos = ci.GetComponent<RectTransform>().localPosition;

                    //在方块位置创建爆炸特效
                    GameObject bomb = Instantiate(bombEffect);
                    bomb.transform.SetParent(effectRootTrans);
                    bomb.GetComponent<RectTransform>().localPosition = pos;
                }
                //audio 播放
                root.PlayEffectAudio("bomb");
                break;
            case 6:
                //闪电消除
                for (int i = 0; i < 6; i++)
                {
                    //竖排
                    if(i!=y) destroyList.Add(itemArr[x, i]);

                    //横排
                    if(i!=x) destroyList.Add(itemArr[i, y]);

                }
                //创建竖排消除特效
                GameObject lightH = Instantiate(lightingHEffect);
                lightH.transform.SetParent(effectRootTrans);
                lightH.GetComponent<RectTransform>().localPosition = new Vector3(447,ySpace*y,0);

                //创建横版消除特效
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
    /// 是否为已选中的方块
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
    /// 生成随机的二维数组，数组元素值在0~5之间
    /// </summary>
    /// <param name="width">数组的列</param>
    /// <param name="height">数组的行</param>
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
    /// <summary>
    /// 设置消除正在运行
    /// </summary>
    public void SetFightRun()
    {
        isRun = true;
    }
    
    /// <summary>
    /// 清除消除数据
    /// </summary>
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
