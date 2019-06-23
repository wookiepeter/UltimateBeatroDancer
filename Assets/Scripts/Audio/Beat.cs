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
    float _lastBeatTime;
    
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
        _lastBeatTime = _nextBeatTime;
    } 

    public float getTimeToClosestBeat(float songTime)
    {
        return Mathf.Min(Mathf.Abs(songTime - _nextBeatTime), Mathf.Abs(songTime - _nextBeatTime));
    }

    public int UpdateBeat(float songTime)
    {
        if (songTime > _nextBeatTime)
        {
            _beatCounter++;
            _lastBeatTime = _nextBeatTime;
            _nextBeatTime = _beatStart + _beatCounter * 0.125f * BeatTimeDiff;
            return _beatCounter % 8;
        }

        return -1;
    }
}
