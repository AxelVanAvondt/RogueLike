using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        savePath = Application.persistentDataPath + "/Player.json";
        if (Player != null)
        {
            LoadPlayer(Player);
        }
    }
    private string savePath;

    public List<Actor> Enemies = new List<Actor>();
    public Actor Player { get; set; }
    public static GameManager Get { get => instance; }

    public List<Consumable> Items = new List<Consumable>();
    public List<Ladder> Ladders = new List<Ladder>();
    public List<Tombstone> Tombstones = new List<Tombstone>();

    public void SavePlayer(Actor player)
    {
        string json = JsonUtility.ToJson(player);
        File.WriteAllText(savePath, json);
    }
    public void LoadPlayer(Actor player)
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, player);
        }
    }
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
    private void OnApplicationQuit()
    {
        SavePlayer(Player);
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if(location == Player.transform.position)
        {
            return Player;
        }
        else
        {
            foreach(Actor enemy in Enemies)
            {
                if(location == enemy.transform.position)
                {
                    return enemy;
                }
            }
            return null;
        }
    }
    public GameObject CreateGameObject(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        actor.name = name;
        return actor;
    }
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
        UIManager.Get.UpdateEnemies(Enemies.Count);
    }
    public void StartEnemyTurn()
    {
        foreach(var enemy in Enemies)
        {
            enemy.GetComponent<Enemy>().RunAI();
        }
    }
    public void RemoveEnemy(Actor enemy)
    {
        Enemies.Remove(enemy);
        UIManager.Get.UpdateEnemies(Enemies.Count);
    }

    public void AddItem(Consumable item)
    {
        Items.Add(item);
    }
    public void RemoveItem(Consumable item)
    {
        Items.Remove(item);
    }
    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (Consumable item in Items)
        {
            if (location == item.transform.position)
            {
                return item;
            }
        }
        return null;
    }
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        // Find all colliders within the specified radius
        Collider[] hitColliders = Physics.OverlapSphere(location, 5);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the collider's game object has the Actor component
            Actor actor = hitCollider.GetComponent<Actor>();
            if (actor != null)
            {
                // Add the actor to the list of nearby enemies
                nearbyEnemies.Add(actor);
            }
        }

        return nearbyEnemies;
    }
    public void AddLadder(Ladder lad)
    {
        Ladders.Add(lad);
    }
    public Ladder GetLadderAtLocation(Vector3 location)
    {
        foreach (Ladder pal in Ladders)
        {
            if (location == pal.transform.position)
            {
                return pal;
            }
        }
        return null;
    }
    public void AddTombStone(Tombstone stone)
    {
        Tombstones.Add(stone);
    }
    public void ClearFloor()
    {
        foreach(Actor frick in Enemies)
        {
            Destroy(frick.gameObject);
        }
        foreach (Consumable ShutUp in Items)
        {
            Destroy(ShutUp.gameObject);
        }
        foreach (Ladder Bich in Ladders)
        {
            Destroy(Bich.gameObject);
        }
        foreach (Tombstone Die in Tombstones)
        {
            Destroy(Die.gameObject);
        }
        Enemies.Clear();
        Items.Clear();
        Ladders.Clear();
        Tombstones.Clear();
    }
}
