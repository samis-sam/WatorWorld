using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatorWorld;
public abstract class Tile(int x, int y)
{
    public int x { get; set; } = x;
    public int y { get; set; } = y;

    private const byte reprodMax = 10;
    private byte reprod = reprodMax;

    // add a max value for later
    
    protected static readonly List<Moves> directionsTemplate = [Moves.up, Moves.down, Moves.left, Moves.right];

    public abstract List<Moves> GetValidMoves(Dictionary<Moves, Tile?> neighbours);

    // Check if the tile can reproduce, then tick down reprod counter
    public bool ReproduceCheck()
    {
        if (reprod <= 0)
        {
            reprod = reprodMax;
            return true;
        } 
        else
        {
            reprod--;
            return false;
        }
    }
}
