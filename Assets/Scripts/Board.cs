using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    PlayerMove _player;
    Node _playerNode;
    Node _goalNode;
    int _currentCapturePosition = 0;
    Node _startNode;

    public static float spacing = 2f;
    public static readonly Vector2[] directions = {
        new Vector2(spacing, 0f),
        new Vector2(-spacing, 0f),
        new Vector2(0f, spacing),
        new Vector2(0f, -spacing)
    };
    List<Node> _allNodes = new List<Node>();
    List<Node> _chainedNodes = new List<Node>();

    public Node PlayerNode
    {
        get
        {
            return _playerNode;
        }
    }
    public Node GoalNode
    {
        get
        {
            return _goalNode;
        }
    }
    public int CurrentCapturePosition
    {
        get
        {
            return _currentCapturePosition;
        }
        set
        {
            _currentCapturePosition = value;
        }
    }
    public Node StartNode
    {
        get
        {
            return _startNode;
        }
    }

    public List<Transform> capturePosition;
    public List<Node> AllNodes
    {
        get
        {
            return _allNodes;
        }
    }

    public Material onMat;
    public Material offMat;
    public GameObject goalPrefab;
    public float drawGoalTime = 2f;
    public float drawGoalDelay = 2f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;



    void Awake() {
        _player = Object.FindObjectOfType<PlayerMove>().GetComponent<PlayerMove>();
        GetNodeList();
        _goalNode = FindGoalNode();
        _startNode = FindPlayerNode();
        _startNode.HasPath = true;
        _startNode.IsActive = true;

    }

    public void GetNodeList() {
        Node[] nList = GameObject.FindObjectsOfType<Node>();
        _allNodes = new List<Node>(nList);
    }

    public Node FindNodeAt(Vector3 pos) {
        Vector2 boardCoordinate = Utility.Vector2Round(new Vector2(pos.x, pos.z));
        return _allNodes.Find(n => n.Coordinate == boardCoordinate);
    }

    public Node FindPlayerNode() {
        if(_player != null && !_player.isMoving) {            
            return FindNodeAt(_player.transform.position);
        }
        return null;
    }

    public void AddToNodeChain() {
        if (_player != null && !_player.isMoving) {
            
            _playerNode.IsActive = true;
            
            UpdateNodeChain();
        }
    }
    public Node testNode;
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            testNode.IsActive = false;
        }
    }

    public void UpdateAllNodeMaterials() {
        foreach(Node node in AllNodes) {
            node.UpdateNodeMat();
        }
    }

    public void UpdateNodeChain() {

        Queue<Node> chainQueue = new Queue<Node>();
        foreach(Node node in AllNodes) {
            //node.UpdateNodeMat();
            node.IsProcessed = false;
            node.HasPath = false;
        }

        chainQueue.Enqueue(_startNode);
        while(chainQueue.Count > 0) {
            Node curNode = chainQueue.Dequeue();
            curNode.IsProcessed = true;
            
            foreach (Node neighbor in curNode.LinkedNodes) {
                
                neighbor.HasPath = true;
                if (!neighbor.IsProcessed && neighbor.IsActive) {
                    chainQueue.Enqueue(neighbor);
                }

            }

        }
        foreach(Node node in AllNodes) {
            if (!node.HasPath) {
                node.IsActive = false;
            }
        }

    }


    public List<EnemyManager> FindEnemiesAt(Node node) {
        List<EnemyManager> foundEnemies = new List<EnemyManager>();
        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];

        foreach(EnemyManager enemy in enemies) {
            EnemyMove mover = enemy.GetComponent<EnemyMove>();
            if(mover.CurrentNode == node) {
                foundEnemies.Add(enemy);
            }
        }
        return foundEnemies;
    }

    public void UpdatePlayerNode() {
        _playerNode = FindPlayerNode();
    }

    private void OnDrawGizmos() {
        if(_playerNode != null) {
            Gizmos.DrawSphere(_playerNode.transform.position, 0.2f);
        }
    }

    Node FindGoalNode() {
        return _allNodes.Find(n => n.isLevelGoal);
    }

    public void DrawGoal() {
        if(goalPrefab != null && _goalNode != null) {
            GameObject goalInstance = Instantiate(goalPrefab, _goalNode.transform.position, Quaternion.identity);
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "time", drawGoalTime,
                "delay", drawGoalDelay,
                "easetype", drawGoalEaseType
            ));
        }
    }

    public void InitBoard() {
        if(_playerNode != null) {
            _playerNode.InitNode();
        }
    }



}
