using UnityEngine;
using System.Collections;

namespace FORGE3D
{
    public class F3DWarpJump : MonoBehaviour
    {
        public ParticleSystem WarpSpark;
        public Transform ShipPos;
        public float ShipJumpSpeed;
        public Vector3 ShipJumpStartPoint;
        public Vector3 ShipJumpEndPoint;
        public bool SendOnSpawned;
        public bool DebugLoop; 

        private bool isWarping;

        // Use this for initialization
        private void Start()
        {
            if (SendOnSpawned)
                BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);

            if (DebugLoop)
                F3DTime.time.AddTimer(4, Reset);
        }

        private void Reset()
        {
            BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);

            var psys = GetComponentsInChildren<ParticleSystem>();
            foreach (var p in psys)
            {
                p.Stop(true);
                p.Play(true);
            }
        }

        public void OnSpawned()
        {
            isWarping = false;
            WarpSpark.transform.localPosition = ShipJumpStartPoint;
            ShipPos.position = WarpSpark.transform.position;
            F3DTime.time.AddTimer(3, 1, OnWarp);
        }

        private void OnWarp()
        {
            isWarping = true;
        }

        private void ShiftShipPosition()
        {
            WarpSpark.transform.localPosition = Vector3.Lerp(WarpSpark.transform.localPosition, ShipJumpEndPoint,
                Time.deltaTime * ShipJumpSpeed);
            ShipPos.position = WarpSpark.transform.position;
        }

        private void Update()
        {
            if (isWarping)
                ShiftShipPosition();
        }
    }
}