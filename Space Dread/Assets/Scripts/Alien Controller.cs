using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class AlienController : MonoBehaviour
{
  private float timer = 0.0f, waitTime, initialWaitTime = 100.0f;
  private float actionTimeLowerBound = 6.0f, actoinTimeUpperBound = 8.0f;

  private GameObject[,] alienToggles = new GameObject[3,4];
  private GameObject[] spotLights;
  private Alien alien1 = new Alien();
  
  // Start is called before the first frame update
  void Start()
  {
    // Populating alienToggles and deactivate all of them
    GameObject[] alienPlaceholders = GameObject.FindGameObjectsWithTag("Alien Placeholder");
    foreach (GameObject alienPlaceholder in alienPlaceholders){
      if(alienPlaceholder!=null){
        int row = alienPlaceholder.name[0]-'0'-1;
        int col = alienPlaceholder.name[2]-'0'-1;
        alienToggles[row,col] = alienPlaceholder;
        alienToggles[row,col].SetActive(false);
      }
    }

    //Populate spotLights
    spotLights = GameObject.FindGameObjectsWithTag("Spot Light");
    foreach(GameObject spotlight in spotLights){
      spotlight.GetComponent<Light>().intensity = 1;
    }

    // Initialize waitTime because Range() cannot be called in instance field initializer space
    waitTime = UnityEngine.Random.Range(actionTimeLowerBound, actoinTimeUpperBound);
  }

  // Update is called once per frame
  void Update()
  {
    (int numRows, int numCols) = SceneHandler.mapGridDims;

    foreach(GameObject spotlight in spotLights){
      Light light = spotlight.GetComponent<Light>();

      if(light.intensity>1){
        foreach((int,int) despawnPos in SceneHandler.despawnDict[spotlight.name]){
          // Unspawn alien if it is in the position where it is flashed
          if(alien1.Position==despawnPos){
            alien1.Unspawn(alienToggles);
            waitTime += 2.0f;
          } 
        }
      }
    }

    timer += Time.deltaTime;
    if(timer>waitTime+initialWaitTime){
      alien1.Action(alienToggles);

      timer -= waitTime+initialWaitTime;
      waitTime = UnityEngine.Random.Range(actionTimeLowerBound, actoinTimeUpperBound);
      initialWaitTime = 0;
    }
  }
}

class Alien {

  public bool IfExist { get; private set; } = false;
  public (int,int) Position { get; private set; } = (-1,-1); // (x,y)

  private readonly int[] dx = {0,1,0,-1}; // Enumerate horizontal mvment
  private readonly int[] dy = {1,0,-1,0}; // Enumerate vertical mvment
  private readonly char[] dir = {'D','R','U','L'}; // Corresponding dir
  private readonly List<(int,int)> spawns;

  public Alien(){
    (int numRows, int numCols) = SceneHandler.mapGridDims; // Unpack dims

    // Populate spawns with the coordinates of spawn locs
    spawns = new List<(int,int)>();
    for(int i=0; i<numRows; i++){
      for(int j=0; j<numCols; j++){
        if(SceneHandler.mapGrid[i,j]=='S') spawns.Add((j,i));
      }
    }
  }

  // Spawns alien
  public bool Spawn(GameObject[,] alienToggles){
    // Default unspawned members
    if(IfExist || Position!=(-1,-1)) return false;
    
    // Set IfExist and choose random index of spawns
    IfExist = true;
    int chosenInd = UnityEngine.Random.Range(0,spawns.Count);

    // Set Position, deactivate alienToggles[y,x]
    (int x, int y) = spawns[chosenInd];
    Position = (x,y);
    alienToggles[y,x].SetActive(true);

    Debug.Log("Alien spawn at (" + (y+1).ToString() + "," + (x+1).ToString() + ")");
    
    return true;
  }

  // Unspawns alien
  public bool Unspawn(GameObject[,] alienToggles){
    if(!IfExist || Position==(-1,-1)) return false;

    // Set IfExist and Position, Deactivate alienToggles[y,x]
    IfExist = false;
    (int x, int y) = Position;
    alienToggles[y,x].SetActive(false);
    Position = (-1,-1);

    Debug.Log("Alien unspawn from (" + (y+1).ToString() + "," + (x+1).ToString() + ")");

    return true;
  }
  
  // Moves the alien object to random location 1 step away
  public bool Move(GameObject[,] alienToggles){
    if(!IfExist || Position==(-1,-1)) return false;

    (int numRows, int numCols) = SceneHandler.mapGridDims; // Unpack dims
    (int x, int y) = Position; // 0-indexed
    
    List<int> dirs = new List<int>();
    char[,] mapGrid = SceneHandler.mapGrid;
    for(int i=0; i<4; i++){

      // Check if direction is in Non-adjacency List
      bool noGo = false;
      string[,] nonAdjList = SceneHandler.nonAdjList;
      for(int j=0; j<nonAdjList[y,x].Length; j++){
        if(dir[i]==nonAdjList[y,x][j]) noGo = true;
      }
      if(noGo) continue; // If it is, check next dir

      // Check for out-of-bounds movement
      if(y+dy[i]<0 || y+dy[i]>numRows-1) continue;
      if(x+dx[i]<0 || x+dx[i]>numCols-1) continue;
      // Check for movement into a NIL map position
      if(mapGrid[y+dy[i],x+dx[i]]=='N') continue;

      // If the program reaches this point, dir[i] is valid, add to dirs
      dirs.Add(i);

      // Check if the dir is towards the player, if so add it again to increase the probability of stepping in the direction
      string[,] dirToPlayer = SceneHandler.dirToPlayer;
      for(int j=0; j<dirToPlayer[y,x].Length; j++){
        if(dir[i]==dirToPlayer[y,x][j]){
          dirs.Add(i);
          dirs.Add(i);
        }
      }
    }

    // If there is no dir to move in, return false
    if(dirs.Count==0) return false;

    // Choose random index of dirs
    int chosenInd = UnityEngine.Random.Range(0,dirs.Count);

    // Complete the move step
    alienToggles[y,x].SetActive(false);
    x += dx[dirs[chosenInd]];
    y += dy[dirs[chosenInd]];
    Position = (x,y);
    alienToggles[y,x].SetActive(true);
    
    return true;
  }

  // Wrapper for an alien action to either spawn or move
  // Also handles lose condition
  public bool Action(GameObject[,] alienToggles){
    bool ActSuccess = false;
    if(!IfExist || Position==(-1,-1)) ActSuccess = Spawn(alienToggles);
    else ActSuccess = Move(alienToggles);

    // Load Game Over Scene if alien reaches player position
    if(Position==SceneHandler.playerPos){
      SceneManager.LoadScene(SceneHandler.gameOverSceneName);
    }

    return ActSuccess;
  }
}