using UnityEngine;
using Unity.XR.PXR;
using UnityEngine.InputSystem; 

public class SpatialAnchor : MonoBehaviour
{
    [Header("① 要钉住的物体")]
    public Transform target;

    [Header("② 右手手柄 Transform")]
    public Transform rightHand;

    [Header("③ A键  InputActionReference")]
    public InputActionReference aButton;   // 把 .inputactions 里的 A 拖进来

    private ulong anchorHandle = 0;
    private bool following = false;

    /*---------- 生命周期 ----------*/

    private void Start()
    {
        CreateAnchorAt(target.position, target.rotation);
    }

    private void OnEnable()
    {
        // 监听 A 键
        aButton.action.performed += OnAPressed;
        PXR_Manager.AnchorEntityCreated += OnCreated;
    }

    private void OnDisable()
    {
        aButton.action.performed -= OnAPressed;
        PXR_Manager.AnchorEntityCreated -= OnCreated;
    }

    /*---------- A 键回调 ----------*/

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

    /*---------- 每帧同步 ----------*/

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

    /*---------- 锚点创建/销毁 ----------*/

    private void CreateAnchorAt(Vector3 pos, Quaternion rot)
    {
        ulong taskId;
        PxrResult result = PXR_MixedReality.CreateAnchorEntity(pos, rot, out taskId);
        if (result == PxrResult.SUCCESS)
            Debug.Log($"创建请求已发 taskId={taskId}");
        else
            Debug.LogError($"CreateAnchorEntity 失败 {result}");
    }

    private void DestroyAnchor()
    {
        if (anchorHandle == 0) return;
        if (PXR_MixedReality.DestroyAnchorEntity(anchorHandle) == PxrResult.SUCCESS)
        {
            Debug.Log($"锚点已销毁 handle={anchorHandle}");
            anchorHandle = 0;
        }
    }

    /*---------- 事件回调 ----------*/

    private void OnCreated(PxrEventAnchorEntityCreated e)
    {
        if (e.result == PxrResult.SUCCESS)
        {
            anchorHandle = e.anchorHandle;
            Debug.Log($"锚点创建成功 handle={anchorHandle}");
        }
        else
        {
            Debug.LogError($"锚点创建失败 result={e.result}");
        }
    }
}