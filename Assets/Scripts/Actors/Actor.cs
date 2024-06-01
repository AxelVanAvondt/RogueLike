using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Actor : MonoBehaviour
{
    [Header("Powres")]
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int defense;
    [SerializeField] private int power;


    public int MaxHitPoints {  get { return maxHitPoints; } }
    public int HitPoints { get { return hitPoints; } }
    public int Defense { get { return defense; } }
    public int Power { get { return power; } }

    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
        if (GetComponent<Player>())
        {
            UIManager.Get.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }
    private void Die()
    {
        if (GetComponent<Player>())
        {
            UIManager.Get.AddMessage("You Died! Idiot!", Color.red);
            GameObject grave = GameManager.Get.CreateGameObject("Dead", this.transform.position);
            grave.name = $"Remains of {this.name}";
        }
        else if (GetComponent<Enemy>())
        {
            UIManager.Get.AddMessage($"{this.name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this);
        }
        GameObject.Destroy(this.gameObject);
    }
    public void DoDamage(int hp)
    {
        hitPoints -= hp;
        if(hitPoints <= 0)
        {
            hitPoints = 0;
            Die();
        }
        if(GetComponent<Player>())
        {
            UIManager.Get.UpdateHealth(hitPoints, maxHitPoints);
        }
    }
    public void Heal(int hp)
    {
        hitPoints += hp;
        if(hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
        if(GetComponent<Player>())
        {
            int ihp = hp;
            UIManager.Get.UpdateHealth(hitPoints, maxHitPoints);
            if(hitPoints + hp > maxHitPoints)
            {
                ihp = maxHitPoints - hitPoints;
            }
            UIManager.Get.AddMessage($"You health increased by {ihp}... I literally don't care", Color.green);
        }
    }
}
