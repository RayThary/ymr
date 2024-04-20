using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLauncher
{
    private List<IAttackType> _attackTypes = new();
    public List<IAttackType> AttackTypes { get { return _attackTypes; } }

    public void AddAttackType(IAttackType attackType)
    {
        _attackTypes.Add(attackType);
    }
    public void RemoveAttackType(IAttackType attackType)
    {
        _attackTypes.Remove(attackType);
    }

    public void LeftDown()
    {
        for(int i = 0; i < _attackTypes.Count; i++)
        {
            _attackTypes[i].LeftDown();
        }
    }
    public void LeftUp()
    {
        for (int i = 0; i < _attackTypes.Count; i++)
        {
            _attackTypes[i].LeftUp();
        }
    }
}
