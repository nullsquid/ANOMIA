using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(EnemyMove))]
[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyManager : TurnManager {

    EnemyMove _enemyMove;
    EnemySensor _enemySensor;
    EnemyAttack _enemyAttack;

    Board _gameBoard;

    bool _isDead;

    public bool IsDead {
        get
        {
            return _isDead;
        }
    }

    public UnityEvent enemyDeathEvent;

    protected override void Awake() {
        base.Awake();
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _enemyMove = GetComponent<EnemyMove>();
        _enemySensor = GetComponent<EnemySensor>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    public void PlayTurn() {
        if (_isDead) {
            FinishTurn();
            return;
        }
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine() {
        if (_gameManager != null && !_gameManager.IsGameOver) {
            _enemySensor.UpdateSensor(_enemyMove.CurrentNode);
            yield return new WaitForSeconds(.5f);
            if (_enemySensor.FoundPlayer) {
                _gameManager.LoseLevel();

                Vector3 playerPosition = new Vector3(_gameBoard.PlayerNode.Coordinate.x, 0f,
                                                     _gameBoard.PlayerNode.Coordinate.y);
                _enemyMove.Move(playerPosition, 0f);
                while (_enemyMove.isMoving) {
                    yield return null;
                }
                _enemyAttack.Attack();
                
            }
            //attack player

            //movement
            else {
                _enemyMove.MoveOneTurn();
            }
        }
        //wait
        
    }
    public void Die() {
        if (_isDead) {
            return;
        }
        _isDead = true;
        if(enemyDeathEvent != null) {
            enemyDeathEvent.Invoke();
        }
    }
}
