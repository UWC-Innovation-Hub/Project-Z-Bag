using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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

    #region Public fields
    public bool IsDisplayingItem { get; private set; } = false;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent OnObjectDestroyed;
    [HideInInspector] public UnityEvent InstantiateInformationBox;
    #endregion

    #region Private fields
    private GameObject displayedItem;
    private readonly float _destroyAfterTime = 15.0f;
    private int itemID;
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
                InstantiateInformationBox?.Invoke();
            }
        }
    }

    private IEnumerator DestroyItem(GameObject item)
    {
        yield return new WaitForSeconds(_destroyAfterTime);
        Destroy(item);
        IsDisplayingItem = false;
        OnObjectDestroyed?.Invoke();
    }

    public int GetDisplayedItemID()
    {
        return itemID;
    }
}
