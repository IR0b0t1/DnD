using UnityEngine;
using System.Collections.Generic;

public static class GameState
{
    public static Vector3 playerPosition;
    public static HashSet<string> defeatedEnemies = new HashSet<string>();
    public static HashSet<string> openedChests = new HashSet<string>();
}
