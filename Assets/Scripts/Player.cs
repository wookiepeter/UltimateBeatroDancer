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
    bool targetBlocked = false;
    int _currentOffBeatCounter = 0; 
    

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

    void HandleInput()
    {
        EDirection direction = _inputManager.GetDirectionInputForPlayer(playerIndex);

        if (direction != EDirection.NONE)
        {
            Debug.Log("Received Direction");
            HandleInputDirectionPress(direction);
        }
    }

    void HandleInputDirectionPress(EDirection direction)
    {
        if (_nextDirection == EDirection.NONE && _beatController.CurrentlyInInputWindow() && _currentOffBeatCounter < 2)
        {
            Debug.Log("Received valid inputdirection: " + _nextDirection.ToString());
            comboBarController.SetBar(true);
            startMovement(direction);
        }
        else
        {
            comboBarController.SetBar(false);
            if (_nextDirection != EDirection.NONE)
            {
                Debug.Log("Next move was already set -> Invalid Input! -> You lose some focus");
            }
            else
            {
                Debug.Log("Currently not in Input window -> invalid button press!");
            }
        }
    }

    void startMovement(EDirection direction)
    {
        _nextDirection = direction;
        changeDirection(_nextDirection);
    }

    public override void OffBeat(int offBeatCounter)
    {
        _currentOffBeatCounter = offBeatCounter;
        MovePlayer(offBeatCounter);
        base.OffBeat(offBeatCounter);
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
