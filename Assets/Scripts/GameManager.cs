using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[System.Serializable]
public enum Turn{
    Player,
    Enemy
}
public class GameManager : MonoBehaviour {

    Board _gameBoard;
    PlayerManager _player;

    List<EnemyManager> _enemies;
    Turn _currentTurn = Turn.Player;


    bool _hasLevelStarted = false;
    bool _isGamePlaying = false;
    bool _isGameOver = false;
    bool _hasLevelFinished = false;

    public bool HasLevelStarted
    {
        get
        {
            return _hasLevelStarted;
        }

        set
        {
            _hasLevelStarted = value;
        }
    }
    public bool IsGamePlaying
    {
        get
        {
            return _isGamePlaying;
        }

        set
        {
            _isGamePlaying = value;
        }
    }
    public bool IsGameOver
    {
        get
        {
            return _isGameOver;
        }

        set
        {
            _isGameOver = value;
        }
    }
    public bool HasLevelFinished
    {
        get
        {
            return _hasLevelFinished;
        }

        set
        {
            _hasLevelFinished = value;
        }
    }

    public float delay = 1.0f;

    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;


    public Turn CurrentTurn
    {
        get
        {
            return _currentTurn;
        }
    }

    private void Awake() {
        _gameBoard = Object.FindObjectOfType<Board>().GetComponent<Board>();
        _player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        EnemyManager[] enemies = Object.FindObjectsOfType<EnemyManager>() as EnemyManager[];
        _enemies = enemies.ToList();
        
    }
    // Use this for initialization
    void Start () {
		if(_player != null && _gameBoard != null) {
            StartCoroutine("RunGameLoop");
        }
        
	}
	
    IEnumerator RunGameLoop() {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    IEnumerator StartLevelRoutine() {
        if(setupEvent != null) {
            setupEvent.Invoke();
        }
        _player.playerInput.InputEnabled = false;
        while (!_hasLevelStarted) {
            yield return null;
        }

        if(startLevelEvent != null) {
            startLevelEvent.Invoke();
        }
        
    }

    IEnumerator PlayLevelRoutine() {

        _isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        _player.playerInput.InputEnabled = true;
        
        if (playLevelEvent != null) {
            playLevelEvent.Invoke();
        }
        while (!_isGameOver) {
            yield return null;
            _isGameOver = IsWinner();
            
        }

        Debug.Log("WIN==================");

    }

    IEnumerator EndLevelRoutine() {
        Debug.Log("End Level");

        _player.playerInput.InputEnabled = false;

        //show end screen
        if(endLevelEvent != null) {
            endLevelEvent.Invoke();
        }
        while (!_hasLevelFinished) {
            //user presses buttons to continue
            //HasLevelFinished = true
            yield return null;
        }

        RestartLevel();
    }

    public void LoseLevel() {
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine() {
        _isGameOver = true;
        yield return new WaitForSeconds(.75f);
        if(loseLevelEvent != null) {
            loseLevelEvent.Invoke();
        }
        yield return new WaitForSeconds(2f);

        Debug.Log("LOSE!!!==================");

        RestartLevel();
    }

    void RestartLevel() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel() {
        _hasLevelStarted = true;
    }

    bool IsWinner() {
        if(_gameBoard.PlayerNode != null) {
            return (_gameBoard.PlayerNode == _gameBoard.GoalNode);
        }
        return false;
    }
    void PlayPlayerTurn() {
        _currentTurn = Turn.Player;
        _player.IsTurnComplete = false;
    }

    void PlayEnemyTurn() {
        _currentTurn = Turn.Enemy;
        foreach(EnemyManager enemy in _enemies) {
            if(enemy != null && !enemy.IsDead) {
                enemy.IsTurnComplete = false;
                enemy.PlayTurn();
            }
        }
    }

    bool IsEnemyTurnComplete() {
        foreach(EnemyManager enemy in _enemies) {
            if (enemy.IsDead) {
                continue;
            }
            if (!enemy.IsTurnComplete) {
                return false;
            }
        }
        return true;
    }

    bool AreEnemiesAllDead() {
        foreach(EnemyManager enemy in _enemies) {
            if (!enemy.IsDead) {
                return false;
            }
        }
        return true;
    }

    public void UpdateTurn() {
        if(_currentTurn == Turn.Player && _player != null) {
            if (_player.IsTurnComplete && !AreEnemiesAllDead()) {
                _gameBoard.UpdateAllNodeMaterials();
                PlayEnemyTurn();
            }

        }
        else if(_currentTurn == Turn.Enemy) {
            if (IsEnemyTurnComplete()) {
                PlayPlayerTurn();
            }
        }
    }


}
