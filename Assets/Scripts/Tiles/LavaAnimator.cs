using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaAnimator : TileAnimator
{
    public override void OffBeat(int offBeatCounter)
    {
        if (offBeatCounter % 4 == 0)
        {
            UpdateTile();
        }
    }
}
