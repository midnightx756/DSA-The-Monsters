using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject roomPrefab;
    public Transform firstSpawnPoint;

    // Increased default distance because of your large scale (Y = 15). 
    // If the player stands slightly off-center, we still want to detect them.
    public float spawnTriggerDistance = 10.0f; 

    GameObject Player;
    private Transform currentExitPoint;
    
    // We track the EntryPoint of the last room explicitly
    private Transform lastRoomEntryPoint; 

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        SpawnPiece(firstSpawnPoint.position);
    }

    void Update()
    {
        // Check if player exists and we have a room tracked
        if (Player != null && lastRoomEntryPoint != null)
        {
            // Check distance specifically to the Entry Point (the start of the platform)
            float distanceToStart = Vector3.Distance(Player.transform.position, lastRoomEntryPoint.position);

            // If player is close to the start of the platform, spawn the next one
            if (distanceToStart < spawnTriggerDistance)
            {
                SpawnPiece(currentExitPoint.position);
            }
        }
    }

    void SpawnPiece(Vector3 spawnAt)
    {
        GameObject newRoom = Instantiate(roomPrefab);

        Transform entry = newRoom.transform.Find("EntryPoint");
        Transform exit = newRoom.transform.Find("ExitPoint");

        Vector3 offset = newRoom.transform.position - entry.position;
        newRoom.transform.position = spawnAt + offset;

        currentExitPoint = exit;

        // CRITICAL: Update the lastRoomEntryPoint to the NEW room's entry.
        // As soon as this happens, the 'distanceToStart' in Update() becomes huge 
        // (because the new room is far ahead), effectively stopping the spawning 
        // until the player runs all the way there.
        lastRoomEntryPoint = entry;
    }
}