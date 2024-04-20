using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentObject : MonoBehaviour
{
    protected Player _player;
    public Player Player { get { return _player; } }
    [SerializeField]
    protected float _time;
    protected List<IComponentObject> components = new();

    public void AddComponent(IComponentObject componentBullet)
    {
        components.Add(componentBullet);
    }
}
