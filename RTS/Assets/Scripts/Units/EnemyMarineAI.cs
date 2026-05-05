using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(FactionComponent))]
public class EnemyMarineAI : MonoBehaviour
{
    [Header("Detection & Attack Settings")]
    public float detectionRadius = 10f;        // 찾을 수 있는 범위
    public float attackRange = 2f;         // 실제 공격 시 반경
    public float attackCooldown = 1.2f;       // 공격 간 최소 간격 (초)
    public int attackDamage = 10;         // 공격 데미지

    private NavMeshAgent agent;
    private Transform currentTarget;
    private FactionComponent meFaction;
    private float lastAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        meFaction = GetComponent<FactionComponent>();
        lastAttackTime = -attackCooldown; // 시작 시 즉시 공격 가능
    }

    void Update()
    {
        if (currentTarget == null)
        {
            SearchForTarget();
        }
        else
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist <= attackRange)
            {
                // 사거리 안에 들어오면 멈추고 공격
                agent.isStopped = true;
                TryAttack();
            }
            else
            {
                // 아니면 계속 추격
                agent.isStopped = false;
                agent.destination = currentTarget.position;
            }

            // 사정거리를 벗어나면 타겟 해제하고 재탐색
            if (dist > detectionRadius * 1.2f)
                currentTarget = null;
        }
    }

    void SearchForTarget()
    {
        // Physics.OverlapSphere로 범위 내 콜라이더 검색
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            var fc = hit.GetComponent<FactionComponent>();
            if (fc != null && fc.faction != meFaction.faction)
            {
                currentTarget = hit.transform;
                break;
            }
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        // 여기에 애니메이션 트리거 넣어도 좋습니다
        // anim.SetTrigger("Attack");

        // 데미지 처리 (HealthComponent 예시)
        var hp = currentTarget.GetComponent<Health>();
        if (hp != null)
            hp.TakeDamage(attackDamage);
    }

    // 에디터에서 시야/사거리 시각화 용도
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
