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

    [Header("Spawn Configuration")]
    [SerializeField] private GameObject informationBoxSpawnPosition;

    [Header("Text")]
    [SerializeField] private TextMeshPro informationBoxText;
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent ChooseText;
    #endregion

    #region Private fields
    private GameObject informationBox;
    #endregion


    private void Start()
    {
        itemManager.InstantiateInformationBox.AddListener(InstantiateInformationBox);
        itemManager.OnObjectDestroyed.AddListener(DestroyInformationBox);
    }

    private void InstantiateInformationBox()
    {
        informationBox = (GameObject)Instantiate(informationBoxPrefab, informationBoxSpawnPosition.transform.position, informationBoxSpawnPosition.transform.rotation);
    }

    private void DestroyInformationBox()
    {
        if (informationBox != null)
            Destroy(informationBox);
    }

    private void DisplayText()
    {
        
    }
}
