using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpriteBasedOnInput : MonoBehaviour
{
    [SerializeField] private Sprite multiplayerSprite;
    [SerializeField] private Sprite singleplayerSprite;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        bool isSinglePlayer = PlayerPrefs.GetInt("IsSinglePlayer") == 1;
        _spriteRenderer.sprite = isSinglePlayer ? singleplayerSprite : multiplayerSprite;
    }
}
