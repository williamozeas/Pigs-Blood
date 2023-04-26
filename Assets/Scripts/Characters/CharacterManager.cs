using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    Boy,
    Wolf,
    Straw,
    Sticks,
    Bricks,
    TinkerBell,
    MatchGirl
}

public class CharacterManager : Singleton<CharacterManager>
{
    public Wolf Wolf { get; private set; }
    public Straw Straw { get; private set; }
    public Sticks Sticks { get; private set; }
    public Bricks Bricks { get; private set; }
    public TinkerBell TinkerBell { get; private set; }
    public MatchGirl MatchGirl { get; private set; }
    
    private List<Character> activeCharacters;
    
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        Wolf = GetComponentInChildren<Wolf>(true);
        Straw = GetComponentInChildren<Straw>(true);
        Sticks = GetComponentInChildren<Sticks>(true);
        Bricks = GetComponentInChildren<Bricks>(true);
        TinkerBell = GetComponentInChildren<TinkerBell>(true);
        MatchGirl = GetComponentInChildren<MatchGirl>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Characters GetCharacterEnum(string characterName)
    {
        switch (characterName.ToLower())
        {
            case ("wolf"):
            {
                return Characters.Wolf;
            }
            case ("straw"):
            {
                return Characters.Straw;
            }
            case ("sticks"):
            {
                return Characters.Sticks;
            }
            case ("bricks"):
            {
                return Characters.Bricks;
            }
            case ("tinkerbell"):
            {
                return Characters.TinkerBell;
            }
            case ("matchgirl"):
            {
                return Characters.MatchGirl;
            }
            case ("player"):
            case ("boy"):
            case ("boywhocriedwolf"):
            {
                return Characters.Boy;
            }
        }
        Debug.LogWarning("Unmatched character: " + characterName);
        return Characters.Boy;
    }

    public Character GetCharacter(Characters character)
    {
        switch (character)
        {
            case (Characters.Wolf):
            {
                return Wolf;
            }
            case (Characters.Straw):
            {
                return Straw;
            }
            case (Characters.Sticks):
            {
                return Sticks;
            }
            case (Characters.Bricks):
            {
                return Bricks;
            }
            case (Characters.TinkerBell):
            {
                return TinkerBell;
            }
            case (Characters.MatchGirl):
            {
                return MatchGirl;
            }
            case (Characters.Boy):
            {
                Debug.LogWarning("Boy character class requested!");
                return null;
            }
        }
        Debug.LogWarning("Unknown character selected!");
        return null;
    }
}
