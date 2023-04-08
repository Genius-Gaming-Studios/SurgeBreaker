using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FORGE3D
{
    public class F3DShotgun : MonoBehaviour
    {
        private readonly List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
        private ParticleSystem _ps;

        private void Start()
        {
            _ps = GetComponent<ParticleSystem>();
        }

        // On particle collision
        private void OnParticleCollision(GameObject other)
        {
            var numCollisionEvents = _ps.GetCollisionEvents(other, _collisionEvents);

            // Play collision sound and apply force to the rigidbody was hit
            for (var j = 0; j < numCollisionEvents; j++)
            {
                F3DAudioController.instance.ShotGunHit(_collisionEvents[j].intersection);

                var rb = other.GetComponent<Rigidbody>();
                if (!rb) continue;
                var pos = _collisionEvents[j].intersection;
                var force = _collisionEvents[j].velocity.normalized * 50f;

                rb.AddForceAtPosition(force, pos);
            }
        }
    }
}