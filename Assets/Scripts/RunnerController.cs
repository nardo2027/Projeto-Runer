using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RunnerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float groundCheckDistance = 1.15f;

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezePositionX |
                           RigidbodyConstraints.FreezePositionZ |
                           RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (RunGameManager.Instance == null || !RunGameManager.Instance.IsRunning)
            return;

        if (RunnerInput.JumpPressedThisFrame() && IsGrounded())
            Jump();
    }

    private void Jump()
    {
        Vector3 velocity = body.linearVelocity;
        velocity.y = 0f;
        body.linearVelocity = velocity;
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.05f;
        return Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);
    }
}
