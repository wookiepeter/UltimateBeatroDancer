using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSubject : IBeatSubject
{
    List<IBeatObserver> beatObservers;
    List<IBeatObserver> toRemove;

    public BeatSubject()
    {
        beatObservers = new List<IBeatObserver>();
        toRemove = new List<IBeatObserver>();
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
        foreach (IBeatObserver observer in toRemove)
        {
            beatObservers.Remove(observer);
        }
        toRemove.Clear();

    }

    public void RemoveObserver(IBeatObserver beatObserver)
    {
        toRemove.Add(beatObserver);
    }
}
