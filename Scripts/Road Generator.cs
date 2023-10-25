using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoadGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public Transform startPoint; //Point from where ground tiles will start
    public RoadScript roadPrefab;
    public PlayerScript player;
    public float movingSpeed = 12;
    public int tilesToPreSpawn = 30; //How many tiles should be pre-spawned
    public int tilesWithoutObstacles = 5; //How many tiles at the beginning should not have obstacles, good for warm-up
    private bool gamePaused = false;

    List<RoadScript> spawnedTiles = new List<RoadScript>();
    int nextTileToActivate = -1;
    [HideInInspector]
    public bool gameOver = false;
    static bool gameStarted = true;

    [SerializeField]
    AudioSource determination;

    [SerializeField]
    AudioSource theme;

    [SerializeField]
    AudioSource title;

    [SerializeField]
    Canvas pauseMenu;

    [SerializeField]
    Canvas gameOverMenu;

    [SerializeField]
    TMP_Text finalScore;

    [SerializeField]
    TMP_Text cheatText;

    public static RoadGenerator instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        pauseMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(false);
        player.material.SetColor("_Color", Color.white);
        theme.Play();
        title.Stop();
        Vector3 spawnPosition = startPoint.position;
        int tilesWithNoObstaclesTmp = tilesWithoutObstacles;
        for (int i = 0; i < tilesToPreSpawn; i++)
        {
            spawnPosition -= roadPrefab.startPoint.localPosition;
            RoadScript spawnedTile = Instantiate(roadPrefab, spawnPosition, Quaternion.identity) as RoadScript;
            if (tilesWithNoObstaclesTmp > 0)
            {
                spawnedTile.DeactivateAll();
                tilesWithNoObstaclesTmp--;
            }
            else
            {
                spawnedTile.ActivateRandom();
            }

            spawnPosition = spawnedTile.endPoint.position;
            spawnedTile.transform.SetParent(transform);
            spawnedTiles.Add(spawnedTile);
        }
    }

    private void Update()
    {
        if(!gameOver)
        {
            gameOver = !player.getAlive();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (!gamePaused)
            {
                PauseGame();
                gamePaused = true;
            }
            else
            {
                ResumeGame();
                gamePaused = false;
            }
        }

    }

    void FixedUpdate()
    {
        if (!gameOver && gameStarted)
        {
            transform.Translate(-spawnedTiles[0].transform.forward * Time.deltaTime * (movingSpeed + (player.getScore() / 20)), Space.World);
        }

        if (gameStarted && player.transform.position.z - spawnedTiles[0].endPoint.position.z > 5)
        {
            RoadScript tileTmp = spawnedTiles[0];
            spawnedTiles.RemoveAt(0);
            tileTmp.transform.position = spawnedTiles[spawnedTiles.Count - 1].endPoint.position - tileTmp.startPoint.localPosition;
            tileTmp.DeactivateAll();
            tileTmp.ActivateRandom();
            spawnedTiles.Add(tileTmp);
        }

        if (gameOver)
        {
            theme.Stop();
            finalScore.SetText("Final Score: "+player.getScore());
            if (player.getCheater())
            {
                cheatText.SetText("You cheated and still managed to die? Yikes");
            }
            gameOverMenu.gameObject.SetActive(true);
        }

        if (player.getNuke())
        {
            for (int i = 0; i < spawnedTiles.Count; i++) 
            {
                spawnedTiles[i].Nuke();
            }
            player.setNuke(false);
        }
    }
    public void PauseGame()
    {
        theme.Pause();
        title.Play();
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        title.Stop();
        theme.Play();
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        title.Stop();
        determination.Stop();
        pauseMenu.gameObject.SetActive(false);
        gameOverMenu.gameObject.SetActive(true);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        title.Stop();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}