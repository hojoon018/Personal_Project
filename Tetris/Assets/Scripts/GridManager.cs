using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    // 벡터를 정수로 반올림해서 좌표에 맞게 위치 조정
    public static Vector2 RoundVector(Vector2 vec)
    {
        return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
    }

    // 주어진 위치가 그리드 안에 있는지 확인
    public static bool IsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y < height);
        // y >= 0 조건을 제거해서, 화면 위쪽에 있는 건 허용되도록
    }

    // 한 줄이 꽉 찼는지 검사
    public static bool IsFullRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    // 한 줄 제거
    public static void DeleteRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            GameObject.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // 위의 줄들을 한 칸 아래로 내리기
    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // 위치도 실제로 내려야 함
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // y번째 줄 위에 있는 줄 전부 내리기
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height; i++)
        {
            DecreaseRow(i);
        }
    }

    // 모든 줄 검사해서 꽉 찬 줄 삭제
    public static void DeleteFullRows()
    {
        int linesCleared = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsFullRow(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                y--; // 줄이 내려갔으니 한 줄 더 확인
                linesCleared++;
            }
        }

        if (linesCleared > 0)
            ScoreManager.Instance.AddScore(linesCleared);
    }

}
