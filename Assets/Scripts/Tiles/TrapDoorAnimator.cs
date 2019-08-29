using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorAnimator : MonoBehaviour, IBeatObserver
{
    [SerializeField]
    protected Sprite[] sprites;
    [SerializeField]
    protected SpriteRenderer renderer;
    [SerializeField]
    protected int currentIndex = 0;

    public bool isActivated { get; set; } = false;
    public bool isOpen { get; private set; } = false;

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

    public virtual void OffBeat(int offBeatCounter)
    {
        if (isOpen && offBeatCounter % 2 == 0)
        {
            UpdateTile();
        }
    }

    public virtual void OnBeat()
    {
        if(isActivated == true)
        {
            isOpen = true;
            Debug.Log("Trapdoor was activated now");
        }
        UpdateTile();
    }

    protected virtual void UpdateTile()
    {
        if (isOpen)
        {
            currentIndex++;
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
                isOpen = false;
                isActivated = false;
                Debug.Log("Trapdoor is deactivated again!");
            }

            renderer.sprite = sprites[currentIndex];
        }
    }
}
