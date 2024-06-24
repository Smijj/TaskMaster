using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class Explodable : MonoBehaviour
{
    public System.Action<List<GameObject>> OnFragmentsGenerated;

    public bool allowRuntimeFragmentation = false;
    [SerializeField] private bool m_DrawGizmos = false;
    public int extraPoints = 0;
    public int subshatterSteps = 0;

    [SerializeField] public string fragmentLayer = "Default";
    [SerializeField] public string sortingLayerName = "Default";
    [SerializeField] public int orderInLayer = 0;

    public enum ShatterType
    {
        Triangle,
        Voronoi
    };
    public ShatterType shatterType;
    [SerializeField] private Transform m_FragmentsParent;
    public List<GameObject> fragments = new List<GameObject>();
    private List<List<Vector2>> polygons = new List<List<Vector2>>();


    private void Awake() {
        if (!m_FragmentsParent) m_FragmentsParent = transform;
    }

    /// <summary>
    /// Creates fragments if necessary and destroys original gameobject
    /// </summary>
    [ContextMenu("Explode")]
    public void explode(float explodeForce = 5f)
    {
        //if fragments were not created before runtime then create them now
        if (fragments.Count == 0 && allowRuntimeFragmentation)
        {
            //generateFragments();
        }
        //otherwise unparent and activate them
        else
        {
            foreach (GameObject frag in fragments)
            {
                frag.SetActive(true);

                // Explode in random direction
                Vector2 randomForce = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * explodeForce;
                frag.GetComponent<Rigidbody2D>().AddForce(randomForce, ForceMode2D.Impulse);
            }
        }
        //if fragments exist Hide the original
        if (fragments.Count > 0)
        {
            //Destroy(gameObject);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Creates fragments and then disables them
    /// </summary>
#if UNITY_EDITOR
    public void fragmentInEditor()
    {
        string assetPath = "Assets/DSmyth/Prefabs/VideoEnemy.prefab";
        var go = PrefabUtility.LoadPrefabContents(assetPath);
        
        if (fragments.Count > 0) {
            Object[] prefabAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            Debug.Log(prefabAssets.Length);
            foreach (var asset in prefabAssets) {
                if (asset.GetType() == typeof(Mesh)) {
                    DestroyImmediate(asset, true);
                }
            }
            deleteFragments();
        }
        generateFragments(go);
        if (m_DrawGizmos) setPolygonsForDrawing();

        foreach (GameObject frag in fragments) {
            frag.transform.parent = m_FragmentsParent;
            frag.SetActive(false);

            EditorUtility.SetDirty(frag);
        }

        EditorUtility.SetDirty(go);
        EditorSceneManager.MarkSceneDirty(go.scene);

        PrefabUtility.SaveAsPrefabAsset(go, assetPath);
        //PrefabUtility.UnloadPrefabContents(go);
    }
#endif
    public void deleteFragments()
    {
        foreach (GameObject frag in fragments)
        {
            if (Application.isEditor)
            {
                // Need to get the prefab and destroy the mesh on each fragment before destroying the fragment here
                Mesh fragMesh = frag.GetComponent<MeshFilter>().sharedMesh;
                if (fragMesh) DestroyImmediate(fragMesh, true);
                DestroyImmediate(frag);
            }
            else
            {
                Destroy(frag);
            }
        }
        fragments.Clear();
        polygons.Clear();
    }
    /// <summary>
    /// Turns Gameobject into multiple fragments
    /// </summary>
    private void generateFragments(GameObject prefabGameObject)
    {
        fragments = new List<GameObject>();
        switch (shatterType)
        {
            case ShatterType.Triangle:
                fragments = SpriteExploder.GenerateTriangularPieces(gameObject, extraPoints, subshatterSteps);
                break;
            case ShatterType.Voronoi:
                fragments = SpriteExploder.GenerateVoronoiPieces(prefabGameObject, extraPoints, subshatterSteps);
                break;
            default:
                Debug.Log("invalid choice");
                break;
        }
        //sets additional aspects of the fragments
        foreach (GameObject p in fragments)
        {
            if (p != null)
            {
                p.layer = LayerMask.NameToLayer(fragmentLayer);
                p.GetComponent<Renderer>().sortingLayerName = sortingLayerName;
                p.GetComponent<Renderer>().sortingOrder = orderInLayer;
            }
        }

        foreach (ExplodableAddon addon in GetComponents<ExplodableAddon>())
        {
            if (addon.enabled)
            {
                addon.OnFragmentsGenerated(fragments);
            }
        }
    }
    private void setPolygonsForDrawing()
    {
        polygons.Clear();
        List<Vector2> polygon;

        foreach (GameObject frag in fragments)
        {
            polygon = new List<Vector2>();
            foreach (Vector2 point in frag.GetComponent<PolygonCollider2D>().points)
            {
                Vector2 offset = rotateAroundPivot((Vector2)frag.transform.position, (Vector2)transform.position, Quaternion.Inverse(transform.rotation)) - (Vector2)transform.position;
                offset.x /= transform.localScale.x;
                offset.y /= transform.localScale.y;
                polygon.Add(point + offset);
            }
            polygons.Add(polygon);
        }
    }
    private Vector2 rotateAroundPivot(Vector2 point, Vector2 pivot, Quaternion angle)
    {
        Vector2 dir = point - pivot;
        dir = angle * dir;
        point = dir + pivot;
        return point;
    }

    void OnDrawGizmos()
    {
        if (Application.isEditor && m_DrawGizmos)
        {
            if (polygons.Count == 0 && fragments.Count != 0)
            {
                setPolygonsForDrawing();
            }

            Gizmos.color = Color.blue;
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector2 offset = (Vector2)transform.position * 0;
            foreach (List<Vector2> polygon in polygons)
            {
                for (int i = 0; i < polygon.Count; i++)
                {
                    if (i + 1 == polygon.Count)
                    {
                        Gizmos.DrawLine(polygon[i] + offset, polygon[0] + offset);
                    }
                    else
                    {
                        Gizmos.DrawLine(polygon[i] + offset, polygon[i + 1] + offset);
                    }
                }
            }
        }
    }
}
