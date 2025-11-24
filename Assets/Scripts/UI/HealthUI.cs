using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    // Reference to the player's stats (for current and max hearts)
    [SerializeField] private PlayerStats playerStats;

    // List of heart image components in the correct order (left to right)
    [SerializeField] private List<Image> heartImages = new List<Image>();

    // Sprite to use when a heart is full
    [SerializeField] private Sprite fullHeartSprite;

    // Sprite to use when a heart is empty
    [SerializeField] private Sprite emptyHeartSprite;

    private void Update()
    {
        if (playerStats == null)
        {
            return;
        }

        UpdateHeartIcons();
    }

    private void UpdateHeartIcons()
    {
        int currentHearts = playerStats.GetCurrentHearts();
        int maxHearts = playerStats.GetMaxHearts();

        // Safety check: do nothing if the lists do not match
        if (heartImages.Count < maxHearts)
        {
            return;
        }

        // For each heart index, decide if it should be full or empty
        for (int i = 0; i < maxHearts; i++)
        {
            bool shouldBeFull = i < currentHearts;

            Image heartImage = heartImages[i];
            heartImage.sprite = shouldBeFull ? fullHeartSprite : emptyHeartSprite;
        }
    }
}
