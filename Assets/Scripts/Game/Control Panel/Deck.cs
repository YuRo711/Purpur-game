using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck<T>
{
    private readonly T[] elements;
    private Stack<T> currentDeck;

    public Deck(IEnumerable<T> elements)
    {
        this.elements = elements.ToArray();
        ConstructDeck();
    }

    public T TakeNext()
    {
        if (currentDeck.Count == 0)
            ConstructDeck();

        return currentDeck.Pop();
    }

    private void ConstructDeck()
    {
        var rng = new System.Random();
        var randomizedList = elements.ToList().OrderBy(x => rng.Next());
        currentDeck = new Stack<T>(randomizedList);
    }
}