// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using UnityEngine;

public class PowerStoneEffect : CollectibleEffect
{
    [SerializeField]
    private bool m_isLastPiece = true;

    public override void Apply(GameObject instigator)
    {
        PlayerCharaCosmo cosmo = instigator.GetComponent<PlayerCharaCosmo>();
        Player player = cosmo.GetControllingPlayer();
        if (cosmo != null && player != null)
        {
            if (player.stoneCollection != null)
            {
                player.gameState.collectedCrystals += 1;
                player.stoneCollection.AddItem(m_collectible);
            }
        }
    }


    public bool IsLastPiece()
    {
        return m_isLastPiece;
    }
}
