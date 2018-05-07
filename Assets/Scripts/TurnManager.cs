using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    protected GameManager _gameManager;
    protected bool _isturnComplete;

    public bool IsTurnComplete
    {
        get
        {
            return _isturnComplete;
        }

        set
        {
            _isturnComplete = value;
        }
    }
    protected virtual void Awake() {
        _gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public virtual void FinishTurn() {
        _isturnComplete = true;

        //update game manager
        if(_gameManager != null) {
            _gameManager.UpdateTurn();
        }
    }

    
}
