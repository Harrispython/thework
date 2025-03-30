using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typeSpeed = 0.05f; // 打字速度
    [SerializeField] private float fadeInSpeed = 1f; // 渐现速度
    [SerializeField] private TextMeshProUGUI displayText; // 显示文本的UI组件
    [SerializeField] private string[] introTexts; // 要显示的介绍文本数组
    [SerializeField] private bool isFadeMode = false; // 是否使用渐现模式
    
    private void Start()
    {
        if (isFadeMode)
        {
            StartCoroutine(FadeInText());
        }
        else
        {
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        foreach (string text in introTexts)
        {
            displayText.text = ""; // 清空文本
            string[] lines = text.Split('\n'); // 分割换行
            
            foreach (string line in lines)
            {
                foreach (char letter in line)
                {
                    displayText.text += letter; // 逐字添加
                    yield return new WaitForSeconds(typeSpeed);
                }
                displayText.text += "\n"; // 添加换行符
                yield return new WaitForSeconds(typeSpeed * 2); // 换行时稍微多等待一下
            }
            yield return new WaitForSeconds(1f); // 每段文本显示完后等待1秒
        }
    }

    // 新添加的渐现效果方法
    private IEnumerator FadeInText()
    {
        string fullText = string.Join("\n\n", introTexts);
        displayText.text = fullText;
        
        // 初始设置文本完全透明
        displayText.color = new Color(displayText.color.r, 
                                    displayText.color.g, 
                                    displayText.color.b, 
                                    0);
        
        // gradually fade in
        while (displayText.color.a < 1.0f)
        {
            Color newColor = displayText.color;
            newColor.a += Time.deltaTime * fadeInSpeed;
            displayText.color = newColor;
            yield return null;
        }
    }

    // 修改跳过方法以支持渐现模式
    public void SkipTyping()
    {
        StopAllCoroutines();
        string fullText = string.Join("\n\n", introTexts);
        displayText.text = fullText;
        
        // 如果是渐现模式，直接设置为完全不透明
        if (isFadeMode)
        {
            displayText.color = new Color(displayText.color.r, 
                                        displayText.color.g, 
                                        displayText.color.b, 
                                        1);
        }
    }
} 