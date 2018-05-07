using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    Vector2 _coordinate;
    List<Node> _neighborNodes = new List<Node>();
    List<Node> _linkedNodes = new List<Node>();
    Board _board;
    bool _isInitialized = false;
    bool _isPowered = false;
    bool _isChained = false;
    bool _hasPath = false;
    bool _isProcessed = false;
    bool _isActive = false;

    public Material poweredMaterial;
    public Material unpoweredMaterial;

    public GameObject linkPrefab;
    public GameObject geometry;
    public float scaleTime = 0.3f;
    public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

    public float delay = 1f;

    public LayerMask obstacleLayer;

    public Vector2 Coordinate
    {
        get
        {
            return Utility.Vector2Round(_coordinate);
        }
    }
    public List<Node> NeighborNodes
    {
        get
        {
            return _neighborNodes;
        }
    }
    public List<Node> LinkedNodes
    {
        get
        {
            return _linkedNodes;
        }
    }
    public bool IsPowered
    {
        get
        {
            return _isPowered;
        }
        set
        {
            _isPowered = value;
        }
    }
    public bool IsChained
    {
        get
        {
            return _isChained;
        }

        set
        {
            _isChained = value;
        }
    }
    public bool HasPath
    {
        get
        {
            return _hasPath;
        }
        set
        {
            _hasPath = value;
        }
    }
    public bool IsProcessed
    {
        get
        {
            return _isProcessed;
        }
        set
        {
            _isProcessed = value;
        }
    }
    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
        }
    }

    public bool isLevelGoal = false;
    public bool isGenerator = false;

    void Awake() {
        _board = Object.FindObjectOfType<Board>();
        _coordinate = new Vector2(transform.position.x, transform.position.z);
    }
    // Use this for initialization
    void Start () {
		if(geometry != null) {
            geometry.transform.localScale = Vector3.zero;

            
        }

        if(_board != null) {
            _neighborNodes = FindNeighbors(_board.AllNodes);
        }
	}

    public void ShowGeometry() {
        if(geometry != null) {
            iTween.ScaleTo(geometry, iTween.Hash(
                "time", scaleTime,
                "scale", Vector3.one,
                "easetype", easeType,
                "delay", delay
            ));
        }
    }

    private void Update() {
        UpdateNodeMat();
    }
    public void UpdateNodeMat() {
        if(_isActive == true && _hasPath == true) {
            gameObject.GetComponentInChildren<Renderer>().material = poweredMaterial;
        }
        else {
            gameObject.GetComponentInChildren<Renderer>().material = unpoweredMaterial;
        }
    }

    public List<Node> FindNeighbors(List<Node> nodes) {
        List<Node> nList = new List<Node>();

        foreach(Vector2 dir in Board.directions) {
            Node foundNeighbor = nodes.Find(n => n.Coordinate == Coordinate + dir);
            if(foundNeighbor != null && !nList.Contains(foundNeighbor)) {
                nList.Add(foundNeighbor);
            }
        }

        return nList;
    }

    public Node FindNeighborAt(List<Node> nodes, Vector2 dir) {
        return nodes.Find(n => n.Coordinate == Coordinate + dir);
    }

    public Node FindNeighborAt(Vector2 dir) {
        return FindNeighborAt(NeighborNodes, dir);
    }

    public void InitNode() {
        if (!_isInitialized) {
            ShowGeometry();
            InitNeighbors();
            _isInitialized = true;
        }
    }
	
    void InitNeighbors() {
        StartCoroutine(InitNeighborsRoutine());
    }

    IEnumerator InitNeighborsRoutine() {
        yield return new WaitForSeconds(delay);
        foreach(Node n in _neighborNodes) {
            if (!_linkedNodes.Contains(n)) {
                Obstacle obstacle = FindObstacle(n);
                if (obstacle == null) {
                    LinkNode(n);
                    n.InitNode();
                }
            }
        }
    }

    void LinkNode(Node targetNode) {
        if (linkPrefab != null) {
            GameObject linkInstance = Instantiate(linkPrefab, 
                                      transform.position, Quaternion.identity);
            linkInstance.transform.parent = transform;

            Link link = linkInstance.GetComponent<Link>();
            if (link != null) {
                link.DrawLink(transform.position, targetNode.transform.position);
            }
            if (!_linkedNodes.Contains(targetNode)) {
                _linkedNodes.Add(targetNode);
            }
            if (!targetNode.LinkedNodes.Contains(this)) {
                targetNode.LinkedNodes.Add(this);
            }
        }
    }

    Obstacle FindObstacle(Node targetNode) {
        Vector3 checkDirection = targetNode.transform.position - transform.position;
        RaycastHit raycastHit;

        if(Physics.Raycast(transform.position, checkDirection, out raycastHit, Board.spacing + 0.1f, obstacleLayer)) {
            return raycastHit.collider.GetComponent<Obstacle>();
        }
        return null;
    }
	
}
