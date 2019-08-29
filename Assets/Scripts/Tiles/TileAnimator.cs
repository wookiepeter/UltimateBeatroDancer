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

    public bool isActive { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        BeatController.GetInstance().BeatSubject.AddObserver(this);
        // Debug.Log("Starting animation for " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void OnActivation()
    {
        // Debug.Log("Trap is active!");
        isActive = true;
    }

    public virtual void OnDeactivation()
    {
        // Debug.Log("Traps is inactive!");
        isActive = false;
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
