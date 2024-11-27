using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using zSpace.Core.Samples;

public class Card : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Public Fields
    public GameObject cardVisualTop;
    public GameObject cardVisualBottom;
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
    private ItemManager itemManager;
    private Material visualTopMaterial;
    private Material visualBottomMaterial;

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
        if (cardVisualBottom == null || cardVisualTop == null)
        {
            Debug.LogError("Card visual not assigned.");
            return;
        }

        visualTopMaterial = cardVisualTop.GetComponent<MeshRenderer>().material;
        visualBottomMaterial = cardVisualBottom.GetComponent<MeshRenderer>().material;

        itemManager = FindObjectOfType<ItemManager>();

        _startRotation = transform.rotation;
        _endRotation = _startRotation * Quaternion.Euler(0, 0, _rotationAngle);
    }

    public void Initialise(int id, GameManager gameManager)
    {
        CardID = id;
        this.gameManager = gameManager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isRotated)
        {
            visualBottomMaterial.color = HighlightBottomCard(visualBottomMaterial);
            visualTopMaterial.color = HighlightTopCard(visualTopMaterial);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isRotated)
        {
            visualBottomMaterial.color = HighlightBottomCard(visualBottomMaterial);
            visualTopMaterial.color = HighlightTopCard(visualTopMaterial);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isRotating && !gameManager.IsChecking && !_isRotated && !gameManager.IsGameOver && !itemManager.IsDisplayingItem)
        {
            StartCoroutine(FlipCard());
            gameManager.OnCardFlipped(this); // Notify the gameManager about the flip
        }
    }

    // Makes the top card visual tranparent or not transparent
    private Color HighlightTopCard(Material cardVisualTopMaterial)
    {
        Color visualColour = cardVisualTopMaterial.color;

        if (visualColour.a == 1)
            visualColour.a = Mathf.Clamp(0.5f, 0, 1);
        else
            visualColour.a = Mathf.Clamp(1, 0, 1);
        return visualColour;
    }

    // Makes the bottom card visual black or normal
    private Color HighlightBottomCard(Material cardVisualBottomMaterial)
    {
        Color visualColour = cardVisualBottomMaterial.color;

        if (visualColour.a == 1)
            return visualColour;
        else
        {
            visualColour.r = 0;
            visualColour.g = 0;
            visualColour.b = 0;
            //visualColour.a = 1;
        }
        return visualColour;
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
