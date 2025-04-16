using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 光标管理器，负责管理游戏中的光标状态和动画
    /// </summary>
    public class CursorManager : MonoBehaviour
    {
        [Header("光标设置")]
        [Tooltip("默认光标贴图")]
        public Texture2D defaultCursor;
        
        [Tooltip("点击时的打击光标贴图")]
        public Texture2D clickCursor;
        
        [Header("光标动画设置")]
        [Tooltip("打击动画持续时间")]
        public float clickAnimationDuration = 0.1f;
        
        [Tooltip("光标热点位置")]
        public Vector2 cursorHotspot = new Vector2(16, 16);  // 默认在中心点
        
        // 私有变量
        private bool _isAnimating = false;
        private Coroutine _animationCoroutine;
        private Texture2D _originalCursor;  // 存储原始系统光标

        private void Awake()
        {
            
            
           
        }
        
        private void OnEnable()
        {
            _isAnimating = false;
            // 存储原始光标
            _originalCursor = null;  // Unity没有直接获取当前光标的API
            
            // 初始化事件监听 - 只在完全退出游戏时恢复默认光标
            GameEvents.OnGameStart += SetGameCursor;
            GameEvents.OnGameEnd += RestoreOriginalCursor;
            
            // 添加返回主菜单的事件监听 - 返回主菜单时不改变光标
            GameEvents.OnReturnToMainMenu += HandleReturnToMainMenu;
            // 初始化光标
            SetGameCursor();
        }
        
        private void OnDisable()
        {
            // 恢复默认光标
            RestoreOriginalCursor();
            
            // 取消事件监听
            GameEvents.OnGameStart -= SetGameCursor;
            GameEvents.OnGameEnd -= RestoreOriginalCursor;
            GameEvents.OnReturnToMainMenu -= HandleReturnToMainMenu;
        }
        
        private void Update()
        {
            // 检测鼠标左键点击
            if (Input.GetMouseButtonDown(0) && !_isAnimating)
            {
                PlayClickAnimation();
            }
        }
        
        /// <summary>
        /// 设置游戏光标
        /// </summary>
        public void SetGameCursor()
        {
            if (defaultCursor != null)
            {
                Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
            }
        }
        
        /// <summary>
        /// 处理返回主菜单事件
        /// </summary>
        private void HandleReturnToMainMenu()
        {
            // 返回主菜单时保持游戏自定义光标
            SetGameCursor();
        }
        
        /// <summary>
        /// 恢复原始系统光标
        /// </summary>
        public void RestoreOriginalCursor()
        {
            // 停止所有可能正在进行的动画
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
            
            // 恢复系统默认光标
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        
        /// <summary>
        /// 播放点击动画
        /// </summary>
        private void PlayClickAnimation()
        {
            if (clickCursor != null)
            {
                // 如果有正在播放的动画，先停止
                if (_animationCoroutine != null)
                {
                    StopCoroutine(_animationCoroutine);
                }
                
                // 开始新的动画
                _animationCoroutine = StartCoroutine(ClickAnimationCoroutine());
            }
        }
        
        /// <summary>
        /// 点击动画协程
        /// </summary>
        private IEnumerator ClickAnimationCoroutine()
        {
            
            _isAnimating = true;
            
            // 显示点击光标
            Cursor.SetCursor(clickCursor, cursorHotspot, CursorMode.Auto);
            
            // 等待动画时间
            yield return new WaitForSeconds(clickAnimationDuration);
            
            // 恢复默认光标
            if (defaultCursor != null)
            {
                Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
            }
            
            _isAnimating = false;
            _animationCoroutine = null;
        }
    }
}