using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    public GameObject gameOverPanel;

    private Queue<GameObject> bagQueue = new Queue<GameObject>();
    private bool isGameOver = false;


    public Transform nextBlockPreviewPosition;
    private GameObject currentPreviewBlock;

    void Start()
    {
        FillBag();
        SpawnNewBlock();
    }

    void FillBag()
    {
        List<GameObject> bag = new List<GameObject>(tetrominoPrefabs);

        // Fisher-Yates Shuffle
        for (int i = 0; i < bag.Count; i++)
        {
            int rand = Random.Range(i, bag.Count);
            (bag[i], bag[rand]) = (bag[rand], bag[i]);
        }

        foreach (GameObject block in bag)
            bagQueue.Enqueue(block);
    }

    public void SpawnNewBlock()
    {
        if (isGameOver) return;

        // ť�� ��� �ٽ� ä���
        if (bagQueue.Count == 0)
            FillBag();

        GameObject prefab = bagQueue.Dequeue();

        // ���� ���� ����� y���� ã�Ƽ� ������ ��ġ�� ����
        float maxY = float.MinValue;
        foreach (Transform child in prefab.transform)
        {
            if (child.localPosition.y > maxY)
                maxY = child.localPosition.y;
        }

        float spawnY = GridManager.height - 1 - maxY;
        GameObject newBlock = Instantiate(prefab, new Vector3(5, spawnY, 0), Quaternion.identity);

        Tetromino tetromino = newBlock.GetComponent<Tetromino>();
        if (!tetromino.IsValidMove())
        {
            isGameOver = true;
            GameOver();
        }

        UpdateNextPreview();
    }

    void UpdateNextPreview()
    {
        // ���� ������ ����
        if (currentPreviewBlock != null)
            Destroy(currentPreviewBlock);

        if (bagQueue.Count == 0)
            FillBag();

        // ���� ��� �������� (Queue�� �����ϱ� Peek)
        GameObject nextPrefab = bagQueue.Peek();
        currentPreviewBlock = Instantiate(nextPrefab, nextBlockPreviewPosition.position, Quaternion.identity, nextBlockPreviewPosition);

        Destroy(currentPreviewBlock.GetComponent<Tetromino>());

        // �������� �ʵ���
        foreach (Transform t in currentPreviewBlock.GetComponentsInChildren<Transform>())
            t.gameObject.layer = LayerMask.NameToLayer("UI"); // ������ �ϰ� ���ӿ� ���� �� �ϰ�

        currentPreviewBlock.transform.localScale = Vector3.one * 1f; // ũ�� ����
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // ���� �Ͻ�����
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
