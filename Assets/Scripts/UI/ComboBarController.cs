using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBarController : MonoBehaviour
{
    [SerializeField]
    ImageAnimator imageAnimator;
    [SerializeField]
    Player player;
    [SerializeField]
    ComboBarAnimator comboBarAnimator;

    [SerializeField]
    Sprite[] hitSprites;
    [SerializeField]
    Sprite[] missSprites;

    public void SetBar(bool hitBeat)
    {
        if(hitBeat)
        {
            imageAnimator.Sprites = hitSprites;
            comboBarAnimator.ChangeComboAmount(1);
        }
        else
        {
            imageAnimator.Sprites = missSprites;
            comboBarAnimator.ChangeComboAmount(-1);
        }
    }

    public void ResetIndicator()
    {
        imageAnimator.Sprites = hitSprites;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
