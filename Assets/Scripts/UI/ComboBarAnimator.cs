using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBarAnimator : MonoBehaviour, IBeatObserver
{
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    Image image;
    [SerializeField]
    [Range(0, 19)]
    int currentIndex;

    private void Start()
    {
        BeatController.GetInstance().BeatSubject.AddObserver(this);
        updateSprite();
    }

    public void ChangeComboAmount(int change)
    {
        currentIndex += change;
        currentIndex = Mathf.Clamp(currentIndex, 0, 19);
    }

    public void updateSprite()
    {
        image.sprite = sprites[currentIndex];
    }

    public void OnBeat()
    {
        updateSprite();
    }

    public void OffBeat(int offBeatCounter)
    {
        
    }
}
