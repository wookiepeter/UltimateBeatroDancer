using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour, IBeatObserver
{
    [SerializeField]
    private Player playerToSpawn;

    private int initialOffset = 9;
    private int currentBeat = 0;

    public void OffBeat(int offBeatCounter)
    {
       // throw new System.NotImplementedException();
    }

    public void OnBeat()
    {
        currentBeat += 1;
        if (currentBeat >= initialOffset)
        {
            currentBeat = 0;
            Player player = GameObject.Instantiate(playerToSpawn);
            player.SetSpawner(this);
            player.transform.position = this.transform.position;
            enabled = false;
        }
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
