using System;
using System.Collections;
using System.Collections.Generic;
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
    #endregion

    #region Unity Events
    [HideInInspector] public UnityEvent ChooseText;
    #endregion

    #region Private fields
    private readonly float _destroyAfterTime = 7.0f;
    #endregion


    private void Start()
    {
        itemManager.InstantiateInformationBox.AddListener(InstantiateInformationBox);
    }

    private void InstantiateInformationBox(Item item)
    {
        GameObject informationBox = (GameObject)Instantiate(informationBoxPrefab, informationBoxSpawnPosition.transform.position, informationBoxSpawnPosition.transform.rotation, item.transform);

        StartCoroutine(DestroyInformationBox(informationBox));
    }

    private IEnumerator DestroyInformationBox(GameObject informationBox)
    {
        yield return new WaitForSeconds(_destroyAfterTime);
        Destroy(informationBox);
    }
}
