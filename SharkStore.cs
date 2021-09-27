using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkStore : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Player _player = GameObject.Find("Player").GetComponent<Player>();
                if (_player != null)
                {
                    if(_player._hasCoins == true)
                    {
                        _player.SpendCoin();
                        _player.EquipWeapon();
                        _audioSource.Play();
                    }
                }
                
            }
        }
    }
}
