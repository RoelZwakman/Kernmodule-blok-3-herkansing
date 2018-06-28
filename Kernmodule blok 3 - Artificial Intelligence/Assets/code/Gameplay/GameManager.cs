using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public DungeonGenerator generator;

    [Header("AI")]
    public bool executeAI = true;

    [Header("Exit reached variables")]
    public int exitScene;

    [Header("Player death variables")]
    public float timeBeforeSceneLoad;

    [Header("Tags")]
    public string playerTag = "Player";
    public string enemyTag = "Enemy";

    [Header("Prefabs for populating")]
    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject enemyTwoPrefab;
    public GameObject keyPrefab;
    public GameObject exitPrefab;

    private GameObject key;
    private GameObject exit;
    private GameObject player;

    private void Awake()
    {
        if(generator != null) ////Nullcheck om errors te verwijderen in AI vs AI
        {
            generator.dungeonGenerated += DungeonGenerated;
        }
        

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void DungeonGenerated()
    {
        PopulateRooms(generator.rooms);
    }

    private void PopulateRooms(List<DungeonRoom> rooms)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            switch (rooms[i].roomType)
            {
                case RoomType.Player:
                    SpawnPlayer(rooms[i]);
                    break;
                case RoomType.EnemyOne:
                    SpawnEnemy(rooms[i]);
                    break;
                case RoomType.EnemyTwo:
                    SpawnEnemy(rooms[i]);
                    break;
                case RoomType.Key:
                    SpawnKey(rooms[i]);
                    break;
                case RoomType.Exit:
                    SpawnExit(rooms[i]);
                    break;
            }
        }
    }

    private void SpawnPlayer(DungeonRoom room)
    {
        Vector3 spawnPos = new Vector3(room.xPos + generator.roomW * 0.5f, 0.5f, room.yPos + generator.roomH * 0.5f) * generator.dungeonScale;
        player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        player.name = playerPrefab.name;
        player.tag = playerTag;
        player.GetComponent<HealthSystem>().OnDeath += PlayerDeath;
    }

    private void SpawnEnemy(DungeonRoom room)
    {
        Vector3 spawnPos = new Vector3(room.xPos + generator.roomW * 0.5f, 0.5f, room.yPos + generator.roomH * 0.5f) * generator.dungeonScale;

        if (room.roomType == RoomType.EnemyOne)
        {
            GameObject enemy = Instantiate(enemyOnePrefab, spawnPos, Quaternion.identity);
            enemy.name = enemyOnePrefab.name;
            enemy.tag = enemyTag;
            enemy.GetComponent<HealthSystem>().OnDeath += EnemyDeath;
            
        }

        else if (room.roomType == RoomType.EnemyTwo)
        {
            GameObject enemy = Instantiate(enemyTwoPrefab, spawnPos, Quaternion.identity);
            enemy.name = enemyTwoPrefab.name;
            enemy.tag = enemyTag;
            enemy.GetComponent<HealthSystem>().OnDeath += EnemyDeath;
        }
    }

    private void SpawnKey(DungeonRoom room) /////Spawns the key and adds the local KeyTaken() method to key's OnKeyTaken UnityAction
    {
        Vector3 spawnPos = new Vector3(room.xPos + generator.roomW * 0.5f, 0.5f, room.yPos + generator.roomH * 0.5f) * generator.dungeonScale;
        key = Instantiate(keyPrefab, spawnPos, Quaternion.identity);
        key.name = keyPrefab.name;
        key.GetComponent<Key>().OnKeyTaken += KeyTaken;
    }

    private void KeyTaken()
    {
        Debug.Log("Key was taken.");
        Destroy(key);
        exit.SetActive(true);
    }

    private void SpawnExit(DungeonRoom room)
    {
        Vector3 spawnPos = new Vector3(room.xPos + generator.roomW * 0.5f, 0.5f, room.yPos + generator.roomH * 0.5f) * generator.dungeonScale;
        exit = Instantiate(exitPrefab, spawnPos, Quaternion.identity);
        exit.name = exitPrefab.name;
        exit.GetComponent<Exit>().OnExit += ExitReached;
        exit.SetActive(false);
    }

    private void ExitReached()
    {
        Debug.Log("Exit was taken.");
        SceneManager.LoadScene(exitScene);
    }

    private void PlayerDeath()
    {
        Debug.Log("Player died.");
        StartCoroutine(IPlayerDeath());
    }

    private void EnemyDeath()
    {
        Debug.Log("Enemy died.");
    }

    private IEnumerator IPlayerDeath()
    {
        executeAI = false;
        yield return new WaitForSeconds(timeBeforeSceneLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
