using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // 添加DoTween引用

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 编钟游戏主界面管理器
    /// </summary>
    public class BellGameMainMenu : MonoBehaviour
    {
        [Header("UI引用")]
        public UIReferences uiRefs;

        [Header("背景介绍内容")]
        [TextArea(5, 10)]
        public string bellIntroduction = "编钟是中国古代的打击乐器，起源于西周，盛行于春秋战国时期。它由大小不同的青铜钟按照音调高低悬挂在一个巨大的钟架上，演奏时用木槌敲击发声。\n\n曾侯乙编钟是中国出土的规模最大、保存最好的一套青铜编钟，1978年在湖北随县（今随州市）出土，共65件，音域跨越5个八度，可演奏复杂的乐曲。\n\n编钟具有'一钟双音'的特性，每个钟的正面和侧面敲击会发出不同的音调，这在世界乐器史上极为罕见。";
        
        [TextArea(5, 10)]
        public string musicScaleIntroduction = "宫商角徵羽是中国古代五声音阶的名称，相当于现代简谱的1、2、3、5、6。\n\n宫(gōng)：相当于简谱的1音\n商(shāng)：相当于简谱的2音\n角(jué)：相当于简谱的3音\n徵(zhǐ)：相当于简谱的5音\n羽(yǔ)：相当于简谱的6音\n\n这种五声音阶是中国传统音乐的基础，形成于先秦时期，至今仍然广泛应用于中国民族音乐中。";
        
        [TextArea(5, 10)]
        public string physicsIntroduction = "编钟的制作体现了古代中国人对声学原理的深刻理解：\n\n1. 钟体形状：编钟呈扁平的梯形，而非圆形，这种特殊设计使得钟的正面和侧面敲击会产生不同的谐振频率，形成一钟双音。\n\n2. 厚度控制：编钟壁的厚度经过精确计算和控制，以产生特定的音高。\n\n3. 悬挂方式：编钟悬挂时采用特定角度，这影响了钟的振动方式和发声特性。\n\n4. 合金比例：青铜合金的成分比例直接影响音色，古人通过经验掌握了最佳配比。";

        [Header("音效管理")]
        [SerializeField] private AudioManager audioManager;
        
        [Header("光标管理")]
        [SerializeField] private CursorManager cursorManager;

        [Header("面板切换设置")]
        [SerializeField] private float panelFadeTime = 0.3f;
        [SerializeField] private float panelMoveDistance = 50f;

        private BellGameManager _gameManager;
        private BellGameUIManager _uiManager;
        private GameObject _currentPanel;
        private CanvasGroup _currentPanelGroup;

        private void Awake()
        {
            _gameManager = GetComponent<BellGameManager>();
            _uiManager = GetComponent<BellGameUIManager>();
            
            // 获取光标管理器
            if (cursorManager == null)
            {
                cursorManager = GetComponent<CursorManager>();
                if (cursorManager == null)
                {
                    cursorManager = gameObject.AddComponent<CursorManager>();
                }
            }
            
            InitializePanelCanvasGroups();
            CheckUIPanelTags();
            RegisterButtonEvents();
            SetInitialState();
        }

        private void Start()
        {
            // 初始化光标
            if (cursorManager != null)
            {
                cursorManager.SetGameCursor();
            }
            
            // 触发游戏开始事件
            GameEvents.TriggerGameStart();
        }

        private void InitializePanelCanvasGroups()
        {
            GameObject[] panels = {
                uiRefs.mainMenuPanel,
                uiRefs.gamePanel,
                uiRefs.freePlayPanel,
                uiRefs.backgroundPanel
            };

            foreach (GameObject panel in panels)
            {
                if (panel != null)
                {
                    // 确保每个面板都有CanvasGroup组件
                    if (!panel.TryGetComponent<CanvasGroup>(out var canvasGroup))
                    {
                        canvasGroup = panel.AddComponent<CanvasGroup>();
                    }
                    canvasGroup.alpha = 0f;
                    panel.SetActive(false);
                }
            }
        }

        private void CheckUIPanelTags()
        {
            GameObject[] panels = {
                uiRefs.mainMenuPanel,
                uiRefs.gamePanel,
                uiRefs.freePlayPanel,
                uiRefs.backgroundPanel
            };
            
            foreach (GameObject panel in panels)
            {
                if (panel != null && !panel.CompareTag("UIPanel"))
                {
                    Debug.LogWarning($"警告：面板 {panel.name} 没有设置'UIPanel'标签，可能无法被自动隐藏！");
                }
            }
        }

        private void OnEnable()
        {
            RegisterButtonEvents();
        }

        private void OnDisable()
        {
            UnregisterButtonEvents();
        }

        private void RegisterButtonEvents()
        {
            UnregisterButtonEvents(); // 确保先清除可能存在的旧监听器

            // 主菜单按钮
            RegisterButtonClick(uiRefs.startChallengeButton, StartChallenge);
            RegisterButtonClick(uiRefs.freePlayButton, StartFreePlay);
            RegisterButtonClick(uiRefs.backgroundButton, ShowBackground);
            RegisterButtonClick(uiRefs.exitGameButton, ExitGame);
            
            // 各面板返回按钮
            RegisterButtonClick(uiRefs.gameBackButton, ReturnToMainMenu);
            RegisterButtonClick(uiRefs.freePlayBackButton, ReturnToMainMenu);
            RegisterButtonClick(uiRefs.backgroundBackButton, ReturnToMainMenu);
        }

        private void UnregisterButtonEvents()
        {
            // 主菜单按钮
            UnregisterButtonClick(uiRefs.startChallengeButton, StartChallenge);
            UnregisterButtonClick(uiRefs.freePlayButton, StartFreePlay);
            UnregisterButtonClick(uiRefs.backgroundButton, ShowBackground);
            UnregisterButtonClick(uiRefs.exitGameButton, ExitGame);
            
            // 各面板返回按钮
            UnregisterButtonClick(uiRefs.gameBackButton, ReturnToMainMenu);
            UnregisterButtonClick(uiRefs.freePlayBackButton, ReturnToMainMenu);
            UnregisterButtonClick(uiRefs.backgroundBackButton, ReturnToMainMenu);
        }

        private void RegisterButtonClick(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button != null)
            {
                button.onClick.RemoveListener(action); // 确保不会重复添加
                button.onClick.AddListener(action);
            }
        }

        private void UnregisterButtonClick(Button button, UnityEngine.Events.UnityAction action)
        {
            if (button != null)
            {
                button.onClick.RemoveListener(action);
            }
        }

        private void SetInitialState()
        {
            ShowPanel(uiRefs.mainMenuPanel);
            
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = false;
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = true;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = false;
        }

        private void StartChallenge()
        {
            ShowPanel(uiRefs.gamePanel);
            
            if (_uiManager != null)
            {
                _uiManager.UpdateButtonLabels("演奏", "准备", "确认");
            }
            
            SetChallengeButtonStates();
            
            if (_gameManager != null)
            {
                _gameManager.InitializeChallenge();
            }
            
            if (_uiManager != null)
            {
                _uiManager.UpdatePlayerRecord("等待演奏示范...");
            }
        }

        private void SetChallengeButtonStates()
        {
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = true;
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = false;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = false;
        }

        private void StartFreePlay()
        {
            if (_gameManager != null)
            {
                _gameManager.EnterFreePlayMode();
            }
            else
            {
                ShowPanel(uiRefs.freePlayPanel);
                
                if (_uiManager != null)
                {
                    _uiManager.UpdatePlayerRecord("自由演奏模式：尽情享受编钟的魅力吧！");
                }
            }
        }

        private void ShowBackground()
        {
            if (_gameManager != null)
            {
                _gameManager.ShowBackgroundPanel();
            }
            else
            {
                ShowPanel(uiRefs.backgroundPanel);
                
                if (uiRefs.backgroundText != null)
                {
                    uiRefs.backgroundText.text = GetBackgroundIntroduction();
                }
            }
        }

        /// <summary>
        /// 获取背景介绍内容
        /// </summary>
        public string GetBackgroundIntroduction()
        {
            return $"<b>【编钟介绍】</b>\n{bellIntroduction}\n\n<b>【宫商角徵羽介绍】</b>\n{musicScaleIntroduction}\n\n<b>【古代物理知识介绍】</b>\n{physicsIntroduction}";
        }

        public void ReturnToMainMenu()
        {
            // 先取消事件订阅以避免循环调用
            bool isAlreadyReturning = false;
            
            try
            {
                if (isAlreadyReturning) return;
                isAlreadyReturning = true;
                
                // 重置游戏状态
                if (_gameManager != null)
                {
                    _gameManager.ResetGameState();
                }
                
                // 确保其他面板隐藏
                if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
                if (uiRefs.freePlayPanel != null) uiRefs.freePlayPanel.SetActive(false);
                if (uiRefs.backgroundPanel != null) uiRefs.backgroundPanel.SetActive(false);
                if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);

                // 显示主菜单面板
                if (uiRefs.mainMenuPanel != null)
                {
                    // 显示主菜单并初始化
                    uiRefs.mainMenuPanel.SetActive(true);
                    
                    // 确保CanvasGroup是可见的
                    CanvasGroup canvasGroup = uiRefs.mainMenuPanel.GetComponent<CanvasGroup>();
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = 1f;
                        canvasGroup.blocksRaycasts = true;
                        canvasGroup.interactable = true;
                    }
                    
                    // 初始化按钮
                    InitializeMainMenuPanel();
                }
                
                // 播放返回音效
                if (audioManager != null)
                {
                    audioManager.PlayPanelSwitchSound();
                }
                
                // 确保光标仍然是自定义光标
                if (cursorManager != null)
                {
                    cursorManager.SetGameCursor();
                }
                
                // 触发返回主菜单事件
                GameEvents.TriggerReturnToMainMenu();
            }
            finally
            {
                isAlreadyReturning = false;
            }
        }

        private void ExitGame()
        {
            // 恢复原始光标
            if (cursorManager != null)
            {
                cursorManager.RestoreOriginalCursor();
            }
            
            // 隐藏所有面板
            if (uiRefs.mainMenuPanel != null) uiRefs.mainMenuPanel.SetActive(false);
            if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
            if (uiRefs.freePlayPanel != null) uiRefs.freePlayPanel.SetActive(false);
            if (uiRefs.backgroundPanel != null) uiRefs.backgroundPanel.SetActive(false);
            if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);
            
            // 触发游戏结束事件
            GameEvents.TriggerGameEnd();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private IEnumerator SwitchPanelCoroutine(GameObject targetPanel)
        {
            // 播放面板切换音效
            if (audioManager != null)
            {
                audioManager.PlayPanelSwitchSound();
            }

            if (targetPanel == null) yield break;

            // 获取或添加CanvasGroup组件
            CanvasGroup targetPanelGroup = targetPanel.GetComponent<CanvasGroup>();
            if (targetPanelGroup == null)
            {
                targetPanelGroup = targetPanel.AddComponent<CanvasGroup>();
            }

            // 记录目标面板的原始位置
            RectTransform targetRect = targetPanel.GetComponent<RectTransform>();
            Vector2 originalTargetPos = targetRect.anchoredPosition;

            // 如果有当前面板，先淡出
            if (_currentPanel != null && _currentPanel != targetPanel)
            {
                // 记录当前面板的原始位置
                RectTransform currentRect = _currentPanel.GetComponent<RectTransform>();
                Vector2 originalCurrentPos = currentRect.anchoredPosition;
                float startAlpha = _currentPanelGroup.alpha;
                
                // 淡出当前面板
                float elapsedTime = 0f;
                while (elapsedTime < panelFadeTime)
                {
                    float t = elapsedTime / panelFadeTime;
                    _currentPanelGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
                    currentRect.anchoredPosition = Vector2.Lerp(originalCurrentPos, 
                        new Vector2(originalCurrentPos.x, originalCurrentPos.y + panelMoveDistance), t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _currentPanelGroup.alpha = 0f;
                // 确保当前面板回到原位，避免位置累积偏差
                currentRect.anchoredPosition = originalCurrentPos;
                _currentPanel.SetActive(false);
            }

            // 准备新面板
            targetPanel.SetActive(true);
            targetPanelGroup.alpha = 0f;
            // 设置动画起始位置
            targetRect.anchoredPosition = new Vector2(originalTargetPos.x, originalTargetPos.y - panelMoveDistance);

            // 淡入新面板
            float fadeInTime = 0f;
            while (fadeInTime < panelFadeTime)
            {
                float t = fadeInTime / panelFadeTime;
                targetPanelGroup.alpha = Mathf.Lerp(0f, 1f, t);
                targetRect.anchoredPosition = Vector2.Lerp(
                    new Vector2(originalTargetPos.x, originalTargetPos.y - panelMoveDistance),
                    originalTargetPos,
                    t
                );
                fadeInTime += Time.deltaTime;
                yield return null;
            }

            // 确保最终状态正确
            targetPanelGroup.alpha = 1f;
            targetRect.anchoredPosition = originalTargetPos;

            // 更新当前面板引用
            _currentPanel = targetPanel;
            _currentPanelGroup = targetPanelGroup;

            // 根据面板类型初始化
            if (targetPanel == uiRefs.mainMenuPanel) InitializeMainMenuPanel();
            else if (targetPanel == uiRefs.gamePanel) InitializeGamePanel();
            else if (targetPanel == uiRefs.freePlayPanel) InitializeFreePlayPanel();
            else if (targetPanel == uiRefs.backgroundPanel) InitializeBackgroundPanel();

            // 触发面板切换事件
            GameEvents.TriggerPanelSwitched(targetPanel.name);
        }

        private void ShowPanel(GameObject panelToShow)
        {
            if (panelToShow == null) return;
            
            // 触发事件通知其他组件
            GameEvents.TriggerHideAllPanels();
            
            // 启动面板切换协程
            StartCoroutine(SwitchPanelCoroutine(panelToShow));
        }

        /// <summary>
        /// 初始化主菜单面板，使按钮可见且可交互
        /// </summary>
        public void InitializeMainMenuPanel()
        {
            // 确保主菜单所有按钮都是可见和可交互的
            SetButtonActive(uiRefs.startChallengeButton, true);
            SetButtonActive(uiRefs.freePlayButton, true);
            SetButtonActive(uiRefs.backgroundButton, true);
            SetButtonActive(uiRefs.exitGameButton, true);
            
            // 确保主菜单面板可见
            if (uiRefs.mainMenuPanel != null)
            {
                CanvasGroup canvasGroup = uiRefs.mainMenuPanel.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.interactable = true;
                }
            }
            
            // 隐藏其他面板
            if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
            if (uiRefs.freePlayPanel != null) uiRefs.freePlayPanel.SetActive(false);
            if (uiRefs.backgroundPanel != null) uiRefs.backgroundPanel.SetActive(false);
            if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);
        }

        // 辅助方法：设置按钮状态
        private void SetButtonActive(Button button, bool active)
        {
            if (button != null)
            {
                button.gameObject.SetActive(active);
                button.interactable = active;
            }
        }

        private void InitializeGamePanel()
        {
            // 设置游戏面板的初始状态
            if (uiRefs.playButton != null)
            {
                uiRefs.playButton.gameObject.SetActive(true);
                uiRefs.playButton.interactable = true;
            }
            if (uiRefs.prepareButton != null)
            {
                uiRefs.prepareButton.gameObject.SetActive(true);
                uiRefs.prepareButton.interactable = false;
            }
            if (uiRefs.confirmButton != null)
            {
                uiRefs.confirmButton.gameObject.SetActive(true);
                uiRefs.confirmButton.interactable = false;
            }
            if (uiRefs.gameBackButton != null)
            {
                uiRefs.gameBackButton.gameObject.SetActive(true);
                uiRefs.gameBackButton.interactable = true;
            }
            
            // 确保结果面板是隐藏的
            if (uiRefs.resultPanel != null)
            {
                uiRefs.resultPanel.SetActive(false);
            }
        }

        private void InitializeFreePlayPanel()
        {
            // 初始化自由演奏面板
            if (uiRefs.freePlayBackButton != null)
            {
                uiRefs.freePlayBackButton.gameObject.SetActive(true);
                uiRefs.freePlayBackButton.interactable = true;
            }
        }

        private void InitializeBackgroundPanel()
        {
            // 初始化背景介绍面板
            if (uiRefs.backgroundBackButton != null)
            {
                uiRefs.backgroundBackButton.gameObject.SetActive(true);
                uiRefs.backgroundBackButton.interactable = true;
            }
            if (uiRefs.backgroundText != null)
            {
                uiRefs.backgroundText.text = GetBackgroundIntroduction();
            }
        }

        public void EnterChallengeMode()
        {
            if (_uiManager != null)
            {
                _uiManager.ShowGamePanel();
                _uiManager.UpdateButtonLabels("演奏", "准备", "确认");
            }
            
            if (_gameManager != null)
            {
                _gameManager.StartChallengeMode();
                RegisterChallengeButtonEvents();
                SetInitialChallengeButtonStates();
            }
        }

        private void RegisterChallengeButtonEvents()
        {
            if (_gameManager == null) return;

            if (uiRefs.playButton != null)
            {
                uiRefs.playButton.onClick.RemoveAllListeners();
                uiRefs.playButton.onClick.AddListener(_gameManager.PlayMelody);
            }
            
            if (uiRefs.prepareButton != null)
            {
                uiRefs.prepareButton.onClick.RemoveAllListeners();
                uiRefs.prepareButton.onClick.AddListener(_gameManager.ReadyToMimic);
            }
            
            if (uiRefs.confirmButton != null)
            {
                uiRefs.confirmButton.onClick.RemoveAllListeners();
                uiRefs.confirmButton.onClick.AddListener(_gameManager.CheckPlayerInput);
            }
        }

        private void SetInitialChallengeButtonStates()
        {
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = true;
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = false;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = false;
        }
    }
} 