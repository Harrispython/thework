using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    // 要应用闪烁效果的 UI 图片
    [SerializeField]
    private Image _img;

    // 闪烁数据的数组，存储目标值和变化时长
    [SerializeField, Header("最后一组的duration无实际作用！")]
    private BlinkData[] _data;

    // 可切换的材质数组
    [SerializeField]
    private Material _mats;

    // 当前材质的索引
    private int _curMatId;

    // 存储当前正在运行的协程
    private IEnumerator _blinkCoroutine;

    private bool istrue;
    // 当脚本启用时重置材质索引
    private void OnEnable() 
    {
        _curMatId = 0; // 初始化材质索引为第一个材质
    }

    // 当脚本销毁时将材质值复位
    private void OnDestroy() 
    {
        SetValue(0); // 重置材质的相关参数
    }
    // 启动闪烁效果
    public void PlayBlink()
    {
        // 如果已有闪烁协程在运行，停止它
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
        }

        // 启动新的闪烁协程
        _blinkCoroutine = BlinkCoroutine();
        StartCoroutine(_blinkCoroutine);
    }

    // 闪烁效果的协程，依次按照 _data 中的参数进行过渡
    IEnumerator BlinkCoroutine()
    {
        // 设置初始值为数据数组的第一个值
        SetValue(_data[0].Value);

        // 遍历每一段闪烁数据（倒数第二组之前）
        for (int i = 0; i < _data.Length - 1; i++)
        {
            var begin = _data[i].Value;        // 当前段的起始值
            var end = _data[i + 1].Value;     // 下一段的目标值
            var duration = _data[i].Duration; // 当前段的持续时间
            var timer = 0f;                   // 时间计时器

            // 按时间插值计算渐变
            while (timer < duration)
            {
                SetValue(Mathf.Lerp(begin, end, timer / duration)); // 插值计算
                timer += Time.deltaTime; // 更新时间
                yield return null; // 等待下一帧
            }

            // 确保设置到最终目标值
            SetValue(end);
        }
    }

    // 根据当前材质的索引设置值
    public void SetValue(float y)
    {
        if (_curMatId == 0)
        {
            // 如果当前材质为第一个材质，设置 "_Param" 参数
            _img.material.SetVector("_Param", new Vector4(0.8f, y, 1, 1));
        } 
        else if (_curMatId == 1)
        {
            // 如果当前材质为第二个材质，设置 "_Height" 参数
            _img.material.SetFloat("_Height", y);
        }
    }

    // 每帧更新输入检测
    private void Update() 
    {
        
        // 按下空格键启动闪烁效果
        if (!istrue)
        {
            PlayBlink();
            istrue = true;
        }
    }

    // 用于存储闪烁数据的类
    [System.Serializable]
    private class BlinkData
    {
        public float Value;    // 目标值
        public float Duration; // 过渡时间
    }
}
