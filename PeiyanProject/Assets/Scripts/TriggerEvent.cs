using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 当物体进入浮空岛时调用
    }

    private void OnTriggerExit(Collider other)
    {
        // 当物体离开浮空岛时调用
    }
}