using UnityEngine;

public class BeybladeCombat : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isInvulnerable = false;
    private Rigidbody rb;
    private float invulnerabilityDuration = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int damage, Vector3 knockbackDirection)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} dostal {damage} damage. HP: {currentHealth}");

        rb.AddForce(knockbackDirection.normalized * 10f, ForceMode.Impulse);

        StartCoroutine(BecomeInvulnerable());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} byl poražen");

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        MonoBehaviour controller = GetComponent<BeybladeController>();
        if (controller == null)
            controller = GetComponent<BeybladeControllerPlayer2>();

        if (controller != null)
            controller.enabled = false;
    }


    private System.Collections.IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
