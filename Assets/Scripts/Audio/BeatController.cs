using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    private static BeatController instance; 
   
    [SerializeField]
    TextAsset _beatLabelFile;
    [SerializeField]
    AudioClip _audioClip;
    [SerializeField]
    BeatLabelProcessorParams beatLabelProcessorParams = new BeatLabelProcessorParams(0.01f, 0.75f, 15, 0.65f);
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    [Range(0.5f, 8f)]
    float _playSpeed = 1f;
    [SerializeField]
    [Range(0.05f, 0.5f)]
    float beatInputWindow = 0.12f;

    Beat _beat;
    BeatLabelProcessorParams _oldBeatLabelProcessorParams;
    BeatLabelProcessor _beatLabelProcessor;
    BeatSubject _beatSubject;

    public BeatSubject BeatSubject { get => _beatSubject; }
    public float ActualBeatTime { get => _beat.BeatTimeDiff / _playSpeed; }

    public static BeatController GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("Cannot retrieve BeatControler before Awake was called!");
        }
        return instance;
    }

    private void Awake()
    {
        _beatLabelProcessor = new BeatLabelProcessor(_beatLabelFile);
        _oldBeatLabelProcessorParams = null;
        _audioSource.clip = _audioClip;
        _beat = new Beat(0.5f, 16.12f);
        _beatSubject = new BeatSubject();

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _beatLabelProcessor = new BeatLabelProcessor(_beatLabelFile);
        _oldBeatLabelProcessorParams = null;
        _audioSource.clip = _audioClip;
        _beat = new Beat(0.5f, 16.12f);
        _audioSource.Play();
        _audioSource.pitch = _playSpeed;
        _audioSource.time = 15f;

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int beatCounter = _beat.UpdateBeat(_audioSource.time);
        if (beatCounter > 0)
        {
            // Update Beats here
            BeatSubject.NotifyOnOffBeat(beatCounter);
        } else if (beatCounter >= 0)
        {
            BeatSubject.NotifyOnBeat();

            _audioSource.pitch = _playSpeed;
        } else
        {

        }

        if(_audioSource.isPlaying == false)
        {
            _beat.reset();
            _audioSource.clip = _audioClip;
            _audioSource.Play();
        }
    }

    public bool CurrentlyInInputWindow()
    {
        Debug.Log("Time To Beat" + _beat.getTimeToClosestBeat(_audioSource.time) + " - BeatInputWindow: " + beatInputWindow);
        return (_beat.getTimeToClosestBeat(_audioSource.time) < beatInputWindow);
    }

    void updateBeatprocessor()
    {
        if(beatLabelProcessorParams.Equals(_oldBeatLabelProcessorParams) == false)
        {
            _beatLabelProcessor.ComputeBeat(beatLabelProcessorParams);
            _oldBeatLabelProcessorParams = beatLabelProcessorParams.Clone();
        }
    }

    

    [SerializeField]
    public class BeatLabelProcessorParams : IEqualityComparer
    {
        [SerializeField]
        [Range(0.0001f, 0.025f)]
        public float allowedErrorRange;
        [SerializeField]
        [Range(0f, 1f)]
        public float fakeMedian;
        [SerializeField]
        [Range(10, 25)]
        public int beatFinderStepRange;
        [SerializeField]
        [Range(0.5f, 1f)]
        public float beatFinderHitThreshold;

        public BeatLabelProcessorParams(float allowedErrorRange, float fakeMedian, int beatFinderStepRange, float beatFinderHitThreshold)
        {
            this.allowedErrorRange = allowedErrorRange;
            this.fakeMedian = fakeMedian;
            this.beatFinderStepRange = beatFinderStepRange;
            this.beatFinderHitThreshold = beatFinderHitThreshold;
        }

        public BeatLabelProcessorParams Clone()
        {
            return new BeatLabelProcessorParams(allowedErrorRange, fakeMedian, beatFinderStepRange, beatFinderHitThreshold);
        }

        public override bool Equals(object obj)
        {
            var @params = obj as BeatLabelProcessorParams;
            return @params != null &&
                   allowedErrorRange == @params.allowedErrorRange &&
                   fakeMedian == @params.fakeMedian &&
                   beatFinderStepRange == @params.beatFinderStepRange &&
                   beatFinderHitThreshold == @params.beatFinderHitThreshold;
        }

        public new bool Equals(object x, object y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            var hashCode = 1339572219;
            hashCode = hashCode * -1521134295 + allowedErrorRange.GetHashCode();
            hashCode = hashCode * -1521134295 + fakeMedian.GetHashCode();
            hashCode = hashCode * -1521134295 + beatFinderStepRange.GetHashCode();
            hashCode = hashCode * -1521134295 + beatFinderHitThreshold.GetHashCode();
            return hashCode;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }

}
