using System.Collections;
using System.Collections.Generic;
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
    }
    public List<Actor> Enemies = new List<Actor>();
    public Actor Player { get; set; }
    public static GameManager Get { get => instance; }

    public List<Consumable> Items = new List<Consumable>();

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
}
