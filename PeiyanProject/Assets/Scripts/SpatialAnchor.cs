using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.PXR;

public class SpatialAnchor : MonoBehaviour
{
    // 用于存储创建的锚点的 Handle
    private ulong anchorHandle = 0;

    // 是否已经创建了锚点
    private bool isAnchorCreated = false;

    // 引用 InputAction
    public InputActionProperty gripAction;

    void Update()
    {
        // 检测是否按下了 Grip 按键
        float gripValue = gripAction.action.ReadValue<float>();

        if (gripValue > 0.1f && !isAnchorCreated)
        {
            // 在物体的当前位置创建锚点
            CreateAnchorAtCurrentPosition();
        }
    }

    private void CreateAnchorAtCurrentPosition()
    {
        // 获取物体的当前位置和旋转
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        // 调用 Pico SDK 创建锚点
        PXR_MixedReality.CreateAnchorEntity(position, rotation, out ulong taskId);

        // 监听锚点创建事件
        PXR_Manager.AnchorEntityCreated += OnAnchorEntityCreated;

        Debug.Log("Anchor creation task started with taskId: " + taskId);
    }

    private void OnAnchorEntityCreated(PxrEventAnchorEntityCreated result)
    {
        // 检查锚点是否创建成功
        if (result.result == PxrResult.SUCCESS)
        {
            anchorHandle = result.anchorHandle;
            isAnchorCreated = true;

            Debug.Log("Anchor created successfully with handle: " + anchorHandle);

            // 将物体的父级设置为锚点，确保物体跟随锚点移动
            BindObjectToAnchor(result.anchorHandle);
        }
        else
        {
            Debug.LogError("Failed to create anchor: " + result.result);
        }
    }

    private void BindObjectToAnchor(ulong anchorHandle)
    {
        // 获取锚点的实时姿态
        PXR_MixedReality.GetAnchorPose(anchorHandle, out Quaternion anchorRotation, out Vector3 anchorPosition);

        // 将物体的位置和旋转设置为锚点的位置和旋转
        transform.position = anchorPosition;
        transform.rotation = anchorRotation;

        // 每帧更新物体的位置和旋转，以确保物体始终跟随锚点
        StartCoroutine(UpdateObjectPosition(anchorHandle));
    }

    private System.Collections.IEnumerator UpdateObjectPosition(ulong anchorHandle)
    {
        while (isAnchorCreated)
        {
            // 获取锚点的实时姿态
            PXR_MixedReality.GetAnchorPose(anchorHandle, out Quaternion anchorRotation, out Vector3 anchorPosition);

            // 更新物体的位置和旋转
            transform.position = anchorPosition;
            transform.rotation = anchorRotation;

            yield return new WaitForEndOfFrame();
        }
    }

    void OnDestroy()
    {
        // 取消监听事件
        PXR_Manager.AnchorEntityCreated -= OnAnchorEntityCreated;

        // 如果创建了锚点，销毁它
        if (isAnchorCreated)
        {
            PXR_MixedReality.DestroyAnchorEntity(anchorHandle);
            Debug.Log("Anchor destroyed with handle: " + anchorHandle);
        }
    }
}