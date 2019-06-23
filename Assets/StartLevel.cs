using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    [SerializeField]
    List<GameObject> levelSwitches; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int next_level = -1;
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Debug.Log(player.transform.position);
            for (int i = 0; i < levelSwitches.Count; i++){
                GameObject level = levelSwitches[i];
                Debug.Log(Vector3.Distance(player.transform.position, level.transform.position));
                if ( Vector3.Distance(player.transform.position, level.transform.position) < 0.5f)
                {
                    next_level = i + 1;
                }
            }
        }
        if (next_level != -1)
        {
            SceneManager.LoadScene(next_level);

        }
    }
}
