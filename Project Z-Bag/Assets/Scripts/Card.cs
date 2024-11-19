using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{
    public GameObject cardVisual;

    public int CardID { get; private set; } // Unique ID for each card

    private CardManager cardManager;

    private readonly float rotationAngle = 180f;
    private readonly float duration = 0.5f;

    private bool isRotating = false;
    private bool isRotated = false;

    Quaternion startRotation;
    Quaternion endRotation;

    private void Start()
    {
        if (cardVisual == null)
        {
            Debug.LogError("Card visual not assigned.");
            return;
        }

        startRotation = transform.rotation;
        endRotation = startRotation * Quaternion.Euler(0, 0, rotationAngle);
    }

    public void Initialise(int id, CardManager cardManager)
    {
        CardID = id;
        this.cardManager = cardManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isRotating && !cardManager.IsChecking)
        {
            StartCoroutine(FlipCard());
            cardManager.OnCardFlipped(this); // Notify manager about the flip
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
        isRotating = true;
        float elapsedTime = 0f;

        // Check if the card is rotated or not
        Quaternion initialRotation = isRotated ? endRotation : startRotation;
        Quaternion targetRotation = isRotated ? startRotation : endRotation;

        while (elapsedTime < duration)
        {
            float t = Mathf.Clamp01(elapsedTime / duration); // Ensures 't' stays in [0, 1]
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final rotation after coroutine finshes
        transform.rotation = targetRotation;
        isRotated = !isRotated;
        isRotating = false;

        Debug.Log($"Card is now {(isRotated ? "rotated" : "unrotated")}");
    }
}
