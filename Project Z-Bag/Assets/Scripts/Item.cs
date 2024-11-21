using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Public Fields
    public GameObject itemVisual;
    #endregion

    #region Properties
    public int ItemID { get; private set; } // Unique ID for each item
    #endregion

    private void Start()
    {
        if (itemVisual == null)
        {
            Debug.LogError("Item visual not assigned.");
        }
    }

    public void Initialize(int id)
    {
        ItemID = id;
    }
}
