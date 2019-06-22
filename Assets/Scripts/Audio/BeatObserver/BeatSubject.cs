using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSubject : IBeatSubject
{
    List<IBeatObserver> beatObservers;

    public BeatSubject()
    {
        beatObservers = new List<IBeatObserver>();
    }

    public void AddObserver(IBeatObserver beatObserver)
    {
        beatObservers.Add(beatObserver);   
    }

    public void NotifyOnBeat()
    {
        foreach (IBeatObserver observer in beatObservers)
        {
            observer.OnBeat();
        }
    }

    public void NotifyOnOffBeat(int offBeatCounter)
    {
        foreach (IBeatObserver observer in beatObservers)
        {
            observer.OffBeat(offBeatCounter);
        }
    }

    public void RemoveObserver(IBeatObserver beatObserver)
    {
        beatObservers.Remove(beatObserver);
    }
}
