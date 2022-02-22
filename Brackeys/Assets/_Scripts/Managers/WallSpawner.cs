using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSpawner : MonoBehaviour {

    #region Variables
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject endPrefab;
    [SerializeField]
    private Transform environementParent;
    [SerializeField]
    private int obstaclesNb = 3;
    [SerializeField]
    private int arenaSize = 5;
    private GameObject[][] obstacles;


    #endregion


    private void Start() {
        GenerateFloor();
        GenerateWalls();
        GenerateObtacles();
    }

    private void GenerateObtacles() {
        //init array table to 
        obstacles = new GameObject[arenaSize][];
        for(int i=0; i<obstacles.Length; i++) {
            obstacles[i] = new GameObject[arenaSize];
        }
        // add player in the midel of the arena
        int xPlayer = Random.Range(1, arenaSize - 1);
        int yPlayer = Random.Range(1, arenaSize - 1);
        GameObject spawn = Instantiate(playerPrefab, new Vector3(xPlayer + 0.5f, yPlayer + 0.5f, 0), Quaternion.identity) as GameObject;
        spawn.transform.parent = environementParent;
        obstacles[xPlayer][yPlayer] = spawn;

        // add end but not on the same line/column as the player
        int x = Random.Range(0, arenaSize);
        while (x == xPlayer) {
            x = Random.Range(0, arenaSize);
        }
        int y = Random.Range(0, arenaSize);
        while (y == yPlayer) {
            y = Random.Range(0, arenaSize);
        }
        GameObject end = Instantiate(endPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
        end.transform.parent = environementParent;
        obstacles[x][y] = end;

        // add x obstacles on free square
        GameObject obstacle;
        for (int i=0; i<23; i++) { //TODO use nb obstacle
            x = Random.Range(0, arenaSize);
            y = Random.Range(0, arenaSize);
            while (obstacles[x][y] != null) {
                x = Random.Range(0, arenaSize);
                y = Random.Range(0, arenaSize);
            }
            obstacle = Instantiate(TileAccess.Instance.GetRandomObstaclesStone(), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
            obstacle.transform.parent = TileAccess.Instance.GetObstacle().transform;
            obstacles[x][y] = obstacle;

        }
    }

    private void GenerateFloor() {
        Tilemap tilemap = TileAccess.Instance.GetFloor();
        for (int i = -10; i < arenaSize + 10; i++) {
            for (int j = -10; j < arenaSize + 10; j++) {
                Vector3Int p = new Vector3Int(i, j, 0);
                Tile tile = Random.Range(0, 10) < 7 ? TileAccess.Instance.GetRandomGrassBase() : TileAccess.Instance.GetRandomGrassFlower();
                tilemap.SetTile(p, tile);
            }
        }
    }
    private void GenerateWalls() {
        Tilemap tilemap = TileAccess.Instance.GetWall();
        Vector3Int p;
        Tile tile;

        for (int i = 0; i < arenaSize; i++) { //bottom
            p = new Vector3Int(i, -1, 0);
            tile = TileAccess.Instance.GetRandomWallBottom();
            tilemap.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize; i++) { //lower top
            p = new Vector3Int(i, arenaSize, 0);
            tile = TileAccess.Instance.GetRandomWallLowerTop();
            tilemap.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize; i++) { //upper top
            p = new Vector3Int(i, arenaSize+1, 0);
            tile = TileAccess.Instance.GetRandomWallUpperTop();
            tilemap.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize+1; i++) { //bottom
            p = new Vector3Int(-1, i, 0);
            tile = TileAccess.Instance.GetRandomWallLeft();
            tilemap.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize+1; i++) { //lower top
            p = new Vector3Int(arenaSize, i, 0);
            tile = TileAccess.Instance.GetRandomWallRight();
            tilemap.SetTile(p, tile);
        }
        //corners
        p = new Vector3Int(-1, -1, 0);
        tile = TileAccess.Instance.GetWallBottomLeft();
        tilemap.SetTile(p, tile);
        p = new Vector3Int(arenaSize, -1, 0);
        tile = TileAccess.Instance.GetWallBottomRight();
        tilemap.SetTile(p, tile);
        p = new Vector3Int(-1, arenaSize+1, 0);
        tile = TileAccess.Instance.GetWallTopLeft();
        tilemap.SetTile(p, tile);
        p = new Vector3Int(arenaSize, arenaSize+1, 0);
        tile = TileAccess.Instance.GetWallTopRight();
        tilemap.SetTile(p, tile);
    }


}
