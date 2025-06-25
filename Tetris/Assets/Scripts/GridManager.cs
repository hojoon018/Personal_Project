using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    // ���͸� ������ �ݿø��ؼ� ��ǥ�� �°� ��ġ ����
    public static Vector2 RoundVector(Vector2 vec)
    {
        return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
    }

    // �־��� ��ġ�� �׸��� �ȿ� �ִ��� Ȯ��
    public static bool IsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y < height);
        // y >= 0 ������ �����ؼ�, ȭ�� ���ʿ� �ִ� �� ���ǵ���
    }

    // �� ���� �� á���� �˻�
    public static bool IsFullRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    // �� �� ����
    public static void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            GameObject.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // ���� �ٵ��� �� ĭ �Ʒ��� ������
    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // ��ġ�� ������ ������ ��
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // y��° �� ���� �ִ� �� ���� ������
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height; i++)
        {
            DecreaseRow(i);
        }
    }

    // ��� �� �˻��ؼ� �� �� �� ����
    public static void DeleteFullRows()
    {
        int linesCleared = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsFullRow(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                y--; // ���� ���������� �� �� �� Ȯ��
                linesCleared++;
            }
        }

        if (linesCleared > 0)
            ScoreManager.Instance.AddScore(linesCleared);
    }

}
