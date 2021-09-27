using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _ammoText, _coinText;
    
    
    public void UpdateAmmo (int count)
    {
        _ammoText.text = "Ammo: " + count;
    }
    public void UpdateCoins (int count)
    {
        _coinText.text = "Coins: " + count;
    }
}
