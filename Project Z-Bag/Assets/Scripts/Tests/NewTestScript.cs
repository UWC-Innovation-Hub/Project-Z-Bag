public class NewTestScript
{
    /*private GameObject managerGameObject;
    private InformationBoxManager manager;

    [SetUp]
    public void Setup()
    {
        // Create a temporary GameObject with the InformationBoxManager component
        managerGameObject = new GameObject();
        manager = managerGameObject.AddComponent<InformationBoxManager>();

        // Mock required serialized fields
        managerGameObject.AddComponent<TMP_Text>(); // Mock TextMeshPro
        manager.informationBoxPrefab = new GameObject(); // Mock prefab
        manager.informationBoxSpawnPosition = new GameObject(); // Mock spawn position
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(managerGameObject);
    }

    [Test]
    public void Test_DisplayText_CorrectlySetsText()
    {
        // Arrange
        manager.informationBox = new GameObject().AddComponent<InformationBox>();
        manager.informationBox.text = new GameObject().AddComponent<TMP_Text>();

        // Act
        manager.DisplayText(1); // Jupiter case

        // Assert
        Assert.AreEqual("Jupiter: A world of extremes. It's the largest planet in our solar system – if it were a hollow shell, 1,000 Earths could fit inside.", manager.informationBox.text.text);
    }

    [UnityTest]
    public IEnumerator Test_InstantiateInformationBox_CreatesObject()
    {
        // Act
        manager.InstantiateInformationBox(new Item { ItemID = 1 });

        // Wait a frame (for GameObject instantiation)
        yield return null;

        // Assert
        Assert.IsNotNull(manager.informationBoxDisplayed);
        Assert.IsInstanceOf<GameObject>(manager.informationBoxDisplayed);
    }*/
}
