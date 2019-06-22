using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : System.IFormattable
{
    float _beatTimeDiff;
    float _beatStart;
    float _bpm;

    int _beatCounter;
    float _nextBeatTime;
    
    public Beat(float beatTimeDiff, float beatStart) 
    {
        _beatTimeDiff = beatTimeDiff;
        _beatStart = beatStart;
        _bpm = 60f / BeatTimeDiff;
        reset();
    }

    public float BeatTimeDiff { get => _beatTimeDiff; }
    public float BeatStart { get => _beatStart; }
    public float Bpm { get => _bpm; } 

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return "Beat: with " + Bpm + " bpm (BeatTime: " + BeatTimeDiff + " s) starts at " + BeatStart;
    }

    public void reset()
    {
        _beatCounter = 0;
        _nextBeatTime = BeatStart;
    } 



    public int UpdateBeat(float songTime)
    {
        if (songTime > _nextBeatTime)
        {
            _beatCounter++;
            _nextBeatTime = _beatStart + _beatCounter * 0.25f * BeatTimeDiff;
            return _beatCounter % 4;
        }

        return -1;
    }
}
