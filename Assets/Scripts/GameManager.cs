using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour
{
    public float difficulty = 1.0f; // Base difficulty, controls the initial speed of instantiation
    public GameObject player; // Reference to the player object
    public GameObject breakables; // Reference to the parent object containing breakable models

    public GameObject cameraa; // Reference to the parent object containing breakable models
    public float spawnInterval = 15f; // Distance between each spawn along the z-axis

    private List<GameObject> spawnedObjects; // List to keep track of spawned objects
    private float lastSpawnZ; // Track the z position of the last spawn
    public float score;
    public TextMeshProUGUI text;

    public ParticleSystem explosion;
    public ParticleSystem playerDeath;
    public CameraShake cameraShake;

    private bool isGame = false;
    private float speed = 6f;
    private bool endGame = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = new List<GameObject>();

        // Spawn initial objects
        if (player != null)
        {
            for (int i = 0; i < 15; i++)
            {
                SpawnBreakable();
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject is null or has no children.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increase difficulty over time

        Camera camera = Camera.main;
        Vector3 newPosition = new Vector3 (0, 2, player.transform.position.z-10);
        camera.transform.position = newPosition;
        difficulty += Time.deltaTime * 0.01f;

        // Check if it's time to remove old objects and spawn new ones
        if (!isGame)
        {
        }
        if (Input.GetMouseButtonDown(0)&&!isGame &&!endGame)
        {
            isGame = true; 
            text.transform.GetChild(0).gameObject.SetActive(false);

        }
        if (isGame)
        {

            ManageSpawnedObjects();
            score += 1;

            text.text = "" + score;

            difficulty += Time.deltaTime * 0.01f;
            speed = Mathf.Min(6f * difficulty, 15f);
            Vector3 newPlayerPosition = (player.transform.position + Vector3.forward * speed * Time.deltaTime);

            // Apply the new position to the GameObject
            player.transform.position = newPlayerPosition;
        }
    }

    void ManageSpawnedObjects()
    {
        // Remove objects that the player has passed
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i].transform.position.z < player.transform.position.z)
            {
                if (spawnedObjects[i].transform.localScale.y > 0)
                {
                    if (spawnedObjects[i].transform.localScale.x > 0)
                    {
                        if (spawnedObjects[i].transform.localScale.y > player.transform.localScale.y && spawnedObjects[i].transform.localScale.x > player.transform.localScale.x)
                        {
                            if (player.transform.localScale.y > spawnedObjects[i].transform.localScale.y * 0.5f && player.transform.localScale.x > spawnedObjects[i].transform.localScale.x * 0.5f)
                            {
                                TriggerExplosion(spawnedObjects[i].transform.position, explosion);

                                Destroy(spawnedObjects[i]);
                                spawnedObjects.RemoveAt(i);
                            }
                            else
                            {
                                EndGame();
                            }
                        }
                        else
                        {
                            EndGame();
                        }
                    }
                    else
                    {
                        if (spawnedObjects[i].transform.localScale.y > player.transform.localScale.y && spawnedObjects[i].transform.localScale.x < player.transform.localScale.x)
                        {
                            if (player.transform.localScale.y > spawnedObjects[i].transform.localScale.y * 0.5f && player.transform.localScale.x < spawnedObjects[i].transform.localScale.x * 0.5f)
                            {
                                TriggerExplosion(spawnedObjects[i].transform.position, explosion);

                                Destroy(spawnedObjects[i]);
                                spawnedObjects.RemoveAt(i);
                            }
                            else
                            {
                                EndGame();
                            }
                        }
                        else
                        {
                            EndGame();
                        }
                    }
                }
                else
                {
                    if (spawnedObjects[i].transform.localScale.x > 0)
                    {
                        if (spawnedObjects[i].transform.localScale.y < player.transform.localScale.y && spawnedObjects[i].transform.localScale.x > player.transform.localScale.x)
                        {
                            if (player.transform.localScale.y < spawnedObjects[i].transform.localScale.y * 0.5f && player.transform.localScale.x > spawnedObjects[i].transform.localScale.x * 0.5f)
                            {
                                TriggerExplosion(spawnedObjects[i].transform.position, explosion);

                                Destroy(spawnedObjects[i]);
                                spawnedObjects.RemoveAt(i);
                            }
                            else
                            {
                                EndGame();
                            }
                        }
                        else
                        {
                            EndGame();
                        }
                    }
                    else
                    {
                        if (spawnedObjects[i].transform.localScale.y < player.transform.localScale.y && spawnedObjects[i].transform.localScale.x < player.transform.localScale.x)
                        {
                            if (player.transform.localScale.y < spawnedObjects[i].transform.localScale.y * 0.5f && player.transform.localScale.x < spawnedObjects[i].transform.localScale.x * 0.5f)
                            {
                                TriggerExplosion(spawnedObjects[i].transform.position, explosion);

                                Destroy(spawnedObjects[i]);
                                spawnedObjects.RemoveAt(i);
                            }
                            else
                            {
                                EndGame();
                            }
                        }
                        else
                        {
                            EndGame();
                        }
                    }
                }
                
                

            }
        }

        // Spawn new objects if needed
        while (spawnedObjects.Count < 15)
        {
            SpawnBreakable();
        }
    }

    void SpawnBreakable()
    {
        if (breakables.transform.childCount > 0)
        {

            float randomXScale = Random.Range(0.8f, 5f) * (Random.Range(0, 2) == 0 ? -1f : 1f);
            float randomYScale = Random.Range(0.8f, 5f) * (Random.Range(0, 2) == 0 ? -1f : 1f);
            
            GameObject objToSpawn = breakables.transform.gameObject;
            Vector3 startingPoint = new Vector3(randomXScale/2, randomYScale/2, lastSpawnZ + spawnInterval);

            GameObject spawnedObject = Instantiate(objToSpawn, startingPoint, Quaternion.identity);

            // Set random scale
            spawnedObject.transform.localScale = new Vector3(randomXScale, randomYScale, 1);

            spawnedObject.SetActive(true);

            // Add the spawned object to the list and update the last spawn position
            spawnedObjects.Add(spawnedObject);
            lastSpawnZ += spawnInterval;
        }
    }
    void TriggerExplosion(Vector3 position, ParticleSystem particlesystem)
    {
        // Instantiate the explosion particle system at the given position
        ParticleSystem explosionInstance = Instantiate(particlesystem, position, Quaternion.identity);
        explosionInstance.Play();

        // Destroy the particle system after it finishes
        Destroy(explosionInstance.gameObject, explosionInstance.main.duration);
    }
    void EndGame()
    {
        endGame = true;
        // Trigger the explosion effect at the player's position
        TriggerExplosion(player.transform.position, playerDeath);
        player.GetComponent<PlayerSizing>().enabled = false;
        // Reset the score and game state
        isGame = false;

        // Display "Game Over" text

        // Trigger camera shake effect
        cameraShake.TriggerShake();

        // Start the coroutine to handle the player mesh visibility
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        // Make the player invisible by disabling its MeshRenderer
        
        // Wait for 2 seconds
        yield return new WaitForSeconds(3.5f);

        // Reset the player's position
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedObjects[i]);
            spawnedObjects.RemoveAt(i);
        }
        spawnedObjects = new List<GameObject>();

        // Spawn initial objects

        lastSpawnZ = 0f;
        if (player != null)
        {
            for (int i = 0; i < 15; i++)
            {
                SpawnBreakable();
            }
        }
        difficulty = 1f;
        score = 0;
        player.GetComponent<PlayerSizing>().enabled = true;
        player.transform.position = Vector3.zero;

        endGame = false;

        text.text = "FIT";
        text.transform.GetChild(0).gameObject.SetActive(true);

    }

}

public struct ObjectData
{
    public float randomXScale;
    public float randomYScale;

    public ObjectData(float randomXScale, float randomYScale)
    {
        this.randomXScale = randomXScale;
        this.randomYScale = randomYScale;
    }
}
