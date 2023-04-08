using UnityEngine;

namespace FORGE3D
{
    public static class F3DPredictTrajectory
    {
        public static Vector3 Predict(Vector3 sPos, Vector3 tPos, Vector3 tLastPos, float pSpeed)
        {
            // Target velocity
            var tVel = (tPos - tLastPos) / Time.deltaTime;

            // Time to reach the target
            var flyTime = GetProjFlightTime(tPos - sPos, tVel, pSpeed);

            if (flyTime > 0)
                return tPos + flyTime * tVel;
            return tPos;
        }

        private static float GetProjFlightTime(Vector3 dist, Vector3 tVel, float pSpeed)
        {
            var a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
            var b = 2.0f * Vector3.Dot(tVel, dist);
            var c = Vector3.Dot(dist, dist);

            var det = b * b - 4 * a * c;

            if (det > 0)
                return 2 * c / (Mathf.Sqrt(det) - b);
            return -1;
        }
    }
}