using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : TileAnimator
{

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

    Dictionary<EDirection, Sprite[]> movementDictionary;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        movementDictionary = createMoveAnimationDictionary();
        playerController = transform.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Dictionary<EDirection, Sprite[]> createMoveAnimationDictionary()
    {
        Dictionary<EDirection, Sprite[]> result = new Dictionary<EDirection, Sprite[]>();
        result.Add(EDirection.DOWN, downWardsAnim);
        result.Add(EDirection.NONE, downWardsAnim);
        result.Add(EDirection.LEFT, leftAnim);
        result.Add(EDirection.RIGHT, rightAnim);
        result.Add(EDirection.UP, upWardsAnim);
        return result;
    }
}
