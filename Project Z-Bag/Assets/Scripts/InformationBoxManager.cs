using UnityEngine;

public class InformationBoxManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Game Objects")]
    [SerializeField] private GameObject informationBoxPrefab;
    [SerializeField] private InformationBox informationBox;

    [Header("Spawn Configuration")]
    [SerializeField] private GameObject informationBoxSpawnPosition;
    #endregion

    #region Private fields
    private GameObject informationBoxDisplayed;
    #endregion

    private void OnEnable()
    {
        GameEvents.OnDisplayingItem += InstantiateBox;
    }

    private void OnDisable()
    {
        GameEvents.OnDisplayingItem -= InstantiateBox;
    }

    private void InstantiateBox(object sender, Item item)
    {
        InstantiateInformationBox(item);
    }

    private void InstantiateInformationBox(Item item)
    {
        informationBoxDisplayed = (GameObject)Instantiate(informationBoxPrefab, informationBoxSpawnPosition.transform.position, informationBoxSpawnPosition.transform.rotation);
        informationBox = informationBoxDisplayed.GetComponent<InformationBox>();
        DisplayText(item.ItemID);
    }

    private void DisplayText(int itemID)
    {
        informationBox.text.text = itemID switch
        {
            0 => "Earth: While Earth is only the fifth largest planet in the solar system, it is the only world in our solar system with liquid water on the surface.",
            1 => "Jupiter: A world of extremes. It's the largest planet in our solar system � if it were a hollow shell, 1,000 Earths could fit inside.",
            2 => "Mars: The fourth planet from the Sun � is a dusty, cold, desert world with a very thin atmosphere. This dynamic planet has seasons, polar ice caps, extinct volcanoes, canyons and weather.",
            3 => "Mercury: The smallest planet in our solar system and nearest to the Sun, Mercury is only slightly larger than Earth's Moon.",
            4 => "Earth's Moon: Makes Earth more livable by moderating our home planet's wobble on its axis, leading to a relatively stable climate.",
            5 => "Saturn: Like fellow gas giant Jupiter, Saturn is a massive ball made mostly of hydrogen and helium. Saturn is not the only planet to have rings, but none are as spectacular or as complex as Saturn's.",
            _ => "This is a planet",
        };
    }
}
