using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    [Header("Image Settings")]
    public Texture2D level;

    [Header("Tiles")]
    public GameObject basicFloor;
    public PixelToObject [] Objects;

    private int width, height;
    private Color[] pix;

    public void LoadLevel()
    {
        string holderName = "Level";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        width = Mathf.FloorToInt(level.width);
        height = Mathf.FloorToInt(level.height);
        pix = level.GetPixels(0, 0, width, height);

        /*Create empty chunks*/
        int horizontalChunkCount = width / 16 + 1;
        int verticalChunkCount = height / 9 + 1;
        Transform[,] chunks = new Transform[horizontalChunkCount, verticalChunkCount];
        for (int i = 0; i < horizontalChunkCount; i++)
        {
            for (int j = 0; j < verticalChunkCount; j++)
            {
                chunks[i, j] = new GameObject("Chunk " + i + ", " + j).transform;
                chunks[i, j].parent = mapHolder;
            }
        }

        /*Spawn tiles and places them in chunks*/
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                foreach (PixelToObject pto in Objects)
                {
                    if (pix[x + y * width] == pto.inputColor)
                    {
                        GameObject tile = Instantiate(pto.outputObject, new Vector3(x, 0 + pto.extraHeight, y), Quaternion.identity) as GameObject;
                        tile.transform.parent = chunks[x / 16, y / 9];
                        if (pto.spawnFloor)
                        {
                            GameObject floor = Instantiate(basicFloor, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
                            floor.transform.parent = chunks[x / 16, y / 9];
                        }
                    }  
                }
            }
        }

        /*Remove empty chunks*/
        for (int i = 0; i < horizontalChunkCount; i++)
        {
            for (int j = 0; j < verticalChunkCount; j++)
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