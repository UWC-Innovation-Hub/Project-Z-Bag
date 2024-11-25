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
    public bool isDisplayingItem { get; private set; } = false;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent OnObjectDestroyed;
    #endregion

    #region Private fields
    private readonly float _destroyAfterTime = 3.0f;
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
        isDisplayingItem = true;

        IReadOnlyList<Card> currentlyFlipped = gameManager.CurrentlyFlipped;

        Card currentlyFlippedCard = currentlyFlipped[0];

        foreach (Item item in items)
        {
            if (item.ItemID == currentlyFlippedCard.CardID)
            {
                GameObject displayedItem = (GameObject)Instantiate(item.gameObject, itemSpawnPosition.transform.position, itemSpawnPosition.transform.rotation);
                StartCoroutine(DestroyItem(displayedItem));
            }
        }
        isDisplayingItem = false;
    }

    private IEnumerator DestroyItem(GameObject item)
    {
        yield return new WaitForSeconds(_destroyAfterTime);
        Destroy(item);
        OnObjectDestroyed?.Invoke();
    }
}
