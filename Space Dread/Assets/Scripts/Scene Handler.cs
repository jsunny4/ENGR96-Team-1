using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    /* Map: [ Docking Bay, Crdor, Storage,  Crdor  ]
    *       [ NIL,         Crdor, Character, Crdor  ]
    *      [ NIl,       Engines, Crdor,     Oxygen ]
    *
    * Map (Char array) Legend
    * - Spawn Area: 'S'
    * - Crdor: 'C'
    * - NIL: 'N'
    *
    * Non-adjacency List (i.e. edges where movement is not possible): 
    * [ , , D, D, ]
    * [ , , UD, U, ]
    * [ , , U, , ]
    */
    public readonly static (int,int) mapGridDims = (3, 4);
    public readonly static char[,] mapGrid = { {'S','C','S','C'},
                                               {'N','C','X','C'},
                                               {'N','S','C','S'} };
    public readonly static string[,] nonAdjList = { { "","","D","" },
                                                    { "","","UD","" },
                                                    { "","","U",""} };
    public bool [,] alienOccupation = new bool[3,5];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
