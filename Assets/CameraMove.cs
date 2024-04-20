using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 cameraOffset;
    public float smoothFactor = 0.5f;
    public bool lookAtPlayer = false;
    public bool rotateAroundPlayer = true;
    public float rotationSpeed = 5.0f;
    public LayerMask obstructionLayer; // Define which layers count as obstructions

    void Start()
    {
        cameraOffset = transform.position - playerTransform.position;
    }

    void LateUpdate()
    {
        if (rotateAroundPlayer)
        {
            // Horizontal rotation
            Quaternion camTurnAngleHorizontal = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            // Vertical rotation
            Quaternion camTurnAngleVertical = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * rotationSpeed, transform.right);
            // Combine rotations and apply to the camera offset
            cameraOffset = camTurnAngleHorizontal * camTurnAngleVertical * cameraOffset;
        }

        // Update camera position and adjust for obstructions
        Vector3 newPos = playerTransform.position + cameraOffset;
        CheckCameraObstruction(ref newPos);

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        if (lookAtPlayer || rotateAroundPlayer)
            transform.LookAt(playerTransform);
    }

    private void CheckCameraObstruction(ref Vector3 targetPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, targetPos - playerTransform.position, out hit, cameraOffset.magnitude, obstructionLayer))
        {
            targetPos = playerTransform.position + (targetPos - playerTransform.position).normalized * (hit.distance - 0.5f); // Move camera forward slightly before the hit point
        }
    }
}
