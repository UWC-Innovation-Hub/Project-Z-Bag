using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] Transform cardSpawnPosition;

    private Vector3 startPosition = new(-2.5f, 2.25f, 1.25f);
    private Vector3 offset = new(1.5f, 1.52f);

    private readonly List<GameObject> cardList = new();

    // Start is called before the first frame update
    void Start()
    {
        SpawnCardMesh(3, 4, cardSpawnPosition.position, offset, false);
        MoveCard(3, 4, startPosition, offset);
    }

    // Spawns a collection of cards at the spawn position and then stores it in a list
    private void SpawnCardMesh(int rows, int columns, Vector3 position, Vector3 offset, bool scaleDown)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var tempCard = (GameObject)Instantiate(cardPrefab, cardSpawnPosition.position, cardSpawnPosition.transform.rotation);

                tempCard.name = $"{"Card {i} Col{j}"}";
                cardList.Add(tempCard);
            }
        }
    }


    // Moves the card from the spawned position into the grid format
    private void MoveCard(int rows, int columns, Vector3 position, Vector3 offset)
    {
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var targetPosition = new Vector3((position.x + (offset.x * j)), 2.25f, (position.z - (offset.y * i)));
                StartCoroutine(MoveToPosition(targetPosition, cardList[index]));
                cardList[index].transform.rotation = Quaternion.Euler(-15f, 0f, 0f);
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, GameObject card)
    {
        var randomDistance = 7;

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
            card.transform.position = Vector3.MoveTowards(card.transform.position, targetPosition, randomDistance * Time.deltaTime);
            yield return null;
        }

        card.transform.position = targetPosition; // Snap to the target position at the end
    }
}
