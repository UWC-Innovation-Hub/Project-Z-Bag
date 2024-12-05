using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InformationBoxManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Managers")]
    [SerializeField] private ItemManager itemManager;

    [Header("Game Objects")]
    [SerializeField] private GameObject informationBoxPrefab;
    [SerializeField] private InformationBox informationBox;

    [Header("Spawn Configuration")]
    [SerializeField] private GameObject informationBoxSpawnPosition;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent ChooseText;
    #endregion

    #region Private fields
    private GameObject informationBoxDisplayed;
    #endregion


    private void Start()
    {
        itemManager.InstantiateInformationBox.AddListener(InstantiateInformationBox);
        itemManager.OnObjectDestroyed.AddListener(DestroyInformationBox);
    }

    private void InstantiateInformationBox()
    {
        informationBoxDisplayed = (GameObject)Instantiate(informationBoxPrefab, informationBoxSpawnPosition.transform.position, informationBoxSpawnPosition.transform.rotation);
        informationBox = informationBoxDisplayed.GetComponent<InformationBox>();
        int itemID = itemManager.GetDisplayedItemID();
        DisplayText(itemID);
    }

    private void DestroyInformationBox()
    {
        if (informationBoxDisplayed != null)
            Destroy(informationBoxDisplayed);
    }

    private void DisplayText(int itemID)
    {
        informationBox.text.text = itemID switch
        {
            0 => "This is Earth",
            1 => "This is Jupiter",
            2 => "This is Mars",
            3 => "This is Mercury",
            4 => "This is Earth's Moon",
            5 => "This is Saturn",
            _ => "This is a planet",
        };
    }
}
