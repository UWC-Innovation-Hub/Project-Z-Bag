using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject cardVisual;

    private readonly float rotationAngle = 180f;
    private readonly float duration = 0.5f;

    private bool isRotating = false;

    private void Start()
    {
        if (cardVisual == null)
        {
            Debug.LogError("Card visual not assigned.");
            return;
        }
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

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, rotationAngle);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset rotation
        transform.rotation = endRotation;
        isRotating = false;
    }
}
