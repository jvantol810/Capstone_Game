using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class GameEvents
{

    //public delegate void OnEnemyMove(Vector2Int enemyTilePosition);
    //public static event OnEnemyMove onEnemyMove;
    public class Vector2IntEvent : UnityEvent<Vector2Int>
    {
    }
    public static Vector2IntEvent OnEnemyMove = new Vector2IntEvent();
}
