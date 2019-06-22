using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IBeatObserver 
{
    void OnBeat();

    void OffBeat(int offBeatCounter);
}
