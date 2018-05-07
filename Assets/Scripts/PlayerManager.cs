using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerManager : TurnManager {

    //encapsulate?
    public PlayerMove playerMove;
    public PlayerInput playerInput;

    Board _gameBoard;

    protected override void Awake() {
        base.Awake();
        playerMove = GetComponent<PlayerMove>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.InputEnabled = true;
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    // Update is called once per frame
    void Update () {
        if (playerMove.isMoving || _gameManager.CurrentTurn != Turn.Player) {
            return;
        }
        playerInput.GetKeyInput();
        if(playerInput.V == 0) {
            if(playerInput.H < 0) {
                playerMove.MoveLeft();
            }
            else if(playerInput.H > 0) {
                playerMove.MoveRight();
            }
        }
        else if(playerInput.H == 0) {
            if (playerInput.V < 0) {
                playerMove.MoveBackward();
            }
            else if (playerInput.V > 0) {
                playerMove.MoveForward();
            }
        }


	}

    void EliminateEnemies() {
        if (_gameBoard != null) {
            List<EnemyManager> enemies = _gameBoard.FindEnemiesAt(_gameBoard.PlayerNode);
            if (enemies.Count != 0) {
                foreach (EnemyManager enemy in enemies) {
                    if (enemy != null) {
                        enemy.Die();
                    }
                }
            }
        }
    }

    public override void FinishTurn() {
        EliminateEnemies();
        base.FinishTurn();
    }
}
