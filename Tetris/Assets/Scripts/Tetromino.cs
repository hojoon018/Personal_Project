using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 1f;
    private float previousTime;

    void Start()
    {
        previousTime = Time.time;
    }

    void Update()
    {
        HandleInput();
        HandleFall();
    }

    void HandleInput()
    {
        // 좌우 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            if (!IsValidMove()) transform.position -= Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if (!IsValidMove()) transform.position -= Vector3.right;
        }

        // 회전
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (!IsValidMove()) transform.Rotate(0, 0, 90);
        }

        // 빠르게 내리기
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += Vector3.down;
            if (!IsValidMove()) transform.position -= Vector3.down;
        }
    }

    void HandleFall()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position += Vector3.down;
            if (!IsValidMove())
            {
                transform.position -= Vector3.down;
                AddToGrid();
                GridManager.DeleteFullRows();
                enabled = false; // 고정되면 더 이상 움직이지 않음

                FindObjectOfType<GameManager>().SpawnNewBlock(); // 새 블록 생성
            }
            previousTime = Time.time;
        }
    }

    public bool IsValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = GridManager.RoundVector(child.position);
            int x = (int)pos.x;
            int y = (int)pos.y;

            if (x < 0 || x >= GridManager.width || y < 0 || y >= GridManager.height)
                return false;

            if (GridManager.grid[x, y] != null)
                return false;
        }

        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = GridManager.RoundVector(child.position);
            GridManager.grid[(int)pos.x, (int)pos.y] = child;
        }
    }
}
