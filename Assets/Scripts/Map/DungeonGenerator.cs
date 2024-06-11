using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator
{
    private int width;
    private int height;
    private int roomMaxSize;
    private int roomMinSize;
    private int maxRooms;
    private int maxEnemies;
    private int maxItems;
    private int currentFloor; // Toegevoegd om huidige verdieping bij te houden

    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetRoomSize(int minSize, int maxSize)
    {
        roomMinSize = minSize;
        roomMaxSize = maxSize;
    }

    public void SetMaxRooms(int maxRooms)
    {
        this.maxRooms = maxRooms;
    }

    public void SetMaxEnemies(int maxEnemies)
    {
        this.maxEnemies = maxEnemies;
    }

    public void SetMaxItems(int maxItems)
    {
        this.maxItems = maxItems;
    }

    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }

    public void Generate()
    {
        // Initialiseren van de dungeon en het genereren van kamers en gangen
        // Hieronder is een vereenvoudigde versie van een dungeon generator

        // Maak een lijst om de gegenereerde kamers bij te houden
        List<Rect> rooms = new List<Rect>();

        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = Random.Range(roomMinSize, roomMaxSize + 1);
            int roomHeight = Random.Range(roomMinSize, roomMaxSize + 1);
            int roomX = Random.Range(0, width - roomWidth);
            int roomY = Random.Range(0, height - roomHeight);

            Rect newRoom = new Rect(roomX, roomY, roomWidth, roomHeight);

            bool overlaps = false;
            foreach (Rect room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);

                // Hier tekenen we de kamer in de tilemap
                for (int x = (int)newRoom.x; x < (int)newRoom.xMax; x++)
                {
                    for (int y = (int)newRoom.y; y < (int)newRoom.yMax; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
                    }
                }
            }
        }

        // Gangen genereren tussen kamers (hier een vereenvoudigde versie)
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int currentRoomCenter = new Vector2Int(
                (int)rooms[i].center.x,
                (int)rooms[i].center.y
            );
            Vector2Int nextRoomCenter = new Vector2Int(
                (int)rooms[i + 1].center.x,
                (int)rooms[i + 1].center.y
            );

            if (Random.value < 0.5f)
            {
                CreateHorizontalTunnel(currentRoomCenter.x, nextRoomCenter.x, currentRoomCenter.y);
                CreateVerticalTunnel(currentRoomCenter.y, nextRoomCenter.y, nextRoomCenter.x);
            }
            else
            {
                CreateVerticalTunnel(currentRoomCenter.y, nextRoomCenter.y, currentRoomCenter.x);
                CreateHorizontalTunnel(currentRoomCenter.x, nextRoomCenter.x, nextRoomCenter.y);
            }
        }

        // Voeg ladders toe
        if (currentFloor > 0)
        {
            AddLadder(currentFloor - 1, rooms[0].center); // Ladder naar vorige verdieping
        }
        AddLadder(currentFloor + 1, rooms[rooms.Count - 1].center); // Ladder naar volgende verdieping

        // Enemies en items genereren
        GenerateEnemies(rooms);
        GenerateItems(rooms);
    }

    private void CreateHorizontalTunnel(int xStart, int xEnd, int y)
    {
        for (int x = Mathf.Min(xStart, xEnd); x <= Mathf.Max(xStart, xEnd); x++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
        }
    }

    private void CreateVerticalTunnel(int yStart, int yEnd, int x)
    {
        for (int y = Mathf.Min(yStart, yEnd); y <= Mathf.Max(yStart, yEnd); y++)
        {
            Vector3Int pos = new Vector3Int(x, y, 0);
            MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
        }
    }

    private void GenerateEnemies(List<Rect> rooms)
    {
        foreach (var room in rooms)
        {
            int numEnemies = Random.Range(0, maxEnemies + 1);

            for (int i = 0; i < numEnemies; i++)
            {
                Vector2 position = new Vector2(
                    Random.Range(room.x + 1, room.xMax - 1),
                    Random.Range(room.y + 1, room.yMax - 1)
                );

                GameObject enemyObject = GameManager.Get.CreateActor("Enemy", position);
                GameManager.Get.AddEnemy(enemyObject.GetComponent<Actor>());
            }
        }
    }

    private void GenerateItems(List<Rect> rooms)
    {
        foreach (var room in rooms)
        {
            int numItems = Random.Range(0, maxItems + 1);

            for (int i = 0; i < numItems; i++)
            {
                Vector2 position = new Vector2(
                    Random.Range(room.x + 1, room.xMax - 1),
                    Random.Range(room.y + 1, room.yMax - 1)
                );

                GameObject itemObject = GameManager.Get.CreateActor("Item", position);
                GameManager.Get.AddItem(itemObject.GetComponent<Consumable>());
            }
        }
    }

    private void AddLadder(int targetFloor, Vector2 position)
    {
        GameObject ladderObject = GameManager.Get.CreateActor("Ladder", position);
        Ladder ladderComponent = ladderObject.GetComponent<Ladder>();

        if (ladderComponent != null)
        {
            ladderComponent.TargetFloor = targetFloor;
            GameManager.Get.AddLadder(ladderComponent);
        }
        else
        {
            Debug.LogError("Ladder component is niet gevonden op het Ladder GameObject!");
        }
    }
}
