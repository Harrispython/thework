using System;
using UnityEngine;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 编钟类，负责单个编钟的交互
    /// </summary>
    public class Bell : MonoBehaviour
    {
        [Header("音频设置")]
        [Tooltip("编钟音效")]
        public AudioClip bellSound;
        
        [Header("编钟属性")]
        [Tooltip("编钟对应的音高编号（1-7）")]
        public int bellIndex;
        
        [Header("视觉反馈")]
        [Tooltip("编钟被敲击时的视觉反馈对象")]
        public GameObject hitEffect;
        [Tooltip("视觉反馈持续时间")]
        public float hitEffectDuration = 0.5f;

        // 添加声音开关控制
        [Header("交互控制")]
        [Tooltip("是否允许发出声音")]
        public bool allowSound = true;
        
        private AudioSource _audioSource;
        private Animator _animator;

        public event Action<int> OnBellHit; // 编钟敲击事件
        
        private void Awake()
        {
            // 获取或添加AudioSource组件
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // 获取动画控制器（如果有）
            _animator = GetComponent<Animator>();
            
            // 初始化视觉反馈对象
            if (hitEffect != null)
            {
                hitEffect.SetActive(false);
            }
        }
        
        private void OnMouseDown()
        {
            // 无论是否允许声音，始终触发敲击事件，但只在允许时播放声音
            PlayBell();
        }
        
        /// <summary>
        /// 播放编钟声音及动画
        /// </summary>
        public void PlayBell()
        {
            // 播放声音（只在允许时播放）
            if (bellSound != null && allowSound)
            {
                _audioSource.clip = bellSound;
                _audioSource.Play();
            }
            
            // 播放动画 - 无论是否有声音，都显示动画效果
            if (_animator != null)
            {
                _animator.SetTrigger("Hit");
            }
            
            // 显示视觉反馈 - 无论是否有声音，都显示视觉反馈
            ShowHitEffect();
            
            // 触发敲击事件 - 无论是否有声音，都触发事件以便记录玩家操作
            OnBellHit?.Invoke(bellIndex);
        }
        
        /// <summary>
        /// 显示敲击视觉效果
        /// </summary>
        private void ShowHitEffect()
        {
            if (hitEffect == null) return;
            
            hitEffect.SetActive(true);
            CancelInvoke(nameof(HideHitEffect));
            Invoke(nameof(HideHitEffect), hitEffectDuration);
        }
        
        /// <summary>
        /// 隐藏敲击视觉效果
        /// </summary>
        private void HideHitEffect()
        {
            if (hitEffect != null)
            {
                hitEffect.SetActive(false);
            }
        }

        /// <summary>
        /// 设置编钟声音开关
        /// </summary>
        /// <param name="enable">是否允许发出声音</param>
        public void SetSoundEnabled(bool enable)
        {
            allowSound = enable;
        }
    }
} 