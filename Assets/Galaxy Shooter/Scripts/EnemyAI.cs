using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyExplosionPrefab;
    private float _speed = 2.0f;

    [SerializeField]
    private AudioClip _clip;

    private UIManager _uiManager;
    private GameManager _gameManager;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6.5f)
        {
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 6.5f, 0);
        }
        if (_gameManager.gameOver == true)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);

        if(other.tag == "Laser")
        {
            if (_uiManager != null) 
                _uiManager.UpdateScore();
            Destroy(other.gameObject);
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player")
        {
            if (_uiManager != null) 
                _uiManager.UpdateScore();
            Player player = other.GetComponent<Player>();

            if(player != null)
                player.Damage();
            Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
    }
}
