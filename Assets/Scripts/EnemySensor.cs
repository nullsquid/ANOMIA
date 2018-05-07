using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour {

    public Vector3 directionToSearch = new Vector3(0f, 0f, 2f);

    Node _nodeToSearch;
    Board _gameBoard;

    bool _foundPlayer = false;


    public bool FoundPlayer
    {
        get
        {
            return _foundPlayer;
        }
    } 

	// Use this for initialization
	void Awake () {
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();

	}
	
    public void UpdateSensor(Node enemyNode) {
        Vector3 worldSpacePositionToSearch = transform.TransformVector(directionToSearch) + transform.position;
        if(_gameBoard != null) {

            _nodeToSearch = _gameBoard.FindNodeAt(worldSpacePositionToSearch);

            //Checks to see if the adjacent node is linked to the one the enemy is on
            if(!enemyNode.LinkedNodes.Contains(_nodeToSearch)) {
                _foundPlayer = false;
                return;
            }
            if(_nodeToSearch == _gameBoard.PlayerNode) {
                _foundPlayer = true;
            }
        }
    }
	
}
