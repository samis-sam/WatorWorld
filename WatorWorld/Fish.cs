using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatorWorld;
public class Fish(int x, int y) : Tile(x, y)
{
    public override List<Moves> GetValidMoves(Dictionary<Moves, Tile?> neighbours)
    {
        List<Moves> validMoves = new();

        foreach (KeyValuePair<Moves, Tile?> tile in neighbours)
        {
            if (tile.Value == null)
            {
                validMoves.Add(tile.Key);
            }
        }
        return validMoves;
    }
}
