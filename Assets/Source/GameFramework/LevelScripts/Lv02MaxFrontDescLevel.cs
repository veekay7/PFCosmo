// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System.Collections.Generic;

public class Lv02MaxFrontDescLevel : Lv02BaseLevel
{
    protected override void Start()
    {
        base.Start();

        // On start of the level, we check how many platforms are there.
        // For each platforms that are already there, we set a value to them.
        int[] usingValues = { 1, 3 };
        for (int i = 0; i < usingValues.Length; i++)
        {
            PlatformSlot s = mainPuzzle.slotsCtrl.Get(i);
            Platform p = s.GetPlatform();

            Kana kana = kanaTable.GetUnusedKana();
            p.SetKana(kana);

            int value = 0;
            value = valueManager.GetValueAndUse(usingValues[i]);
            p.SetValue(value);
        }
    }


    public override void CheckSolution()
    {
        base.CheckSolution();

        // Get a list of all active platforms in the main puzzle and check the number of active platforms available
        List<Platform> activePlatforms = mainPuzzle.m_activePlatforms;
        int currentActiveCount = activePlatforms.Count;

        // CONDITION: If the number of active platforms in the level is less than total amount of slots, solution not complete.
        if (currentActiveCount < mainPuzzle.slotsCtrl.Count)
        {
            isSolved = false;
            return;
        }

        // CONDITION: Exclusive to this level, we want to get the smallest value to the end of the array
        // Let's convert all the values into an array.
        int[] temp = new int[currentActiveCount];
        int idx = 0;
        Platform current = mainPuzzle.head;
        while (current != null)
        {
            temp[idx] = current.value;
            current = current.next;
            idx++;
        }

        // CONDITION: Check is the minimum at the back
        bool result = GameController.instance.gameRules.IsMaxFront(temp) &&
            GameController.instance.gameRules.IsDescending(temp);
        if (!result)
        {
            isSolved = false;
            return;
        }

        // Show all crystals and open portal
        portalArea.gameObject.SetActive(true);
        for (int i = 0; i < crystals.Count; i++)
        {
            crystals[i].gameObject.SetActive(true);
        }

        isSolved = true;

        PlayVictoryChime();
    }
}
