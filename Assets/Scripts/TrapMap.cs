using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMap : MonoBehaviour
{
    Dictionary<Vector3Int, List<GameObject>> trapMap;
    Grid grid;


    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponentInParent<Grid>();
        Debug.Log("Gameobject: " + gameObject);
        List<Transform> children = new List<Transform>(gameObject.transform.GetComponentsInChildren<Transform>());
        trapMap = new Dictionary<Vector3Int, List<GameObject>>();
        foreach (Transform transform in children)
        {
            GameObject gameObject = transform.gameObject;
            Vector3Int cellPos = grid.WorldToCell(gameObject.transform.position);
            if(gameObject.CompareTag("SpearTrap"))
            {
                AddGameObjectToDictionary(gameObject, cellPos);
                AddGameObjectToDictionary(gameObject, cellPos + Vector3Int.down);
            }
            else
            {
                AddGameObjectToDictionary(gameObject, cellPos);
            }
        }
    }

    void AddGameObjectToDictionary(GameObject gameObject, Vector3Int cellPos)
    {
        if(trapMap.ContainsKey(cellPos))
        {
            trapMap[cellPos].Add(gameObject);
        }
        else
        {
            trapMap.Add(cellPos, new List<GameObject>() { gameObject });
        }
    }

    public bool TileHasTrap(Vector3Int position)
    {
        return trapMap.ContainsKey(position);
    }

    public List<GameObject> GetTrapTileList(Vector3Int position)
    {
        return new List<GameObject>(trapMap[position]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
