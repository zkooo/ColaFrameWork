using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SoundEventHandler(SoundHandler sender, bool isDestroy);

public class SoundHandler : MonoBehaviour
{
    /// <summary>
    /// 声音是否处于播放状态
    /// </summary>
    public bool IsPlaying { get; set; }

    /// <summary>
    ///设定的音量
    /// </summary>
    public float Volume
    {
        get { return volume; }
        set
        {
            if (value != volume)
            {
                volumeSpeed = Math.Abs(value - volume) / fadeInTime;
            }
            volume = value;
            fadeDetroyVolume = volume;
        }
    }

    /// <summary>
    /// 声音的渐入渐出速度
    /// </summary>
    public float VolumeSpeed
    {
        get { return volumeSpeed; }
        set { volumeSpeed = value; }
    }

    /// <summary>
    /// 最小距离
    /// </summary>
    public float MinDistance
    {
        get { return minDistance; }
        set { minDistance = value; }
    }

    /// <summary>
    /// 最大距离
    /// </summary>
    public float MaxDistance
    {
        get { return maxDistance; }
        set { maxDistance = value; }
    }

    /// <summary>
    /// 当前的音量
    /// </summary>
    public float CurVolume
    {
        get { return curVolume; }
        set { curVolume = value; }
    }

    /// <summary>
    /// 是否静音
    /// </summary>
    public bool Mute
    {
        get { return mute; }
        set
        {
            mute = value;
            UpdateAudioSourceVolume();
        }
    }

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
    /// 设定的音量
    /// </summary>
    private float volume;
    /// <summary>
    /// 音乐的类型
    /// </summary>
    private string type;
    /// <summary>
    /// 声音的渐入渐出速度
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

    private float fadeDetroyVolume;
    /// <summary>
    /// 当前的音量
    /// </summary>
    private float curVolume;
    /// <summary>
    /// 是否静音
    /// </summary>
    private bool mute;

    /// <summary>
    /// 声音播放结束时的回调
    /// </summary>
    public SoundEventHandler onPlayEnd;
    /// <summary>
    /// 声音播放依赖组件
    /// </summary>
    private AudioSource audioSource;

    public Action<bool> playCallback;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if (volumeSpeed > 0 && curVolume != fadeDetroyVolume)
        {
            float speed = fadeDetroyVolume - curVolume > 0.0f ? 1.0f : -1.0f;
            float dSpeed = speed * volumeSpeed * Time.deltaTime;
            if (Mathf.Abs(dSpeed) < Mathf.Abs(fadeDetroyVolume - curVolume))
            {
                curVolume += dSpeed;
            }
            else
            {
                curVolume = fadeDetroyVolume;
            }
        }

        if (IsPlaying == false && fadeDetroyVolume == 0.0f && curVolume == 0.0f)
        {
            if (IsInvoking())
            {
                CancelInvoke();
            }
            PlayEnd(false);
        }
    }

    /// <summary>
    /// 播放音频(含参数)
    /// </summary>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    /// <param name="isLoop"></param>
    /// <param name="spatialBlend"></param>
    public void Play(float fadeInTime, float fadeOutTime, bool isLoop, float spatialBlend)
    {
        IsPlaying = true;
        this.fadeInTime = fadeInTime;
        loop = isLoop;
        audioSource.loop = loop;
        audioSource.maxDistance = maxDistance;
        audioSource.minDistance = minDistance;
        audioSource.spatialBlend = spatialBlend;
        this.fadeOutTime = fadeOutTime;

        if (fadeInTime > 0)
        {
            curVolume = 0.0f;
            volumeSpeed = volume / this.fadeInTime;
            fadeDetroyVolume = volume;
        }
        else
        {
            curVolume = volume;
            volumeSpeed = 0.0f;
            fadeDetroyVolume = volume;
        }
    }

    /// <summary>
    /// 播放音频(无参数)
    /// </summary>
    public void DoPlay()
    {
        if (null != audioSource.clip)
        {
            audioSource.Play();
            if (IsInvoking())
            {
                CancelInvoke();
            }
            duration = audioSource.clip.length;
            if (!loop)
            {
                if (fadeOutTime > 0)
                {
                    StartCoroutine(PlayEndFadeOut());
                }
                else
                {
                    StartCoroutine(PlayEndNormal());
                }
            }
        }
    }

    IEnumerator PlayEndFadeOut()
    {
        yield return new WaitForSeconds(duration - fadeOutTime);
        Stop(fadeOutTime);
    }

    IEnumerator PlayEndNormal()
    {
        yield return new WaitForSeconds(duration);
        PlayEnd(IsPlaying);
    }

    /// <summary>
    /// 结束音频播放
    /// </summary>
    /// <param name="fadeTime"></param>
    public void Stop(float fadeTime)
    {
        if (!IsPlaying)
        {
            return;
        }

        IsPlaying = false;
        if (fadeTime <= 0.0f)
        {
            StopAllCoroutines();
            PlayEnd(false);
        }
        else
        {
            volumeSpeed = curVolume / fadeTime;
            fadeDetroyVolume = 0.0f;
        }
    }

    /// <summary>
    /// 结束播放
    /// </summary>
    protected void PlayEnd(bool isOver)
    {
        IsPlaying = false;
        if (null != onPlayEnd)
        {
            onPlayEnd(this, false);
        }
        audioSource.Stop();
        if (null != playCallback)
        {
            playCallback(isOver);
        }
    }

    /// <summary>
    /// 更新音频音量
    /// </summary>
    void UpdateAudioSourceVolume()
    {
        if (audioSource != null)
            audioSource.volume = mute ? 0 : curVolume;
    }
}
