using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public float MaxHealth { get; set; }
    public float currentHealth { get; set; }
    public Rigidbody rb { get; set; }

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }

    #region Idle var
    public float randomMovementRange = 5f;
    public float randomMovementSpeed = 2f;
    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
         
        IdleState = new EnemyIdleState(this, StateMachine);
        chaseState = new EnemyChaseState(this, StateMachine);
        attackState = new EnemyAttackState(this, StateMachine);

    }
    private void Start()
    {
        currentHealth = MaxHealth;

        rb = GetComponent<Rigidbody>(); 

        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        StateMachine.currentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentEnemyState.PhysicsUpdate();

    }
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Move(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.currentEnemyState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        Enemydamaged,
        PlayFootstep,
    }
}
