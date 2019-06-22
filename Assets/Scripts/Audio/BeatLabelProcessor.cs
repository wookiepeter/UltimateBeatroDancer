using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatLabelProcessor 
{
    TextAsset _beatFile;
    char[] _lineSplitter;

   public BeatLabelProcessor(TextAsset beatFile)
    {
        _beatFile = beatFile;
        _lineSplitter = new char[] { ' ', '\t' };
    }

    public Beat ComputeBeat(BeatController.BeatLabelProcessorParams beatParams)
    {
        Beat beat = null;
        if(_beatFile == null)
        {
            Debug.LogError("No BeatLabel File handed to BeatProcessor!");
            return beat;
        }

        float[] beatTimes = parseBeatTimes();
        if (beatTimes.Length < 100)
        {
            Debug.LogError("Cannot parse Beattimes -> " + beatTimes.Length + " beats are not enough!");
        }
        else
        {
            float[] beatTimeDifferences = computeBeatDifferences(beatTimes);

            System.Array.Sort(beatTimeDifferences);

            float predictedBeatTimeDiff = beatTimeDifferences[(int)(beatTimeDifferences.Length * beatParams.fakeMedian)];

            //foreach(float f in beatTimeDifferences)
            //{
            //    Debug.Log("BeatTime: " + f);
            //}

            float firstBeatTime = FindFirstBeatTime(beatTimes, predictedBeatTimeDiff, beatParams);

            beat = new Beat(predictedBeatTimeDiff, firstBeatTime);

            Debug.Log(beat);
        } 
        return beat;
    }

    private float[] parseBeatTimes()
    {
        string[] lines = _beatFile.text.Split('\n');
        int numberOfLines = lines.Length;

        float[] overSizedBeatTimesArray = new float[numberOfLines];
        int numberOfBeats = 0;

        for (int i = 0; i < numberOfLines; i++)
        {
            if (lines[i].Length > 5)
            {
                overSizedBeatTimesArray[numberOfBeats] = ParseLine(lines[i]);
                numberOfBeats++;
            }
        }

        float[] beatTimes = new float[numberOfBeats];
        System.Array.Copy(overSizedBeatTimesArray, beatTimes, numberOfBeats);

        return beatTimes;
    }

    private float[] computeBeatDifferences(float[] beatTimes)
    {
        float[] result = new float[beatTimes.Length - 1];
        for(int i = 0; i < beatTimes.Length - 1; i++)
        {
            result[i] = beatTimes[i+1] - beatTimes[i];
        }
        return result;
    }

    private float FindFirstBeatTime(float[] beatTimes, float predictedBeatTimeDiff, BeatController.BeatLabelProcessorParams beatParams)
    {
        float result = 0;


        int requiredHits = (int)((float)beatParams.beatFinderStepRange * beatParams.beatFinderHitThreshold);
        int allowedMisses = beatParams.beatFinderStepRange - requiredHits;

        for(int beatIndex = 0; beatIndex < beatTimes.Length - beatParams.beatFinderStepRange - 1; beatIndex++)
        {
            float currentBeatTime = beatTimes[beatIndex];
            float predictedNextBeat = currentBeatTime + predictedBeatTimeDiff;
            int missCounter = 0;
            float nextTime = 0;
            float error = 0;
            bool isStartingBeat = false;
            for (int i = beatIndex + 1; i < beatIndex + beatParams.beatFinderStepRange; i++)
            {
                nextTime = beatTimes[i];
                while (nextTime > predictedNextBeat + beatParams.allowedErrorRange)
                {
                    predictedNextBeat += predictedBeatTimeDiff;
                    // didnt miss miss beat but skipped too much!
                    missCounter++;
                }

                error = Mathf.Abs(predictedNextBeat - nextTime); 
                if (error > beatParams.allowedErrorRange)
                {
                    missCounter++;
                }

                if ((i - beatIndex) - missCounter > requiredHits)
                {
                    isStartingBeat = true;
                    result = currentBeatTime;
                    break;
                } else if (missCounter > allowedMisses)
                {
                    // too many misses -> try next beat
                    break;
                }
            }
        }

        return result;
    }

    private float ParseLine(string line)
    {
        string firstFloat = line.Split(_lineSplitter)[0];
        return float.Parse(firstFloat);
    }
}
