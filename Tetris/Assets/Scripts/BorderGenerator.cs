using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator : MonoBehaviour
{
    public GameObject borderPrefab;
    public int width = 10;
    public int height = 20;

    void Start()
    {
        GenerateBorders();
    }

    void GenerateBorders()
    {
        // ÁÂ¿ì Å×µÎ¸®
        for (int y = 0; y < height; y++)
        {
            Instantiate(borderPrefab, new Vector3(-1, y, 0), Quaternion.identity, transform); // ¿ÞÂÊ
            Instantiate(borderPrefab, new Vector3(width, y, 0), Quaternion.identity, transform); // ¿À¸¥ÂÊ
        }

        // ¹Ù´Ú Å×µÎ¸®
        for (int x = -1; x <= width; x++)
        {
            Instantiate(borderPrefab, new Vector3(x, -1, 0), Quaternion.identity, transform); // ¹Ù´Ú
        }
    }
}
