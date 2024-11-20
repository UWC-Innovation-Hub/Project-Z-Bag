using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject itemSpawnPosition;
    [SerializeField] private List<Item> items = new();

    private void Start()
    {
        cardManager.OnMatchFound.AddListener(DisplayItem);
        AssignItemID();
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
        IReadOnlyList<Card> currentlyFlipped = cardManager.CurrentlyFlipped;

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
