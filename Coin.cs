using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    private Player _player;
    private AudioSource _audioSource;
    private Collider _collider;
    
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<Collider>();
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _audioSource.Play();
                _player.CoinUpdate();
                _collider.enabled = false;
                Destroy(this.gameObject, 0.3f);
            }
            
        }
    }
}
