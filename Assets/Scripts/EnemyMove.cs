using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
    Standing,
    Patrol,
    Sentry
}
public class EnemyMove : ActorMove {

    public Vector3 directionToMove = new Vector3(0f, 0f, Board.spacing);

    public MovementType movementType = MovementType.Standing;

    public float standTime = 1f;
    protected override void Awake() {
        base.Awake();
        faceDestination = true;
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
    }

    public void MoveOneTurn() {
        switch (movementType) {
            case MovementType.Standing:
                Stand();
                break;
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.Sentry:
                SentrySpin();
                break;
        }
        
    }

    void Stand() {
        StartCoroutine(StandRoutine());

    }

    IEnumerator StandRoutine() {
        yield return new WaitForSeconds(standTime);
        base.finishMovementEvent.Invoke();
    }
    void Patrol() {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine() {
        Vector3 startPos = new Vector3(_currentNode.Coordinate.x, 0f, _currentNode.Coordinate.y);

        Vector3 newDestination = startPos + transform.TransformVector(directionToMove);

        Vector3 nextDestination = startPos + transform.TransformVector(directionToMove * 2f);

        Move(newDestination, 0f);

        while (isMoving) {
            yield return null;
        }
        if(_gameBoard != null) {
            Node newDestinationNode = _gameBoard.FindNodeAt(newDestination);
            Node nextDestinationNode = _gameBoard.FindNodeAt(nextDestination);

            if(nextDestinationNode == null) {
                destination = startPos;
                FaceDestination();
                yield return new WaitForSeconds(rotateTime);
            }
            else if (!_currentNode.LinkedNodes.Contains(nextDestinationNode)) {
                destination = startPos;
                FaceDestination();
                yield return new WaitForSeconds(rotateTime);
            }
        }
        base.finishMovementEvent.Invoke();

    }
    
    void SentrySpin() {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine() {
        Vector3 localForward = new Vector3(0f, 0f, Board.spacing);
        destination = transform.TransformVector(localForward * -1) + transform.position;

        FaceDestination();

        yield return new WaitForSeconds(rotateTime);
        base.finishMovementEvent.Invoke();
    }


}
