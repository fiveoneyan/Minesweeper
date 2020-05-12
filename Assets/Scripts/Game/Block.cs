using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
using UnityEngine.UI;

public class Block : ComponentEx
{
    public Vector2 _blockIndex;
    public BlockType _blockType = BlockType.Normal;
    public bool _hasChecked;
    public Button btnBlock;
    public Image imgRedFlag;
    public Image imgQueFlag;
    //public Image imgUnclick;
    public Image imgClicked;
    public Image imgBoom;
    public Image imgRedBoom;
    public BlockType OriginalType;
    private Sprite Defallsprit;
    public bool isPress = false;
    public Image imgOne;
    public static Block instance;
    public ButtonExtension buttonExtension;
    

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        btnBlock = GetComponent<Button>();
        btnBlock.onClick.RemoveAllListeners();
        btnBlock.onClick.AddListener(Click);
        imgRedFlag = transform.Find("RedFlag").GetComponent<Image>();
        imgQueFlag = transform.Find("QueFlag").GetComponent<Image>();
       // imgUnclick = transform.Find("Unclick").GetComponent<Image>();
        imgClicked = transform.Find("Clicked").GetComponent<Image>();
        imgBoom = transform.Find("Boom").GetComponent<Image>();
        imgRedBoom = transform.Find("imgRedBoom").GetComponent<Image>();
       
       // btnBlock.onPress.AddListener(Press);

    }
    public override string[] ListNotificationInterests()
    {
        return new string[] {

            "MSG_GameOver",
            "MSG_GameVictory",
            "MSG_GameVicNOFlag"
            };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.name)
        {
            case "MSG_GameOver":
                imgRedFlag.gameObject.SetActive(false);
                imgQueFlag.gameObject.SetActive(false);
                if (_blockType == BlockType.RedFlag&& OriginalType == BlockType.Normal) {
                  imgRedBoom.gameObject.SetActive(true);
                  imgRedBoom.sprite = Lib.instance.FlagB[1];
                    transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];

                }
                if (_blockType == BlockType.Boom|| OriginalType == BlockType.Boom) {
                    imgBoom.gameObject.SetActive(true);
                    imgOne.gameObject.SetActive(false);
                    transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
                    btnBlock.interactable = false;
                }

              
                break;
            case "MSG_GameVictory":
                if (_blockType == BlockType.Normal)
                {
                    imgClicked.gameObject.SetActive(true);
                    transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];

                }
                break;
            case "MSG_GameVicNOFlag":
                if (_blockType == BlockType.Boom)
                {
                    imgRedFlag.gameObject.SetActive(true);


                }
                break;

        }
    }

  
    public void Init(Vector2 index)
    {

        _blockIndex = index;
        _blockType = BlockType.Normal;
        OriginalType = BlockType.Normal;
        imgOne.gameObject.SetActive(false);
        //myImage.sprite = Resources.Load("Image/未点击", typeof(Sprite)) as Sprite;
        //GetComponent<Image>().color = Color.white;
        //imgUnclick.gameObject.SetActive(true);
        imgRedFlag.gameObject.SetActive(false);
        imgQueFlag.gameObject.SetActive(false);
        imgBoom.gameObject.SetActive(false);
        imgClicked.gameObject.SetActive(false);
        imgRedBoom.gameObject.SetActive(false);
        transform.GetComponent<Image>().sprite = Lib.instance.FlagB[3];

        GetComponentInChildren<Text>().text = "";
        _hasChecked = false;
        btnBlock.interactable = true;
        
    }

    
    public int GetBoomCount(List<Block> blocks)
    {
        if (_blockType == BlockType.RedFlag) {

            return 0;
        }
        _hasChecked = true;
       // btnBlock.interactable = false;
        transform.GetComponent<Image>().sprite = Lib.instance.FlagB[6];
        int count = 0;
        for (float x = _blockIndex.x - 1; x <= _blockIndex.x + 1; x++)
        {
            for (float y = _blockIndex.y - 1; y <= _blockIndex.y + 1; y++)
            {
                foreach (var v in blocks)
                {
                    if (v._blockIndex == new Vector2(x, y) && (v._blockType == BlockType.Boom|| v.OriginalType == BlockType.Boom))
                    {
                        count++;
                    }
                }
            }
        }
        if (count == 0)
        {
            //myImage.sprite = Resources.Load("Image/已点击", typeof(Sprite)) as Sprite;
            imgClicked.gameObject.SetActive(true);
            _blockType = BlockType.UnFlag;
            //GetComponent<Image>().color = Color.green;
        }
        else 
        {
            int num = count-1;
            _blockType = BlockType.NumFlag;
            imgOne.gameObject.SetActive(true);
            imgOne.sprite = Lib.instance.dicnumber[num];
            imgQueFlag.gameObject.SetActive(false);
        }
        return count;
    }

    void Click()
    {
        facade.SendNotification("MSG_Click", this);
       

    }

   

}


public enum BlockType
{
    Normal,
    Boom,
    RedFlag,
    QueFlag,
    UnFlag,
    NumFlag
}