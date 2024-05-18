using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor target { get; set; }
    public bool IsFighting { get; set; }
    public AStar algorithm { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
        algorithm = GetComponent<AStar>();
    }
    public void MoveAlongPath(Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.Move(GetComponent<Actor>(), direction);
    }
    public void RunAI()
    {
        // TODO: If target is null, set target to player (from gameManager)
        if(target  == null)
        {
            target = GameManager.Get.Player;
        }
        // TODO: convert the position of the target to a gridPosition
        Grid gp = target.transform.GetComponent<Grid>();
        Vector3Int gridPosition = gp.WorldToCell(transform.position);

        // First check if already fighting, because the FieldOfView check costs more cpu
        if (IsFighting || GetComponent<Actor>().FieldOfView.Contains(gridPosition))
        {
            // TODO: If the enemy was not fighting, is should be fighting now
            if(IsFighting == false)
            {
                IsFighting = true;
            }
            // TODO: call MoveAlongPath with the gridPosition
            MoveAlongPath(gridPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
