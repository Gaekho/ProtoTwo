using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PartyState
{
    private Dictionary<string, CharacterState> characters = new();
    public IReadOnlyDictionary<string, CharacterState> Characters => characters;

    public CharacterState GetCharacterState(string name)
    {
        if(!characters.TryGetValue(name, out CharacterState state))
        {
            state = new CharacterState();
            characters[name] = state;
        }
        return state;
    }
    public void SetCharacterState(string name, CharacterState state)
    {
        characters[name] = state;
    }

    public void GetRest()
    {
        // When Interact with RestPoint
        // Restore Hp & Talisman
    }

}

public class CharacterState
{
    private int currentHp;
    private int talisman;

    // persistentBuffs
    // 전투 종료 후에도 유지되는 버프&디버프
    // 전투 중 사용되는 버프들(BuffInstance)로 쓸지, 전용버프로 정의할지 미정.
    public int CurrentHp => currentHp;
    public int Talisman => talisman;

    public void SetHp(int value) => currentHp = value;
    public void SetTalisman(int value) => talisman = value;
    
}
