using System.Collections;
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

    #region Public properties
    public bool IsDisplayingItem { get; private set; } = false;
    #endregion

    #region Private fields
    private GameObject displayedItem;
    private readonly float _destroyAfterTime = 7.0f;
    private int itemID;
    #endregion

    private void OnEnable()
    {
        GameEvents.OnMatchFound += DisplayItem;
        GameEvents.OnCardSpawn += AssignItemID;
    }

    private void OnDisable()
    {
        GameEvents.OnMatchFound -= DisplayItem;
        GameEvents.OnCardSpawn -= AssignItemID;
    }

    private void AssignItemID(object sender, System.EventArgs e)
    {
        IReadOnlyDictionary<int, List<Card>> cardPairs = cardManager.CardPairs;
        int index = 0;

        foreach (int key in cardPairs.Keys)
        {
            items[index].Initialize(key);
            index++;
        }
    }

    private void DisplayItem(object sender, System.EventArgs e)
    {
        DisplayItem();
    }

    // Search the list for the item with the ItemID that matches the currently flipped cards CardID
    private void DisplayItem()
    {
        IsDisplayingItem = true;

        IReadOnlyList<Card> currentlyFlipped = gameManager.CurrentlyFlipped;

        Card currentlyFlippedCard = currentlyFlipped[0];

        foreach (Item item in items)
        {
            if (item.ItemID == currentlyFlippedCard.CardID)
            {
                displayedItem = (GameObject)Instantiate(item.gameObject, itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation);
                StartCoroutine(DestroyItem(displayedItem));
                itemID = item.ItemID;
                GameEvents.TriggerInformationBoxInstantiation(item);
                cardManager.RemoveCard(currentlyFlippedCard.CardID);
                GameEvents.TriggerHideCards();
            }
        }
    }

    private IEnumerator DestroyItem(GameObject item)
    {
        yield return new WaitForSeconds(_destroyAfterTime);
        Destroy(item);
        IsDisplayingItem = false;
        GameEvents.TriggerOnDestroyedItem(item);
        GameEvents.TriggerUnhideCards();
    }

    public int GetDisplayedItemID()
    {
        return itemID;
    }
}
