using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{
    #region Public Fields
    public GameObject cardVisual;
    #endregion

    #region Properties
    public int CardID { get; private set; } // Unique ID for each card
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent<Card> OnCardRotation;
    #endregion

    #region Private Fields
    // References
    private GameManager gameManager;

    // Animation settings
    private readonly float _rotationAngle = 180f;
    private readonly float _duration = 0.5f;

    // State
    private bool _isRotating = false;
    private bool _isRotated = false;

    // Rotation data
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    #endregion

    private void Start()
    {
        if (cardVisual == null)
        {
            Debug.LogError("Card visual not assigned.");
            return;
        }

        _startRotation = transform.rotation;
        _endRotation = _startRotation * Quaternion.Euler(0, 0, _rotationAngle);
    }

    public void Initialise(int id, GameManager gameManager)
    {
        CardID = id;
        this.gameManager = gameManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isRotating && !gameManager.IsChecking && !_isRotated)
        {
            StartCoroutine(FlipCard());
            gameManager.OnCardFlipped(this); // Notify the gameManager about the flip
        }
    }

    // Helper function to flip the card back to its original state
    public void FlipCardExternally()
    {
        StartCoroutine(FlipCard());
    }

    // Flips the card 
    private IEnumerator FlipCard()
    {
        _isRotating = true;
        float elapsedTime = 0f;

        // Check if the card is rotated or not
        Quaternion initialRotation = _isRotated ? _endRotation : _startRotation;
        Quaternion targetRotation = _isRotated ? _startRotation : _endRotation;

        while (elapsedTime < _duration)
        {
            float t = Mathf.Clamp01(elapsedTime / _duration); // Ensures 't' stays in [0, 1]
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final rotation after coroutine finshes
        transform.rotation = targetRotation;
        _isRotated = !_isRotated;
        _isRotating = false;

        Debug.Log($"Card is now {(_isRotated ? "rotated" : "unrotated")}");
    }
}
