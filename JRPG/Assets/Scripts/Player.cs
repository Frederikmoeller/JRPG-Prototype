using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerStats playerSetup;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI HP;
    [SerializeField] private TextMeshProUGUI Mana;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _newSprite;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.sprite = _newSprite;
        nameText.text = playerSetup.CharacterName;
        HP.text = playerSetup.HP + "/" + playerSetup.MaxHP;
        Mana.text = playerSetup.Mana + "/" + playerSetup.MaxMana;
    }
}
