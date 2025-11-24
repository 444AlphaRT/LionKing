using UnityEngine;

public class FoodCollectible : MonoBehaviour
{
    // How many food units this pickup grants to the player
    [SerializeField] private int foodUnitsGranted = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react when the player touches this object
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            return;
        }

        // Add food to the player
        playerStats.AddFoodUnits(foodUnitsGranted);

        // Remove this food item from the world
        Destroy(gameObject);
    }
}