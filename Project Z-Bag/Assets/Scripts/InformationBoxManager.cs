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
        DisplayText();
    }

    private void DestroyInformationBox()
    {
        if (informationBoxDisplayed != null)
            Destroy(informationBoxDisplayed);
    }

    private void DisplayText()
    {
        Item displayedItem = itemManager.GetItemDisplayed();

        switch (displayedItem.ItemID)
        {
            case 0:
                informationBox.text.text = "This is Earth";
                break;
            case 1:
                informationBox.text.text = "This is Jupiter";
                break;
            case 2:
                informationBox.text.text = "This is Mars";
                break;
            case 3:
                informationBox.text.text = "This is Mercury";
                break;
            case 4:
                informationBox.text.text = "This is Earth's Moon";
                break;
            case 5:
                informationBox.text.text = "This is Saturn";
                break;
            default:
                informationBox.text.text = "This is a planet";
                break;
        }

    }
}
