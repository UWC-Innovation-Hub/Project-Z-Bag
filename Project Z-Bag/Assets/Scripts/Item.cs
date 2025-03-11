using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Public Fields
    public GameObject itemVisual;
    #endregion

    #region Properties
    public int ItemID { get; private set; } // Encapsulation
    #endregion

    private void Start()
    {
        if (itemVisual == null)
        {
            Debug.LogError("Item visual not assigned.");
        }
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
            // Rotates the item
            gameObject.transform.Rotate(0f, 10f * Time.deltaTime, 0f, Space.Self);
    }

    public void Initialize(int id)
    {
        ItemID = id;
    }
}
