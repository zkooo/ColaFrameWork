using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SoundEventHandler(SoundHandler sender, bool isDestroy);

public class SoundHandler : MonoBehaviour
{
    /// <summary>
    /// 声音播放结束时的回调
    /// </summary>
    private SoundHandler onPlayEnd;
    /// <summary>
    /// 声音播放依赖组件
    /// </summary>
    private AudioSource audioSource;
    /// <summary>
    /// 声音是否处于播放状态
    /// </summary>
    public bool IsPlaying { get; set; }
    /// <summary>
    /// 音频的长度
    /// </summary>
    private float duration;
    /// <summary>
    /// 淡入时间
    /// </summary>
    private float fadeInTime;
    /// <summary>
    /// 淡出时间
    /// </summary>
    private float fadeOutTime;
    /// <summary>
    /// 是否循环播放
    /// </summary>
    private bool loop;
    /// <summary>
    /// 音量
    /// </summary>
    private float volume;
    /// <summary>
    /// 音乐的类型
    /// </summary>
    private string type;
    /// <summary>
    /// 声音的播放速度
    /// </summary>
    private float volumeSpeed;
    /// <summary>
    /// 最小距离
    /// </summary>
    private float minDistance;
    /// <summary>
    /// 最大距离
    /// </summary>
    private float maxDistance;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
