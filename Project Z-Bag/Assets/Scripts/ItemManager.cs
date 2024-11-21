using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Managers")]
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameManager gameManager;

    [Header("Spawn Configuration")]
    [SerializeField] private GameObject itemSpawnPosition;

    [Header("Items")]
    [SerializeField] private List<Item> items = new();
    #endregion

    private void Start()
    {
        gameManager.OnMatchFound.AddListener(DisplayItem);
        cardManager.OnCardSpawn.AddListener(AssignItemID);
    }

    // Assign the ItemID based on the card CardID's found in the dictionary 
    private void AssignItemID()
    {
        IReadOnlyDictionary<int, List<Card>> cardPairs = cardManager.CardPairs;
        int index = 0;

        foreach (int key in cardPairs.Keys)
        {
            items[index].Initialize(key);
            index++;
        }

    }

    // Search the list for the item with the ItemID that matches the currently flipped cards CardID
    private void DisplayItem()
    {
        IReadOnlyList<Card> currentlyFlipped = gameManager.CurrentlyFlipped;

        Card currentlyFlippedCard = currentlyFlipped[0];

        foreach (Item item in items)
        {
            if (item.ItemID == currentlyFlippedCard.CardID)
            {
                Instantiate(item, itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation);
            }
        }
    }
}
