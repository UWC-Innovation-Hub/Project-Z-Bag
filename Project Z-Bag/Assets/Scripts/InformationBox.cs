using TMPro;
using UnityEngine;

public class InformationBox : MonoBehaviour
{
    [SerializeField] private Transform item;
    [SerializeField] private Vector3 offset = new(3, 1, 0);
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        GameEvents.OnDisplayingItem += AssignItemReference;
    }

    private void OnDisable()
    {
        GameEvents.OnDisplayingItem -= AssignItemReference;
    }

    private void AssignItemReference(object sender, Item item)
    {
        this.item = item.transform;
        Debug.Log("Item assigned");
    }

    private void LateUpdate()
    {
        if (item != null)
            transform.position = item.position + offset;
        else
            Destroy(gameObject);
    }
}
