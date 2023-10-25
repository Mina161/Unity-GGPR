using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] obstacles;
    public GameObject[] obstaclesLeft;
    public GameObject[] obstaclesRight;
    public GameObject[] powerups;
    public GameObject[] powerupsLeft;
    public GameObject[] powerupsRight;


    public void ActivateRandom()
    {
        DeactivateAll();

        System.Random random = new System.Random();
        int numObstacles = random.Next(0, 3);
        int currObstacles = 0;
        int renderWhat = random.Next(0, 3);
        switch(renderWhat)
        {
            case 0: break;
            case 1:
                {
                    if (currObstacles == numObstacles) break;
                    currObstacles++;
                    int randomNumber = random.Next(0, obstaclesLeft.Length);
                    obstaclesLeft[randomNumber].SetActive(true);
                    break;
                }
            case 2:
                {
                    int randomNumber = random.Next(0, powerupsLeft.Length);
                    powerupsLeft[randomNumber].SetActive(true);
                    break;
                }
        }

        renderWhat = random.Next(0, 3);
        switch (renderWhat)
        {
            case 0: break;
            case 1:
                {
                    if (currObstacles == numObstacles) break;
                    currObstacles++;
                    int randomNumber = random.Next(0, obstacles.Length);
                    obstacles[randomNumber].SetActive(true);
                    break;
                }
            case 2:
                {
                    int randomNumber = random.Next(0, powerups.Length);
                    powerups[randomNumber].SetActive(true);
                    break;
                }
        }

        renderWhat = random.Next(0, 3);
        switch (renderWhat)
        {
            case 0: break;
            case 1:
                {
                    if (currObstacles == numObstacles) break;
                    currObstacles++;
                    int randomNumber = random.Next(0, obstaclesRight.Length);
                    obstaclesRight[randomNumber].SetActive(true);
                    break;
                }
            case 2:
                {
                    int randomNumber = random.Next(0, powerupsRight.Length);
                    powerupsRight[randomNumber].SetActive(true);
                    break;
                }
        }
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
            obstaclesRight[i].SetActive(false);
            obstaclesLeft[i].SetActive(false);
        }
        for (int i = 0; i < powerups.Length; i++)
        {
            powerups[i].SetActive(false);
            powerupsRight[i].SetActive(false);
            powerupsLeft[i].SetActive(false);
        }
    }

    public void Nuke()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(false);
            obstaclesRight[i].SetActive(false);
            obstaclesLeft[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
