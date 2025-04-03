using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.PXR;

public class SpatialAnchor : MonoBehaviour
{
    // ���ڴ洢������ê��� Handle
    private ulong anchorHandle = 0;

    // �Ƿ��Ѿ�������ê��
    private bool isAnchorCreated = false;

    // ���� InputAction
    public InputActionProperty gripAction;

    void Update()
    {
        // ����Ƿ����� Grip ����
        float gripValue = gripAction.action.ReadValue<float>();

        if (gripValue > 0.1f && !isAnchorCreated)
        {
            // ������ĵ�ǰλ�ô���ê��
            CreateAnchorAtCurrentPosition();
        }
    }

    private void CreateAnchorAtCurrentPosition()
    {
        // ��ȡ����ĵ�ǰλ�ú���ת
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        // ���� Pico SDK ����ê��
        PXR_MixedReality.CreateAnchorEntity(position, rotation, out ulong taskId);

        // ����ê�㴴���¼�
        PXR_Manager.AnchorEntityCreated += OnAnchorEntityCreated;

        Debug.Log("Anchor creation task started with taskId: " + taskId);
    }

    private void OnAnchorEntityCreated(PxrEventAnchorEntityCreated result)
    {
        // ���ê���Ƿ񴴽��ɹ�
        if (result.result == PxrResult.SUCCESS)
        {
            anchorHandle = result.anchorHandle;
            isAnchorCreated = true;

            Debug.Log("Anchor created successfully with handle: " + anchorHandle);

            // ������ĸ�������Ϊê�㣬ȷ���������ê���ƶ�
            BindObjectToAnchor(result.anchorHandle);
        }
        else
        {
            Debug.LogError("Failed to create anchor: " + result.result);
        }
    }

    private void BindObjectToAnchor(ulong anchorHandle)
    {
        // ��ȡê���ʵʱ��̬
        PXR_MixedReality.GetAnchorPose(anchorHandle, out Quaternion anchorRotation, out Vector3 anchorPosition);

        // �������λ�ú���ת����Ϊê���λ�ú���ת
        transform.position = anchorPosition;
        transform.rotation = anchorRotation;

        // ÿ֡���������λ�ú���ת����ȷ������ʼ�ո���ê��
        StartCoroutine(UpdateObjectPosition(anchorHandle));
    }

    private System.Collections.IEnumerator UpdateObjectPosition(ulong anchorHandle)
    {
        while (isAnchorCreated)
        {
            // ��ȡê���ʵʱ��̬
            PXR_MixedReality.GetAnchorPose(anchorHandle, out Quaternion anchorRotation, out Vector3 anchorPosition);

            // ���������λ�ú���ת
            transform.position = anchorPosition;
            transform.rotation = anchorRotation;

            yield return new WaitForEndOfFrame();
        }
    }

    void OnDestroy()
    {
        // ȡ�������¼�
        PXR_Manager.AnchorEntityCreated -= OnAnchorEntityCreated;

        // ���������ê�㣬������
        if (isAnchorCreated)
        {
            PXR_MixedReality.DestroyAnchorEntity(anchorHandle);
            Debug.Log("Anchor destroyed with handle: " + anchorHandle);
        }
    }
}