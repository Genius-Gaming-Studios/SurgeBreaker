using System.Collections;
using System.Collections.Generic;
using FORGE3D;
using UnityEngine;
using UnityEngine.WSA;

public class ClusterEmitter : MonoBehaviour
{
    public List<Transform> Clusters = new List<Transform>();

    private List<Transform> _clusters = new List<Transform>();
    private List<float> _clustersLifetime = new List<float>();

    public int MaxClusters;


    public float ClusterAcceleration;

    public float SpawnRate;
    public float LifeTime;

    public Vector2 Size;
    public Vector2 Shape;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < MaxClusters; i++)
        {
            var clusterId = Random.Range(0, Clusters.Count);


            var cluster = Instantiate(Clusters[clusterId], transform.position, Random.rotation);
            cluster.gameObject.SetActive(false);

            cluster.localScale = Vector3.one * Random.Range(Size.x, Size.y);
            
            _clusters.Add(cluster);
            _clustersLifetime.Add(0.0f);
        }

        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            for (int i = 0; i < _clusters.Count; i++)
            {
                if (_clusters[i].gameObject.activeSelf == false)
                {
                    var randPos = Vector3.ProjectOnPlane(Random.insideUnitSphere, transform.forward) * Random.Range(Shape.x, Shape.y);
                    _clusters[i].position = transform.position + randPos; 
                    _clusters[i].rotation = Random.rotation;
                    _clusters[i].localScale = Vector3.one * Random.Range(Size.x, Size.y);
                    _clusters[i].gameObject.SetActive(true);

                    _clustersLifetime[i] = 0.0f;
                    
                    break;
                }
            }

            yield return new WaitForSeconds(SpawnRate);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _clusters.Count; i++)
        {
            if (!_clusters[i].gameObject.activeSelf) continue;

            _clusters[i].position += transform.forward * ClusterAcceleration * Time.deltaTime;

            if (_clustersLifetime[i] >= LifeTime)
                _clusters[i].gameObject.SetActive(false);
            else
                _clustersLifetime[i] += Time.deltaTime;
        }
    }
}