using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InformationBox : MonoBehaviour
{
    [SerializeField] private Transform item;
    [SerializeField] private Vector3 offset = new(3, 1, 0);
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        item = FindAnyObjectByType<Item>().transform;
    }

    private void LateUpdate()
    {
        transform.position = item.position + offset;
        //transform.rotation = item.rotation;
    }
}
