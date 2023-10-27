using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SuccessUI : MonoBehaviour
{
    private Button _btnRestart;
    private Button _btnQuit;
    private Text _timeText;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);
        _btnRestart = transform.Find("btn_restart").GetComponent<Button>();
        _btnRestart.onClick.AddListener(OnClickBtnRestart);
        _btnQuit = transform.Find("btn_quit").GetComponent<Button>();
        _btnQuit.onClick.AddListener(OnClickBtnQuit);
        _timeText = transform.Find("time").GetComponent<Text>();
    }

    private void OnClickBtnRestart()
    {
        LevelManager.Instance.LoadScene(0);
        gameObject.SetActive(false);
    }

    private void OnClickBtnQuit()
    {
        Application.Quit();
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
        if (state)
        {
            LevelManager.Instance.AudioManager.Stop(AudioType.BG);
            LevelManager.Instance.AudioManager.PlayAudio("success", AudioType.Operation);
            int seconds = (int)(Time.time - LevelManager.Instance.StartGameTime);
            int hours = seconds / 3600;
            seconds %= 3600;
            int minutes = seconds / 60;
            seconds = seconds % 60;
            _timeText.text = string.Format("Game Time:{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }
    }
}
