using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private int powerupID;      //0 - Triple shot || 1 - Speed boost || 3 - Shield

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <-7.0f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collided with: " + other.name);

        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                if (powerupID == 0)
                    player.TripleShotPowerupOn();
                else if (powerupID == 1)
                    player.SpeedBoostPowerupOn();
                else if (powerupID == 2)
                    player.ShieldPowerupOn();
            }
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);            
            Destroy(this.gameObject);

        }
    }
}
