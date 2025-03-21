using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI; // 添加UI命名空间

public class Npc : MonoBehaviour
{
    public string NpcName;
    public bool canChart;
    
    [Header("UI设置")]
    public Image[] promptUIs; // UI提示图片数组
    public float fadeSpeed = 2f; // 渐变速度
    private float targetAlpha; // 目标透明度
    private List<Coroutine> fadeCoroutines; // 用于存储所有正在运行的渐变协程
    
    private void Start()
    {
        fadeCoroutines = new List<Coroutine>();
        // 确保所有UI初始时是隐藏的
        if (promptUIs != null)
        {
            foreach (Image ui in promptUIs)
            {
                if (ui != null)
                {
                    Color c = ui.color;
                    c.a = 0;
                    ui.color = c;
                }
            }
        }
    }

    
    // 新增方法，用于设置NpcName
    public void SetNpcName(string newName)
    {
        NpcName = newName; // 设置新的NPC名称
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canChart = true;
            targetAlpha = 1f; // 设置目标透明度为完全显示
            StartAllFades(); // 开始所有UI的渐变显示
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canChart = false;
            targetAlpha = 0f; // 设置目标透明度为完全隐藏
            StartAllFades(); // 开始所有UI的渐变隐藏
        }
    }

    // 开始所有UI的渐变效果
    private void StartAllFades()
    {
        // 停止所有正在运行的渐变协程
        if (fadeCoroutines != null)
        {
            foreach (Coroutine coroutine in fadeCoroutines)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
            fadeCoroutines.Clear();
        }

        // 为每个UI启动新的渐变协程
        if (promptUIs != null)
        {
            foreach (Image ui in promptUIs)
            {
                if (ui != null)
                {
                    Coroutine newCoroutine = StartCoroutine(FadePromptUI(ui));
                    fadeCoroutines.Add(newCoroutine);
                }
            }
        }
    }

    // 渐变效果协程
    private IEnumerator FadePromptUI(Image ui)
    {
        if (ui == null) yield break;

        while (!Mathf.Approximately(ui.color.a, targetAlpha))
        {
            Color currentColor = ui.color;
            float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            currentColor.a = newAlpha;
            ui.color = currentColor;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canChart)
        {
            Flowchart flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
            if (flowchart.HasBlock(NpcName))
            {
                flowchart.ExecuteBlock(NpcName); // 播放对话
            }
        }
    }
}
