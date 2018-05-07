using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour {
    public Vector3 offScreenOffset = new Vector3(0f, 10f, 0f);

    Board _gameBoard;
    public float deathDelay = 0f;
    public float offscreenDelay = 1f;

    public float iTweenDelay = 0f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutQuint;
    public float moveTime = 0.5f;
	// Use this for initialization
	void Awake() {
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void MoveOffBoard(Vector3 target) {
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", target.x,
            "y", target.y,
            "z", target.z,
            "delay", iTweenDelay,
            "easetype", easeType,
            "time", moveTime
        ));
    }

    public void Die() {
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine() {
        yield return new WaitForSeconds(deathDelay);
        Vector3 offscreenPos = transform.position + offScreenOffset;

        MoveOffBoard(offscreenPos);
        yield return new WaitForSeconds(moveTime + offscreenDelay);

        if(_gameBoard.capturePosition.Count != 0
           && _gameBoard.CurrentCapturePosition < _gameBoard.capturePosition.Count) {
            //Vector3 capturePos = _gameBoard.capturePositions[_gameBoard.CurrentCapturePosition].position;
        }
    }
}
