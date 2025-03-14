using TMPro;
using UnityEngine;

public class InformationBox : MonoBehaviour
{
    [SerializeField] private Transform item;
    [SerializeField] private Vector3 offset = new(3, 1, 0);
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        item = FindAnyObjectByType<Item>().transform;
        Debug.Log("Found Item");
    }

    private void LateUpdate()
    {
        if (item != null)
            transform.position = item.position + offset;
        else
            Destroy(gameObject);
        //transform.rotation = item.rotation;
    }
}
