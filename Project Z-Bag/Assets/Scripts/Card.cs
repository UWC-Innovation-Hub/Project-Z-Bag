using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler // Inheritence
{
    #region Public Fields
    public GameObject cardVisualTop;
    public GameObject cardVisualBottom;
    #endregion

    #region Properties
    public int CardID { get; private set; } // Unique ID for each card
    #endregion

    #region Private Fields
    // References
    private Outline outline;

    // Animation settings
    private const float ROTATIONANGLE = 180f;
    private const float DURATION = 0.5f;

    // State
    private bool _isRotating = false;
    private bool _isRotated = false;
    private bool _isChecking = false;
    private bool _isGameOver = false;
    private bool _isDisplayingItem = false;

    // Rotation data
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    #endregion

    private void Start()
    {
        if (cardVisualBottom == null || cardVisualTop == null)
        {
            Debug.LogError("Card visual not assigned.");
            return;
        }

        outline = gameObject.AddComponent<Outline>();
        outline.enabled = false; 

        _startRotation = transform.rotation;
        _endRotation = _startRotation * Quaternion.Euler(0, 0, ROTATIONANGLE);
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChange += ChangeMatchCheckingState;
        GameEvents.OnItemDisplayStateChanged += ChangeItemDisplayState;
        GameEvents.OnGameStateChange += ChangeGameOverState;
    }


    private void OnDisable()
    {
        GameEvents.OnGameStateChange -= ChangeMatchCheckingState;
        GameEvents.OnItemDisplayStateChanged -= ChangeItemDisplayState;
        GameEvents.OnGameStateChange -= ChangeGameOverState;
    }

    private void ChangeGameOverState(object sender, bool isGameOver)
    {
        if (sender == null)
            return;
        _isGameOver = isGameOver;
    }

    private void ChangeItemDisplayState(object sender, bool isDisplaying)
    {
        if (sender == null)
            return;
        _isDisplayingItem = isDisplaying;
    }

    private void ChangeMatchCheckingState(object sender, bool isCheckingState)
    {
        if (sender == null)
            return;
        _isChecking = isCheckingState;
    }

    public void Initialise(int id)
    {
        CardID = id;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isRotated)
        {
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 7f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isRotated)
        {
            outline.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isRotating && !_isChecking && !_isRotated && !_isGameOver && !_isDisplayingItem)
        {
            StartCoroutine(FlipCard());
            GameEvents.TriggerCardFlipped(this);
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

        while (elapsedTime < DURATION)
        {
            float t = Mathf.Clamp01(elapsedTime / DURATION); // Ensures 't' stays in [0, 1]
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final rotation after coroutine finshes
        transform.rotation = targetRotation;
        _isRotated = !_isRotated;
        _isRotating = false;
    }
}
