using Level;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [Header("Image Settings")]
    public Texture2D level;

    [Header("Generation properties")] 
    public Vector2Int chunkSize;
    
    [Header("Tiles")]
    public PixelToObjectSO[] objects;

    private int _imageWidth;
    private int _imageHeight;
    private Color[] _pixels;

    public void LoadLevel()
    {
        const string holderName = "Level";
        
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        var mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        _imageWidth = Mathf.FloorToInt(level.width);
        _imageHeight = Mathf.FloorToInt(level.height);
        _pixels = level.GetPixels(0, 0, _imageWidth, _imageHeight);

        /*Create empty chunks*/
        var horizontalChunkCount = _imageWidth / chunkSize.x + 1;
        var verticalChunkCount = _imageHeight / chunkSize.y + 1;
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
        for (var x = 0; x < _imageWidth; x++)
        {
            for (var y = 0; y < _imageHeight; y++)
            {
                foreach (var pixelToObjectSO in objects)
                {
                    if (_pixels[x + y * _imageWidth] != pixelToObjectSO.inputColor) continue;

                    var chunk = chunks[x / chunkSize.x, y / chunkSize.y];
                    
                    pixelToObjectSO.SpawnObject(
                        new Vector3(x, 0, y), 
                        chunk
                    );
                }
            }
        }

        /*Remove empty chunks*/
        for (var i = 0; i < horizontalChunkCount; i++)
        {
            for (var j = 0; j < verticalChunkCount; j++)
            {
                if (chunks[i, j].childCount == 0) DestroyImmediate(chunks[i, j].gameObject);
            }
        }
    }

}
