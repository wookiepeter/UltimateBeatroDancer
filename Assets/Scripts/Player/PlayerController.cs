using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState
{
    MOVING, 
    DYING,
}

public enum ERythmMode
{
    TimedOnly,
    SnapToBeat,
}

public class PlayerController : MonoBehaviour
{

    [Header("Player Fields")]
    [SerializeField]
    private int _playerIndex;
    [SerializeField]
    private ERythmMode _rythmMode;
    [SerializeField, Range(0f, 1f)]
    private float _beatBlockThreshhold;

    private InputManager _inputManager;
    private BeatController _beatController;

    // input helper 
    private float _lastMoveTime = 0f;

    public float BeatBlockThreshhold { get => _beatBlockThreshhold; }
    public ERythmMode RythmMode { get => _rythmMode; }
    public float LastMoveTime { get => _lastMoveTime; }



    // Start is called before the first frame update
    void Start()
    {
        _inputManager = InputManager.GetInstance();
        _beatController = BeatController.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        EDirection direction = _inputManager.GetDirectionInputForPlayer(_playerIndex);

        if (direction != EDirection.NONE)
        {
            switch (RythmMode)
            {
                case ERythmMode.TimedOnly:
                    handleTimedOnlyInput(direction);
                    break;
                case ERythmMode.SnapToBeat:
                    handleSnapToBeatInput(direction);
                    break;
            }
        }
    }

    void handleTimedOnlyInput(EDirection eDirection)
    {
        bool isValidInputTime = checkInputTiming();

        if (isValidInputTime)
        {
            
        }
        else
        {
            Debug.Log("Received quick consecutive and therefore invalid inputs");
            Debug.Log("Time since last input: " + (Time.deltaTime - _lastMoveTime)
                + " min time between inputs: " + (_beatController.ActualBeatTime * BeatBlockThreshhold));
        }
    }

    void handleSnapToBeatInput(EDirection eDirection)
    {
        
    }
    
    private bool checkInputTiming()
    {
        bool result = false;
        if (Time.deltaTime - LastMoveTime >= _beatController.ActualBeatTime * BeatBlockThreshhold)
        {
            result = true;
            _lastMoveTime = Time.deltaTime;
        }
        return result;
    }
}
