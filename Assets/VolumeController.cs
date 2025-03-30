using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // UI滑动条组件
    [Tooltip("音量控制滑动条")]
    public Slider volumeSlider;

    private void Start()
    {
        if (volumeSlider != null)
        {
            // 设置滑动条的初始值为当前音量
            volumeSlider.value = BackgroundMusicManager.Instance.GetVolume();

            // 添加滑动条值改变事件监听
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    // 当滑动条值改变时调用
    private void OnVolumeChanged(float volume)
    {
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.SetVolume(volume);
        }
    }

    private void OnDestroy()
    {
        // 移除事件监听
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        }
    }
} 