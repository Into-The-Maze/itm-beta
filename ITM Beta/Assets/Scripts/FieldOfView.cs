using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class FieldOfView : MonoBehaviour
{
    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    [SerializeField] private LayerMask mask;
    static float fov = 90f;
    static int rayCount = 50;
    static float angle;
    static float angleIncrease = fov / rayCount;
    static float viewDistance = 50f;


    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, 0));
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int triangleIndex = 0;
        int vertexIndex = 1;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, mask);
            if (raycastHit2D.collider == null) {
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else {
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;
            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }
}
