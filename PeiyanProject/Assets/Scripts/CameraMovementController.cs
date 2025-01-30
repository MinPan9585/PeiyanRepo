using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Locomotion provider that allows the user to smoothly move their rig continuously over time
    /// using a specified input action. Also includes camera movement based on left hand grip and trigger.
    /// </summary>
    /// <seealso cref="LocomotionProvider"/>
    [AddComponentMenu("XR/Locomotion/Continuous Move Provider (Action-based)", 11)]
    // 注释掉引用 XRHelpURLConstants 的代码
    // [HelpURL(XRHelpURLConstants.k_ActionBasedContinuousMoveProvider)]
    public class CameraMovementController : ContinuousMoveProviderBase
    {
        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the left hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_LeftHandMoveAction = new InputActionProperty(new InputAction("Left Hand Move", expectedControlType: "Vector2"));
        /// <summary>
        /// The Input System Action that Unity uses to read Move data from the left hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty leftHandMoveAction
        {
            get => m_LeftHandMoveAction;
            set => SetInputActionProperty(ref m_LeftHandMoveAction, value);
        }

        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the right hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_RightHandMoveAction = new InputActionProperty(new InputAction("Right Hand Move", expectedControlType: "Vector2"));
        /// <summary>
        /// The Input System Action that Unity uses to read Move data from the right hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty rightHandMoveAction
        {
            get => m_RightHandMoveAction;
            set => SetInputActionProperty(ref m_RightHandMoveAction, value);
        }

        // 新增：用于检测左手grip键的输入动作属性
        [SerializeField]
        [Tooltip("The Input System Action that will be used to detect left hand grip key press.")]
        InputActionProperty m_LeftGripAction = new InputActionProperty(new InputAction("Left Grip", expectedControlType: "Button"));
        public InputActionProperty leftGripAction
        {
            get => m_LeftGripAction;
            set => SetInputActionProperty(ref m_LeftGripAction, value);
        }

        // 新增：用于检测左手扳机键的输入动作属性
        [SerializeField]
        [Tooltip("The Input System Action that will be used to detect left hand trigger key press.")]
        InputActionProperty m_LeftTriggerAction = new InputActionProperty(new InputAction("Left Trigger", expectedControlType: "Button"));
        public InputActionProperty leftTriggerAction
        {
            get => m_LeftTriggerAction;
            set => SetInputActionProperty(ref m_LeftTriggerAction, value);
        }

        // 新增：摄像头移动速度
        [SerializeField]
        [Tooltip("Speed of camera movement.")]
        float m_CameraMoveSpeed = 1f;

        // 新增：主摄像头引用
        Camera m_MainCamera;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            m_LeftHandMoveAction.EnableDirectAction();
            m_RightHandMoveAction.EnableDirectAction();
            // 新增：启用grip键和扳机键的输入动作
            m_LeftGripAction.EnableDirectAction();
            m_LeftTriggerAction.EnableDirectAction();
            // 新增：获取主摄像头
            m_MainCamera = Camera.main;
            if (m_MainCamera == null)
            {
                Debug.LogError("未找到主摄像头，请确保场景中有标记为MainCamera的摄像头。");
            }
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            m_LeftHandMoveAction.DisableDirectAction();
            m_RightHandMoveAction.DisableDirectAction();
            // 新增：禁用grip键和扳机键的输入动作
            m_LeftGripAction.DisableDirectAction();
            m_LeftTriggerAction.DisableDirectAction();
        }

        /// <inheritdoc />
        protected override Vector2 ReadInput()
        {
            var leftHandValue = m_LeftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
            var rightHandValue = m_RightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

            return leftHandValue + rightHandValue;
        }

        void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
        {
            if (Application.isPlaying)
                property.DisableDirectAction();

            property = value;

            if (Application.isPlaying && isActiveAndEnabled)
                property.EnableDirectAction();
        }

        // 新增：更新方法用于处理摄像头移动
        void Update()
        {
            if (m_MainCamera == null) return;
            // 检测左手grip键是否按下
            if (m_LeftGripAction.action.IsPressed())
            {
                m_MainCamera.transform.Translate(Vector3.down * m_CameraMoveSpeed * Time.deltaTime);
            }
            // 检测左手扳机键是否按下
            if (m_LeftTriggerAction.action.IsPressed())
            {
                m_MainCamera.transform.Translate(Vector3.up * m_CameraMoveSpeed * Time.deltaTime);
            }
        }
    }
}