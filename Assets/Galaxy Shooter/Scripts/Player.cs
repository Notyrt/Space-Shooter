using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool canTripleShot = false;
    public bool canSpeedBoost = false;
    public bool canShield = false;

    public int lives = 3;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _shieldGameObject;
    [SerializeField]
    private GameObject[] _engines;

    //fire rate:
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;

    [SerializeField]
    private float _speed = 5.0f;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager !=null)
        {
            _uiManager.UpdateLives(lives);
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent <SpawnManager>();

        if(_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
        
    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            if (canTripleShot)
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            else
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(canSpeedBoost)
        {
            transform.Translate(Vector3.right * _speed * 1.5f * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * 1.5f * verticalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }
        

        //vertical border
        if (transform.position.y > 2.0f)
            transform.position = new Vector3(transform.position.x, 2.0f, 0);
        else if (transform.position.y < -4.2f)
            transform.position = new Vector3(transform.position.x, -4.2f, 0);

        //horizontal bound 
        if (transform.position.x > 9.4f)
            transform.position = new Vector3(-9.4f, transform.position.y, 0);
        else if (transform.position.x < -9.4f)
            transform.position = new Vector3(9.4f, transform.position.y, 0);
    }

    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostPowerupOn()
    {
        canSpeedBoost = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldPowerupOn()
    {
        canShield = true;
        _shieldGameObject.SetActive(true);
    }

    public void Damage()
    {
        if (canShield)
        {
            canShield = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        lives--;
        _uiManager.UpdateLives(lives);

        if (lives == 2)
            _engines[0].SetActive(true);
        else if (lives == 1)
            _engines[1].SetActive(true);

        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }

    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canSpeedBoost = false;
    }

}
