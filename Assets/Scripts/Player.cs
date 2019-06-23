using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TileAnimator
{

    [Header("Player Fields")]


    [SerializeField]
    int playerIndex;
    [SerializeField]
    ComboBarController comboBarController;

    [Header("Player Animation")]
    [SerializeField]
    private Sprite[] upWardsAnim;
    [SerializeField]
    private Sprite[] rightAnim;
    [SerializeField]
    private Sprite[] downWardsAnim;
    [SerializeField]
    private Sprite[] leftAnim;

    [SerializeField]
    EDirection _currentDirection = EDirection.UP;

    EDirection _nextDirection = EDirection.NONE;
    EDirection _currentlyQueuedDirection = EDirection.NONE;
    bool targetBlocked = false;
    int _currentOffBeatCounter = 0;
    SpawnPlayer spawner = null;
    

    BeatController _beatController;
    InputManager _inputManager;

    // Start is called before the first frame update
    void Start()
    {
        _beatController = BeatController.GetInstance();
        _beatController.BeatSubject.AddObserver(this);
        _inputManager = InputManager.GetInstance();
        changeDirection(_currentDirection);
        Debug.Log("Starting animation");
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void SetSpawner(SpawnPlayer spawner)
    {
        this.spawner = spawner;
    }

    void HandleInput()
    {
        EDirection direction = _inputManager.GetDirectionInputForPlayer(playerIndex);

        HandleInputDirectionPress(direction);
    }

    void HandleInputDirectionPress(EDirection direction)
    {
        if (direction != EDirection.NONE)
        {
            Debug.Log("Received valid inputdirection: " + _nextDirection.ToString());
            _currentlyQueuedDirection = direction;
        }
        else
        {
            Debug.Log("Currently not in Input window -> invalid button press!");
        }
    }

    public override void OffBeat(int offBeatCounter)
    {
        _currentOffBeatCounter = offBeatCounter;
        MovePlayer(offBeatCounter);
        base.OffBeat(offBeatCounter);
        if(offBeatCounter == 1)
        { 
            if(_currentlyQueuedDirection != EDirection.NONE)
            {
                startMovement();
                comboBarController.SetBar(true);
            }
            else
            {
                comboBarController.SetBar(false);
            }
        }
        if (offBeatCounter == 7)
        {
            comboBarController.ResetIndicator();
        } 
    }

    void startMovement()
    {
        _nextDirection = _currentlyQueuedDirection;
        _currentlyQueuedDirection = EDirection.NONE;
        changeDirection(_nextDirection);
    }

    void MovePlayer(int offBeatCounter)
    {
        Vector2Int moveVec = _inputManager.GetVector(_nextDirection);

        if(_nextDirection != EDirection.NONE)
        {
            if (targetBlocked && offBeatCounter == 4)
            {
                moveVec *= -1;
                transform.position += 0.625f * new Vector3(moveVec.x, moveVec.y);
            }
            else if (offBeatCounter == 2 || offBeatCounter == 4)
            {
                float moveDistance = (offBeatCounter == 2) ? 0.625f : 0.375f;
                transform.position += moveDistance  * new Vector3(moveVec.x, moveVec.y);
            }

            if (offBeatCounter > 4)
            {
                _nextDirection = EDirection.NONE;
                targetBlocked = false;
            }
        }
    }

    public override void OnBeat()
    {
        /*
        if(_currentDirection == EDirection.LEFT)
        {
            _currentDirection = EDirection.UP;
        } else {
            _currentDirection += 1;
        }
        changeDirection(_currentDirection);
        Debug.Log("Changing Direction");
        */
        base.OnBeat();
    }

    // should be called on Beat!
    void changeDirection(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.UP:
                sprites = upWardsAnim;
                break;
            case EDirection.RIGHT:
                sprites = rightAnim;
                break;
            case EDirection.DOWN:
                sprites = downWardsAnim;
                break;
            case EDirection.LEFT:
                sprites = leftAnim; 
                break;
        }
    }
}
