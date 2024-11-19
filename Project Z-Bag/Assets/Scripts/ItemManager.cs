using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private CardManager cardManager;

    private void Start()
    {
        cardManager.OnMatchFound.AddListener(DisplayObject);
    }

    private void DisplayObject()
    {
        GameObject itemObject = Instantiate(itemPrefab, itemPrefab.transform.position, itemPrefab.transform.rotation);

        itemObject.SetActive(true);
    }
}
