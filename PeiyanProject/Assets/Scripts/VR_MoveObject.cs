using UnityEngine;
using UnityEngine.InputSystem;


public class VR_MoveObject : MonoBehaviour
{

    public InputActionReference leftGripAction;
    public InputActionReference leftTriggerAction;
    public float moveSpeed = 1f;

    private void OnEnable()
    {
        leftGripAction.action.Enable();
        leftTriggerAction.action.Enable();
    }

    private void OnDisable()
    {
        leftGripAction.action.Disable();
        leftTriggerAction.action.Disable();
    }

    private void Update()
    {
        if (leftGripAction.action.IsPressed())
        {
            MoveObject(Vector3.down);
        }
        else if (leftTriggerAction.action.IsPressed())
        {
            MoveObject(Vector3.up);
        }
    }

    private void MoveObject(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}