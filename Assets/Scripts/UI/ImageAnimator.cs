using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : TileAnimator 
{

    [SerializeField]
    protected Image _image;

    public Sprite[] Sprites { get => sprites; set => sprites = value; }

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
}
