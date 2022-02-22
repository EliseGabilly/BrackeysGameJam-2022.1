using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileAccess : MonoBehaviour {

    #region Variables
    [Header("Grids")]
    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap wall;
    [SerializeField]
    private GameObject obstacles;
    [Header("Floor")]
    [SerializeField]
    private List<Tile> grassTiles_base;
    [SerializeField]
    private List<Tile> grassTiles_flower;
    [Header("Wall")]
    [SerializeField]
    private List<Tile> wallTiles_left;
    [SerializeField]
    private List<Tile> wallTiles_right;
    [SerializeField]
    private List<Tile> wallTiles_bottom;
    [SerializeField]
    private List<Tile> wallTiles_upperTop;
    [SerializeField]
    private List<Tile> wallTiles_lowertop;
    [Header("Wall corner")]
    [SerializeField]
    private Tile wallTiles_topleft;
    [SerializeField]
    private Tile wallTiles_topright;
    [SerializeField]
    private Tile wallTiles_bottomleft;
    [SerializeField]
    private Tile wallTiles_bottomright;
    [Header("Obstacles")]
    [SerializeField]
    private List<GameObject> obstacles_wood;
    [SerializeField]
    private List<GameObject> obstacles_stone;
    #endregion


    public Tilemap GetFloor() {
        return floor;
    }
    public Tilemap GetWall() {
        return wall;
    }
    public GameObject GetObstacle() {
        return obstacles;
    }

    public Tile GetRandomGrassBase() {
        return grassTiles_base[Random.Range(0, grassTiles_base.Count)];
    }
    public Tile GetRandomGrassFlower() {
        return grassTiles_flower[Random.Range(0, grassTiles_flower.Count)];
    }
    public Tile GetRandomWallLeft() {
        return wallTiles_left[Random.Range(0, wallTiles_left.Count)];
    }
    public Tile GetRandomWallRight() {
        return wallTiles_right[Random.Range(0, wallTiles_right.Count)];
    }
    public Tile GetRandomWallBottom() {
        return wallTiles_bottom[Random.Range(0, wallTiles_bottom.Count)];
    }
    public Tile GetRandomWallUpperTop() {
        return wallTiles_upperTop[Random.Range(0, wallTiles_upperTop.Count)];
    }
    public Tile GetRandomWallLowerTop() {
        return wallTiles_lowertop[Random.Range(0, wallTiles_lowertop.Count)];
    }
    public Tile GetWallTopLeft() {
        return wallTiles_topleft;
    }
    public Tile GetWallTopRight() {
        return wallTiles_topright;
    }
    public Tile GetWallBottomLeft() {
        return wallTiles_bottomleft;
    }
    public Tile GetWallBottomRight() {
        return wallTiles_bottomright;
    }
    public GameObject GetRandomObstaclesWood() {
        return obstacles_wood[Random.Range(0, obstacles_wood.Count)];
    }
    public GameObject GetRandomObstaclesStone() {
        return obstacles_stone[Random.Range(0, obstacles_stone.Count)];
    }

}
