using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BeybladeController : MonoBehaviour
{
    [Header("beyblade logic")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float rotationSpeed = 500f;
    public float tiltAngle = 15f;
    public float airTiltAmount = 10f;
    public float tiltSmoothness = 5f;

    private Rigidbody rb;
    private bool isGrounded;

    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    [System.Obsolete]
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        HandleMovement();
        HandleJump();
        ApplySpin();
    }

    [System.Obsolete]
    void HandleMovement()
    {
        Vector3 velocity = rb.velocity; 
        velocity.x = horizontalInput * moveSpeed;
        rb.velocity = velocity;

        rb.angularVelocity = new Vector3(0, -rotationSpeed * Time.deltaTime, 0);

        float targetTilt = horizontalInput * -tiltAngle;
        if (!isGrounded)
        {
            targetTilt = Mathf.Sin(Time.time * 3f) * airTiltAmount;
        }

        Quaternion targetRotation = Quaternion.Euler(targetTilt, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSmoothness);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void ApplySpin()
    {
        transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
