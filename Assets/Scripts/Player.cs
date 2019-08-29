using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//TODO: Remove
public class Player : TileAnimator
{

    [Header("Player Fields")]
    [SerializeField]
    int _playerIndex;
    [SerializeField]
    public ComboBarController comboBarController;
    [SerializeField]
    private Grid _grid;
    private Tilemap _collisionMap;
    private Tilemap _goalMap;
    private TrapMap _trapMap;
    
    [SerializeField]
    private Vector2Int _position; 

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
    private Sprite[] deathAnim;
    [SerializeField]
    private Sprite[] trapDoorAnim;

    [SerializeField]
    private EDirection _currentDirection = EDirection.DOWN;

    private EDirection _nextDirection = EDirection.NONE;
    private EDirection _currentlyQueuedDirection = EDirection.NONE;
    private bool _targetBlocked = false;
    private int _currentOffBeatCounter = 0;
    private SpawnPlayer _spawner = null;

    private bool _isDead;
    
    private BeatController _beatController;
    private InputManager _inputManager;

    private List<GameObject> _currentTrapTileList;

    private void Awake()
    {
        calculatePosition();
    }

    public void calculatePosition()
    {
        _position = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
    }

    // Start is called before the first frame update
    void Start()
    {
        _beatController = BeatController.GetInstance();
        _beatController.BeatSubject.AddObserver(this);
        _inputManager = InputManager.GetInstance();
        changeDirection(_currentDirection);
        Debug.Log("Starting animation");
        _grid = GameObject.FindGameObjectWithTag("TileGrid").GetComponent<Grid>();
        _collisionMap = GameObject.FindGameObjectWithTag("ColliderMap").GetComponent<Tilemap>();
        _goalMap = GameObject.FindGameObjectWithTag("GoalMap").GetComponent<Tilemap>();
        _trapMap = GameObject.FindGameObjectWithTag("TrapMap").GetComponent<TrapMap>();
        _currentTrapTileList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    public void SetSpawner(SpawnPlayer spawner)
    {
        this._spawner = spawner;
    }

    void HandleInput()
    {
        EDirection direction = _inputManager.GetDirectionInputForPlayer(_playerIndex);

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
            if(_currentlyQueuedDirection != EDirection.NONE && _isDead == false)
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
            if (_isDead)
            {
                BeatController.GetInstance().BeatSubject.RemoveObserver(this);
                this._spawner.activate();
                Destroy(this.gameObject);
            }
        } 
    }

    void startMovement()
    {
        _nextDirection = _currentlyQueuedDirection;
        _currentlyQueuedDirection = EDirection.NONE;
        changeDirection(_nextDirection);
        Vector2Int nextTilePos = _position + _inputManager.GetVector(_nextDirection);
        Vector3Int nextPos = _grid.LocalToCell(new Vector3(nextTilePos.x, nextTilePos.y));
        TileBase tile = _collisionMap.GetTile(nextPos);
        if(tile != null)
        {
            Debug.Log("Tile should be blocked: " + tile.ToString());
            _targetBlocked = true;
        }
        else
        {
            _position = nextTilePos;
            _currentTrapTileList.Clear();
            if(_trapMap.TileHasTrap(nextPos))
            {
                GameObject trapTile = _trapMap.GetTrapTileList(nextPos)[0];
                Debug.Log("trapTile: " + (trapTile != null));

                if (trapTile != null)
                {
                    _currentTrapTileList = _trapMap.GetTrapTileList(nextPos);
                }  
            }
        }
    }

    void endMovement()
    {
        Vector3Int currentPos = _grid.LocalToCell(new Vector3(_position.x, _position.y));
        TileBase tile = _goalMap.GetTile(currentPos); 
        if(tile != null)
        {
            Debug.Log("Player " + gameObject.name + " Reached Goal -> Should end Game");
            CanvasGroup endScreen = GameObject.FindGameObjectWithTag("EndScreen").GetComponent<CanvasGroup>();
            endScreen.alpha = 1f;
            TMPro.TextMeshProUGUI textMash = GameObject.FindGameObjectWithTag("WinnerText").GetComponent<TMPro.TextMeshProUGUI>();
            textMash.text = "Player " + (_playerIndex + 1) + " won the Round!";
        }
    }

    void MovePlayer(int offBeatCounter)
    {
        Vector2Int moveVec = _inputManager.GetVector(_nextDirection);

        if(_nextDirection != EDirection.NONE)
        {
            if (_targetBlocked && offBeatCounter == 4)
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
                _targetBlocked = false;
            }
        }
    }

    public override void OnBeat()
    {
        if (_isDead)
        {
            //this.enabled = false;
            //this.GetComponent<TileAnimator>().enabled = false;
            //this.get
            Debug.Log("isdead");
            BeatController.GetInstance().BeatSubject.RemoveObserver(this);
            Destroy(this.gameObject);
        }
        base.OnBeat();

        endMovement();

        foreach (GameObject gameObject in _currentTrapTileList)
        {
            Debug.Log("Currently on trap " + gameObject.name);
            TileAnimator tileAnimator = gameObject.GetComponent<TileAnimator>();
            if (tileAnimator != null && tileAnimator.isActive)
            {
                switch (gameObject.tag)
                {
                    case "SpikeTrap":
                        sprites = deathAnim;
                        break;
                    case "Lava":
                        sprites = deathAnim;
                        break;
                    case "SpearTrap":
                        sprites = deathAnim;
                        break;
                }
                _isDead = true;
            }
            TrapDoorAnimator trapDoorAnimator = gameObject.GetComponent<TrapDoorAnimator>();
            if (trapDoorAnimator != null && gameObject.tag == "TrapDoor")
            {
                if(trapDoorAnimator.isOpen)
                {
                    _isDead = true;
                    sprites = trapDoorAnim;
                } 
                else
                {
                    trapDoorAnimator.isActivated = true;
                }
            }
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
