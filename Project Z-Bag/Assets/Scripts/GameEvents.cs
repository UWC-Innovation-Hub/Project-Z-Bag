using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event EventHandler<Card> OnCardFlip;

    public static event EventHandler OnMatchFound;

    public static event EventHandler OnHideCards;

    public static event EventHandler OnUnhideCards;

    public static void TriggerCardFlipped(Card card) => OnCardFlip?.Invoke(null, card);

    public static void TriggerMatchFound() => OnMatchFound?.Invoke(null, EventArgs.Empty);

    public static void TriggerHideCards() => OnHideCards?.Invoke(null, EventArgs.Empty);

    public static void TriggerUnhideCards() => OnUnhideCards?.Invoke(null, EventArgs.Empty);
}
