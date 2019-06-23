using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum EDirection
{
    UP,
    RIGHT, 
    DOWN, 
    LEFT,
    NONE,
}

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("Cannot request input manager before Awake() was called!");
        }
        return instance;
    }

    [SerializeField]
    InputRequester _inputRequester;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputRequester.InputButtonDown(EInputButtons.Start))
        {
            SceneManager.LoadScene(0);
        }
    }

    public EDirection GetDirectionInputForPlayer(int playerIndex)
    {
        EInputButtons[] inputButtons = new EInputButtons[] { EInputButtons.A, EInputButtons.B, EInputButtons.X, EInputButtons.Y};
        foreach (EInputButtons button in inputButtons)
        {
            if( _inputRequester.InputButtonDown(button, playerIndex))
            {
                return GetDirectionFromButton(button);
            }
        }
        return EDirection.NONE;
    }

    public EDirection GetDirectionFromButton(EInputButtons inputButtons)
    {
        switch (inputButtons)
        {
            case EInputButtons.A:
                return EDirection.DOWN;
            case EInputButtons.B:
                return EDirection.RIGHT;
            case EInputButtons.X:
                return EDirection.LEFT;
            case EInputButtons.Y:
                return EDirection.UP;
            default:
                return EDirection.NONE;
        }
    }

    public Vector2Int GetVector(EDirection direction)
    {
        switch (direction)
        {
            case EDirection.UP:
                return Vector2Int.up;
            case EDirection.RIGHT:
                return Vector2Int.right;
            case EDirection.DOWN:
                return Vector2Int.down;
            case EDirection.LEFT:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }
}