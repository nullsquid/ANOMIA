using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    PlayerManager _player;

    private void Awake() {
        _player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    public void Attack() {
        if(_player != null) {

        }
    }
}
