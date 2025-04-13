using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// UI引用管理类，集中管理所有UI元素引用
    /// </summary>
    [System.Serializable]
    public class UIReferences
    {
        [Header("面板引用")]
        [Tooltip("Canvas/MainMenuPanel | 主菜单面板，包含开始挑战等主要功能按钮")]
        public GameObject mainMenuPanel;    // 主菜单面板
        
        [Tooltip("Canvas/GamePanel | 游戏主面板，包含演奏、准备、确认等游戏操作按钮")]
        public GameObject gamePanel;        // 游戏面板
        
        [Tooltip("Canvas/FreePlayPanel | 自由演奏面板，供玩家自由创作乐曲")]
        public GameObject freePlayPanel;    // 自由演奏面板
        
        [Tooltip("Canvas/BackgroundPanel | 背景介绍面板，展示编钟相关知识")]
        public GameObject backgroundPanel;  // 背景介绍面板
        
        [Tooltip("Canvas/GamePanel/ResultPanel | 结果面板，显示挑战成功或失败")]
        public GameObject resultPanel;      // 结果面板

        [Header("文本引用")]
        [Tooltip("Canvas/GamePanel/TopBar/LevelText | 关卡进度文本，如：第1/10关")]
        public TextMeshProUGUI levelText;           // 关卡文本
        
        [Tooltip("Canvas/GamePanel/InstructionArea/InstructionText | 游戏指引文本，提示当前应该进行的操作")]
        public TextMeshProUGUI instructionText;     // 指引文本
        
        [Tooltip("Canvas/GamePanel/PlayerArea/RecordText | 玩家演奏记录，显示已敲击的音符序列")]
        public TextMeshProUGUI playerRecordText;    // 玩家记录文本
        
        [Tooltip("Canvas/GamePanel/ResultPanel/ContentArea/ResultText | 结果详细说明文本")]
        public TextMeshProUGUI resultText;          // 结果文本
        
        [Tooltip("Canvas/GamePanel/ResultPanel/TitleArea/ResultTitleText | 结果标题，显示成功或失败")]
        public TextMeshProUGUI resultTitleText;     // 结果标题文本
        
        [Tooltip("Canvas/GamePanel/TopBar/PlayCountText | 剩余演奏次数显示")]
        public TextMeshProUGUI playCountText;       // 演奏次数文本
        
        [Tooltip("Canvas/BackgroundPanel/ScrollView/Viewport/Content/BackgroundText | 背景知识详细内容")]
        public TextMeshProUGUI backgroundText;      // 背景内容文本

        [Header("主菜单按钮")]
        [Tooltip("Canvas/MainMenuPanel/ButtonGroup/StartChallengeButton | 开始挑战按钮，点击进入挑战模式")]
        public Button startChallengeButton;  // 开始挑战按钮
        
        [Tooltip("Canvas/MainMenuPanel/ButtonGroup/FreePlayButton | 自由演奏按钮，点击进入自由创作模式")]
        public Button freePlayButton;        // 自由演奏按钮
        
        [Tooltip("Canvas/MainMenuPanel/ButtonGroup/BackgroundButton | 背景介绍按钮，点击查看相关知识")]
        public Button backgroundButton;      // 背景介绍按钮
        
        [Tooltip("Canvas/MainMenuPanel/ButtonGroup/ExitButton | 退出游戏按钮")]
        public Button exitGameButton;        // 退出游戏按钮

        [Header("返回按钮")]
        [Tooltip("Canvas/GamePanel/TopBar/BackButton | 游戏面板的返回按钮，返回主菜单")]
        public Button gameBackButton;      // 游戏面板返回按钮
        
        [Tooltip("Canvas/FreePlayPanel/TopBar/BackButton | 自由演奏面板的返回按钮，返回主菜单")]
        public Button freePlayBackButton;  // 自由演奏面板返回按钮
        
        [Tooltip("Canvas/BackgroundPanel/TopBar/BackButton | 背景介绍面板的返回按钮，返回主菜单")]
        public Button backgroundBackButton;  // 背景介绍面板返回按钮

        [Header("游戏按钮")]
        [Tooltip("Canvas/GamePanel/ButtonGroup/PlayButton | 演奏按钮，播放示范乐句")]
        public Button playButton;            // 演奏按钮
        
        [Tooltip("Canvas/GamePanel/ButtonGroup/PrepareButton | 准备按钮，开始记录玩家演奏")]
        public Button prepareButton;         // 准备按钮
        
        [Tooltip("Canvas/GamePanel/ButtonGroup/ConfirmButton | 确认按钮，提交玩家演奏进行判定")]
        public Button confirmButton;         // 确认按钮

        [Header("结果按钮")]
        [Tooltip("Canvas/GamePanel/ResultPanel/ButtonGroup/RetryButton | 重试按钮，重新挑战当前关卡")]
        public Button retryButton;           // 再来一次按钮
        
        [Tooltip("Canvas/GamePanel/ResultPanel/ButtonGroup/NextLevelButton | 下一关按钮，进入下一个挑战")]
        public Button nextLevelButton;       // 下一关按钮
        
        [Tooltip("Canvas/GamePanel/ResultPanel/ButtonGroup/BackToMenuButton | 返回主菜单按钮，结束当前挑战")]
        public Button backToMenuButton;      // 返回主菜单按钮

        [Header("其他UI元素")]
        [Tooltip("Canvas/BackgroundPanel/ScrollView/Viewport/Content/BackgroundImage | 背景介绍配图")]
        public Image backgroundImage;        // 背景介绍图片
    }
} 