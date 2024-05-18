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
    public GameObject CreateActor(string name, Vector2 position)
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
}
