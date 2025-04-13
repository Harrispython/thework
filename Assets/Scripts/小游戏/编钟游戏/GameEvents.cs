using UnityEngine;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 游戏事件管理器，处理组件间的通信
    /// </summary>
    public static class GameEvents
    {
        // 游戏状态事件
        public static event System.Action OnGameStart;
        public static event System.Action OnGamePause;
        public static event System.Action OnGameResume;
        public static event System.Action OnGameEnd;
        public static event System.Action OnReturnToMainMenu;

        // 编钟相关事件
        public static event System.Action<int> OnBellHit;
        public static event System.Action OnMelodyComplete;
        public static event System.Action OnPlayerInputComplete;

        // UI相关事件
        public static event System.Action<bool> OnShowResult;
        public static event System.Action OnHideAllPanels;
        public static event System.Action<string> OnShowInstruction;
        public static event System.Action<string> OnPanelSwitched;

        // 事件触发方法
        public static void TriggerGameStart() => OnGameStart?.Invoke();
        public static void TriggerGamePause() => OnGamePause?.Invoke();
        public static void TriggerGameResume() => OnGameResume?.Invoke();
        public static void TriggerGameEnd() => OnGameEnd?.Invoke();
        public static void TriggerReturnToMainMenu() => OnReturnToMainMenu?.Invoke();
        public static void TriggerBellHit(int bellIndex) => OnBellHit?.Invoke(bellIndex);
        public static void TriggerMelodyComplete() => OnMelodyComplete?.Invoke();
        public static void TriggerPlayerInputComplete() => OnPlayerInputComplete?.Invoke();
        public static void TriggerShowResult(bool success) => OnShowResult?.Invoke(success);
        public static void TriggerHideAllPanels() => OnHideAllPanels?.Invoke();
        public static void TriggerShowInstruction(string text) => OnShowInstruction?.Invoke(text);
        public static void TriggerPanelSwitched(string panelName) => OnPanelSwitched?.Invoke(panelName);
    }
} 