using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 编钟游戏管理器
    /// </summary>
    public class BellGameManager : MonoBehaviour
    {
        [Header("UI引用")]
        public UIReferences uiRefs;

        [Header("游戏设置")]
        [Tooltip("编钟音序数据")]
        public List<MelodyData> melodyDatas = new List<MelodyData>();
        [Tooltip("当前关卡")]
        public int currentLevel = 0;
        [Tooltip("演奏次数限制")]
        public int maxPlayCount = 3;
        [Tooltip("节奏判定容差(秒)")]
        public float rhythmTolerance = 0.3f;
        [Tooltip("是否启用节奏判定")]
        public bool enableRhythmJudgment = false;
        
        [Header("编钟引用")]
        [Tooltip("编钟对象列表")]
        public List<Bell> bells = new List<Bell>();
        
        // 私有变量
        private List<int> _currentMelody = new List<int>();  // 当前关卡的乐句
        private List<float> _currentRhythm = new List<float>(); // 当前关卡的节奏
        private List<int> _playerInputs = new List<int>();   // 玩家输入的音序
        private List<float> _playerTimestamps = new List<float>(); // 玩家输入的时间戳
        private int _playCount = 0;                          // 已使用的演奏次数
        private float _startTime = 0f;                       // 开始记录时间
        private bool _isListening = false;                   // 是否正在播放乐句
        private bool _isPlayerTurn = false;                  // 是否是玩家回合
        private bool _isChallengeMode = false;               // 是否是挑战模式
        private bool _isReadyToMimic = false;                // 是否准备好模仿
        private bool _isExiting = false;                     // 防止循环调用的标志位
        private bool _canPlayBellSound = false;              // 是否允许编钟发声
        
        private BellGameUIManager _uiManager;               // UI管理器引用
        
        private void Awake()
        {
            _uiManager = GetComponent<BellGameUIManager>();
        }

        private void OnEnable()
        {
            print("BellGameManager启动");
            InitializeGame();
            
            // 订阅事件
            GameEvents.OnBellHit += OnBellHit;
            GameEvents.OnGameStart += StartGame;
            GameEvents.OnGameEnd += HandleGameEnd;  // 改为使用新的处理方法
        }

        private void OnDisable()
        {
            // 取消订阅事件
            GameEvents.OnBellHit -= OnBellHit;
            GameEvents.OnGameStart -= StartGame;
            GameEvents.OnGameEnd -= HandleGameEnd;  // 改为使用新的处理方法
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
           
        }
        
        private void InitializeGame()
        {
            // 注册编钟事件
            foreach (var bell in bells)
            {
                bell.OnBellHit += OnBellHit;
                // 初始情况下禁用编钟声音（挑战模式）
                bell.SetSoundEnabled(false);
            }
            
            // 注册按钮事件
            uiRefs.playButton.onClick.AddListener(PlayMelody);
            uiRefs.prepareButton.onClick.AddListener(ReadyToMimic);
            uiRefs.confirmButton.onClick.AddListener(CheckPlayerInput);
            
            // 注册返回按钮事件
            if (uiRefs.gameBackButton != null) uiRefs.gameBackButton.onClick.AddListener(ExitGame);
            
            // 注册结果面板按钮事件
            if (uiRefs.retryButton != null) uiRefs.retryButton.onClick.AddListener(RetryLevel);
            if (uiRefs.backToMenuButton != null) uiRefs.backToMenuButton.onClick.AddListener(ExitGame);
            if (uiRefs.nextLevelButton != null) uiRefs.nextLevelButton.onClick.AddListener(NextLevel);
            
            // 初始化UI
            UpdateUI();
            
            // 隐藏结果面板
            uiRefs.resultPanel.SetActive(false);
            
            // 游戏会通过BellGameStarter启动，所以这里不需要自动开始
            // StartGame();
        }
        
        /// <summary>
        /// 初始化挑战
        /// </summary>
        public void InitializeChallenge()
        {
            _isChallengeMode = true;
            
            // 确保有关卡数据
            if (melodyDatas.Count == 0 || currentLevel >= melodyDatas.Count)
            {
                Debug.LogError("没有可用的关卡数据！");
                return;
            }
            
            // 设置当前关卡的乐句和节奏
            _currentMelody = new List<int>(melodyDatas[currentLevel].melody);
            _currentRhythm = new List<float>(melodyDatas[currentLevel].rhythm);
            
            // 重置游戏状态
            _playerInputs.Clear();
            _playerTimestamps.Clear();
            _playCount = 0;
            _isReadyToMimic = false;
            _isListening = false;
            _isPlayerTurn = false;
            _canPlayBellSound = false;
            
            // 禁用所有编钟声音
            DisableAllBellSounds();
            
            UpdateUI();
            
            // 显示指引文本
            ShowInstruction("请点击演奏按钮以听取示范乐句。");
            
            // 禁用确认按钮和准备按钮
            if (uiRefs.confirmButton != null)
            {
                uiRefs.confirmButton.interactable = false;
            }
            
            if (uiRefs.prepareButton != null)
            {
                uiRefs.prepareButton.interactable = false;
            }
            
            if (uiRefs.playButton != null)
            {
                uiRefs.playButton.interactable = true;
            }
            
            // 更新演奏次数显示
            UpdatePlayCountDisplay();
        }
        
        /// <summary>
        /// 更新演奏次数显示
        /// </summary>
        private void UpdatePlayCountDisplay()
        {
            if (uiRefs.playCountText != null)
            {
                uiRefs.playCountText.text = $"剩余演奏次数: {maxPlayCount - _playCount}";
            }
        }
        
        /// <summary>
        /// 播放示范乐句
        /// </summary>
        public void PlayMelody()
        {
            // 检查演奏次数限制
            if (_playCount >= maxPlayCount)
            {
                ShowInstruction("抱歉，您已用完演奏次数。");
                return;
            }
            
            // 增加演奏计数
            _playCount++;
            
            // 更新演奏次数显示
            UpdatePlayCountDisplay();
            
            // 设置状态
            _isListening = true;
            _isPlayerTurn = false;
            _isReadyToMimic = false;
            _canPlayBellSound = false;  // 在播放示范时禁止玩家敲击发声
            
            // 在演奏期间禁用所有按钮
            SetButtonsInteractable(false);
            
            // 清空玩家输入（如果有）
            _playerInputs.Clear();
            _playerTimestamps.Clear();
            UpdatePlayerRecord();
            
            UpdateUI();
            
            // 播放乐句序列
            StartCoroutine(PlayMelodySequence());
        }
        
        /// <summary>
        /// 开始游戏
        /// </summary>
        public void StartGame()
        {
            
            // 设置关卡为第一关（索引为0）
            currentLevel = 0;
    
            // 重置游戏状态
            ResetGameState();
            // 初始化挑战
            InitializeChallenge();
            // // 延迟后播放乐句示范
            // StartCoroutine(DelayedAction(1f, () => {
            //     PlayMelody();
            // }));
        }
        
        
        /// <summary>
        /// 播放乐句序列的协程
        /// </summary>
        private IEnumerator PlayMelodySequence()
        {
            ShowInstruction("请听编钟的演奏...");
            
            // 等待少许时间后开始
            yield return new WaitForSeconds(1f);
            
            for (int i = 0; i < _currentMelody.Count; i++)
            {
                int bellIndex = _currentMelody[i];
                
                // 查找对应编号编钟
                Bell targetBell = bells.Find(b => b.bellIndex == bellIndex);
                if (targetBell != null)
                {
                    // 临时启用声音，播放音效后再禁用
                    targetBell.SetSoundEnabled(true);
                    targetBell.PlayBell();
                    targetBell.SetSoundEnabled(false);
                }
                else
                {
                    Debug.LogWarning($"找不到编号为 {bellIndex} 的编钟！");
                }
                
                // 使用节奏数据等待
                float waitTime = i < _currentRhythm.Count ? _currentRhythm[i] : 1f;
                yield return new WaitForSeconds(waitTime);
            }
            
            // 演奏结束
            _isListening = false;
            _isPlayerTurn = false;
            _canPlayBellSound = false; // 确保示范结束后编钟仍保持静音
            
            ShowInstruction("演奏结束。请点击准备按钮开始模仿。");
            
            // 启用按钮交互
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = _playCount < maxPlayCount;
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = true;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = false;
            
            UpdateUI();
        }
        
        /// <summary>
        /// 准备模仿
        /// </summary>
        public void ReadyToMimic()
        {
            _isReadyToMimic = true;
            _isPlayerTurn = true;
            _canPlayBellSound = true;  // 允许编钟在模仿阶段发声
            
            // 启用编钟声音
            EnableAllBellSounds();
            
            // 清空玩家之前的输入
            _playerInputs.Clear();
            _playerTimestamps.Clear();
            
            // 记录开始时间
            _startTime = Time.time;
            
            // 更新UI
            UpdatePlayerRecord();
            ShowInstruction("请开始模仿刚才听到的乐句！完成后点击确认按钮。");
            
            // 更新按钮状态
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = false;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = true;
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = false;
        }
        
        /// <summary>
        /// 编钟被敲击时的回调
        /// </summary>
        private void OnBellHit(int bellIndex)
        {
            // 如果正在播放示范乐句，忽略玩家输入
            if (_isListening) return;
            
            // 检查是否允许编钟发声
            if (!_canPlayBellSound)
            {
                return;
            }
            
            // 挑战模式下，只有在准备模仿状态才记录玩家输入
            if (_isChallengeMode && !_isReadyToMimic) return;
            
            // 如果不是玩家回合，则忽略
            if (!_isPlayerTurn) return;
            
            // 记录玩家输入
            _playerInputs.Add(bellIndex);
            _playerTimestamps.Add(Time.time - _startTime);
            
            // 更新UI显示
            UpdatePlayerRecord();
            
            // 如果有输入，启用确认按钮
            if (uiRefs.confirmButton != null)
            {
                uiRefs.confirmButton.interactable = true;
            }
        }
        
        /// <summary>
        /// 检查玩家输入是否正确
        /// </summary>
        public void CheckPlayerInput()
        {
            // 如果不是玩家回合，则忽略
            if (!_isPlayerTurn) return;
            
            // 禁用编钟声音
            _canPlayBellSound = false;
            DisableAllBellSounds();
            
            bool isCorrect = false;
            string resultMessage = "";
            
            // 检查音高序列是否正确
            if (_playerInputs.Count != _currentMelody.Count)
            {
                resultMessage = "音符数量不匹配！";
            }
            else
            {
                // 检查每个音高是否匹配
                bool melodyCorrect = true;
                for (int i = 0; i < _currentMelody.Count; i++)
                {
                    if (_playerInputs[i] != _currentMelody[i])
                    {
                        melodyCorrect = false;
                        break;
                    }
                }
                
                // 如果音高正确，且启用了节奏判定，则检查节奏
                if (melodyCorrect && enableRhythmJudgment)
                {
                    bool rhythmCorrect = true;
                    
                    // 计算累积时间
                    float[] expectedTimes = new float[_currentRhythm.Count];
                    float accTime = 0f;
                    
                    for (int i = 0; i < _currentRhythm.Count; i++)
                    {
                        accTime += _currentRhythm[i];
                        expectedTimes[i] = accTime;
                    }
                    
                    // 检查每个时间点是否在容差范围内
                    for (int i = 0; i < _playerTimestamps.Count - 1; i++) // 最后一个时间戳不需要检查
                    {
                        float playerInterval = _playerTimestamps[i + 1] - _playerTimestamps[i];
                        float expectedInterval = _currentRhythm[i];
                        
                        if (Mathf.Abs(playerInterval - expectedInterval) > rhythmTolerance)
                        {
                            rhythmCorrect = false;
                            break;
                        }
                    }
                    
                    if (rhythmCorrect)
                    {
                        isCorrect = true;
                        resultMessage = "太棒了！您完美地模仿了演奏！";
                    }
                    else
                    {
                        resultMessage = "音高正确，但节奏不够准确。";
                    }
                }
                else if (melodyCorrect)
                {
                    isCorrect = true;
                    resultMessage = "很好！您成功模仿了演奏！";
                }
                else
                {
                    resultMessage = "音高序列不正确。";
                }
            }
            
            // 显示结果前禁用编钟声音
            DisableAllBellSounds();
            
            // 重置状态
            _isReadyToMimic = false;
            _isPlayerTurn = false;
            
            // 显示结果
            ShowResult(isCorrect, resultMessage);
        }
        
        /// <summary>
        /// 获取乐句字符串表示
        /// </summary>
        private string GetMelodyString(List<int> melody)
        {
            string result = "";
            foreach (var note in melody)
            {
                result += note.ToString() + " ";
            }
            return result.Trim();
        }
        
        /// <summary>
        /// 显示游戏结果
        /// </summary>
        private void ShowResult(bool isSuccess, string message)
        {
            // 隐藏游戏面板，显示结果面板
            if (uiRefs.gamePanel != null)
            {
                uiRefs.gamePanel.SetActive(false);
            }

            // 激活结果面板
            if (uiRefs.resultPanel != null)
            {
                // 设置结果面板可见性
                uiRefs.resultPanel.SetActive(true);
                CanvasGroup resultGroup = uiRefs.resultPanel.GetComponent<CanvasGroup>();
                if (resultGroup != null)
                {
                    resultGroup.alpha = 1f;
                    resultGroup.blocksRaycasts = true;
                    resultGroup.interactable = true;
                }
                
                // 设置结果标题
                if (uiRefs.resultTitleText != null)
                {
                    uiRefs.resultTitleText.text = isSuccess ? "挑战成功" : "挑战失败";
                    uiRefs.resultTitleText.color = isSuccess ? Color.green : Color.red;
                }
                
                // 设置结果文本
                if (uiRefs.resultText != null)
                {
                    uiRefs.resultText.text = message;
                }
            }
            
            // 触发结果事件
            GameEvents.TriggerShowResult(isSuccess);
            
            // 播放成功/失败音效
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                if (isSuccess)
                {
                    // 播放成功音效
                    if (_uiManager != null && _uiManager.successSound != null)
                    {
                        audioSource.clip = _uiManager.successSound;
                        audioSource.Play();
                    }
                }
                else
                {
                    // 播放失败音效
                    if (_uiManager != null && _uiManager.failureSound != null)
                    {
                        audioSource.clip = _uiManager.failureSound;
                        audioSource.Play();
                    }
                }
            }
            
            // 更新按钮状态
            // 成功时，显示下一关(如果有)和返回主菜单按钮
            // 失败时，显示重试和返回主菜单按钮
            bool hasNextLevel = currentLevel < melodyDatas.Count - 1;
            
            if (uiRefs.retryButton != null)
            {
                uiRefs.retryButton.gameObject.SetActive(!isSuccess || !hasNextLevel);
                uiRefs.retryButton.interactable = true;
            }
            
            if (uiRefs.backToMenuButton != null)
            {
                uiRefs.backToMenuButton.gameObject.SetActive(true);
                uiRefs.backToMenuButton.interactable = true;
            }
            
            if (uiRefs.nextLevelButton != null)
            {
                uiRefs.nextLevelButton.gameObject.SetActive(isSuccess && hasNextLevel);
                uiRefs.nextLevelButton.interactable = isSuccess && hasNextLevel;
            }
            
            // 禁用游戏界面按钮
            SetButtonsInteractable(false);
        }
        
        /// <summary>
        /// 重试当前关卡
        /// </summary>
        public void RetryLevel()
        {
            // 隐藏结果面板
            if (uiRefs.resultPanel != null)
            {
                uiRefs.resultPanel.SetActive(false);
            }
            
            // 显示游戏面板
            if (uiRefs.gamePanel != null)
            {
                uiRefs.gamePanel.SetActive(true);
                CanvasGroup gameGroup = uiRefs.gamePanel.GetComponent<CanvasGroup>();
                if (gameGroup != null)
                {
                    gameGroup.alpha = 1f;
                    gameGroup.blocksRaycasts = true;
                    gameGroup.interactable = true;
                }
            }
            
            // 重置当前关卡状态
            _playerInputs.Clear();
            _playerTimestamps.Clear();
            _playCount = 0;
            _isReadyToMimic = false;
            DisableAllBellSounds();
            
            // 重置按钮状态
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = true;
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = false;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = false;
            
            // 重置演奏次数显示
            UpdatePlayCountDisplay();
            // 显示指引
            ShowInstruction($"第{currentLevel + 1}关: 请点击演奏按钮听取示范乐句。");
            //
            // 更新UI
            UpdateUI();
        }
        
        /// <summary>
        /// 进入下一关
        /// </summary>
        public void NextLevel()
        {
           
            // 隐藏结果面板
            if (uiRefs.resultPanel != null)
            {
                uiRefs.resultPanel.SetActive(false);
            }
            
                    // 增加关卡
                    currentLevel++;
                    
                    // 如果还有下一关，则继续
                    if (currentLevel < melodyDatas.Count)
                    {
                // 显示游戏面板
                if (uiRefs.gamePanel != null)
                {
                    uiRefs.gamePanel.SetActive(true);
                    CanvasGroup gameGroup = uiRefs.gamePanel.GetComponent<CanvasGroup>();
                    if (gameGroup != null)
                    {
                        gameGroup.alpha = 1f;
                        gameGroup.blocksRaycasts = true;
                        gameGroup.interactable = true;
                    }
                }
                
                // 显示当前关卡信息
                if (uiRefs.levelText != null)
                {
                    uiRefs.levelText.text = $"关卡: {currentLevel + 1}/{melodyDatas.Count}";
                }
                
                // 重置状态并初始化新关卡
                _playerInputs.Clear();
                _playerTimestamps.Clear();
                _playCount = 0;
                _isReadyToMimic = false;
                
                // 获取新关卡的音序和节奏
                if (currentLevel < melodyDatas.Count)
                {
                    _currentMelody = new List<int>(melodyDatas[currentLevel].melody);
                    _currentRhythm = new List<float>(melodyDatas[currentLevel].rhythm);
                }
                
                // 显示提示
                ShowInstruction($"第{currentLevel + 1}关: 请点击演奏按钮听取示范乐句。");
                
                // 禁用编钟声音
                DisableAllBellSounds();
                
                // 重置按钮状态
                if (uiRefs.playButton != null) uiRefs.playButton.interactable = true;
                if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = false;
                if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = false;
                
                // 重置演奏次数显示
                UpdatePlayCountDisplay();
                // 更新UI
                UpdateUI();
                    }
                    else
                    {
                        // 游戏通关
                        ShowInstruction("恭喜您完成了所有挑战！");
                        _isPlayerTurn = false;
                _isReadyToMimic = false;
                DisableAllBellSounds();

                // 返回主菜单
                BellGameMainMenu mainMenu = GetComponent<BellGameMainMenu>();
                if (mainMenu != null)
                {
                    mainMenu.ReturnToMainMenu();
                }
            }
            if (uiRefs.prepareButton != null)
            {
                uiRefs.prepareButton.interactable = false;
            }
        }
        
        /// <summary>
        /// 显示指引文本
        /// </summary>
        private void ShowInstruction(string text)
        {
            GameEvents.TriggerShowInstruction(text);
            if (uiRefs.instructionText != null)
            {
                uiRefs.instructionText.text = text;
            }
        }
        
        /// <summary>
        /// 更新玩家记录显示
        /// </summary>
        private void UpdatePlayerRecord()
        {
            string record = "已敲击: ";
            foreach (var input in _playerInputs)
            {
                record += input.ToString() + " ";
            }
            uiRefs.playerRecordText.text = record;
        }
        
        /// <summary>
        /// 更新UI显示
        /// </summary>
        private void UpdateUI(bool updatePlayCount = true)
        {
            if (uiRefs.levelText != null)
            {
                uiRefs.levelText.text = $"关卡: {currentLevel + 1}/{melodyDatas.Count}";
            }
            
            if (_isChallengeMode)
            {
                uiRefs.playButton.interactable = !_isListening && !_isReadyToMimic && _playCount < maxPlayCount;
                uiRefs.prepareButton.interactable = !_isListening && !_isReadyToMimic;
                uiRefs.confirmButton.interactable = _isReadyToMimic && _playerInputs.Count > 0;
                
            }
            else
            {
                uiRefs.playButton.interactable = !_isListening;
                uiRefs.prepareButton.interactable = false;
                uiRefs.confirmButton.interactable = _isPlayerTurn && _playerInputs.Count > 0;
            }
            
            UpdatePlayerRecord();
            if (updatePlayCount)
            {
                UpdatePlayCountDisplay();
            }
        }
        
        /// <summary>
        /// 处理游戏结束事件
        /// </summary>
        private void HandleGameEnd()
        {
            if (!_isExiting)  // 防止循环调用
            {
                ExitGame();
            }
        }
        
        /// <summary>
        /// 退出游戏（实际是返回主菜单）
        /// </summary>
        private void ExitGame()
        {
            if (_isExiting) return;  // 防止循环调用
            _isExiting = true;

            try
            {
                // 触发游戏结束事件（先解除订阅以防循环调用）
                GameEvents.OnGameEnd -= HandleGameEnd;
                
                // 隐藏结果面板
                if (uiRefs.resultPanel != null)
                {
                    uiRefs.resultPanel.SetActive(false);
                }
                
                // 重置游戏状态
                ResetGameState();
                
                // 隐藏游戏面板
                if (uiRefs.gamePanel != null)
                {
                    uiRefs.gamePanel.SetActive(false);
                }

                // 显示并激活主菜单面板
                if (uiRefs.mainMenuPanel != null) 
                {
                    uiRefs.mainMenuPanel.SetActive(true);
                    CanvasGroup mainMenuGroup = uiRefs.mainMenuPanel.GetComponent<CanvasGroup>();
                    if (mainMenuGroup != null)
                    {
                        mainMenuGroup.alpha = 1f;
                        mainMenuGroup.blocksRaycasts = true;
                        mainMenuGroup.interactable = true;
                    }
                    InitializeMainMenuPanel();
                }

                // 直接调用BellGameMainMenu的处理方法

                BellGameMainMenu mainMenu = GetComponent<BellGameMainMenu>();
                if (mainMenu != null)
                {
                    // 不要调用ReturnToMainMenu，因为它会触发事件
                    mainMenu.InitializeMainMenuPanel();
                }

                // 触发返回主菜单事件而不是游戏结束事件
                GameEvents.TriggerReturnToMainMenu();
            }
            finally
            {
                // 重新订阅事件
                GameEvents.OnGameEnd += HandleGameEnd;
                _isExiting = false;  // 确保标志位被重置
            }
        }
        
        /// <summary>
        /// 初始化主菜单面板
        /// </summary>
        private void InitializeMainMenuPanel()
        {
            // 确保主菜单面板CanvasGroup的alpha不为0
            if (uiRefs.mainMenuPanel != null)
            {
                CanvasGroup mainMenuGroup = uiRefs.mainMenuPanel.GetComponent<CanvasGroup>();
                if (mainMenuGroup != null)
                {
                    mainMenuGroup.alpha = 1f;
                    mainMenuGroup.blocksRaycasts = true;
                    mainMenuGroup.interactable = true;
                }
            }
            
            // 确保主菜单所有按钮都是可见和可交互的
            if (uiRefs.startChallengeButton != null)
            {
                uiRefs.startChallengeButton.gameObject.SetActive(true);
                uiRefs.startChallengeButton.interactable = true;
            }
            
            if (uiRefs.freePlayButton != null)
            {
                uiRefs.freePlayButton.gameObject.SetActive(true);
                uiRefs.freePlayButton.interactable = true;
            }
            
            if (uiRefs.backgroundButton != null)
            {
                uiRefs.backgroundButton.gameObject.SetActive(true);
                uiRefs.backgroundButton.interactable = true;
            }
            
            if (uiRefs.exitGameButton != null)
            {
                uiRefs.exitGameButton.gameObject.SetActive(true);
                uiRefs.exitGameButton.interactable = true;
            }
        }
        
        /// <summary>
        /// 延迟执行操作的协程
        /// </summary>
        private IEnumerator DelayedAction(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        
        /// <summary>
        /// 进入自由演奏模式
        /// </summary>
        public void EnterFreePlayMode()
        {
            // 隐藏其他面板
            if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
            if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);
            if (uiRefs.mainMenuPanel != null) uiRefs.mainMenuPanel.SetActive(false);
            if (uiRefs.backgroundPanel != null) uiRefs.backgroundPanel.SetActive(false);
            
            // 显示自由演奏面板
            if (uiRefs.freePlayPanel != null)
            {
                uiRefs.freePlayPanel.SetActive(true);
                CanvasGroup freePlayGroup = uiRefs.freePlayPanel.GetComponent<CanvasGroup>();
                if (freePlayGroup != null)
                {
                    freePlayGroup.alpha = 1f;
                    freePlayGroup.blocksRaycasts = true;
                    freePlayGroup.interactable = true;
                }
            }
            
            // 设置非挑战模式
            _isChallengeMode = false;
            
            // 重置所有状态
            ResetGameState();
            
            // 设置为自由演奏状态
            _isPlayerTurn = true;
            _isListening = false;
            
            // 启用所有编钟声音
            EnableAllBellSounds();
            
            // 显示指引
            ShowInstruction("自由演奏模式：尽情享受编钟的魅力吧！\n点击每个编钟可以发出不同的音色。");
            
            // 确保返回主菜单按钮可见且可交互
            if (uiRefs.freePlayBackButton != null)
            {
                uiRefs.freePlayBackButton.gameObject.SetActive(true);
                uiRefs.freePlayBackButton.interactable = true;
                
                // 注册返回事件（如果尚未注册）
                uiRefs.freePlayBackButton.onClick.RemoveListener(ExitGame);
                uiRefs.freePlayBackButton.onClick.AddListener(ExitGame);
            }
            
            // 启用所有编钟
            foreach (var bell in bells)
            {
                bell.gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        /// 显示背景介绍面板
        /// </summary>
        public void ShowBackgroundPanel()
        {
            // 隐藏其他面板
            if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
            if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);
            if (uiRefs.mainMenuPanel != null) uiRefs.mainMenuPanel.SetActive(false);
            if (uiRefs.freePlayPanel != null) uiRefs.freePlayPanel.SetActive(false);
            
            // 显示背景介绍面板
            if (uiRefs.backgroundPanel != null)
            {
                uiRefs.backgroundPanel.SetActive(true);
                CanvasGroup backgroundGroup = uiRefs.backgroundPanel.GetComponent<CanvasGroup>();
                if (backgroundGroup != null)
                {
                    backgroundGroup.alpha = 1f;
                    backgroundGroup.blocksRaycasts = true;
                    backgroundGroup.interactable = true;
                }
            }
            
            // 确保返回主菜单按钮可见且可交互
            if (uiRefs.backgroundBackButton != null)
            {
                uiRefs.backgroundBackButton.gameObject.SetActive(true);
                uiRefs.backgroundBackButton.interactable = true;
                
                // 注册返回事件（如果尚未注册）
                uiRefs.backgroundBackButton.onClick.RemoveListener(ExitGame);
                uiRefs.backgroundBackButton.onClick.AddListener(ExitGame);
            }

            // 更新背景文本内容
            BellGameMainMenu mainmenu = this.GetComponent<BellGameMainMenu>();
            if (mainmenu != null && uiRefs.backgroundText != null)
            {
                uiRefs.backgroundText.text = mainmenu.GetBackgroundIntroduction();
            }
        }

        /// <summary>
        /// 重置游戏状态
        /// </summary>
        public void ResetGameState()
        {
            // 清空玩家输入
            _playerInputs.Clear();
            _playerTimestamps.Clear();
            
            // 重置计数器和状态
            _playCount = 0;
            _isListening = false;
            _isPlayerTurn = false;
            _isReadyToMimic = false;
            _canPlayBellSound = false;
            
            // 禁用编钟声音（如果是挑战模式）
            if (_isChallengeMode)
            {
                DisableAllBellSounds();
            }
            else
            {
                EnableAllBellSounds();
            }
            
            // 更新UI（不更新演奏次数，避免循环调用）
            UpdateUI(false);
            UpdatePlayerRecord();
        }
        
        /// <summary>
        /// 开始玩家回合
        /// </summary>
        public void StartPlayerTurn()
        {
            // 设置为玩家回合
            _isPlayerTurn = true;
            _isListening = false;
            
            // 记录开始时间
            _startTime = Time.time;
            
            // 清空玩家之前的输入
            _playerInputs.Clear();
            _playerTimestamps.Clear();
            
            // 更新UI显示
            UpdatePlayerRecord();
            
            // 显示指引
            ShowInstruction("请开始模仿刚才听到的乐句！");
        }
        
        /// <summary>
        /// 启用所有编钟声音
        /// </summary>
        private void EnableAllBellSounds()
        {
            foreach (var bell in bells)
            {
                bell.SetSoundEnabled(true);
            }
        }
        
        /// <summary>
        /// 禁用所有编钟声音
        /// </summary>
        private void DisableAllBellSounds()
        {
            foreach (var bell in bells)
            {
                bell.SetSoundEnabled(false);
            }
        }
        
        /// <summary>
        /// 设置所有按钮交互状态
        /// </summary>
        private void SetButtonsInteractable(bool interactable)
        {
            if (uiRefs.playButton != null) uiRefs.playButton.interactable = interactable;
            if (uiRefs.prepareButton != null) uiRefs.prepareButton.interactable = interactable;
            if (uiRefs.confirmButton != null) uiRefs.confirmButton.interactable = interactable;
            // 退出按钮始终保持可点击
            // if (exitButton != null) exitButton.interactable = interactable;
        }
        
        /// <summary>
        /// 开始挑战模式
        /// </summary>
        public void StartChallengeMode()
        {
            // 设置为挑战模式
            _isChallengeMode = true;
            
            // 重置游戏状态
            ResetGameState();
            
            // 重置关卡（可选，如果需要从第一关开始）
            // currentLevel = 0;
            
            // 初始化挑战
            InitializeChallenge();
            
            // 更新UI
            UpdateUI();
            
            // 显示指引文本
            ShowInstruction("请点击演奏按钮以听取示范乐句。");
        }
    }
    
    /// <summary>
    /// 乐句数据类
    /// </summary>
    [System.Serializable]
    public class MelodyData
    {
        [Tooltip("乐句音序")]
        public List<int> melody = new List<int>();
        [Tooltip("乐句节奏")]
        public List<float> rhythm = new List<float>();
    }
} 