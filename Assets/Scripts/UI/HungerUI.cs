using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HungerUI : MonoBehaviour
{
    // Reference to the player's stats (for current and max food units)
    [SerializeField] private PlayerStats playerStats;

    // List of food icon images in the correct order (left to right)
    [SerializeField] private List<Image> foodImages = new List<Image>();

    // Sprite used when a food unit is full (colored drumstick)
    [SerializeField] private Sprite fullFoodSprite;

    // Sprite used when a food unit is empty (grey or transparent drumstick)
    [SerializeField] private Sprite emptyFoodSprite;

    private void Update()
    {
        if (playerStats == null)
        {
            return;
        }

        UpdateFoodIcons();
    }

    private void UpdateFoodIcons()
    {
        int currentFoodUnits = playerStats.GetCurrentFoodUnits();
        int maxFoodUnits = playerStats.GetMaxFoodUnits();

        // Safety check: make sure we have enough icons in the list
        if (foodImages.Count < maxFoodUnits)
        {
            return;
        }

        // Decide for each icon if it should be full or empty
        for (int i = 0; i < maxFoodUnits; i++)
        {
            bool shouldBeFull = i < currentFoodUnits;

            Image foodImage = foodImages[i];
            foodImage.sprite = shouldBeFull ? fullFoodSprite : emptyFoodSprite;
        }
    }
}
