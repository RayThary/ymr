using UnityEngine;

public interface IComponentObject
{
    void Fire(ComponentObject componentObject);
    void Update();
    void Enter(Collider other);
}
