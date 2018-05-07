using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour {

    Board _gameBoard;
    List<GameObject> _arrows = new List<GameObject>();

    public GameObject arrowPrefab;
    public float scale = 1f;
    public float startDistance = .25f;
    public float endDistance = 0.5f;

    public float moveTime = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    public float delay = 0f;
    private void Awake() {
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();
        SetupArrows();
    }

    void SetupArrows() {
        if(arrowPrefab == null) {
            return;
        }
        foreach(Vector2 dir in Board.directions) {
            Vector3 dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            GameObject arrowInstance = Instantiate(arrowPrefab, 
                transform.position + dirVector * startDistance, 
                rotation);
            arrowInstance.transform.localScale = new Vector3(scale, scale, scale);
            arrowInstance.transform.parent = transform;
            _arrows.Add(arrowInstance);
        }
    }

    void MoveArrow(GameObject arrowInstance) {
        iTween.MoveBy(arrowInstance, iTween.Hash(
            "z",endDistance-startDistance,
            "looptype", iTween.LoopType.loop,
            "time", moveTime,
            "easetype", easeType,
            "delay", delay
            
        ));
    }

    void MoveArrows() {
        foreach(GameObject arrow in _arrows) {
            MoveArrow(arrow);
        }
    }

    public void ShowArrows(bool state) {
        if(_gameBoard == null) {
            Debug.LogWarning("no board found");
            return;
        }
        if (_arrows == null || _arrows.Count != Board.directions.Length) {
            Debug.LogWarning("no arrows found");
            return;
        }
        if(_gameBoard.PlayerNode != null) {
            for(int i = 0; i < Board.directions.Length; i++) {
                Node neighbor = _gameBoard.PlayerNode.FindNeighborAt(Board.directions[i]);

                if(neighbor == null || !state) {
                    _arrows[i].SetActive(false);
                }
                else {
                    bool activeState = _gameBoard.PlayerNode.LinkedNodes.Contains(neighbor);
                    _arrows[i].SetActive(activeState);
                }
            }
        }
        ResetArrows();
        MoveArrows();
    }

    void ResetArrows() {
        for(int i = 0; i < Board.directions.Length; i++) {
            iTween.Stop(_arrows[i]);
            Vector3 dirVector = new Vector3(Board.directions[i].normalized.x, 0f,
                                            Board.directions[i].normalized.y);
            _arrows[i].transform.position = transform.position + dirVector * startDistance;
        }
    }
}
