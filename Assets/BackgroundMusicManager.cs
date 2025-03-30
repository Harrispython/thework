using UnityEngine;
using System.Collections.Generic;

public class BackgroundMusicManager : MonoBehaviour
{
    // 单例实例
    public static BackgroundMusicManager Instance { get; private set; }

    // 背景音乐列表
    [Tooltip("要循环播放的背景音乐列表")]
    public List<AudioClip> backgroundMusics = new List<AudioClip>();

    // 音频源组件
    private AudioSource audioSource;

    // 当前播放的音乐索引
    private int currentMusicIndex = 0;

    // 是否正在播放
    private bool isPlaying = false;

    // 保存的音量值
    private const string VolumeKey = "MusicVolume";

    private void Awake()
    {
        // 单例模式实现
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 获取或添加AudioSource组件
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // 加载保存的音量设置
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            audioSource.volume = savedVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 如果有背景音乐，开始播放第一首
        if (backgroundMusics.Count > 0)
        {
            PlayCurrentMusic();
        }
    }

    private void Update()
    {
        // 检查当前音乐是否播放完毕
        if (isPlaying && !audioSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    // 播放当前音乐
    private void PlayCurrentMusic()
    {
        if (backgroundMusics.Count > 0 && currentMusicIndex < backgroundMusics.Count)
        {
            audioSource.clip = backgroundMusics[currentMusicIndex];
            audioSource.Play();
            isPlaying = true;
        }
    }

    // 播放下一首音乐
    private void PlayNextMusic()
    {
        // 更新音乐索引
        currentMusicIndex = (currentMusicIndex + 1) % backgroundMusics.Count;
        PlayCurrentMusic();
    }

    // 暂停播放
    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
        }
    }

    // 继续播放
    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
            isPlaying = true;
        }
    }

    // 停止播放
    public void StopMusic()
    {
        audioSource.Stop();
        isPlaying = false;
        currentMusicIndex = 0;
    }

    // 设置音量
    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioSource.volume = volume;
        // 保存音量设置
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    // 获取当前音量
    public float GetVolume()
    {
        return audioSource.volume;
    }
} 