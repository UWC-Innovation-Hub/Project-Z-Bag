using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private GameObject cardVisual;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isRotating)
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
