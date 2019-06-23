using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour, IBeatObserver
{
    [SerializeField]
    private Player playerToSpawn;

    [SerializeField]
    private ComboBarController comboBar;

    [SerializeField]
    private int initialOffset = 16;

    [SerializeField]
    private int currentBeat = 0;

    

    public void OffBeat(int offBeatCounter)
    { 
    
        if (currentBeat != -1)
            currentBeat += 1;

        if (currentBeat >= initialOffset && currentBeat != -1)
        {
            currentBeat = -1;
            Player player = GameObject.Instantiate(playerToSpawn);
            player.SetSpawner(this);
            player.transform.position = this.transform.position;
            player.calculatePosition();
            player.transform.parent = this.transform;
            GetComponent<TileAnimator>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            player.comboBarController = this.comboBar;
        }
        // throw new System.NotImplementedException();
    }

    public void activate()
    {
        currentBeat = 0;
        this.GetComponent<TileAnimator>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.enabled = true;
    }

    public void OnBeat()
    {
       
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        BeatController.GetInstance().BeatSubject.AddObserver(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
