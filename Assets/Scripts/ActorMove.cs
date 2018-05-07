using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ActorMove : MonoBehaviour {
    public Vector3 destination;
    public bool isMoving = false;

    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

    public float moveSpeed = 1.5f;
    public float tweenDelay = 0f;
    public float rotateTime = 0.5f;
    public bool faceDestination = false;

    protected Node _currentNode;
    protected Board _gameBoard;


    public UnityEvent finishMovementEvent;

    public Node CurrentNode
    {
        get
        {
            return _currentNode;
        }
    }

    protected virtual void Awake() {
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }
    // Use this for initialization
    protected virtual void Start() {
        UpdateCurrentNode();
        
    }

    public void Move(Vector3 destinationPos, float delayTime = 0.25f) {
        if (isMoving) {
            return;
        }

        if (_gameBoard != null) {
            Node targetNode = _gameBoard.FindNodeAt(destinationPos);
            if (targetNode != null && _currentNode != null) {
                if (_currentNode.LinkedNodes.Contains(targetNode)) {
                    StartCoroutine(MoveRoutine(destinationPos, delayTime));
                }
            }
            else {
                Debug.LogWarning("current node is not connected to target node");
            }
        }
    }

    protected virtual IEnumerator MoveRoutine(Vector3 destination, float delayTime) {
        
        isMoving = true;
        if(faceDestination == true) {
            FaceDestination();
        }
        yield return new WaitForSeconds(delayTime);
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", destination.x,
            "y", destination.y,
            "z", destination.z,
            "delay", tweenDelay,
            "easetype", easeType,
            "speed", moveSpeed
        ));
        while (Vector3.Distance(destination, transform.position) > 0.01f) {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destination;
        isMoving = false;

        UpdateCurrentNode();
    }

    IEnumerator Test() {
        yield return new WaitForSeconds(1f);
        MoveBackward();
        yield return new WaitForSeconds(1f);
        MoveRight();
        yield return new WaitForSeconds(1f);
        MoveForward();
        yield return new WaitForSeconds(1f);
    }

    //Wrapper methods for movement
    public void MoveLeft() {
        Vector3 newPos = transform.position + new Vector3(-Board.spacing, 0f, 0f);
        Move(newPos, 0);
    }

    public void MoveRight() {
        Vector3 newPos = transform.position + new Vector3(Board.spacing, 0f, 0f);
        Move(newPos, 0);
    }

    public void MoveForward() {
        Vector3 newPos = transform.position + new Vector3(0f, 0f, Board.spacing);
        Move(newPos, 0);
    }

    public void MoveBackward() {
        Vector3 newPos = transform.position + new Vector3(0f, 0f, -Board.spacing);
        Move(newPos, 0);
    }

    protected void UpdateCurrentNode() {
        if(_gameBoard != null) {
            _currentNode = _gameBoard.FindNodeAt(transform.position);
        }
    }

    protected void FaceDestination() {
        Vector3 relativePosition = destination - transform.position;

        Quaternion newRotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        float newY = newRotation.eulerAngles.y;

        iTween.RotateTo(gameObject, iTween.Hash(
            "y", newY,
            "delay", 0f,
            "easetype", easeType,
            "time", rotateTime

        ));
    }
}
