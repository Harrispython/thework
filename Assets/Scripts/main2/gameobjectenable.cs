using UnityEngine;

public class gameobjectenable : MonoBehaviour
{
    // 创建一个结构体来存储每个对象的状态
    [System.Serializable]
    public struct ObjectState
    {
        public GameObject targetObject; // 目标对象
        public bool isEnabled;         // 是否启用
    }

    // 使用数组存储所有对象的状态
    [SerializeField] 
    private ObjectState[] objectStates;

    // 外部调用方法,可以控制单个对象
    public void EnableSingle(int index, bool target)
    {
        if(index >= 0 && index < objectStates.Length)
        {
            objectStates[index].isEnabled = target;
            objectStates[index].targetObject.SetActive(target);
        }
    }

    // 外部调用方法,控制所有对象
    public void EnableAll(bool target)
    {
        // 使用 for 循环代替 foreach
        for(int i = 0; i < objectStates.Length; i++)
        {
            objectStates[i].isEnabled = target;
            objectStates[i].targetObject.SetActive(target);
        }
    }

    // 获取指定对象的状态
    public bool GetObjectState(int index)
    {
        if(index >= 0 && index < objectStates.Length)
        {
            return objectStates[index].isEnabled;
        }
        return false;
    }

    // 外部设置所有对象的状态
    public void enableStart()
    {
        // 根据初始设置的状态激活/禁用对象
        for(int i = 0; i < objectStates.Length; i++)
        {
            objectStates[i].targetObject.SetActive(objectStates[i].isEnabled);
            objectStates[i].isEnabled = !objectStates[i].isEnabled;
        }

    }
}