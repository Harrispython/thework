using UnityEngine;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 编钟游戏启动器 - 显示主界面并提供四个按钮选项
    /// </summary>
    public class BellGameStarter : MonoBehaviour
    {
        [Tooltip("主界面面板")]
        public GameObject mainMenuPanel;
        
        [Tooltip("游戏面板")]
        public GameObject gamePanel;
        
        [Tooltip("是否自动启动游戏")]
        public bool autoStart = true;
        
        [Tooltip("自动启动延迟（秒）")]
        public float autoStartDelay = 0.5f;
        
        private void Start()
        {
            // 确保初始状态
            if (gamePanel != null)
            {
                gamePanel.SetActive(false);
            }
            
            // 如果设置为自动启动，则延迟启动游戏
            if (autoStart)
            {
                Invoke(nameof(ShowMainMenu), autoStartDelay);
            }
        }
        
        /// <summary>
        /// 隐藏所有面板
        /// </summary>
        private void HideAllPanels()
        {
            // 获取所有可能的面板并隐藏它们
            GameObject[] allPanels = GameObject.FindGameObjectsWithTag("UIPanel");
            foreach (var panel in allPanels)
            {
                panel.SetActive(false);
            }
            
            // 明确隐藏已知的面板
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (gamePanel != null) gamePanel.SetActive(false);
        }
        
        /// <summary>
        /// 显示主菜单
        /// </summary>
        public void ShowMainMenu()
        {
            // 隐藏所有面板
            HideAllPanels();
            
            // 显示主菜单面板
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// 启动游戏（直接跳过主菜单，进入挑战模式）
        /// </summary>
        public void StartGameDirectly()
        {
            // 隐藏所有面板
            HideAllPanels();
            
            // 显示游戏面板
            if (gamePanel != null)
            {
                gamePanel.SetActive(true);
            }
            
            // 获取游戏管理器并开始游戏
            BellGameManager gameManager = FindObjectOfType<BellGameManager>();
            if (gameManager != null)
            {
                // 确保游戏UI管理器也准备好
                BellGameUIManager uiManager = gameManager.GetComponent<BellGameUIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowGamePanel();
                }
            }
        }
        
        /// <summary>
        /// 公共方法，可以从其他脚本或按钮调用
        /// </summary>
        public void StartGamePublic()
        {
            ShowMainMenu();
        }
    }
} 