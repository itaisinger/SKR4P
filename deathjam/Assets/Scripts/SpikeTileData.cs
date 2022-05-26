using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "SpikeTileData")]
public class SpikeTileData : ScriptableObject
{
    public List<Tile> tiles;
    public string dir = "none";
}