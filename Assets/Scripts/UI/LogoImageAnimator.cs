using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoImageAnimator : ImageAnimator
{

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
        else
        {
            if (offBeatCounter < 2)
            {
                transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            } else
            {
                transform.localScale = Vector3.one;
            }
        }  
    }
}
