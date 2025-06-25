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
        // �¿� �׵θ�
        for (int y = 0; y < height; y++)
        {
            Instantiate(borderPrefab, new Vector3(-1, y, 0), Quaternion.identity, transform); // ����
            Instantiate(borderPrefab, new Vector3(width, y, 0), Quaternion.identity, transform); // ������
        }

        // �ٴ� �׵θ�
        for (int x = -1; x <= width; x++)
        {
            Instantiate(borderPrefab, new Vector3(x, -1, 0), Quaternion.identity, transform); // �ٴ�
        }
    }
}
