using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSpawner : Singleton<WallSpawner> {

    #region Variables
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject stephPrefab;
    [SerializeField]
    private GameObject endPrefab;
    [SerializeField]
    private Transform environementParent;
    private int obstaclesNb;
    private int arenaSize;
    private int[][] obstacles;

    private Camera mainCamera;
    private int newObstacleDecompte = 3;
    private Vector2Int playerPreviousPos;
    private Vector3 endPos;
    #endregion

    protected override void Awake() {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void Start() {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart() {
        yield return new WaitForSeconds(0.01f);
        int lvl = Player.Level;
        arenaSize = lvl < 30 ? 5 : Mathf.FloorToInt(lvl / 5);
        obstaclesNb = Mathf.FloorToInt(arenaSize * arenaSize * 0.15f);
        GenerateFloor();
        GenerateWalls();
        GenerateObtacles();
        FitCamera();
        if (lvl == 1) {
            SpawnSteph();
        }
    }

    private void GenerateObtacles() {
        //init array table to 
        obstacles = new int[arenaSize][];
        for(int i=0; i<obstacles.Length; i++) {
            obstacles[i] = new int[arenaSize];
        }
        // add player in the midel of the arena
        int xPlayer = Random.Range(1, arenaSize - 1);
        int yPlayer = Random.Range(1, arenaSize - 1);
        GameObject spawn = Instantiate(playerPrefab, new Vector3(xPlayer + 0.5f, yPlayer + 0.5f, 0), Quaternion.identity) as GameObject;
        spawn.transform.parent = environementParent;
        SpriteRenderer sr = spawn.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = arenaSize - yPlayer;
        obstacles[xPlayer][yPlayer] = 1;
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
        endPos = new Vector3(x + 0.5f, y + 0.5f, 0);
        GameObject end = Instantiate(endPrefab, endPos, Quaternion.identity) as GameObject;
        end.transform.parent = environementParent;
        sr = end.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = arenaSize - y -1 ;
        obstacles[x][y] = 1;

        // add x obstacles on free square
        GameObject obstacle;
        for (int i=0; i<obstaclesNb; i++) { 
            x = Random.Range(0, arenaSize);
            y = Random.Range(0, arenaSize);
            while (obstacles[x][y] == 1) {
                x = Random.Range(0, arenaSize);
                y = Random.Range(0, arenaSize);
            }
            obstacle = Instantiate(TileAccess.Instance.GetRandomObstaclesStone(), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
            obstacle.transform.parent = TileAccess.Instance.GetObstacle().transform;
            sr = obstacle.GetComponentInChildren<SpriteRenderer>();
            sr.sortingOrder = arenaSize-y;
            obstacles[x][y] = 1;

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
        obstacles[playerPreviousPos.x][playerPreviousPos.y] = 0;
        obstacles[Mathf.FloorToInt(playerPos.x)][Mathf.FloorToInt(playerPos.y)] = 1;
        playerPreviousPos = new Vector2Int(Mathf.FloorToInt(playerPos.x), Mathf.FloorToInt(playerPos.y));
        newObstacleDecompte--;
        if(newObstacleDecompte == 0) {
            newObstacleDecompte = 3;
            SpawnWoodenObstacles();
        }
    }

    private void SpawnWoodenObstacles() {
        //check if there is a free spot to spawn
        int countEmptySpot = 0;
        foreach(int[] row in obstacles) {
            foreach(int val in row) {
                if (val == 0) {
                    countEmptySpot++;
                    break;
                }
            }
            if (countEmptySpot >= 1) break;
        }
        if (countEmptySpot == 0) return;

        //spawn an obstacle
        int x = Random.Range(0, arenaSize);
        int y = Random.Range(0, arenaSize);
        while (obstacles[x][y] == 1) {
            x = Random.Range(0, arenaSize);
            y = Random.Range(0, arenaSize);
        }
        GameObject obstacle = Instantiate(TileAccess.Instance.GetRandomObstaclesWood(), new Vector3(x + 0.5f, y, 0), Quaternion.identity) as GameObject;
        obstacle.transform.parent = TileAccess.Instance.GetObstacle().transform;
        SpriteRenderer sr = obstacle.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = arenaSize - y;
        obstacles[x][y] = 1;
    }

    public void RemoveObstacleAt(Vector2Int pos) {
        obstacles[pos.x][pos.y] = 0;
    }

    private void SpawnSteph() {
        Vector3 pos = new Vector3(playerPreviousPos.x, playerPreviousPos.y, 0);
        GameObject spawn = Instantiate(stephPrefab, pos, Quaternion.identity) as GameObject;
        spawn.transform.parent = environementParent;
        SpriteRenderer sr = spawn.GetComponentInChildren<SpriteRenderer>();
        sr.sortingOrder = 50;
        Steph steph = spawn.GetComponent<Steph>();
        steph.EndPos = endPos;
    }
}
