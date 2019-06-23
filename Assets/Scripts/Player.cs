using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : TileAnimator
{

    [Header("Player Fields")]


    [SerializeField]
    int playerIndex;
    [SerializeField]
    public ComboBarController comboBarController;
    [SerializeField]
    Grid grid;
    Tilemap collisionMap;
    TrapMap trapMap;
    
    [SerializeField]
    Vector2Int position; 

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
    private Sprite[] deatAnim;
    [SerializeField]
    private Sprite[] trapDoorAnim;

    [SerializeField]
    EDirection _currentDirection = EDirection.DOWN;

    EDirection _nextDirection = EDirection.NONE;
    EDirection _currentlyQueuedDirection = EDirection.NONE;
    bool targetBlocked = false;
    int _currentOffBeatCounter = 0;
    SpawnPlayer spawner = null;

    bool isDead;
    

    BeatController _beatController;
    InputManager _inputManager;

    List<GameObject> currentTrapTileList;

    private void Awake()
    {
        calculatePosition();
    }

    public void calculatePosition()
    {
        position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }

    // Start is called before the first frame update
    void Start()
    {
        _beatController = BeatController.GetInstance();
        _beatController.BeatSubject.AddObserver(this);
        _inputManager = InputManager.GetInstance();
        changeDirection(_currentDirection);
        Debug.Log("Starting animation");
        grid = GameObject.FindGameObjectWithTag("TileGrid").GetComponent<Grid>();
        collisionMap = GameObject.FindGameObjectWithTag("ColliderMap").GetComponent<Tilemap>();
        trapMap = GameObject.FindGameObjectWithTag("TrapMap").GetComponent<TrapMap>();
        currentTrapTileList = new List<GameObject>();
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
            //Debug.Log("Currently not in Input window -> invalid button press!");
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
            if (isDead)
            {
                BeatController.GetInstance().BeatSubject.RemoveObserver(this);
                this.spawner.activate();
                Destroy(this.gameObject);
            }
        } 
    }

    void startMovement()
    {
        _nextDirection = _currentlyQueuedDirection;
        _currentlyQueuedDirection = EDirection.NONE;
        changeDirection(_nextDirection);
        Vector2Int nextTilePos = position + _inputManager.GetVector(_nextDirection);
        Vector3Int nextPos = grid.LocalToCell(new Vector3(nextTilePos.x, nextTilePos.y));
        TileBase tile = collisionMap.GetTile(nextPos);
        if(tile != null)
        {
            Debug.Log("Tile should be blocked: " + tile.ToString());
            targetBlocked = true;
        }
        else
        {
            currentTrapTileList.Clear();
            position = nextTilePos;
            if(trapMap.TileHasTrap(nextPos))
            {
                GameObject trapTile = trapMap.GetTrapTileList(nextPos)[0];
                Debug.Log("trapTile: " + (trapTile != null));

                if (trapTile != null)
                {
                    currentTrapTileList = trapMap.GetTrapTileList(nextPos);
                }  
            }
        }
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

        if (isDead)
        {
            //this.enabled = false;
            //this.GetComponent<TileAnimator>().enabled = false;
            //this.get
            Debug.Log("isdead");
            BeatController.GetInstance().BeatSubject.RemoveObserver(this);
            Destroy(this.gameObject);
        }
        base.OnBeat();
    

        foreach (GameObject gameObject in currentTrapTileList)
        {
            Debug.Log("Currently on trap " + gameObject.name);
            switch (gameObject.tag)
            {
                case "TrapDoor":
                    sprites = trapDoorAnim;
                    break;
                case "SpikeTrap":
                    sprites = deatAnim;

                    break;
                case "Lava":
                    sprites = deatAnim;
                    break;
                case "SpearTrap":
                    sprites = deatAnim;
                    break;
            }
            isDead = true;
        }
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
