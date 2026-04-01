using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody rb { get; set; }
    void Move(Vector3 velocity);
}
