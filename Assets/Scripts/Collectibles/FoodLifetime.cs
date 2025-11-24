using UnityEngine;

public class FoodLifetime : MonoBehaviour
{
    // Minimum time before this food object disappears
    [SerializeField] private float minLifetimeSeconds = 5f;

    // Maximum time before this food object disappears
    [SerializeField] private float maxLifetimeSeconds = 10f;

    private void OnEnable()
    {
        // Choose a random lifetime between the configured limits
        float lifetime = Random.Range(minLifetimeSeconds, maxLifetimeSeconds);

        // Destroy this object after the chosen lifetime
        Destroy(gameObject, lifetime);
    }
}