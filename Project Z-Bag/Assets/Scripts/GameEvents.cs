using System;
using UnityEditor;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event EventHandler<Card> OnCardFlip;

    public static event EventHandler<Card> OnMatchFound;

    public static event EventHandler OnHideCards;

    public static event EventHandler OnUnhideCards;

    public static event EventHandler<GameObject> OnObjectDestroyed;

    public static event EventHandler<Item> OnDisplayingItem;

    public static event EventHandler OnCardSpawn;

    public static event EventHandler OnCardsInPosition;

    public static event EventHandler<Card> OnItemDestroyed;

    public static event EventHandler<bool> OnGameStateChange;

    public static event EventHandler<bool> OnItemDisplayStateChanged;

    public static void TriggerCardFlipped(Card card) => OnCardFlip?.Invoke(null, card);

    public static void TriggerMatchFound(Card card) => OnMatchFound?.Invoke(null, card);

    public static void TriggerHideCards() => OnHideCards?.Invoke(null, EventArgs.Empty);

    public static void TriggerUnhideCards() => OnUnhideCards?.Invoke(null, EventArgs.Empty);

    public static void TriggerDestroyItem(GameObject item) => OnObjectDestroyed?.Invoke(null, item);

    public static void TriggerInformationBoxInstantiation(Item item) => OnDisplayingItem?.Invoke(null, item);

    public static void TriggerAssignItemID() => OnCardSpawn?.Invoke(null, EventArgs.Empty);

    public static void TriggerGameTimerStart() => OnCardsInPosition?.Invoke(null, EventArgs.Empty);

    public static void TriggerCardRemove(Card card) => OnItemDestroyed?.Invoke(null, card);

    public static void TriggerGameStateChange(bool stateChange) => OnGameStateChange?.Invoke(null, stateChange);

    public static void TriggerDisplayingItem(bool isDisaplayingItem) => OnItemDisplayStateChanged?.Invoke(null, isDisaplayingItem);
}
