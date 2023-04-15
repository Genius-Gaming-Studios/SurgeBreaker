using UnityEngine;
using System.Collections;

namespace FORGE3D
{
    public class F3DWarpTunnel : MonoBehaviour
    {
        public float RotationFactor = 0.1f;
        public float MinTick = 1f, MaxTick = 2f;
        private float angle;

        // Use this for initialization
        private void Start()
        {
            StartCoroutine(RandomDirection());
        }

        private IEnumerator RandomDirection()
        {
            while (true)
            {
                angle = Random.Range(-360, 360);
                yield return new WaitForSeconds(Random.Range(MinTick, MaxTick));
            }
        }

        // Update is called once per frame
        private void Update()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle),
                Time.deltaTime * RotationFactor);
        }
    }
}