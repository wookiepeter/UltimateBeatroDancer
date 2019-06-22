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

    public void OnActivation()
    {
        // Debug.Log("Trap is active!");
    }

    public void OnDeactivation()
    {
        // Debug.Log("Traps is inactive!");
    }

    public void OffBeat(int offBeatCounter)
    {
        UpdateTile();
    }

    public void OnBeat()
    {
        UpdateTile();
    }

    protected void UpdateTile()
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
