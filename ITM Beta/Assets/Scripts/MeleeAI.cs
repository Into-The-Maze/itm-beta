using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MeleeAI : MonoBehaviour
{
    private (Vector2 direction, float weight, bool blocked)[] moveDirections = new (Vector2, float, bool)[32];
    private Rigidbody2D rb;
    private Vector2 target = new Vector2(0f, 0f);
    private bool attacking = false;
    [SerializeField] private float attackCooldown = 2f;
    private bool favourRight = false;
    private bool lineOfSight = false;
    private bool chasing = false;
    private Vector2 vectorToPlayer;
    private float distanceToPlayer;
    private bool playerDetected;
    private GameObject wanderTarget;
    [SerializeField] private float aggroRadius = 10f;
    [SerializeField] private float range = 4f;
    [SerializeField] private float bodyWidth = 1.5f;
    [SerializeField] private float RotationSpeed = 2f;

    #region IMPORTANT
#if false
    /// <summary>
    /// fully necessary for code to function. SYSTEM32 WILL BE DELETED IF THE CODE RUNS WITHOUT THIS
    /// </summary>
    #region saved for later
#if false
///[SerializeField][HideInInspector] private unsafe async required virtual sealed abstract static const (UnityEngine.UnityAPICompatibilityVersionAttribute[,][,,][], CustomRenderTextureInitializationSource, UnityEngine.RuntimeInitializeOnLoadMethodAttribute[][], (private unsafe async required virtual sealed abstract static const int, private unsafe async required virtual sealed abstract static const double, private unsafe async required virtual sealed abstract static const float, private unsafe async required virtual sealed abstract static const long, private unsafe async required virtual sealed abstract static const short, private unsafe async required virtual sealed abstract static const uint, private unsafe async required virtual sealed abstract static const ushort, private unsafe async required virtual sealed abstract static const ulong, private unsafe async required virtual sealed abstract static const sbyte, private unsafe async required virtual sealed abstract static const byte), UnityEngine.CustomRenderTextureInitializationSource)[,,][,] balls = new (UnityEngine.UnityAPICompatibilityVersionAttribute[,][,,][], CustomRenderTextureInitializationSource, UnityEngine.RuntimeInitializeOnLoadMethodAttribute[][], (private unsafe async required virtual sealed abstract static const int, private unsafe async required virtual sealed abstract static const double, private unsafe async required virtual sealed abstract static const float, private unsafe async required virtual sealed abstract static const long, private unsafe async required virtual sealed abstract static const short, private unsafe async required virtual sealed abstract static const uint, private unsafe async required virtual sealed abstract static const ushort, private unsafe async required virtual sealed abstract static const ulong, private unsafe async required virtual sealed abstract static const sbyte, private unsafe async required virtual sealed abstract static const byte), UnityEngine.CustomRenderTextureInitializationSource)[16, 420][69];
#endif
    #endregion
#endif
    #endregion

    void Awake()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = aggroRadius;
        rb = gameObject.GetComponent<Rigidbody2D>();
        InitialiseMoveDirections();
    }

    private void Start() {
        target = transform.position;
        wanderTarget = makeNewTarget();
        gameObject.GetComponent<AstarSmoothFollow2>().target = wanderTarget.transform;
    }
    void Update() 
    {
        CalculateWeights();
        DeWeightBlocked();
        NormaliseWeights();
        ExtendBlocks();
        DeWeightBlocked();
        NormaliseWeights();
        DrawRays();
        // check states and movement and set correct animation
    }
    private void FixedUpdate() {
        if (Vector3.Distance(gameObject.transform.position, new Vector3(target.x, target.y, 0)) < 1.5f) {
            chasing = false;
        }
        else {
            chasing = true;
        }
        if (chasing) {
            gameObject.GetComponent<AstarSmoothFollow2>().enabled = false;
            int index = HighestWeightIndex();
            float angle = -90f + (Mathf.Atan2(moveDirections[index].direction.y, moveDirections[index].direction.x) * Mathf.Rad2Deg);
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime * 25f);
            if (Quaternion.Dot(transform.rotation, targetRotation) > 0.9f || Quaternion.Dot(transform.rotation, targetRotation) < -0.9f) {
                StartCoroutine(moveToPlayer());
            }
        }
        else {  
            StopCoroutine(moveToPlayer());
            //StartCoroutine(wait());
            if (Vector3.Distance(gameObject.transform.position, wanderTarget.transform.position) < 1f) {
                wanderTarget = makeNewTarget(wanderTarget);
                gameObject.GetComponent<AstarSmoothFollow2>().target = wanderTarget.transform;
            }
            gameObject.GetComponent<AstarSmoothFollow2>().enabled = true;
        }   
    }

    //IEnumerator wait() {
    //    yield return new WaitForSeconds(2);
    //    target = InstantiateMaze.RandomFloorPoint();
    //    StopCoroutine(wait());
    //}

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, collision.transform.position - gameObject.transform.position, aggroRadius + 1, LayerMask.GetMask(new string[2] { "Player", "Wall" }));
            if (hit.collider != null && hit.collider.gameObject != null) {
                if (hit.collider.gameObject.CompareTag("Player")) {
                    target = collision.gameObject.transform.position;
                    vectorToPlayer = collision.gameObject.transform.position - gameObject.transform.position;
                    distanceToPlayer = vectorToPlayer.magnitude;
                    if (distanceToPlayer < 3 && !attacking) {
                        StartCoroutine(damage());
                    }
                    playerDetected = true;
                }
                else {
                    playerDetected = false;
                   
                }
            }
        }
    }

    IEnumerator damage() {
        attacking = true;
        yield return new WaitForSeconds(2f);
        if (distanceToPlayer < 3) {
            // make it start an attack animation
            HealthController.h.handleDamageF(40f); //need to make this damage number dynamic per enemy. maybe actually might need the enemyStats scriptableObject?
            HealthController.h.handleLightBleed(20f); //again, this %chance to bleed ought to be specific to the enemy, not the AI program.
            HealthController.h.handleHeavyBleed(5f); //^^^^
        }
        
        StopCoroutine(damage());
        attacking = false;
    }
    IEnumerator moveToPlayer() {
        rb.AddForce(GetHighestWeightedVector());
        yield return null;
    }
    private GameObject makeNewTarget() {
        GameObject target = new GameObject();
        target.transform.position = InstantiateMaze.RandomFloorPoint();
        return target;
    }
    private GameObject makeNewTarget(GameObject oldTarget) {
        GameObject target = new GameObject();
        target.transform.position = InstantiateMaze.RandomFloorPoint();
        Destroy(oldTarget);
        return target;
    }
    private void InitialiseMoveDirections() {
        int count = 0;
        for (float angle = 0f; angle < 360f; angle += 11.25f) {
            moveDirections[count].direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
            moveDirections[count].weight = 1f;
            moveDirections[count].blocked = false;
            count++;
        }
    }
    private void DrawRays() {
        int highestIndex = HighestWeightIndex();
        for (int i = 0; i < moveDirections.Length; i++) {
            if (i == highestIndex) {
                Debug.DrawRay((transform.position + (new Vector3(moveDirections[i].direction.x, moveDirections[i].direction.y, 0) * bodyWidth)), (moveDirections[i].direction * moveDirections[i].weight), Color.blue, Time.deltaTime);
            }
            else if (moveDirections[i].blocked) {
                Debug.DrawRay((transform.position + (new Vector3(moveDirections[i].direction.x, moveDirections[i].direction.y, 0) * bodyWidth)), (moveDirections[i].direction * moveDirections[i].weight), Color.red, Time.deltaTime);
            }
            else {
                Debug.DrawRay((transform.position + (new Vector3(moveDirections[i].direction.x, moveDirections[i].direction.y, 0) * bodyWidth)), (moveDirections[i].direction * moveDirections[i].weight), Color.green, Time.deltaTime);
            }
        }
        //Debug.Log($"chasing: {chasing}, lineOfSight: {lineOfSight}");
    }
    private Vector2 VectorToTarget() {
        Vector2 vectorToTarget;
        vectorToTarget.y = target.y - gameObject.transform.position.y;
        vectorToTarget.x = target.x - gameObject.transform.position.x;
        return vectorToTarget;
    }
    private void CalculateWeights() {
        for (int i = 0; i < moveDirections.Length; i++) {
            RaycastHit2D hit = Physics2D.Raycast((transform.position + (new Vector3(moveDirections[i].direction.x, moveDirections[i].direction.y, 0) * bodyWidth)), moveDirections[i].direction, 6f, LayerMask.GetMask("Wall"));
            if (hit.collider != null && hit.collider.gameObject != null) {
                moveDirections[i].weight += hit.distance / 10f;
                if (hit.distance < 2f) {
                    moveDirections[i].blocked = true;
                }
                else {
                    moveDirections[i].blocked = false;
                }
            }
            else {
                moveDirections[i].weight += 1f;
                moveDirections[i].blocked = false;
            }
            float angleWeight = Mathf.Abs(Vector2.Angle(moveDirections[i].direction, VectorToTarget()) / 360);
            moveDirections[i].weight *= Mathf.Pow(1 - angleWeight, 2);
        }
    }
    private void ExtendBlocks() {
        (Vector2 direction, float weight, bool blocked)[] newMoveDirections = new (Vector2, float, bool)[moveDirections.Length];
        for (int i = 0; i < moveDirections.Length; i++) {
            newMoveDirections[i] = moveDirections[i];
        }
        for (int i = 0; i < moveDirections.Length; i++) {
            if (moveDirections[i].blocked) {
                if (i == 31) {
                    newMoveDirections[i].blocked = true; newMoveDirections[0].blocked = true; newMoveDirections[i - 1].blocked = true; newMoveDirections[1].blocked = true; newMoveDirections[i - 2].blocked = true;
                }
                else if (i == 30) {
                    newMoveDirections[i].blocked = true; newMoveDirections[i + 1].blocked = true; newMoveDirections[i - 1].blocked = true; newMoveDirections[0].blocked = true; newMoveDirections[i - 2].blocked = true;
                }
                else if (i == 0) {
                    newMoveDirections[i].blocked = true; newMoveDirections[i + 1].blocked = true; newMoveDirections[31].blocked = true; newMoveDirections[i + 2].blocked = true; newMoveDirections[30].blocked = true;
                }
                else if (i == 1) {
                    newMoveDirections[i].blocked = true; newMoveDirections[i + 1].blocked = true; newMoveDirections[i - 1].blocked = true; newMoveDirections[i + 2].blocked = true; newMoveDirections[31].blocked = true;
                }
                else {
                    newMoveDirections[i].blocked = true; newMoveDirections[i + 1].blocked = true; newMoveDirections[i - 1].blocked = true; newMoveDirections[i + 2].blocked = true; newMoveDirections[i - 2].blocked = true;
                }
            }
        }
        moveDirections = newMoveDirections;
    }
    private void DeWeightBlocked() {
        for (int i = 0; i < moveDirections.Length; i++) {
            if (moveDirections[i].blocked) {
                moveDirections[i].weight /= 3;
            }
        }
    }
    private void NormaliseWeights() {
        (Vector2 direction, float weight, bool blocked)[] normalisedMoveDirections = new (Vector2, float, bool)[moveDirections.Length];
        for (int i = 0; i < moveDirections.Length; i++) {
            normalisedMoveDirections[i].direction = moveDirections[i].direction;
            normalisedMoveDirections[i].blocked = moveDirections[i].blocked;
            if (i == 31) {
                normalisedMoveDirections[i].weight = (moveDirections[i - 1].weight + (2 * moveDirections[i].weight) + moveDirections[0].weight) / 4f;
            }
            else if (i == 0) {
                normalisedMoveDirections[i].weight = (moveDirections[31].weight + (2 * moveDirections[i].weight) + moveDirections[i + 1].weight) / 4f;
            }
            else {
                normalisedMoveDirections[i].weight = (moveDirections[i - 1].weight + (2 * moveDirections[i].weight) + moveDirections[i + 1].weight) / 4f;
            }
        }
        moveDirections = normalisedMoveDirections;
    }
    private int HighestWeightIndex() {
        float highestValue = 0f;
        for (int i = 0; i < moveDirections.Length; i++) {
            if (moveDirections[i].weight > highestValue) {
                highestValue = moveDirections[i].weight;
            }
        }
        for (int i = 0; i < moveDirections.Length; i++) {
            if (moveDirections[i].weight == highestValue) {
                return i;
            }
        }
        return 0;
    }

    private Vector2 GetHighestWeightedVector() {
        float highestValue = 0f;
        for (int i = 0; i < moveDirections.Length; i++) {
            if (moveDirections[i].weight > highestValue) {
                highestValue = moveDirections[i].weight;
            }
        }
        for (int i = 0; i < moveDirections.Length; i++) {
            if (moveDirections[i].weight == highestValue) {
                return moveDirections[i].direction;
            }
        }
        return Vector2.zero;
    }
}
