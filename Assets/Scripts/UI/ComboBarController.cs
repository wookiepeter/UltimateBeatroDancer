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
    Sprite[] hitSprites;
    [SerializeField]
    Sprite[] missSprites;

    public void SetBar(bool hitBeat)
    {
        if(hitBeat = true)
        {
            imageAnimator.Sprites = hitSprites;

        }
        else
        {
            imageAnimator.Sprites = missSprites;
        }
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
