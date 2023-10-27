using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;

    [Header("Properties")]
    [SerializeField] private int dmg;

    private float lifetime;

    public void Init(float force, float lifetime)
    {
        this.lifetime = Time.time + lifetime;

        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.forward * force;
    }

    private void Update()
    {
        if (Time.time >= lifetime)
        {
            Debug.Log("Time's out, dying!");

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(typeof(HealthContainer), out var hc))
        {
            Debug.Log($"Hit something hittable, hit: {other.gameObject.name}");

            ((HealthContainer)hc).TakeDamage(dmg, this);

            Destroy(gameObject);
        }
    }
}