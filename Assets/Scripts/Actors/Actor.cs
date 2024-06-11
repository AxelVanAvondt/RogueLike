using System;
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
    [SerializeField] private int level;
    [SerializeField] private int Xp;
    [SerializeField] private int xpToNextLevel;


    public int MaxHitPoints {  get { return maxHitPoints; } }
    public int HitPoints { get { return hitPoints; } }
    public int Defense { get { return defense; } }
    public int Power { get { return power; } }
    public int Level { get { return level; } }
    public int XP { get { return Xp; } }
    public int XPToNextLevel { get { return xpToNextLevel; } }

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
            UIManager.Get.UpdateLevel(1);
            UIManager.Get.UpdateXP(0);
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
        if (GetComponent<Enemy>())
        {
            UIManager.Get.AddMessage($"{this.name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this);
        }
        Destroy(gameObject);
        GameManager.Get.DeleteSave();
    }
    public void DoDamage(int hp, Actor attacker)
    {
        hitPoints -= hp;
        if(hitPoints <= 0)
        {
            hitPoints = 0;
            Die();
            if (attacker.GetComponent<Player>())
            {
                attacker.AddXp(Xp);
            }
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
    public void AddXp(int xp)
    {
        Xp += xp;
        if (Xp >= xpToNextLevel)
        {
            Xp = 0;
            level++;
            xpToNextLevel = (int)Math.Round(xpToNextLevel * 1.3);
            maxHitPoints += 5;
            defense += 2;
            power += 2;
            UIManager.Get.AddMessage("Dear Player, \nIt seems like you gained a level... Well done. \nLove, The Game", Color.blue);
        }
        UIManager.Get.UpdateXP(Xp);
        UIManager.Get.UpdateLevel(level);
    }
}
