using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 音效的管理类
/// </summary>
/// 确保先运行
[DefaultExecutionOrder(-130)]
public class AudioManager:MonoBehaviour
{
    private Dictionary<AudioType, AudioSource> _allAudios;
    private Dictionary<string, AudioClip> _allAudioClips;
    public Dictionary<AudioType, AudioSource> AllAudios => _allAudios;
    private void Start()
    {
        _allAudios = new Dictionary<AudioType, AudioSource>();
        _allAudioClips = new Dictionary<string, AudioClip>();
        foreach(var o in Enum.GetValues(typeof(AudioType)))
        {
            var audioCom = gameObject.AddComponent<AudioSource>();
            _allAudios.Add((AudioType)o, audioCom);
        }
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="audioName">会去Resources/Audio下去找，暂时不支持嵌套，只能寻找该文件下的声音文件</param>
    /// <param name="audioType"></param>
    /// <param name="loop"></param>
    public void PlayAudio(string audioName,AudioType audioType,float volume = 1.0f, bool loop = false)
    {
        AudioClip audioClip;
        if (_allAudioClips.ContainsKey(audioName))
        {
            audioClip = _allAudioClips[audioName];
        }
        else
        {
            audioClip = Resources.Load<AudioClip>($"Audio/{audioName}");
            _allAudioClips.Add(audioName, audioClip);
        }
        if(audioClip != null)
        {
            if(_allAudios.TryGetValue(audioType,out var audioSource))
            {
                audioSource.loop = loop;
                audioSource.clip = audioClip;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {
                Debug.LogError($"Can't play Audio,because there is not audioType {audioType}");

            }
        }
        else
        {
            Debug.LogError($"Can't play Audio,because there is not audioClip {audioName}");
        }
    }

    public void Stop(AudioType audioType)
    {
        if(_allAudios.TryGetValue(audioType,out var audioSource))
        {
            audioSource.Stop();
        }
    }

}

public enum AudioType
{
    BG,
    Operation,
    Info,
    Player,
}
