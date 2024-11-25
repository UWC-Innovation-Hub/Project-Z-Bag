using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private ItemManager itemManager;
    #endregion

    #region Public Fields
    public bool IsChecking { get; private set; } = false; // Prevent input during checking
    #endregion

    #region Private Fields
    // Tracks the currently flipped cards
    private readonly List<Card> _currentlyFlipped = new();
    private int _score = 0;
    #endregion

    #region Properties
    // Read-only access to currently flipped cards
    public IReadOnlyList<Card> CurrentlyFlipped => _currentlyFlipped;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent OnMatchFound;
    #endregion

    private void Start()
    {
        itemManager.OnObjectDestroyed.AddListener(GameOver);
    }

    // Adds the currently flipped card to the list. Notified by the Card.
    public void OnCardFlipped(Card flippedCard)
    {
        Debug.Log("Checking if two cards are flipped");
        _currentlyFlipped.Add(flippedCard);

        if (_currentlyFlipped.Count == 2)
        {
            IsChecking = true;
            StartCoroutine(CheckForMatch(_currentlyFlipped[0], _currentlyFlipped[1]));
        }
    }

    // Check if the two currently flipped cards having matching card ID's
    private IEnumerator CheckForMatch(Card firstCard, Card secondCard)
    {
        yield return new WaitForSeconds(0.5f); // Delay for better UI

        if (firstCard.CardID == secondCard.CardID)
        {
            Debug.Log("Match found!");
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
            _score++;
            OnMatchFound.Invoke();
        }
        else
        {
            Debug.Log("No match!");
            firstCard.FlipCardExternally();
            secondCard.FlipCardExternally();
        }

        _currentlyFlipped.Clear();
        IsChecking = false;
    }

    private void GameOver()
    {
        Debug.Log($"Score: {_score}");
        if (_score != 6)
            return;
        Debug.Log($"Game Over");
        gameOverPanel.SetActive(true);
    }
}
