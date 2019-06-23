using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TileAnimator : MonoBehaviour, IBeatObserver
{
    [SerializeField]
    protected Sprite[] sprites;
    [SerializeField]
    protected SpriteRenderer renderer;
    [SerializeField]
    protected int currentIndex = 0;
    [SerializeField]
    protected int activationIndex;
    [SerializeField]
    protected int deactivationIndex;

    // Start is called before the first frame update
    void Start()
    {
        BeatController.GetInstance().BeatSubject.AddObserver(this);
                
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void OnActivation()
    {
        // Debug.Log("Trap is active!");
    }

    public virtual void OnDeactivation()
    {
        // Debug.Log("Traps is inactive!");
    }

    public virtual void OffBeat(int offBeatCounter)
    {
        if (offBeatCounter % 2 == 0)
        {
            UpdateTile();
        }
    }

    public virtual void OnBeat()
    {
        UpdateTile();
    }

    protected virtual void UpdateTile()
    {
        currentIndex++;
        if(currentIndex >= sprites.Length)
        {
            currentIndex = 0;
        }

        renderer.sprite = sprites[currentIndex];

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
