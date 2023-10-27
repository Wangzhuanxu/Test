using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

[DefaultExecutionOrder(-120)]
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private SuccessUI _successUI;
    private Dictionary<int, GameItem> _allGameItems;
    public List<GameObject> GameItemPfbs;
    public AudioManager AudioManager;
    [HideInInspector]
    public float StartGameTime;
    [HideInInspector]
    public Dictionary<int, GameItem> AllGameItems=>_allGameItems;

    public static LevelManager Instance;
    [HideInInspector]
    public GameRandom RD;
    private GameState _currentGameState = GameState.Loading;
    private int _currentSceneBuildIndex = 0;
    private void Awake()
    {
        _currentGameState = GameState.Loading;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        RD = new GameRandom();
        _allGameItems = new Dictionary<int, GameItem>();
        GenerateLevel();
        _currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        AudioManager.PlayAudio("bg_music", AudioType.BG, 0.8f, true);
        StartGameTime = Time.time;
        Instance = this;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode sceneMode)
    {
        _currentGameState = GameState.Play;
    }
    private void GenerateLevel()
    {
        int childCnt = transform.childCount;
        if(childCnt % 3 != 0)
        {
            Debug.LogError($"场景中的元素数量{childCnt}不能被3整除，有错误");
        }
        else
        {
            int groupCnt = childCnt / 3;
            List<int> childIndexList = new List<int>();
            List<Vector3> childPosList = new List<Vector3>();
            Transform[] childTransArray = new Transform[childCnt];
            for(int i = childCnt-1; i >= 0; --i)
            {
                var child = transform.GetChild(i);
                childIndexList.Add(i);
                childPosList.Add(child.transform.localPosition);
                GameObject.Destroy(child.gameObject);
            }
            for(int i = 0; i < groupCnt; ++i)
            {
                int itemPfbIndex = RD.Next(0, GameItemPfbs.Count);
                var itemPfb = GameItemPfbs[itemPfbIndex];
                for(int j = 0; j < 3; ++j)
                {
                    int tempIndex = RD.Next(0, childIndexList.Count);
                    int childIndex = childIndexList[tempIndex];
                    var itemChild = GameObject.Instantiate(itemPfb).transform;
                    itemChild.SetParent(transform, false);
                    itemChild.localPosition = childPosList[tempIndex];
                    itemChild.name = $"{itemChild.name}_{childIndex}";
                    var gameItem = itemChild.GetComponent<GameItem>();
                    gameItem.SetIndex(childIndex);
                    _allGameItems.Add(itemChild.gameObject.GetInstanceID(), gameItem);
                    childTransArray[childIndex] = itemChild;
                    childIndexList.RemoveAt(tempIndex);
                    childPosList.RemoveAt(tempIndex);
                }
            }
            for(int i = 0; i < childTransArray.Length; ++i)
            {
                var childTrans = childTransArray[i];
                childTrans.SetSiblingIndex(i);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"LevelManager:{_currentGameState}_{_allGameItems.Values.Count}");
        if(_currentGameState == GameState.Play)
        {
            var allItems = _allGameItems.Values.ToList();
            for(int i = 0; i < allItems.Count; ++i)
            {
                bool visiable = true;
                var item1 = allItems[i];
                if (item1.transform.gameObject == null)
                    continue;
                for(int j = 0; j < allItems.Count; ++j)
                {
                    var item2 = allItems[j];
                    if (item2 == null)
                        continue;
                    if (item1.Index < item2.Index)
                    {
                        visiable &= item1.CheckVisiable(item2);
                    }
                }
                item1.SetClickAble(visiable);
            }
           
        }
    }

    public void RemoveGameItem(int goId)
    {
        if (_allGameItems.ContainsKey(goId))
        {
            _allGameItems.Remove(goId);
        }

        //游戏完成
        if(_allGameItems.Count == 0)
        {
            _currentGameState = GameState.Finish;
            var scene = SceneManager.GetActiveScene();
            if(scene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(scene.buildIndex + 1,LoadSceneMode.Single);
            }
            else
            {
                Debug.Log("游戏通关");
                _successUI.SetActive(true);
            }
        }
    }

    public void LoadScene(int buildIndex)
    {
        _allGameItems.Clear();
        _currentGameState = GameState.Loading;
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public enum GameState
{
    Play,
    Finish,
    Loading,
}