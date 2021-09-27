using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private float _reloadSpeed = 2f;
    private UIManager _uiManager;
    private int _coins = 0;
    [SerializeField]
    private float _fireRate = 0.5f, _lastShot;
    [SerializeField]
    private Image _coinImage;
    [SerializeField]
    public bool _hasCoins = false;
    [SerializeField]
    private GameObject _weapon;
    private bool _hasGun = false;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _cursorLocked = true;
        _muzzleFlash.gameObject.SetActive(false);
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmo(_currentAmmo);
        _uiManager.UpdateCoins(_coins);
        _coinImage.gameObject.SetActive(false);
        _weapon.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        CursorBehaviour();
        FireWeapon();
        CanReload();
        HasCoins();
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
        if (Input.GetButton("Fire1") && _currentAmmo > 0 && _reload == false && Time.time > _lastShot && _hasGun == true)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            StartCoroutine(ShotEffect());
            RaycastHit hitInfo;
            _currentAmmo--;
            _uiManager.UpdateAmmo(_currentAmmo);
            _lastShot = Time.time + _fireRate;
            
            if (Physics.Raycast(ray, out hitInfo, 30))
            {
                Debug.Log("HIT: " + hitInfo.transform.name);
                GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(hitMarker, 0.2f);
                //weapon damage
                Destructable crate = hitInfo.transform.GetComponent<Destructable>();
                if(crate != null)
                {
                    crate.DestroyCrate();
                }
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
        if(Input.GetKeyDown(KeyCode.R) && _reload == false && _currentAmmo < _maxAmmo && _hasGun == true)
        {
            
            _reload = true;
            StartCoroutine(Reload());
        }
    }
    public void CoinUpdate()
    {
        _coins++;
        _uiManager.UpdateCoins(_coins);
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadSpeed);
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmo(_currentAmmo);
        _reload = false;
    }
    IEnumerator ShotEffect()
    {
        _muzzleFlash.gameObject.SetActive(true);
        _weaponAudioSource.Play();
        yield return new WaitForSeconds(1.0f);
        _muzzleFlash.gameObject.SetActive(false);

    }

    public void HasCoins()
    {
        if(_coins > 0)
        {
            _hasCoins = true;
            _coinImage.gameObject.SetActive(true);
        }
        else
        {
            _hasCoins = false;
            _coinImage.gameObject.SetActive(false);
        }
    }
    public void SpendCoin()
    {
        _coins -= 1;
        _uiManager.UpdateCoins(_coins);
    }
    public void EquipWeapon()
    {
        _weapon.SetActive(true);
        _hasGun = true;
    }
        
}
