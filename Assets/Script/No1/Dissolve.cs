using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Dissolve : MonoBehaviour
    {
        // 存储所有子物体材质的列表
        List<Material> materials = new List<Material>();
        private float currentValue = 0f; // 当前溶解值
        private float targetValue = 1f; // 目标溶解值
        private float dissolveSpeed = 0.5f; // 溶解速度
        private bool isDissolving = false; // 是否正在溶解

        void Start()
        {
            // 获取所有子物体的渲染器
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }

            // 在Start中获取当前溶解值并开始溶解
            currentValue = GetCurrentDissolveValue();
            isDissolving = true;
        }

        // 当脚本被启用时调用
        private void OnEnable()
        {
            // 确保材质列表已初始化
            if (materials.Count == 0)
            {
                Start();
            }

            // 启用时开始溶解
            currentValue = GetCurrentDissolveValue();
            isDissolving = true;
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        void Update()
        {
            if (isDissolving)
            {
                // 逐渐增加当前值到目标值
                currentValue = Mathf.MoveTowards(currentValue, targetValue, dissolveSpeed * Time.deltaTime);
                SetValue(currentValue);

                // 当达到目标值时停止溶解
                if (Mathf.Approximately(currentValue, targetValue))
                {
                    isDissolving = false;
                }
            }
        }

        // 获取当前溶解值的辅助方法
        private float GetCurrentDissolveValue()
        {
            if (materials.Count > 0)
            {
                return materials[0].GetFloat("_Dissolve");
            }

            return 0f;
        }

        // 设置溶解值的方法
        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
        // ... existing code ...
    }
