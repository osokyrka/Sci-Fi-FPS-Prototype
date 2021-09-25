using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f, _jumpHeight = 3f, _gravity = 9.81f;
    private float _yVelocity;
    private CharacterController _controller;
    private bool _cursorLocked;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _hitMarkerPrefab;
    [SerializeField]
    private AudioSource _weaponAudioSource;
    [SerializeField]
    private int _currentAmmo;
    private int _maxAmmo = 50;
    private bool _reload = false;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _cursorLocked = true;
        _muzzleFlash.gameObject.SetActive(false);
        _currentAmmo = _maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        CursorBehaviour();
        FireWeapon();
        CanReload();
    }



    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _speed;
        velocity.y -= _gravity;

        velocity = transform.transform.TransformDirection(velocity);
        
        //doesn't work with navmesh?
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _yVelocity = _jumpHeight;
        }

        _yVelocity -= _gravity; 
        velocity.y = _yVelocity;
        // 

        _controller.Move(velocity * Time.deltaTime);
    }

    void CursorBehaviour()
    {
        if(Input.GetKeyDown(KeyCode.I) && _cursorLocked == true)
        {
            Cursor.lockState = CursorLockMode.None;
            _cursorLocked = false;
        }
        else if (Input.GetKeyDown(KeyCode.I) && _cursorLocked == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _cursorLocked = true;
        }
    }

    void FireWeapon()
    {
        if (Input.GetButton("Fire1") && _currentAmmo > 0 && _reload == false)
        {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitInfo;
                _muzzleFlash.gameObject.SetActive(true);
                _currentAmmo--;

                if (_weaponAudioSource.isPlaying == false)
                {
                    _weaponAudioSource.Play();
                }
                if (Physics.Raycast(ray, out hitInfo, 30))
                {
                    Debug.Log("HIT: " + hitInfo.transform.name);
                    GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(hitMarker, 0.2f);
                    //weapon damage
                } 
        }
        else
        {
            _muzzleFlash.gameObject.SetActive(false);
            _weaponAudioSource.Stop();
        }
        
    }

    void CanReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && _reload == false && _currentAmmo < _maxAmmo)
        {
            _reload = true;
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        _currentAmmo = _maxAmmo;
        _reload = false;
    }
        
}
