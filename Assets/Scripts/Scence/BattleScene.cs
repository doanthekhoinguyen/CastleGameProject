using System;
using System.Collections;
using System.Collections.Generic;
using Castle.CustomUtil;
using MVC.Controller;
using MVC.Model;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private BattleController battleController;
    private void Start()
    {
        ProcessEntry();
    }

    private void ProcessEntry()
    {
        
        battleController.Init(new BattleModel()
        {
            InitCoin = 25,
            Heart = 5
        });
    }
    
}
