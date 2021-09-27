using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField]
    private GameObject _crateDestoyed;
  
    public void DestroyCrate()
    {
        Instantiate(_crateDestoyed, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
