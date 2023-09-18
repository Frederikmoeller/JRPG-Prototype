using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyStats enemySetup;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _newSprite;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.sprite = _newSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
