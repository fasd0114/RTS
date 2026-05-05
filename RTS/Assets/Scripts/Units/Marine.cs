using UnityEngine;
using UnityEngine.AI;

public class Marine : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;

    enum State { Idle, Moving, Attacking }
    State state = State.Idle;

    NavMeshAgent agent;
    GameObject attackTarget;
    float cooldownTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Moving:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    state = State.Idle;
                break;

            case State.Attacking:
                if (attackTarget == null)
                {
                    state = State.Idle;
                    break;
                }

                float dist = Vector3.Distance(transform.position, attackTarget.transform.position);

                if (dist > attackRange)
                {
                    agent.isStopped = false;
                    agent.SetDestination(attackTarget.transform.position);
                }
                else
                {
                    agent.isStopped = true;
                    if (cooldownTimer <= 0f)
                    {
                        var hp = attackTarget.GetComponent<Health>();
                        if (hp != null) hp.TakeDamage(damage);
                        cooldownTimer = attackCooldown;
                    }
                }
                break;
        }
    }
    public void MoveTo(Vector3 point)
    {
        attackTarget = null;
        agent.isStopped = false;
        agent.SetDestination(point);
        state = State.Moving;
    }

    public void Attack(GameObject tgt)
    {
        var fac = tgt.GetComponent<FactionComponent>();
        if (fac != null && fac.faction == Faction.Enemy)
        {
            attackTarget = tgt;
            agent.isStopped = false;
            agent.SetDestination(tgt.transform.position);
            state = State.Attacking;
        }
    }
}
