﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : TileAnimator 
{

    [SerializeField]
    protected Image _image;

    [SerializeField]
    protected bool slowAnimation;
    

    protected override void UpdateTile()
    {
        currentIndex++;
        if(currentIndex >= sprites.Length)
        {
            currentIndex = 0;
        }

        _image.sprite = sprites[currentIndex];

        if(currentIndex == activationIndex)
        {
            OnActivation();
        } 
        if (currentIndex == deactivationIndex)
        {
            OnDeactivation();
        }

    }

    public override void OffBeat(int offBeatCounter)
    {
        if (!slowAnimation)
        {
            if (offBeatCounter % 2 == 0)
            {
                UpdateTile();
            }
        }
    }
}
