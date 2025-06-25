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

        // 큐가 비면 다시 채우기
        if (bagQueue.Count == 0)
            FillBag();

        GameObject prefab = bagQueue.Dequeue();

        // 가장 높은 블록의 y값을 찾아서 안전한 위치로 스폰
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
        // 기존 프리뷰 제거
        if (currentPreviewBlock != null)
            Destroy(currentPreviewBlock);

        if (bagQueue.Count == 0)
            FillBag();

        // 다음 블록 가져오기 (Queue에 있으니까 Peek)
        GameObject nextPrefab = bagQueue.Peek();
        currentPreviewBlock = Instantiate(nextPrefab, nextBlockPreviewPosition.position, Quaternion.identity, nextBlockPreviewPosition);

        Destroy(currentPreviewBlock.GetComponent<Tetromino>());

        // 움직이지 않도록
        foreach (Transform t in currentPreviewBlock.GetComponentsInChildren<Transform>())
            t.gameObject.layer = LayerMask.NameToLayer("UI"); // 렌더만 하고 게임에 간섭 안 하게

        currentPreviewBlock.transform.localScale = Vector3.one * 1f; // 크기 조절
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
