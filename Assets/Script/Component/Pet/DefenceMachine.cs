using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceMachine : Pet
{
    ComponentDefenseCreator componentDefense;
    // Start is called before the first frame update
    void Start()
    {
        _find = false;
        componentDefense = new ComponentDefenseCreator();
    }

    public override void Action()
    {
        if (_timer > _cycle)
        {
            if(_master != null && !_master.componentController.ContainComponent(componentDefense))
            {
                _master.componentController.AddComponent(componentDefense);
                _timer = 0;
            }
        }
    }
}