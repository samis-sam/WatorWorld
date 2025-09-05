using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WatorWorld;
public class World
{
    private Tile?[,] Grid;

    private List<Tile> Fishes;

    private readonly Random rand;

    private readonly int worldHeight;

    private readonly int worldWidth;

    public World(int Height, int Width)
    {
        Grid = new Tile[Height, Width];
        Fishes = [];
        rand = new();
        worldHeight = Height;
        worldWidth = Width;

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < worldHeight; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                Tile? newTile = GetNewTile(x, y);
                if (newTile != null)
                {
                    Grid[x, y] = newTile;
                    Fishes.Add(newTile);
                }
            }
        }
    }

    // return null to indicate no creature (water tile)
    private Tile? GetNewTile(int x, int y)
    {
        int rng = rand.Next(101);
        if (rng <= 10)
        {
            return new Shark(x, y);
        }
        else if (rng <= 50)
        {
            return new Fish(x, y);
        }
        else
        {
            return null;
        }
    }

    // Returns tile at given coordinate, or null if there is no fish there
    public Tile? GetTile(int x, int y)
    {
        return Grid[x, y] ?? null;
    }

    // Move game world forward by one tick
    public void WorldTick()
    {
        List<Tile> originalFishes = new(Fishes);
        foreach(Tile fish in originalFishes)
        {
            FishTurn(fish);
        }
    }

    private void FishTurn(Tile fish)
    {
        Dictionary<Moves, Tile?> neighbours = GetNeighbours(fish);
        List<Moves> possibleMoves = fish.GetValidMoves(neighbours);

        if (possibleMoves.Count > 0)
        {
            Moves nextMove = possibleMoves[rand.Next(possibleMoves.Count)];
            TakeMove(fish, nextMove);
        } 
        else
        {
            TakeMove(fish, Moves.none);
        }
    }

    private void TakeMove(Tile fish, Moves nextMove)
    {
        int fishX = fish.x;
        int fishY = fish.y;
        (int, int) newCoord;

        switch (nextMove)
        {
            case Moves.up:
                newCoord = GetNextCoord(fishX, fishY, Moves.up);
                break;
            case Moves.down:
                newCoord = GetNextCoord(fishX, fishY, Moves.down);
                break;
            case Moves.left:
                newCoord = GetNextCoord(fishX, fishY, Moves.left);
                break;
            case Moves.right:
                newCoord = GetNextCoord(fishX, fishY, Moves.right);
                break;
            case Moves.none:
                return;
            case Moves.starved:
                Grid[fishX, fishY] = null;
                Fishes.Remove(fish);
                return;
            default:
                return;
        }

        Tile? removeFish = Grid[newCoord.Item1, newCoord.Item2];
        if (removeFish != null) { Fishes.Remove(removeFish); }

        Grid[newCoord.Item1, newCoord.Item2] = fish;
        fish.x = newCoord.Item1;
        fish.y = newCoord.Item2;

        if (!fish.ReproduceCheck())
        {
            Grid[fishX, fishY] = null;
        }
        else if (fish is Fish)
        {
            Tile newFish = new Fish(fishX, fishY);
            Grid[fishX, fishY] = newFish;
            Fishes.Add(newFish);
        }
        else
        {
            Tile newShark = new Shark(fishX, fishY); 
            Grid[fishX, fishY] = newShark;
            Fishes.Add(newShark);
        }
    }

    private Dictionary<Moves, Tile?> GetNeighbours(Tile mainFish)
    {
        int fishX = mainFish.x;
        int fishY = mainFish.y;

        (int, int) upCoord = GetNextCoord(fishX, fishY, Moves.up);
        (int, int) downCoord = GetNextCoord(fishX, fishY, Moves.down);
        (int, int) leftCoord = GetNextCoord(fishX, fishY, Moves.left);
        (int, int) rightCoord = GetNextCoord(fishX, fishY, Moves.right);

        Dictionary<Moves, Tile?> neighbours = new()
        {
            { Moves.up, Grid[upCoord.Item1, upCoord.Item2] },
            { Moves.down, Grid[downCoord.Item1, downCoord.Item2] },
            { Moves.left, Grid[leftCoord.Item1, leftCoord.Item2] },
            { Moves.right, Grid[rightCoord.Item1, rightCoord.Item2] },
        };

        return neighbours;
    }

    // caluculate next coord to make sure not going off map
    private (int, int) GetNextCoord(int x, int y, Moves move)
    {
        switch (move)
        {
            case Moves.up:
                if (y == 0)
                {
                    return (x, worldHeight - 1);
                } 
                else
                {
                    return (x, y - 1);
                }
            case Moves.down:
                if (y == worldHeight - 1)
                {
                    return (x, 0);
                }
                else
                {
                    return (x, y + 1);
                }
            case Moves.left:
                if (x <= 0)
                {
                    return (worldWidth - 1, y);
                }
                else
                {
                    return (x - 1, y);
                }
            case Moves.right:
                if (x == worldWidth - 1)
                {
                    return (0, y);
                }
                else
                {
                    return (x + 1, y);
                }
            case Moves.none:
                return (x, y);
            default:
                return (x, y);
        }
    }
}
