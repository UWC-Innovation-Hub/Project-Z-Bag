using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private List<Item> items = new();
    [SerializeField] private GameObject itemSpawnPosition;

    private void Start()
    {
        cardManager.OnMatchFound.AddListener(DisplayObject);
        AssignItemID();
        foreach (Item item in items)
            Debug.Log($"ItemID: {item.ItemID}");
    }

    // Assign the Item ID based on the card ID
    private void AssignItemID()
    {
        IReadOnlyDictionary<int, List<Card>> cardPairs = cardManager.CardPairs;
        int index = 0;

        Debug.Log($"Dict count: {cardPairs.Count}");

        foreach (int key in cardPairs.Keys)
        {
            Debug.Log($"Dict keys: {key}");
            items[index].Initialize(key);
            index++;
        }

    }

    private void DisplayObject()
    {
        IReadOnlyList<Card> currentlyFlipped = cardManager.CurrentlyFlipped;

        foreach (Item item in items)
        {
            if (item.ItemID == currentlyFlipped[0].CardID)
            {
                Instantiate(item, itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation);
            }
        }
    }
}
