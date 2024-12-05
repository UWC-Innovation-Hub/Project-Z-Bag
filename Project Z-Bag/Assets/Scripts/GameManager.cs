using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Managers")]
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private CardManager cardManager;

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game Objects")]
    [SerializeField] private GameObject gameOverPanel;
    #endregion

    #region Public Fields
    public bool IsChecking { get; private set; } = false; // Prevent input during checking
    public bool IsGameOver { get; private set; } = false; // Prevent input during game over
    #endregion

    #region Private Fields
    // Tracks the currently flipped cards
    private readonly List<Card> _currentlyFlipped = new();
    private int _score = 0;
    private float _startTime = 75.0f;
    private int _levelOneScoreMax = 6;
    #endregion

    #region Properties
    // Read-only access to currently flipped cards
    public IReadOnlyList<Card> CurrentlyFlipped => _currentlyFlipped;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent OnMatchFound;
    #endregion

    private void Awake()
    {
        itemManager.OnObjectDestroyed.AddListener(CheckScore);
        cardManager.StartTimer.AddListener(StartGameTimer);
    }

    private void StartGameTimer()
    {
        StartCoroutine(GameTimer());
    }

    public IEnumerator GameTimer()
    {
        while (_startTime > 0)
        {
            if (!itemManager.IsDisplayingItem)
            {
                _startTime -= Time.deltaTime;

                // Calculate minutes and seconds
                int minutes = Mathf.FloorToInt(_startTime / 60); // Integer division to get minutes
                int seconds = Mathf.FloorToInt(_startTime % 60); // Remainder for seconds

                // Format time as mm:ss
                timerText.text = $"Time: {minutes:00}:{seconds:00}";
            }

            yield return null;
        }

        // Handle timer reaching 0
        _startTime = 0;
        timerText.text = "00:00";
        GameOver();
    }

    // Adds the currently flipped card to the list. Notified by the Card.
    public void OnCardFlipped(Card flippedCard)
    {
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
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
            _score++;
            scoreText.text = $"Score: {_score}";
            OnMatchFound?.Invoke();
        }
        else
        {
            firstCard.FlipCardExternally();
            secondCard.FlipCardExternally();
        }

        _currentlyFlipped.Clear();
        IsChecking = false;
    }

    private void CheckScore()
    {
        if (_score != _levelOneScoreMax)
            return;
        GameOver();
    }

    private void GameOver()
    {
        IsGameOver = true;
        StopCoroutine(GameTimer());
        gameOverPanel.SetActive(true);
    }
}
