using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SlotManager : MonoBehaviour
{
    [HideInInspector]
    public static SlotManager Instance;
    [SerializeField]
    private RestartUI _restartUI;
    private List<Vector3> _slotPosList;
    private List<GameItem> _slotGameItems;
    private int _slotCapacity;
    // Start is called before the first frame update
    void Start()
    {
        _slotCapacity = 7;
        _slotPosList = new List<Vector3>();
        _slotPosList.Add(new Vector3(0, 0,0));
        _slotPosList.Add(new Vector3(105, 0,0));
        _slotPosList.Add(new Vector3(210, 0,0));
        _slotPosList.Add(new Vector3(315, 0,0));
        _slotPosList.Add(new Vector3(420, 0,0));
        _slotPosList.Add(new Vector3(525, 0,0));
        _slotPosList.Add(new Vector3(630, 0, 0));
        _slotGameItems = new List<GameItem>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToSlot(GameItem gameItem)
    {
        if(_slotGameItems.Count < _slotCapacity)
        {
            InsertIntoRightSlotIndex(gameItem);
            RefreshSlots();        
        }
        if(_slotGameItems.Count >= _slotCapacity)
        {
            Debug.Log("超过槽位限制了");
            _restartUI.gameObject.SetActive(true);
            LevelManager.Instance.AudioManager.Stop(AudioType.BG);
            LevelManager.Instance.AudioManager.PlayAudio("failure", AudioType.Operation);
        }
    }


    private void InsertIntoRightSlotIndex(GameItem gameItem)
    {
        int index = 0;
        bool haveSameType = false;
        for (; index < _slotGameItems.Count; ++index)
        {
            var slotItem = _slotGameItems[index];
            if (haveSameType)
            {
                if(slotItem.ItemType != gameItem.ItemType)
                {
                    break;
                }
            }
            else
            {
                if (_slotGameItems[index].ItemType == gameItem.ItemType)
                {
                    haveSameType = true;
                }
            }
        }
        _slotGameItems.Insert(index, gameItem);
        gameItem.transform.SetParent(transform,false);
    }

    private void RefreshSlots()
    {
        for(int i = 0; i < _slotGameItems.Count; ++i)
        {
            var slotGameItem = _slotGameItems[i];
            slotGameItem.transform.localPosition = _slotPosList[i];
        }

        int lastIndex = -1;
        for(int i = _slotGameItems.Count - 3; i >= 0; --i)
        {
            if (_slotGameItems[i].ItemType == _slotGameItems[i+1].ItemType && _slotGameItems[i].ItemType == _slotGameItems[i + 2].ItemType)
            {
                lastIndex = i + 2;
                break;
            }
        }

        for (int i = lastIndex; i >= lastIndex - 2 && i >=0; i--) {
            _slotGameItems[i].SuccessMatch();
            _slotGameItems.RemoveAt(i);
        }

        for (int i = 0; i < _slotGameItems.Count; ++i)
        {
            var slotGameItem = _slotGameItems[i];
            slotGameItem.transform.localPosition = _slotPosList[i];
        }
    }



}
