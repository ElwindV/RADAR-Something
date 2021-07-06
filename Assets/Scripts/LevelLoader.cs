using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    [Header("Image Settings")]
    public Texture2D level;

    [Header("Tiles")]
    public GameObject basicFloor;
    public PixelToObject[] objects;

    private int _width;
    private int _height;
    private Color[] _pix;

    public void LoadLevel()
    {
        var holderName = "Level";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        var mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        _width = Mathf.FloorToInt(level.width);
        _height = Mathf.FloorToInt(level.height);
        _pix = level.GetPixels(0, 0, _width, _height);

        /*Create empty chunks*/
        var horizontalChunkCount = _width / 16 + 1;
        var verticalChunkCount = _height / 9 + 1;
        var chunks = new Transform[horizontalChunkCount, verticalChunkCount];
        for (var i = 0; i < horizontalChunkCount; i++)
        {
            for (var j = 0; j < verticalChunkCount; j++)
            {
                chunks[i, j] = new GameObject("Chunk " + i + ", " + j).transform;
                chunks[i, j].parent = mapHolder;
            }
        }

        /*Spawn tiles and places them in chunks*/
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                foreach (var pto in objects)
                {
                    if (_pix[x + y * _width] != pto.inputColor) continue;
                    
                    var tile = Instantiate(pto.outputObject, new Vector3(x, 0 + pto.extraHeight, y), Quaternion.identity) as GameObject;
                    tile.transform.parent = chunks[x / 16, y / 9];

                    if (!pto.spawnFloor) continue;
                    
                    var floor = Instantiate(basicFloor, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
                    floor.transform.parent = chunks[x / 16, y / 9];
                }
            }
        }

        /*Remove empty chunks*/
        for (var i = 0; i < horizontalChunkCount; i++)
        {
            for (var j = 0; j < verticalChunkCount; j++)
            {
                if (chunks[i, j].childCount == 0) Object.DestroyImmediate(chunks[i, j].gameObject);
            }
        }
    }

}

[System.Serializable]
public struct PixelToObject
{
    public Color inputColor;
    public GameObject outputObject;
    public float extraHeight;
    public bool spawnFloor;
}