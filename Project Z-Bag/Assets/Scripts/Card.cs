using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject cardVisual;

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
        StartCoroutine(FlipCard());
    }

    // Flips the card 
    private IEnumerator FlipCard()
    {
        // Rotate to reveal front
        for (float t = 0; t <= 1; t += Time.deltaTime * 2)
        {
            gameObject.transform.Rotate(Vector3.forward * 180f * t, Space.Self);
            yield return null;
        }

        // Reset rotation
        gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
