using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BeybladeControllerPlayer2 : MonoBehaviour
{
    [Header("beyblade logic")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;
    public float tiltAngle = 15f;
    public float airTiltAmount = 10f;
    public float tiltSmoothness = 5f;

    private Rigidbody rb;
    private bool isGrounded;

    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    [System.Obsolete]
    void Update()
    {
        horizontalInput = 0f;
        verticalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f;

        if (Input.GetKey(KeyCode.UpArrow)) verticalInput = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) verticalInput = -1f;

        HandleMovement();
        ApplySpin();
    }

    [System.Obsolete]
    void HandleMovement()
    {
        Vector3 velocity = rb.velocity;
        velocity.x = horizontalInput * moveSpeed;
        velocity.z = verticalInput * moveSpeed;
        rb.velocity = velocity;

        rb.angularVelocity = new Vector3(0, -rotationSpeed * Time.deltaTime, 0);

        float tiltX = verticalInput * tiltAngle;
        float tiltZ = horizontalInput * -tiltAngle;

        if (!isGrounded)
        {
            tiltX = Mathf.Sin(Time.time * 3f) * airTiltAmount;
            tiltZ = Mathf.Cos(Time.time * 3f) * airTiltAmount;
        }

        Quaternion targetRotation = Quaternion.Euler(tiltX, transform.rotation.eulerAngles.y, tiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSmoothness);
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
            return;
        }

        if (collision.gameObject.CompareTag("Beyblade"))
        {
            var otherCombat = collision.gameObject.GetComponent<BeybladeCombat>();
            var selfCombat = GetComponent<BeybladeCombat>();

            if (otherCombat != null && selfCombat != null)
            {
                if (Random.value > 0.5f && !otherCombat.IsInvulnerable())
                {
                    Vector3 knockback = collision.transform.position - transform.position;
                    otherCombat.TakeDamage(Random.Range(5, 15), knockback);
                }
                else if (!selfCombat.IsInvulnerable())
                {
                    Vector3 knockback = transform.position - collision.transform.position;
                    selfCombat.TakeDamage(Random.Range(5, 15), knockback);
                }
            }
        }
    }

}
