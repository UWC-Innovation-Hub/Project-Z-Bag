using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private GameObject cardVisual;
    [SerializeField] private float fadeAmount = 0f;
    [SerializeField] private float fadeSpeed = 1f;
    private bool isFaded = false;
    private Material originalMaterial;

    private void Start()
    {
        originalMaterial = cardVisual.GetComponent<Renderer>().material;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFaded)
        {
            // If the object is currently faded, fade it back to full opacity
            Debug.Log("Fading back in");
            StartCoroutine(FadeCard(originalMaterial.color.a, 1f));
        }
        else
        {
            // If the object is not faded, fade it to the target opacity (faded)
            Debug.Log("The object has been clicked");
            StartCoroutine(FadeCard(originalMaterial.color.a, fadeAmount));
        }

        // Toggle faded state so it only changes once per click
        isFaded = !isFaded;
    }

    private IEnumerator FadeCard( float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color startColor = new(originalMaterial.color.r, originalMaterial.color.g, originalMaterial.color.b, startAlpha);
        Color targetColor = new(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (elapsedTime < fadeSpeed)
        {
            // Interpolate the alpha value over time
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeSpeed);
            originalMaterial.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final colour is set
        originalMaterial.color = targetColor;
    }
}
