using System.Collections.Generic;

namespace Samurai.Example.Enemies.Data;

public class FlockData
{
    public Enemy Leader;
    public HashSet<Enemy> Members;

    public FlockData(Enemy leader, HashSet<Enemy> members)
    {
        Leader = leader;
        Members = members;
    }
}