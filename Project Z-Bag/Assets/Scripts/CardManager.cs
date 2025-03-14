using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Managers & Prefabs")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardPrefab;

    [Header("Spawn Configuration")]
    [SerializeField] private Transform cardSpawnPosition;

    [Header("Materials")]
    [SerializeField] private Material[] materials;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent OnCardSpawn;
    [HideInInspector] public UnityEvent StartTimer;
    #endregion

    #region Private Variables
    // Card data
    private readonly Dictionary<int, List<Card>> _cardPairs = new();

    // Positioning
    private Vector3 _startPosition = new(-1.5f, 1.75f, 0f);
    private Vector3 _offset = new(1.0f, 1.0f);
    #endregion

    #region Public Properties
    // Expose cardPairs as a read-only property
    public IReadOnlyDictionary<int, List<Card>> CardPairs => _cardPairs;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        SpawnCardMesh(3, 4);
        MoveCard(3, 4, _startPosition, _offset);
    }

    private void OnEnable()
    {
        GameEvents.OnHideCards += HideCards;
        GameEvents.OnUnhideCards += UnhideCards;
    }

    private void OnDisable()
    {
        GameEvents.OnHideCards -= HideCards;
        GameEvents.OnUnhideCards -= UnhideCards;
    }

    // Spawns a collection of cards at the spawn position and then stores it in a list
    private void SpawnCardMesh(int rows, int columns)
    {
        List<int> cardIDs = GenerateCardIDs(rows * columns);

        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardSpawnPosition.position, cardSpawnPosition.transform.rotation);
            Card card = cardObject.GetComponent<Card>();
            card.Initialise(cardIDs[i], gameManager);

            if (!_cardPairs.ContainsKey(cardIDs[i]))
                _cardPairs[cardIDs[i]] = new List<Card>();

            card.name = $"Card {i}";
            _cardPairs[cardIDs[i]].Add(card);
        }

        AssignMaterialsToCardPairs(_cardPairs);
        GameEvents.TriggerAssignItemID();
    }

    // Generates a list of cardIDs
    private List<int> GenerateCardIDs(int totalCards)
    {
        List<int> cardIDs = new();

        for (int i = 0; i < totalCards / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        return cardIDs;
    }

    // Assign materials to the cards ensuring that the cards with the same ID get's the same material
    private void AssignMaterialsToCardPairs(Dictionary<int, List<Card>> cardPairs)
    {
        if (materials == null || materials.Length == 0)
        {
            Debug.LogError("Materials array is empty or not assigned.");
            return;
        }

        int materialCount = materials.Length;
        int index = 0;

        foreach (KeyValuePair<int, List<Card>> kvp in cardPairs)
        {
            // Cycle through materials using modulus to ensure we stay within bounds of the array
            Material materialForKey = materials[index % materialCount];

            foreach (Card card in kvp.Value)
            {
                if (card == null && card.cardVisualBottom == null)
                    return;

                if (card.cardVisualBottom.TryGetComponent<Renderer>(out var renderer))
                {
                    renderer.material = materialForKey; // Assign the material to the card's visual
                }
            }

            index++; // Move to the next material for the next key
        }
    }

    // Moves the card from the spawned position into the grid format
    private void MoveCard(int rows, int columns, Vector3 startPosition, Vector3 offset)
    {
        List<Card> cardList = FlattenCardPairs();
        ShuffleCards(cardList);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                if (index >= cardList.Count)
                    return; // Exit early if out of cards

                Vector3 targetPosition = startPosition + new Vector3(offset.x * j, 0, -offset.y * i);
                StartCoroutine(MoveToPosition(targetPosition, cardList[index].gameObject));

                // Apply rotation immediately for all cards
                cardList[index].transform.rotation = Quaternion.Euler(-15f, 0f, 0f);
            }
        }
        GameEvents.TriggerGameTimerStart();
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, GameObject card)
    {
        float speed = 7f;

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
            card.transform.position = Vector3.MoveTowards(card.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        card.transform.position = targetPosition; // Snap to the target position at the end
    }

    // Uses the Fisher-Yates shuffle
    private void ShuffleCards<T>(List<T> cardList)
    {
        for (int i = cardList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1); // Random index in the range [0, i]
            T temp = cardList[i];
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = temp;
        }
    }

    // Flattens the dictionary so that each card can be moved better
    public List<Card> FlattenCardPairs()
    {
        List<Card> cardList = new();
        foreach (var pair in _cardPairs.Values)
        {
            cardList.AddRange(pair);
        }
        return cardList;
    }

    private void HideCards(object sender, System.EventArgs e)
    {
        List<Card> cards = FlattenCardPairs();

        foreach (Card card in cards)
        {
            card.gameObject.SetActive(false);
        }
    }

    private void UnhideCards(object sender, System.EventArgs e)
    {
        List<Card> cards = FlattenCardPairs();

        foreach (Card card in cards)
        {
            card.gameObject.SetActive(true);
        }
    }

    public void RemoveCard(int id)
    {
        _cardPairs.Remove(id);
    }
}
