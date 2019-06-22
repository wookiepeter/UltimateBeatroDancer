using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBeatSubject
{
    void AddObserver(IBeatObserver beatObserver);

    void RemoveObserver(IBeatObserver beatObserver);

    void NotifyOnBeat();

    void NotifyOnOffBeat(int offBeatCounter);
}
