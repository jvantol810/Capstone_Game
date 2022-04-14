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
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }
    public static Vector2IntEvent OnEnemyMove = new Vector2IntEvent();
    public static UnityEvent OnEnterTeleporter = new UnityEvent();
    public static UnityEvent OnPlayerDie = new UnityEvent();
    public static GameObjectEvent OnEnemyDie = new GameObjectEvent();

}
