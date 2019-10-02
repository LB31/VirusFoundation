using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CreateLevel : MonoBehaviour
{
    //The tiles have been modeled as 4x4 unity unit squares
    private const float tileSize = 4;

    private GameObject root, floor, environment, ball;
    public int xHalfExt = 1;
    public int zHalfExt = 1;

    public GameObject outerWall;
    public GameObject innerWall;
    public GameObject exitTile;
    public GameObject[] floorTiles;

    private int xExt, zExt;
    private int start, end;

    public Cell[] playfield;
    private Cell current;
    private int exitCell;
    private Vector3 resetPosition;

    private Transform outerWallsParent;
    private Transform innerWallsParent;
    private Transform tilesParent;

    public LocalNavMeshBuilder lnmb;

    // Use this for initialization
    void Awake()
    {
        lnmb = GetComponent<LocalNavMeshBuilder>();

        //Gather together all refrences you will need later on
        root = GameObject.Find("MovablePlayfield");
        floor = GameObject.Find("DSBasementFloor");
        environment = GameObject.Find("Environment");
        ball = GameObject.Find("DSPlayerBall");

        //Build the values for xExt and zExt from xHalfExt and zHalfExt
        xExt = 2 * xHalfExt + 1;
        zExt = 2 * zHalfExt + 1;

        //Build an offset for the dyn playfield from the BasePlatform e.g. the bigger halfExtent value in unity units
        Transform maze = GameObject.Find("Maze").transform;
        float biggerHalf = Mathf.Max(xHalfExt, zHalfExt);
        //Vector3 f = floor.transform.position;
        //maze.localPosition = new Vector3(f.x, f.y + biggerHalf, f.z);

        //Calculate a scale factor for scaling the non-movable environment (and therefore the camera) and the BasePlatform 
        float scaleFactor = Mathf.Max(xExt, zExt);

        ////Scale Environment
        //Vector3 oldScaleEnv = environment.transform.localScale;
        //environment.transform.localScale = new Vector3(oldScaleEnv.x * scaleFactor / 3, oldScaleEnv.y * scaleFactor / 3, oldScaleEnv.z * scaleFactor / 3);
        
        //Scale  + position BasePlate
        //Vector3 oldScaleFloor = floor.transform.localScale;
        //floor.transform.localScale = new Vector3(oldScaleFloor.x * scaleFactor / 3, oldScaleFloor.y, oldScaleFloor.z * scaleFactor / 3);


            //Create the outer walls for given extXZ
            outerWallsParent = GameObject.Find("OuterWalls").transform;
            CreateOuterWalls();

            //create a maze
            CalcMaze();

            //Build the maze from the given set of prefabs
            tilesParent = GameObject.Find("Tiles").transform;
            innerWallsParent = GameObject.Find("InnerWalls").transform;
            BuildMaze();

            //Set the walls for the maze (place only one wall between two cells, not two!)
            // already done in CalcMaze()

            //Place the PlayerBall above the playfield
            resetPosition = new Vector3(xExt * 2 - 2, 10, -zExt * 2 + 2);
            placeBallStart(resetPosition);

            // Make nav mesh
            lnmb.enabled = true;


    }

    void CreateOuterWalls() {
        int outerX = xExt * 2;
        float heightOffsetX = outerWall.transform.lossyScale.y / 2f;
        for (int i = 0; i < zExt; i++) {
            Instantiate(outerWall, new Vector3(outerX, heightOffsetX, (i - zHalfExt) * tileSize), Quaternion.Euler(0, 90, 0), outerWallsParent); // right
            Instantiate(outerWall, new Vector3(-outerX, heightOffsetX, (i - zHalfExt) * tileSize), Quaternion.Euler(0, 90, 0), outerWallsParent); // left
        }
        int outerZ = zExt * 2;
        float heightOffsetZ = outerWall.transform.lossyScale.y / 2f;
        for (int i = 0; i < xExt; i++) {
            Instantiate(outerWall, new Vector3((i - xHalfExt) * tileSize, heightOffsetZ, outerZ), Quaternion.Euler(0, 0, 0), outerWallsParent); // top
            Instantiate(outerWall, new Vector3((i - xHalfExt) * tileSize, heightOffsetZ, -outerZ), Quaternion.Euler(0, 0, 0), outerWallsParent); // down
        }
    }

    void BuildMaze() {
        int startX = xExt * 2;
        int startZ = zExt * 2;
        for (int i = 0; i < playfield.Length; i++) {
            int rand = Random.Range(0, floorTiles.Length);
            GameObject tileToUse;
            if (i == exitCell) tileToUse = exitTile;
            else tileToUse = floorTiles[rand];
            Vector2 pos = GetCoordinates(i);
            float x = -startX + pos.x * tileSize + 2;
            float z = startZ - pos.y * tileSize - 2;
            GameObject tile = Instantiate(tileToUse, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0), tilesParent);
            if(tile.name.Contains("Exit"))
            GameManager.Instance.ExitLevel1 = tile.transform;

            Cell c = playfield[i];
            if (c.walls["UP"]) Instantiate(innerWall, new Vector3(x, 0, z + 2), Quaternion.Euler(0, 0, 0), innerWallsParent);
            if (c.walls["DOWN"]) Instantiate(innerWall, new Vector3(x, 0, z - 2), Quaternion.Euler(0, 0, 0), innerWallsParent);
            if (c.walls["LEFT"]) Instantiate(innerWall, new Vector3(x - 2, 0, z), Quaternion.Euler(0, 90, 0), innerWallsParent);
            if (c.walls["RIGHT"]) Instantiate(innerWall, new Vector3(x + 2, 0, z), Quaternion.Euler(0, 90, 0), innerWallsParent);
        }
    }

    void CalcMaze() {
        playfield = new Cell[xExt * zExt];
        for (int i = 0; i < playfield.Length; i++) {
            playfield[i] = new Cell();
            playfield[i].position = GetCoordinates(i);
        }
        // Make the initial cell the current cell and mark it as visited :Recursive backtracker
        current = playfield[0];
        current.visited = true;
        Stack gameStack = new Stack();
        int unvisitedCells = (xExt * zExt) - 1;
        exitCell = 0;
        while (unvisitedCells > 0) { // While there are unvisited cells :Recursive backtracker
            
            Dictionary<string, int> neighbours = GetNeighbours(current.position);

            // Remove the outside inner walls
            foreach (KeyValuePair<string, int> entry in neighbours) {
                if (entry.Value == -1) current.walls[entry.Key] = false;
            }

            if (neighbours.Any(dir => dir.Value >= 0 && dir.Value < xExt * zExt && !playfield[dir.Value].visited)) { // If the current cell has any neighbours which have not been visited :Recursive backtracker
                // Choose randomly one of the unvisited neighbours :Recursive backtracker
                List<int> unvisitedNeighbours = neighbours.Where(dir => dir.Value >= 0 && dir.Value < xExt * zExt && !playfield[dir.Value].visited).Select(x => x.Value).ToList();
                Cell chosenNeighbour = playfield[unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)]];
                
                gameStack.Push(current); // Push the current cell to the stack :Recursive backtracker

                // Remove the walls between the current cell and the chosen cell :Recursive backtracker
                string currentKeyToNeighbour = neighbours.FirstOrDefault(x => x.Value == GetPlayfieldPos(chosenNeighbour.position)).Key;
                current.walls[currentKeyToNeighbour] = false;
                Dictionary<string, int> neighboursFromNeighbour = GetNeighbours(chosenNeighbour.position);
                string neighbourKeyToCurrent = neighboursFromNeighbour.FirstOrDefault(x => x.Value == GetPlayfieldPos(current.position)).Key;
                chosenNeighbour.walls[neighbourKeyToCurrent] = false;
                // Make the chosen cell the current cell and mark it as visited :Recursive backtracker
                current = chosenNeighbour;
                current.visited = true;
                unvisitedCells--;
                if(unvisitedCells == 0) {
                    // Remove the outside inner walls for the last tile
                    foreach (KeyValuePair<string, int> entry in GetNeighbours(current.position)) {
                        if (entry.Value == -1) current.walls[entry.Key] = false;
                    }
                }
            } else if (gameStack.Count != 0) { // Else if stack is not empty :Recursive backtracker
                current = (Cell)gameStack.Pop(); // Pop a cell from the stack + Make it the current cell :Recursive backtracker
                exitCell = GetPlayfieldPos(current.position); // set the exit of the maze
            }
        }

    }

    int GetPlayfieldPos(Vector2 xy) {
        int pos = (int)xy.y * xExt + (int)xy.x;
        return pos;
    }
    Vector2 GetCoordinates(int pos) {
        Vector2 coordinates = new Vector2();
        coordinates.x = pos % xExt;
        coordinates.y = pos / xExt;
        return coordinates;
    }

    Dictionary<string, int> GetNeighbours(Vector2 xy) {
        int x = (int)xy.x;
        int y = (int)xy.y;

        return new Dictionary<string, int>() {
            { "UP", y > 0 ? GetPlayfieldPos(new Vector2(x, y-1)) : -1},
            { "DOWN", y < zExt-1? GetPlayfieldPos(new Vector2(x, y+1)) : -1},
            { "LEFT", x > 0 ? GetPlayfieldPos(new Vector2(x-1, y)) : -1},
            { "RIGHT", x < xExt-1 ? GetPlayfieldPos(new Vector2(x+1, y)) : -1}
        };
    }

    [System.Serializable]
    public class Cell {
        public bool visited;
        public Vector2 position;
        public Dictionary<string, bool> walls = new Dictionary<string, bool>() {
            { "UP", true },
            { "DOWN", true},
            { "LEFT", true},
            { "RIGHT", true}
        };
    }

 

    //You might need this more than once...
    void placeBallStart(Vector3 startPos)
    {
        //Reset Physics
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Place the ball
        ball.transform.position = startPos;

    }

    public void EndzoneTrigger(GameObject other)
    {
        //Check if ball first...
        if (other.Equals(ball)) {
            //Player has fallen onto ground plane, reset
            placeBallStart(resetPosition);
        }
    }
    public void winTrigger(GameObject other)
    {
        //Check if ball first...
        if (other.Equals(ball)) {
            //Destroy this maze
            foreach (Transform child in outerWallsParent) Destroy(child.gameObject);
            foreach (Transform child in innerWallsParent) Destroy(child.gameObject);
            foreach (Transform child in tilesParent) Destroy(child.gameObject);

            //Generate new maze
            CalcMaze();
            root.transform.rotation = Quaternion.identity; // to avoid a crooked generation
            CreateOuterWalls();
            BuildMaze();
            placeBallStart(resetPosition);

        }
        

    }
}
	

