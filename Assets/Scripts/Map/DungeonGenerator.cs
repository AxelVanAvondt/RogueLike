using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    private int maxEnemies;
    private int maxItems;
    private int currentFloor;
    private int frick;
    List<Room> rooms = new List<Room>();
    List<string> enemies = new List<string>() { "Bob", "Goncalo", "Hanging", "Kevin", "Lapis", "DeathMetal", "Huh", "Humpty Dumpty"};

    public void SetCurrentFloor(int FLOOF)
    {
        currentFloor = FLOOF;
        frick = currentFloor;
    }
    public void SetMaxItems(int hehe)
    {
        maxItems = hehe;
    }
    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetRoomSize(int min, int max)
    {
        minRoomSize = min;
        maxRoomSize = max;
    }

    public void SetMaxRooms(int max)
    {
        maxRooms = max;
    }
    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }

    public void Generate()
    {
        rooms.Clear();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            var room = new Room(roomX, roomY, roomWidth, roomHeight);

            // if the room overlaps with another room, discard it
            if (room.Overlaps(rooms))
            {
                continue;
            }

            // add tiles make the room visible on the tilemap
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX
                        || x == roomX + roomWidth - 1
                        || y == roomY
                        || y == roomY + roomHeight - 1)
                    {
                        if (!TrySetWallTile(new Vector3Int(x, y)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y, 0));
                    }

                }
            }

            // create a coridor between rooms
            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }
            PlaceEnemies(room, maxEnemies);
            PlaceItems(room, maxItems);
            rooms.Add(room);
        }
        var player = GameManager.Get.CreateGameObject("Player", rooms[0].Center());

        //I switched the ladders names because it's funny. The ladder that goes up is "Down" and the ladder that goes down is "Up"
        GameManager.Get.CreateGameObject("Up", rooms[maxRooms].Center());
        if (player != null)
        {
            player.transform.position = new Vector3(rooms[0].Center().x, rooms[0].Center().y);
        }
        else
        {
            GameManager.Get.CreateGameObject("Player", rooms[0].Center());
        }
        if (currentFloor > 0)
        {
            GameManager.Get.CreateGameObject("Down", rooms[0].Center());
        }
    }

    private bool TrySetWallTile(Vector3Int pos)
    {
        // if this is a floor, it should not be a wall
        if (MapManager.Get.FloorMap.GetTile(pos))
        {
            return false;
        }
        else
        {
            // if not, it can be a wall
            MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
            return true;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        // this tile should be walkable, so remove every obstacle
        if (MapManager.Get.ObstacleMap.GetTile(pos))
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }
        // set the floor tile
        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            // move horizontally, then vertically
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            // move vertically, then horizontally
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        // Generate the coordinates for this tunnel
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        // Set the tiles for this tunnel
        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }
        }
    }

    private void PlaceEnemies(Room room, int maxEnemies)
    {
        // the number of enemies we want 
        int num = Random.Range(0, maxEnemies + 1);

        for (int counter = 0; counter < num; counter++)
        {
            // The borders of the room are walls, so add and substract by 1 
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            // create different enemies 
            if (Random.value < 0.5f)
            {
                GameManager.Get.CreateGameObject("HotDemon", new Vector2(x, y));
            }
            else
            {
                GameManager.Get.CreateGameObject("DragonBF", new Vector2(x, y));
            }
            int i = 0;
            if(frick != currentFloor)
            {
                frick = currentFloor;
                GameManager.Get.CreateGameObject(enemies[i], new Vector2(x, y));
                i++;
            }
        }
    }
    private void PlaceItems(Room room, int MaxItems)
    {
        // the number of items we want 
        int num = Random.Range(0, MaxItems + 1);

        for (int counter = 0; counter < num; counter++)
        {
            // The borders of the room are walls, so add and substract by 1 
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            // create different items 
            if (Random.value < 0.33f)
            {
                GameManager.Get.CreateGameObject("Fireball", new Vector2(x, y));
            }
            else if (Random.value < 0.66f)
            {
                GameManager.Get.CreateGameObject("HealthPotion", new Vector2(x, y));
            }
            else
            {
                GameManager.Get.CreateGameObject("ScrollOfConfusion", new Vector2(x, y));
            }
        }
    }

}
//^(?([^\r\n])\s)*\r?$\r?\n
