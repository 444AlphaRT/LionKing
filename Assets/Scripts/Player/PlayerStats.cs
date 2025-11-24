using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    // Maximum number of hearts the player can have
    [SerializeField] private int maxHearts = 5;

    // Current number of hearts
    [SerializeField] private int currentHearts = 5;

    // Amount of hearts lost each time starvation damage is applied
    [SerializeField] private int heartsLostPerStarvationTick = 1;

    [Header("Food (Drumsticks)")]
    // Maximum number of food units (drumsticks)
    [SerializeField] private int maxFoodUnits = 5;

    // Current number of food units
    [SerializeField] private int currentFoodUnits = 5;

    // How many seconds it takes to lose one food unit over time
    [SerializeField] private float secondsPerFoodUnit = 10f;

    // How many seconds between heart losses when food is empty
    [SerializeField] private float secondsPerStarvationTick = 2f;

    // Internal timers for hunger and starvation
    private float foodTimer = 0f;
    private float starvationTimer = 0f;

    [Header("Health Regeneration From Food")]
    // Enable or disable automatic health regeneration using food
    [SerializeField] private bool enableHealthRegenFromFood = true;

    // Time between two regeneration steps
    [SerializeField] private float healthRegenIntervalSeconds = 3f;

    // How many food units are consumed to restore one heart
    [SerializeField] private int foodUnitsPerRegenHeart = 2;

    // Internal timer for health regeneration
    private float healthRegenTimer = 0f;

    [Header("Food Statistics")]
    // Total number of food units the player has collected during the run
    [SerializeField] private int totalFoodCollected = 0;

    private void Awake()
    {
        // Start with full health and full food
        currentHearts = maxHearts;
        currentFoodUnits = maxFoodUnits;

        foodTimer = 0f;
        starvationTimer = 0f;
        healthRegenTimer = 0f;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        UpdateFoodAndStarvation(deltaTime);
        UpdateHealthRegeneration(deltaTime);
    }

    private void UpdateFoodAndStarvation(float deltaTime)
    {
        // If the player still has food, it slowly decreases over time
        if (currentFoodUnits > 0)
        {
            foodTimer += deltaTime;

            if (foodTimer >= secondsPerFoodUnit)
            {
                foodTimer -= secondsPerFoodUnit;
                currentFoodUnits--;

                if (currentFoodUnits < 0)
                {
                    currentFoodUnits = 0;
                }
            }

            // While there is food, starvation does not progress
            starvationTimer = 0f;
        }
        else
        {
            // No food left: hearts start to drop over time
            starvationTimer += deltaTime;

            if (starvationTimer >= secondsPerStarvationTick)
            {
                starvationTimer -= secondsPerStarvationTick;
                TakeDamage(heartsLostPerStarvationTick);
            }
        }
    }

    private void UpdateHealthRegeneration(float deltaTime)
    {
        // Regeneration is optional and only works when enabled
        if (!enableHealthRegenFromFood)
        {
            return;
        }

        // Do not regenerate if the player is already dead
        if (IsDead())
        {
            return;
        }

        // No need to regenerate if health is already full
        if (currentHearts >= maxHearts)
        {
            return;
        }

        // Not enough food to pay for a regeneration step
        if (currentFoodUnits < foodUnitsPerRegenHeart)
        {
            return;
        }

        // Count time towards the next regeneration step
        healthRegenTimer += deltaTime;

        if (healthRegenTimer < healthRegenIntervalSeconds)
        {
            return;
        }

        // Time to regenerate one heart
        healthRegenTimer -= healthRegenIntervalSeconds;

        // Consume food for the regeneration
        currentFoodUnits -= foodUnitsPerRegenHeart;

        if (currentFoodUnits < 0)
        {
            currentFoodUnits = 0;
        }

        // Restore one heart
        currentHearts++;

        if (currentHearts > maxHearts)
        {
            currentHearts = maxHearts;
        }
    }

    // Called when something damages the player (enemy, trap, starvation)
    public void TakeDamage(int amount)
    {
        currentHearts -= amount;

        if (currentHearts < 0)
        {
            currentHearts = 0;
        }
    }

    // Called when Simba eats food from the world
    public void AddFoodUnits(int amount)
    {
        currentFoodUnits += amount;

        if (currentFoodUnits > maxFoodUnits)
        {
            currentFoodUnits = maxFoodUnits;
        }

        // Reset hunger timers because the player just ate
        foodTimer = 0f;
        starvationTimer = 0f;

        // Update statistics for collected food
        totalFoodCollected += amount;
    }

    // Optional helper to completely fill the food bar
    public void RefillFoodBar()
    {
        AddFoodUnits(maxFoodUnits);
    }

    // === Getters for UI and statistics ===

    public int GetMaxHearts()
    {
        return maxHearts;
    }

    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    public int GetMaxFoodUnits()
    {
        return maxFoodUnits;
    }

    public int GetCurrentFoodUnits()
    {
        return currentFoodUnits;
    }

    public bool IsDead()
    {
        return currentHearts <= 0;
    }

    public int GetTotalFoodCollected()
    {
        return totalFoodCollected;
    }
}