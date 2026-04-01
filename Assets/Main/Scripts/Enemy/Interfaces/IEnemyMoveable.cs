using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody rb { get; set; }
    void Move(float velocity);
}
