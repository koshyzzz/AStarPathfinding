using System.Collections.Generic;
using UnityEngine;

public class AStar
{   
    private const int MOVE_COST = 1;

    class TileInfo
    {
        public float hCost;
        public int gCost;
        public float FCost
        {
            get
            {
                return hCost + gCost;
            }
        }
        public TileInfo lastTile;

        public Tile thisTile;
    }

    public static List<Tile> FindPath(Tile startTile, Tile endTile)
    {
        Dictionary<Tile, TileInfo> openTiles = new();
        Dictionary<Tile, TileInfo> checkedTiles = new();
        List<List<Tile>> allTiles = TileManager.instance.tileList;

        Vector2Int[] directions = new Vector2Int[]
        {
            new(1, 0), new(0, 1), new(-1, 0), new(0, -1)
        };

        openTiles.Add(startTile, new(){
            gCost = 0, 
            hCost = CalculateHCost(startTile, endTile), 
            thisTile = startTile, 
            lastTile = null
        });

        while(openTiles.Count > 0)
        {
            TileInfo currentTile = GetLowestCostTile(openTiles);
            if(currentTile.thisTile == endTile)
            {
                return Path(currentTile);
            }

            openTiles.Remove(currentTile.thisTile);
            if (checkedTiles.ContainsKey(currentTile.thisTile))
                continue;

            checkedTiles.Add(currentTile.thisTile, currentTile);

            foreach(Vector2Int neighbour in directions)
            {
                int x = neighbour.x + currentTile.thisTile.x;
                int y = neighbour.y + currentTile.thisTile.y;

                if (x < 0 || y < 0 || x >= allTiles.Count || y >= allTiles[0].Count)
                    continue;

                Tile t = allTiles[x][y];

                if (t.type == Tile.Type.Obstacle)
                    continue;

                if (checkedTiles.ContainsKey(t))
                    continue;

                if (openTiles.ContainsKey(t))
                    continue;

                TileInfo ti = new()
                {
                    gCost = currentTile.gCost + MOVE_COST,
                    hCost = CalculateHCost(t, endTile),
                    thisTile = t,
                    lastTile = currentTile
                };
                openTiles.Add(t, ti);
            }
        }
        return null;
    }

    private static TileInfo GetLowestCostTile(Dictionary<Tile, TileInfo> tiles)
    {
        float min = int.MaxValue;
        Tile minTile = null;
        foreach(KeyValuePair<Tile, TileInfo> kvp in tiles)
        {
            if(kvp.Value.FCost < min)
            {
                min = kvp.Value.FCost;
                minTile = kvp.Key;
            }
        }
        return tiles[minTile];
    }

    private static List<Tile> Path(TileInfo endT)
    {
        List<Tile> path = new()
        {
            endT.thisTile
        };
        TileInfo currentT = endT;
        while(currentT.lastTile != null)
        {
            path.Insert(0, currentT.lastTile.thisTile);
            currentT = currentT.lastTile;
        }
        return path;
    }

    private static float CalculateHCost(Tile s, Tile e)
    {
        return (s.x - e.x) * (s.x - e.x) + (s.y - e.y) * (s.y - e.y);
    }
}
