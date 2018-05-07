using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : ActorMove {

   
    PlayerCompass _playerCompass;


    protected override void Awake() {
        base.Awake();
        _playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();
    }

    protected override void Start() {
        base.Start();
        UpdateBoard();
    }

    public void UpdateBoard() {
        if(_gameBoard != null) {
            _gameBoard.UpdatePlayerNode();
        }
    }

    void UpdateNodeStates() {
        if(_gameBoard != null) {
            _gameBoard.AddToNodeChain();
        }
    }

    protected override IEnumerator MoveRoutine(Vector3 destination, float delayTime) {
        if (_playerCompass != null) {
            _playerCompass.ShowArrows(false);
        }

        //parent class' move routine
        yield return StartCoroutine(base.MoveRoutine(destination, delayTime));

        UpdateBoard();
        UpdateNodeStates();
        if (_playerCompass != null) {
            _playerCompass.ShowArrows(true);
        }
        base.finishMovementEvent.Invoke();
    }
}
