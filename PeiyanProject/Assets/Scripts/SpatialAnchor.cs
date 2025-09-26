using UnityEngine;
using Unity.XR.PXR;
using UnityEngine.InputSystem; 

public class SpatialAnchor : MonoBehaviour
{
    [Header("�� Ҫ��ס������")]
    public Transform target;

    [Header("�� �����ֱ� Transform")]
    public Transform rightHand;

    [Header("�� A��  InputActionReference")]
    public InputActionReference aButton;   // �� .inputactions ��� A �Ͻ���

    private ulong anchorHandle = 0;
    private bool following = false;

    /*---------- �������� ----------*/

    private void Start()
    {
        CreateAnchorAt(target.position, target.rotation);
    }

    private void OnEnable()
    {
        // ���� A ��
        aButton.action.performed += OnAPressed;
        PXR_Manager.AnchorEntityCreated += OnCreated;
    }

    private void OnDisable()
    {
        aButton.action.performed -= OnAPressed;
        PXR_Manager.AnchorEntityCreated -= OnCreated;
    }

    /*---------- A ���ص� ----------*/

    private void OnAPressed(InputAction.CallbackContext _)
    {
        if (anchorHandle != 0)
        {
            DestroyAnchor();
            following = true;
        }
        else
        {
            CreateAnchorAt(rightHand.position, rightHand.rotation);
            following = false;
        }
    }

    /*---------- ÿ֡ͬ�� ----------*/

    private void LateUpdate()
    {
        if (following && rightHand != null)
        {
            target.position = rightHand.position;
            target.rotation = rightHand.rotation;
        }
        else if (anchorHandle != 0)
        {
            if (PXR_MixedReality.GetAnchorPose(anchorHandle, out Quaternion rot, out Vector3 pos)
                == PxrResult.SUCCESS)
            {
                target.position = pos;
                target.rotation = rot;
            }
        }
    }

    /*---------- ê�㴴��/���� ----------*/

    private void CreateAnchorAt(Vector3 pos, Quaternion rot)
    {
        ulong taskId;
        PxrResult result = PXR_MixedReality.CreateAnchorEntity(pos, rot, out taskId);
        if (result == PxrResult.SUCCESS)
            Debug.Log($"���������ѷ� taskId={taskId}");
        else
            Debug.LogError($"CreateAnchorEntity ʧ�� {result}");
    }

    private void DestroyAnchor()
    {
        if (anchorHandle == 0) return;
        if (PXR_MixedReality.DestroyAnchorEntity(anchorHandle) == PxrResult.SUCCESS)
        {
            Debug.Log($"ê�������� handle={anchorHandle}");
            anchorHandle = 0;
        }
    }

    /*---------- �¼��ص� ----------*/

    private void OnCreated(PxrEventAnchorEntityCreated e)
    {
        if (e.result == PxrResult.SUCCESS)
        {
            anchorHandle = e.anchorHandle;
            Debug.Log($"ê�㴴���ɹ� handle={anchorHandle}");
        }
        else
        {
            Debug.LogError($"ê�㴴��ʧ�� result={e.result}");
        }
    }
}