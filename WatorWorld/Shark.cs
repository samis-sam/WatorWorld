using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatorWorld;
public class Shark(int x, int y) : Tile(x, y)
{
    private const byte maxHunger = 7;
    private byte hunger = maxHunger;
    public override List<Moves> GetValidMoves(Dictionary<Moves, Tile?> neighbours)
    {
        List<Moves> validMoves = new();
        List<Moves> fishMoves = new();
        foreach (KeyValuePair<Moves, Tile?> tile in neighbours)
        {
            if (tile.Value == null)
            {
                validMoves.Add(tile.Key);
            }
            else if (tile.Value is Fish)
            {
                fishMoves.Add(tile.Key);
            }
        }

        hunger--;

        if (hunger <= 0)
        {
            return [Moves.starved];
        }
        else if (fishMoves.Count > 0)
        {
            hunger = maxHunger;
            return fishMoves;
        }
        else
        {
            return validMoves;
        }
    }
}
