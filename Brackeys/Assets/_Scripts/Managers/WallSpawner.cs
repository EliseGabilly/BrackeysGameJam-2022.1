using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSpawner : Singleton<WallSpawner> {

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

    private Camera mainCamera;
    private int newObstacleDecompte = 3;
    private Vector2Int playerPreviousPos;
    #endregion

    protected override void Awake() {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void Start() {
        GenerateFloor();
        GenerateWalls();
        GenerateObtacles();
        FitCamera();
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
        SpriteRenderer sr = spawn.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = arenaSize - yPlayer;
        obstacles[xPlayer][yPlayer] = spawn;
        playerPreviousPos = new Vector2Int(xPlayer, yPlayer);

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
        sr = end.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = arenaSize - y -1 ;
        obstacles[x][y] = end;

        // add x obstacles on free square
        GameObject obstacle;
        for (int i=0; i<obstaclesNb; i++) { 
            x = Random.Range(0, arenaSize);
            y = Random.Range(0, arenaSize);
            while (obstacles[x][y] != null) {
                x = Random.Range(0, arenaSize);
                y = Random.Range(0, arenaSize);
            }
            obstacle = Instantiate(TileAccess.Instance.GetRandomObstaclesStone(), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
            obstacle.transform.parent = TileAccess.Instance.GetObstacle().transform;
            sr = obstacle.GetComponentInChildren<SpriteRenderer>();
            sr.sortingOrder = arenaSize-y;
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
        Tilemap tilemapTopWall = TileAccess.Instance.GetTopWall();
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
            tilemapTopWall.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize; i++) { //upper top
            p = new Vector3Int(i, arenaSize+1, 0);
            tile = TileAccess.Instance.GetRandomWallUpperTop();
            tilemapTopWall.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize+1; i++) { //left
            p = new Vector3Int(-1, i, 0);
            tile = TileAccess.Instance.GetRandomWallLeft();
            tilemap.SetTile(p, tile);
        }
        for (int i = 0; i < arenaSize+1; i++) { //right
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

    private void FitCamera() {
        mainCamera.transform.position = new Vector3(arenaSize / 2f, arenaSize / 2f, -10);
        Vector3 topLeft = new Vector3(arenaSize+0.2f, arenaSize+2, 0);
        Vector3 bottomRight = new Vector3(-0.2f, -0.2f, 0);

        mainCamera.orthographicSize = 1;
        Vector3 cameraTopLeft = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, -mainCamera.transform.position.z));
        Vector3 cameraBottomRight = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -mainCamera.transform.position.z));
        bool isInFrame = topLeft.x < cameraTopLeft.x && topLeft.y < cameraTopLeft.y && bottomRight.x > cameraBottomRight.x && bottomRight.y > cameraBottomRight.y;
        while (!isInFrame) {
            mainCamera.orthographicSize ++;
            cameraTopLeft = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, -mainCamera.transform.position.z));
            cameraBottomRight = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -mainCamera.transform.position.z));
            isInFrame = topLeft.x < cameraTopLeft.x && topLeft.y < cameraTopLeft.y && bottomRight.x > cameraBottomRight.x && bottomRight.y > cameraBottomRight.y;
        }
    }

    public int GetArenaSize() {
        return arenaSize;
    }

    public void MovePlayerTo(Vector2 playerPos) {
        obstacles[Mathf.FloorToInt(playerPos.x)][Mathf.FloorToInt(playerPos.y)] = obstacles[playerPreviousPos.x][playerPreviousPos.y];
        obstacles[playerPreviousPos.x][playerPreviousPos.y] = null;
        newObstacleDecompte--;
        if(newObstacleDecompte == 0) {
            newObstacleDecompte = 3;
            SpawnWoodenObstacles();
        }
    }

    private void SpawnWoodenObstacles() {
        int x = Random.Range(0, arenaSize);
        int y = Random.Range(0, arenaSize);
        while (obstacles[x][y] != null) {
            x = Random.Range(0, arenaSize);
            y = Random.Range(0, arenaSize);
        }
        GameObject obstacle = Instantiate(TileAccess.Instance.GetRandomObstaclesWood(), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
        obstacle.transform.parent = TileAccess.Instance.GetObstacle().transform;
        SpriteRenderer sr = obstacle.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = arenaSize - y;
        obstacles[x][y] = obstacle;
    }


}
