using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PartyState
{
    private DeckData currentDeck;
    private readonly CharacterState sitaState;
    private CharacterState rigel;
    private CharacterState solphur;

    public void GetState()
    {

    }
    public void CommitState()
    {
        sitaState.sss = 0;
    }
    public void ChangeDeck(DeckData newDeck)
    {
        currentDeck = newDeck;
    }

}

public class CharacterState
{
    protected int hp;
    private int power;
    public int sss;
    
}
