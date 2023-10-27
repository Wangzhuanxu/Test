using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RestartUI : MonoBehaviour
{
    private Button _btnRestart;
    private Button _btnQuit;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        _btnRestart = transform.Find("btn_restart").GetComponent<Button>();
        _btnRestart.onClick.AddListener(OnClickBtnRestart);
        _btnQuit = transform.Find("btn_quit").GetComponent<Button>();
        _btnQuit.onClick.AddListener(OnClickBtnQuit);
    }

    private void OnClickBtnRestart()
    {
        gameObject.SetActive(false);
        LevelManager.Instance.LoadScene(0);
    }

    private void OnClickBtnQuit()
    {
        Application.Quit();
    }
}
