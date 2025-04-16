using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace 小游戏.编钟游戏
{
    /// <summary>
    /// 编钟游戏UI管理器
    /// </summary>
    public class BellGameUIManager : MonoBehaviour
    {
        [Header("UI引用")]
        public UIReferences uiRefs;
        
        [Header("UI动画设置")]
        public float fadeInDuration = 0.5f;
        public float fadeOutDuration = 0.3f;
        public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("音效设置")]
        public AudioClip showUISound;
        public AudioClip hideUISound;
        public AudioClip successSound;
        public AudioClip failureSound;
        
        private AudioSource _audioSource;

        private void OnEnable()
        {
            InitializeAudioSource();
            InitializeUI();
            // 订阅事件
            GameEvents.OnShowResult += HandleShowResult;
            GameEvents.OnHideAllPanels += HideAllPanels;
            GameEvents.OnShowInstruction += UpdateInstructionText;
        }

        private void OnDisable()
        {
            // 取消订阅事件
            GameEvents.OnShowResult -= HandleShowResult;
            GameEvents.OnHideAllPanels -= HideAllPanels;
            GameEvents.OnShowInstruction -= UpdateInstructionText;
        }
        
        private void Awake()
        {
            
        }

        private void InitializeAudioSource()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void InitializeUI()
        {
            if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
            if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);
            InitializeResultPanelButtons();
            InitializeBackButtons();
        }

        private void InitializeResultPanelButtons()
        {
            if (uiRefs.retryButton != null)
            {
                uiRefs.retryButton.gameObject.SetActive(false);
            }

            if (uiRefs.backToMenuButton != null)
            {
                uiRefs.backToMenuButton.gameObject.SetActive(false);
            }

            if (uiRefs.nextLevelButton != null)
            {
                uiRefs.nextLevelButton.gameObject.SetActive(false);
            }
        }

        private void InitializeBackButtons()
        {
            // 初始化各个面板的返回按钮
            if (uiRefs.gameBackButton != null)
            {
                uiRefs.gameBackButton.gameObject.SetActive(false);
            }
            
            if (uiRefs.freePlayBackButton != null)
            {
                uiRefs.freePlayBackButton.gameObject.SetActive(false);
            }
            
            if (uiRefs.backgroundBackButton != null)
            {
                uiRefs.backgroundBackButton.gameObject.SetActive(false);
            }
        }

        public void HideAllPanels()
        {
            GameObject[] allPanels = GameObject.FindGameObjectsWithTag("UIPanel");
            foreach (GameObject panel in allPanels)
            {
                panel.SetActive(false);
            }

            if (uiRefs.mainMenuPanel != null) uiRefs.mainMenuPanel.SetActive(false);
            if (uiRefs.gamePanel != null) uiRefs.gamePanel.SetActive(false);
            if (uiRefs.resultPanel != null) uiRefs.resultPanel.SetActive(false);
        }

        public void UpdateButtonLabels(string playButtonText = "演奏", string mimicButtonText = "准备", string confirmButtonText = "确认")
        {
            UpdateButtonText(uiRefs.playButton, playButtonText);
            UpdateButtonText(uiRefs.prepareButton, mimicButtonText);
            UpdateButtonText(uiRefs.confirmButton, confirmButtonText);
        }

        private void UpdateButtonText(Button button, string text)
        {
            if (button != null)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null) buttonText.text = text;
            }
        }

        public void UpdatePlayCount(int currentCount, int maxCount)
        {
            if (uiRefs.playCountText != null)
            {
                uiRefs.playCountText.text = $"剩余演奏次数: {maxCount - currentCount}";
            }
        }

        public void ShowGamePanel()
        {
            if (uiRefs.gamePanel == null) return;

            StopAllCoroutines();
            HideAllPanels();
            StartCoroutine(FadeInUI(uiRefs.gamePanel));
            PlaySound(showUISound);
        }

        private void HandleShowResult(bool isSuccess)
        {
            if (uiRefs.resultPanel == null) return;

            if (uiRefs.resultTitleText != null)
            {
                uiRefs.resultTitleText.text = isSuccess ? "挑战成功" : "挑战失败";
                uiRefs.resultTitleText.color = isSuccess ? Color.green : Color.red;
            }

            UpdateResultButtons(isSuccess);
            StartCoroutine(FadeInUI(uiRefs.resultPanel));
            PlaySound(isSuccess ? successSound : failureSound);
        }

        private void UpdateResultButtons(bool isSuccess)
        {
            if (uiRefs.retryButton != null)
            {
                uiRefs.retryButton.gameObject.SetActive(true);
            }

            if (uiRefs.backToMenuButton != null)
            {
                uiRefs.backToMenuButton.gameObject.SetActive(true);
            }

            if (uiRefs.nextLevelButton != null)
            {
                uiRefs.nextLevelButton.gameObject.SetActive(isSuccess);
            }
        }

        public void UpdatePlayerRecord(string record)
        {
            if (uiRefs.playerRecordText != null)
            {
                uiRefs.playerRecordText.text = record;
            }
        }

        private void UpdateInstructionText(string text)
        {
            if (uiRefs.instructionText != null)
            {
                uiRefs.instructionText.text = text;
            }
        }

        private IEnumerator FadeInUI(GameObject uiElement)
        {
            if (uiElement == null) yield break;

            uiElement.SetActive(true);
            CanvasGroup canvasGroup = GetOrAddCanvasGroup(uiElement);
            yield return FadeCanvasGroup(canvasGroup, 0f, 1f, fadeInDuration);
        }

        private IEnumerator FadeOutUI(GameObject uiElement)
        {
            if (uiElement == null) yield break;

            CanvasGroup canvasGroup = GetOrAddCanvasGroup(uiElement);
            yield return FadeCanvasGroup(canvasGroup, 1f, 0f, fadeOutDuration);
            uiElement.SetActive(false);
        }

        private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
        {
            CanvasGroup group = obj.GetComponent<CanvasGroup>();
            if (group == null)
            {
                group = obj.AddComponent<CanvasGroup>();
            }
            return group;
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float normalizedTime = elapsedTime / duration;
                group.alpha = Mathf.Lerp(startAlpha, endAlpha, fadeCurve.Evaluate(normalizedTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            group.alpha = endAlpha;
            group.interactable = endAlpha > 0;
            group.blocksRaycasts = endAlpha > 0;
        }

        private void PlaySound(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null || _audioSource == null) return;
            _audioSource.PlayOneShot(clip, volume);
        }
    }
} 