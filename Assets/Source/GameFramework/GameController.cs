// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK

public class GameController : MonoBehaviourSingleton<GameController>
{
    public GameRules gameRules { get; private set; }
    public LevelScript levelScript { get; private set; }


    protected override void OnEnable()
    {
        base.OnEnable();

        gameRules = GetComponent<GameRules>();
        levelScript = GetComponent<LevelScript>();
    }
}
