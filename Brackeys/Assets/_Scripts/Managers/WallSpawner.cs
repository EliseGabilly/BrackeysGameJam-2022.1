using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallSpawner : MonoBehaviour {

    #region Variables
    [SerializeField]
    private int arenaSize = 5;

    #endregion


    private void Start() {
        GenerateFloor();
        GenerateWalls();
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
