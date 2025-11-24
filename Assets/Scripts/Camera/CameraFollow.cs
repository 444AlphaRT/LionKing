using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Target for the camera to follow (Simba)
    [SerializeField] private Transform target;

    // How fast the camera moves towards the target position
    [SerializeField] private float followSpeed = 5f;

    // Offset between the camera and the target
    private Vector3 offset;

    private void Start()
    {
        // If a target is assigned, calculate the initial offset
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    private void LateUpdate()
    {
        // Do nothing if there is no target assigned
        if (target == null)
        {
            return;
        }

        // Desired position is the target position plus the stored offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }

    // Optional helper to set the target from other scripts if needed
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // Recalculate the offset when a new target is assigned
        offset = transform.position - target.position;
    }
}
