using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    [System.Serializable]
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

    [SerializeField]
    TextAsset _beatLabelFile;
    [SerializeField]
    AudioClip _audioClip;
    [SerializeField]
    BeatLabelProcessorParams beatLabelProcessorParams = new BeatLabelProcessorParams(0.01f, 0.75f, 15, 0.65f);
    [SerializeField]
    AudioSource _audioSource;

    Beat _beat;

    BeatLabelProcessorParams oldBeatLabelProcessorParams;

    BeatLabelProcessor beatLabelProcessor;

    // Start is called before the first frame update
    void Start()
    {
        beatLabelProcessor = new BeatLabelProcessor(_beatLabelFile);
        oldBeatLabelProcessorParams = null;
        _audioSource.clip = _audioClip;

        _beat = new Beat(0.5f, 16.12f);
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_beat.UpdateBeat(_audioSource.time))
        {
            // Update Beats here
        }
    }

    void updateBeatprocessor()
    {
        if(beatLabelProcessorParams.Equals(oldBeatLabelProcessorParams) == false)
        {
            beatLabelProcessor.ComputeBeat(beatLabelProcessorParams);
            oldBeatLabelProcessorParams = beatLabelProcessorParams.Clone();
        }
    }
}
