using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameItem : MonoBehaviour
{
    private Button _itemBtn;
    private Image _imgMask;
    private Vector2 _leftBottom;
    private Vector2 _rightUp;
    private int _index;

    public GameItemType ItemType;
    public int Index => _index;
    [HideInInspector]
    public Vector2 LeftBottom => _leftBottom;
    [HideInInspector]
    public Vector2 RightUp => _rightUp;
    private Vector3 _originalPos;
    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.localPosition;
        _itemBtn = transform.GetComponent<Button>();
        _itemBtn.onClick.AddListener(OnClickItemBtn);
        _imgMask = transform.Find("mask").GetComponent<Image>();
        var rectTrans = transform.GetComponent<RectTransform>();
        var halfWidth = rectTrans.sizeDelta.x / 2;
        var halfHeight = rectTrans.sizeDelta.y / 2;
        _leftBottom = new Vector2(transform.localPosition.x - halfWidth, transform.localPosition.y - halfHeight);
        _rightUp = new Vector2(transform.localPosition.x + halfWidth, transform.localPosition.y + halfHeight);
        //Debug.Log($"GameItem:{halfWidth}_{halfHeight}_{_leftBottom}_{_rightUp}");
        SetClickAble(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickItemBtn()
    {
        LevelManager.Instance.AudioManager.PlayAudio("click_item", AudioType.Operation);
        SlotManager.Instance.AddToSlot(this);
        _itemBtn.enabled = false;
        LevelManager.Instance.RemoveGameItem(transform.gameObject.GetInstanceID());
    }

    public void SetClickAble(bool canClick)
    {
        if (_imgMask == null || _itemBtn == null)
            return;
        _imgMask.gameObject.SetActive(!canClick);
        _itemBtn.enabled = canClick;
    }

    public bool CheckVisiable(GameItem otherItem)
    {
        bool ret = true;
        if(_index < otherItem.Index)
        {
            float distanceX = Mathf.Abs(transform.localPosition.x - otherItem.transform.localPosition.x);
            float distanceY = Mathf.Abs(transform.localPosition.y - otherItem.transform.localPosition.y);
            float halfSumx = (_rightUp.x - _leftBottom.x + otherItem.RightUp.x - otherItem.LeftBottom.x) / 2;
            float halfSumY = (_rightUp.y - _leftBottom.y + otherItem.RightUp.y - otherItem.LeftBottom.y) / 2;
            //Debug.Log($"CheckVisiable:{gameObject.name}_{otherItem.gameObject.name}_{distanceX}_{distanceY}_{halfSumx}_{halfSumY}");
            if(distanceX < halfSumx && distanceY < halfSumY)
            {
                ret = false;
            }
        }
        return ret;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }


    public void SuccessMatch()
    {
        GameObject.Destroy(gameObject);
        LevelManager.Instance.AudioManager.PlayAudio("success_match", AudioType.Info);
    }
}


public enum GameItemType
{
    RedBird,
    WhiteBird,
    BlackBird,
    YellowBird,
    BlueBird,
    GreenBird,
    OrangeBird,
}