using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using komal;
using komal.sdk;

public class UIGame : UIBase
{

    public Text txtTime;
    public Text txtBoom;

    public Button btnSetting;
    public Button btnRedflag;
    public Button btnSmile;
    public Button btnDead;
    public Sprite[] Express;
    public bool isGameOver = false;
    public AudioClip AudClick;
    public RectTransform rectChunk;
    // public bool isPress=false;

    public int time;
    public int width;
    public int height;
    public int boomCount;
    public int boomLeft;
    public difficulty diff = difficulty.easy;
   
    public bool isRedFlag = false;
    public bool isGameStart;
    public int Second;
    public int RightCount;
    public int FlagCount;
    public ButtonExtension buttonExtension;

    public bool isFirstBoom;
    public bool isFirstAudio=true;
    public bool isFirstClick=true;
    public bool isFirstAd = true;

    private List<Block> blocks = new List<Block>();
    private List<Block> inCheckBlocks = new List<Block>();
    private List<Block> inBlocks = new List<Block>();

    public GameObject ChunkScript;
    #region LifeTime

    public Image Chunk;
    public override void OnEnter()
    {
        base.OnEnter();
       // Difficulty01();
        
        btnSetting.onClick.AddListener(OnBtnSettingClick);
        btnRedflag.onClick.AddListener(OnBtnRedflagClick);

        StartGame();

        txtBoom = transform.Find("up/boom/txtBoom").GetComponent<Text>();
        boomLeft = boomCount;
        txtBoom.text = boomLeft.ToString();
        btnSmile = transform.Find("up/btnSmile").GetComponent<Button>();
        btnDead = transform.Find("up/btnDead").GetComponent<Button>();
        btnDead.onClick.AddListener(OnBtnbtnDeadClick);
        btnSmile.onClick.AddListener(OnBtnbtnSmileClick);

        btnSmile.gameObject.SetActive(true);

       
    //    Block.instance.btnBlock.onPress.AddListener(Press);

    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnExit()
    {
        base.OnExit();

        ReleaseBlock();
        btnSetting.onClick.RemoveListener(OnBtnSettingClick);
        btnRedflag.onClick.RemoveListener(OnBtnRedflagClick);
        btnDead.onClick.RemoveListener(OnBtnbtnDeadClick);
        btnSmile.onClick.RemoveListener(OnBtnbtnSmileClick);
    }
    #endregion

    void Init(params object[] param)
    {
        txtBoom.text = param[0].ToString();

    }

    public override string[] ListNotificationInterests()
    {
        return new string[] {
                "MSG_Replay",
                "MSG_Click",
                "MSG_Flag",
                "MSG_Press",
                "MSG_Up"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case "MSG_Replay":
                StartGame();
                btnSmile.gameObject.SetActive(true);
                break;

            case "MSG_Click":
                isFirstAudio = true;
                inCheckBlocks = new List<Block>();
                Check((Block)notification.body);
               
                if (CheckIsWin())
                {
                    StopCoroutine("Timer");
                    //                    PlayPasswAudio();
                    UIManager.Instance.OpenUI(EUITYPE.UITips, "成 功", Second, diff);
                    facade.SendNotification("MSG_GameVicNOFlag");
                    
                    btnSmile.GetComponent<Image>().sprite = Express[1];
                }
                break;

            case "MSG_Press":
                //按钮消失
                Vanish((Block)notification.body);
                break;
            case "MSG_Up":
                //按钮出现
                Recover((Block)notification.body);
                break;

        }
    }



    void OnBtnSettingClick()
    {
        PlayClickAudio();
        UIManager.Instance.OpenUI(EUITYPE.UISetting);
    }

    void OnBtnRedflagClick()
    {
        PlayClickAudio();
        isRedFlag = !isRedFlag;
        btnRedflag.GetComponent<Image>().DOFade(isRedFlag ? 0.5f : 1, 0);
     
    }


    void StartGame()
    {
        isFirstAudio = true;
        btnRedflag.GetComponent<Image>().DOFade(1, 0);
        btnSmile.GetComponent<Image>().sprite = Express[0];
        isFirstBoom = true;
        isFirstClick = true;
        FlagCount = 0;
        RightCount = 0;
        boomLeft = boomCount;
        txtBoom.text = boomLeft.ToString();
        int diff = PlayerPrefs.GetInt("diff", 1);
        switch (diff) {
            case 1:
                Difficulty01();
                boomLeft = boomCount;
                txtBoom.text = boomLeft.ToString();
                 // ChunkScript.GetComponent<Move>().enabled = false;

                
               
                    break;
            case 2:
                Difficulty02();
                boomLeft = boomCount;
                txtBoom.text = boomLeft.ToString();
               // ChunkScript.GetComponent<Move>().enabled = true;
                break;
            case 3:
                Difficulty03();
                boomLeft = boomCount;
                txtBoom.text = boomLeft.ToString();
              //  ChunkScript.GetComponent<Move>().enabled = true;

                break;

        }

        isGameOver = false;

        Second = 0;

        isGameStart = false;
       
         StopCoroutine("Timer");
         Second = 0;
         txtTime.text = Second.ToString();
        //StartCoroutine("Timer", 0);
        //StopCoroutine("BoomNumber");
        //StartCoroutine("BoomNumber",0);
        blocks = new List<Block>();
        isRedFlag = false;
        CreateBlock();
        facade.SendNotification("MSG_Aim");
        facade.SendNotification("MSG_SetDragRange");
        
    }

    /// <summary>
    /// 资源回收
    /// </summary>
    void ReleaseBlock()
    {
        int count = rectChunk.childCount;
        for (int i = 0; i < count; i++)
        {
            PoolManager.instance.Add2Pool(GType.Block, rectChunk.GetChild(0).gameObject);
        }
    }


    void CreateBlock()
    {
        //先回收资源
        ReleaseBlock();


        //获取Block的Size
        GameObject objBlock = PoolManager.instance.LoadObj(GType.Block, rectChunk);
        Vector2 size = objBlock.GetComponent<RectTransform>().sizeDelta;
        rectChunk.sizeDelta = new Vector2(size.x * width, size.y * height);
        PoolManager.instance.Add2Pool(GType.Block, objBlock);

        //创建
        for (float x = -(width / 2.0f); x < (width / 2.0f); x++)
        {
            for (float y = -(height / 2.0f); y < (height / 2.0f); y++)
            {
                GameObject obj = PoolManager.instance.LoadObj(GType.Block, rectChunk);
                obj.transform.localPosition = new Vector3(x * size.x + size.x / 2, y * size.y + size.y / 2, 0);
                obj.GetComponent<Block>().Init(new Vector2(x, y));
                blocks.Add(obj.GetComponent<Block>());
            }
        }

        //设置炸弹
        for (int i = 0; i < boomCount; i++)
        {
            int index = Random.Range(0, blocks.Count - 1);
            while (blocks[index]._blockType == BlockType.Boom)
            {
                index = Random.Range(0, blocks.Count - 1);
            }
            blocks[index]._blockType = BlockType.Boom;

            blocks[index].OriginalType = BlockType.Boom;
        }
    }


  

    void Check(Block block)
    {
       
        if (isFirstAudio) {
            AudioManager.Instance.PlaySFX(AudClick);
            isFirstAudio = false;
        }
        if (isFirstClick)
        {
            StopCoroutine("Timer");
            StartCoroutine("Timer", 0);
            isFirstClick = false;
        }

        // Debug.Log(block._blockType+"****"+block.OriginalType + "****" + block._blockIndex);
        if (isGameOver)
        { return; }
        if (!isGameStart) {

            isGameStart = true;
            switch (diff)
            {
                case difficulty.easy:
                    PlayerPrefs.SetInt("play_easy", PlayerPrefs.GetInt("play_easy", 0) + 1);
                    break;
                case difficulty.normal:
                    PlayerPrefs.SetInt("play_normal", PlayerPrefs.GetInt("play_normal", 0) + 1);
                    break;
                case difficulty.hard:
                    PlayerPrefs.SetInt("play_hard", PlayerPrefs.GetInt("play_hard", 0) + 1);
                    break;
            }
        }
       
        if (block._blockType == BlockType.NumFlag) {
            ChangeBlock(block);
            return;
        }

        if (isRedFlag)
        {
            if (block._blockType == BlockType.UnFlag|| block._blockType == BlockType.NumFlag) {
                block.imgRedFlag.gameObject.SetActive(false);
               
                //isFlag(block);

                
            }
            else if (block._blockType == BlockType.Normal || block._blockType == BlockType.Boom)
            {
                FlagCount++;

                block._blockType = BlockType.RedFlag;
                block.imgRedFlag.gameObject.SetActive(true);
                // block.imgUnclick.gameObject.SetActive(false);
                block.imgQueFlag.gameObject.SetActive(false);
                block.imgBoom.gameObject.SetActive(false);
                block.imgClicked.gameObject.SetActive(false);
                //block.GetComponentInChildren<Text>().text = "红";
                if (block.OriginalType == BlockType.Boom) {

                    RightCount++;

                    if (RightCount == boomCount && FlagCount == boomCount) {
                        StopCoroutine("Timer");
                        boomLeft = 0;
                        txtBoom.text = boomLeft.ToString();
                        UIManager.Instance.OpenUI(EUITYPE.UITips, "成 功", Second, diff);
                        btnSmile.GetComponent<Image>().sprite = Express[1];
                        facade.SendNotification("MSG_GameVictory");
                        block.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                        return;
                    }
                }

               
                boomLeft--;

                txtBoom.text = boomLeft.ToString();
            }
            else if (block._blockType == BlockType.RedFlag)
            {
                FlagCount--;

                block._blockType = BlockType.QueFlag;
                if (block.OriginalType == BlockType.Boom)
                {
                    RightCount--;

                }

                block.imgQueFlag.gameObject.SetActive(true);
                block.imgRedFlag.gameObject.SetActive(false);

                //block.GetComponentInChildren<Text>().text = "";
                boomLeft++;
                txtBoom.text = boomLeft.ToString();
                if (RightCount == boomCount && FlagCount == boomCount)
                {
                    StopCoroutine("Timer");
                    boomLeft = 0;
                    txtBoom.text = boomLeft.ToString();
                    UIManager.Instance.OpenUI(EUITYPE.UITips, "成 功", Second, diff);
                    btnSmile.GetComponent<Image>().sprite = Express[1];
                    facade.SendNotification("MSG_GameVictory");
                    block.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                    return;

                }
            }
            else if (block._blockType == BlockType.QueFlag)
            {
                block.imgQueFlag.gameObject.SetActive(false);
                block._blockType = block.OriginalType;
            }
            return;
        }


        ////////
        if (PlayerPrefs.GetInt("Shake", 1) == 1)
        {
            KomalUtil.Instance.TapEngineSelection();

        }
        if (block._blockType == BlockType.Boom)
        {
            if (isFirstBoom) {
                block._blockType = BlockType.Normal;
                block.OriginalType = BlockType.Normal;
                int random = Random.Range(0,blocks.Count-1);

                while (blocks[random]._blockType!= BlockType.Normal) {
                    random = Random.Range(0, blocks.Count - 1); 

                }
                blocks[random]._blockType = BlockType.Boom;
                blocks[random].OriginalType = BlockType.Boom;
                Check(block);
                return;
            }
            StopCoroutine("Timer");
            
            facade.SendNotification("MSG_GameOver");
            
            block.imgBoom.gameObject.SetActive(false);

            block.imgRedBoom.gameObject.SetActive(true);
            //block.GetComponentInChildren<Text>().text = "雷";
          
            btnDead.gameObject.SetActive(true);
            btnSmile.gameObject.SetActive(false);
            //UIManager.Instance.OpenUI(EUITYPE.UITips, "失 败", txtTime.text);


            Resmall();
            isGameOver = true;
            return;
        }

        ///////////////////////////////

       
       // isFlag(block);
     


        //////////////
        if (block._blockType == BlockType.RedFlag) {
            return;
        }


      Circulation(block);

       
        isFirstBoom = false;
    }

    void Resmall()
    {
       
        
        Chunk.transform.localScale = Vector3.one * 0.56f;
        Chunk.transform.position = new Vector3(0f, 0f, 0f);
        int diff = PlayerPrefs.GetInt("diff");
        if (diff == 2)
        {

            Chunk.transform.localScale = Vector3.one * 0.56f;
            Chunk.transform.position = new Vector3(0f, 0f, 0f);
            Debug.Log("Resmall............111");
            //facade.SendNotification("MSG_diff2");
        }
        else if (diff == 3)
        {

            Chunk.transform.localScale = Vector3.one * 0.3f;
            Chunk.transform.position = new Vector3(0f, 0f, 0f);
            Debug.Log("Resmall............222");
            //facade.SendNotification("MSG_diff3");
        }


    }


    void Circulation(Block  block) {
        inCheckBlocks.Add(block);

       
        if (block.GetBoomCount(blocks) == 0)
        {
            for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
            {
                for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
                {

                    foreach (var v in blocks)
                    {
                        if (v._blockIndex == new Vector2(x, y) && !v._hasChecked && !inCheckBlocks.Contains(v))
                        {
                            if (v.GetBoomCount(blocks) == 0)
                            {

                                Check(v);
                            }
                        }
                    }
                }
            }
        }

       
    }

    void Circulation02(Block block)
    {
        inCheckBlocks.Add(block);
      

        if (block.GetBoomCount(blocks) == 0)
        {
            for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
            {
                for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
                {

                    foreach (var v in blocks)
                    {
                        if (v._blockIndex == new Vector2(x, y) && !v._hasChecked && !inCheckBlocks.Contains(v))
                        {
                            if (v.GetBoomCount(blocks) == 0)
                            {
                                Circulation02(v);
                            }
                        }
                    }
                }
            }
        }

        
    }



    void ChangeBlock(Block block) {
           
            int CirleNum = 0;
            int WorseNum = 0;
            for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
            {
                for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
                {

                    foreach (var v in blocks)
                    {
                        if (v._blockIndex == new Vector2(x, y) && v._blockType == BlockType.RedFlag && v.OriginalType == BlockType.Boom)
                        {
                            CirleNum++;
                        }
                        else if (v._blockIndex == new Vector2(x, y) && v._blockType == BlockType.RedFlag && v.OriginalType == BlockType.Normal)
                        {
                           
                            WorseNum++;
                        }

                    }


                }
            }
            //标记正确时
            if (CirleNum == block.GetBoomCount(blocks) && WorseNum==0)
            {
            if (PlayerPrefs.GetInt("Shake", 1) == 1)
            {
                KomalUtil.Instance.TapEngineSelection();

            }
            for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
                {
                    for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
                    {

                        foreach (var v in blocks)
                        {
                            if (v._blockIndex == new Vector2(x, y) && v._blockType == BlockType.Normal)
                            {
                               
                                    
                                    Circulation02(v);

                                    v.imgClicked.gameObject.SetActive(true);
                               
                               
                            }

                        }


                    }
                }
            }
            else if(CirleNum != block.GetBoomCount(blocks) && WorseNum >0)
            {
            if (PlayerPrefs.GetInt("Shake", 1) == 1)
            {
                KomalUtil.Instance.TapEngineSelection();

            }
            for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
                {
                    for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
                    {

                        foreach (var v in blocks)
                        {
                            if (v._blockIndex == new Vector2(x, y) && v._blockType == BlockType.RedFlag && v.OriginalType == BlockType.Normal)
                            {
                                v.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                                facade.SendNotification("MSG_GameOver");
                                v.imgBoom.gameObject.SetActive(false);
                                v.imgRedBoom.gameObject.SetActive(true);
                                v.imgRedBoom.sprite = Lib.instance.FlagB[1];
                                btnDead.gameObject.SetActive(true);
                                btnSmile.gameObject.SetActive(false);
                            Resmall();
                        }
                            else if (v._blockIndex == new Vector2(x, y) && v.OriginalType == BlockType.Boom && v._blockType == BlockType.RedFlag)
                            {
                                v.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                                facade.SendNotification("MSG_GameOver");
                                v.imgBoom.gameObject.SetActive(false);
                                v.imgRedBoom.gameObject.SetActive(true);
                                v.imgRedBoom.sprite = Lib.instance.FlagB[2];
                                btnDead.gameObject.SetActive(true);
                                btnSmile.gameObject.SetActive(false);
                            Resmall();
                        }
                            else if (v._blockIndex == new Vector2(x, y) && v._blockType == BlockType.Boom || v._blockType == BlockType.QueFlag)
                            {
                                v.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                                facade.SendNotification("MSG_GameOver");
                                v.imgBoom.gameObject.SetActive(false);
                                v.imgRedBoom.gameObject.SetActive(true);
                                v.imgRedBoom.sprite = Lib.instance.FlagB[7];
                                btnDead.gameObject.SetActive(true);
                                btnSmile.gameObject.SetActive(false);
                            Resmall();
                        }

                        }


                    }
                }



            }

        }

   

    void Vanish(Block block)
    {
        for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
        {
            for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
            {

                foreach (var v in blocks)
                {
                    if (v._blockIndex == new Vector2(x, y) && (v._blockType == BlockType.Boom || v._blockType == BlockType.Normal))
                    {
                        v.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                    }
                }


            }
        }

    }

    void Recover(Block block)
    {
        for (float x = block._blockIndex.x - 1; x <= block._blockIndex.x + 1; x++)
        {
            for (float y = block._blockIndex.y - 1; y <= block._blockIndex.y + 1; y++)
            {

                foreach (var v in blocks)
                {
                    if (v._blockIndex == new Vector2(x, y) && (v._blockType == BlockType.Boom || v._blockType == BlockType.Normal))
                    {
                       
                        v.transform.GetComponent<Image>().sprite = Lib.instance.FlagB[3];
                    }
                }


            }
        }

    }

    bool CheckIsWin()
    {
        int needCheckCount = 0;
        foreach (var v in blocks)
        {
            if (!v._hasChecked)
                needCheckCount++;
        }
        return needCheckCount == boomCount;
    }

    void OnBtnbtnDeadClick() {
        if (diff == difficulty.easy)
        {
            if (PlayerPrefs.GetInt("play_esay", 0) % 2 == 0)
            {
               
                ShowAd();
            }
            else { StartGame(); }
        }
        else
        {
            
            ShowAd();
        }

        btnSmile.gameObject.SetActive(true);
        PlayClickAudio();


    }
    void OnBtnbtnSmileClick()
    {
        if (isGameStart) {

            if (diff == difficulty.easy)
            {
                if (PlayerPrefs.GetInt("play_esay", 0) % 2 == 0)
                {
                   
                    ShowAd();
                }
                else { StartGame(); }
            }
            else
            {
              
                ShowAd();
            }
        }

        PlayClickAudio();


    }

   


    IEnumerator Timer()
    {
        
        if (Second >=999) {
            Second=0;
        }
        Second++;
        txtTime.text = Second.ToString();
        yield return new WaitForSeconds(1);
        StartCoroutine("Timer", Second);
    }

    IEnumerator boommun(int boom)
    {
        txtBoom.text = ToString();
        yield return new WaitForSeconds(1);
        StartCoroutine("Timer", boom + 1);
    }

    public void Difficulty01()
    {
        diff = difficulty.easy;
        width = 9;
        height = 9;
        boomCount = 10;
        PlayerPrefs.SetInt("diff", 1);
    }
    public void Difficulty02()
    {
        diff = difficulty.normal;
        width = 16;
        height = 16;
        boomCount = 40;
        PlayerPrefs.SetInt("diff", 2);
    }
    public void Difficulty03()
    {
        diff = difficulty.hard;
        width = 30;
        height = 16;
        boomCount = 99;
        PlayerPrefs.SetInt("diff", 3);
    }

    
  
    void ShowAd()
    {

#if UNITY_EDITOR
        StartGame();
#else
        if (KomalUtil.Instance.IsNetworkReachability()&&Client.Instance.HaveAd)
        { 
            SDKManager.Instance.ShowInterstitial((result) =>
            {
                if (result == InterstitialResult.DISMISS)
                {
                   
                    StartGame();
                     Client.Instance.AdOver();
                    AudioManager.Instance.PauseAllListener(false);
                   
                }
                else if (result == InterstitialResult.DISPLAY)
                {

                    AudioManager.Instance.PauseAllListener(true);
                }

            });
        }
        else { StartGame(); };
#endif
    }
}



public enum difficulty{
    easy,
    normal,
    hard
}