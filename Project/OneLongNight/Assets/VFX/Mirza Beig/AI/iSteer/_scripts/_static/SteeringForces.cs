
// =================================	
// Namespaces.
// =================================

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace AI
    {

        namespace iSteer
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            //[RequireComponent(typeof(TrailRenderer))]

            public static class SteeringForces
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                public class Point
                {
                    public Point(Vector3 position, Vector3 velocity)
                    {
                        this.position = position;
                        this.velocity = velocity;
                    }

                    public Vector3 position;
                    public Vector3 velocity;
                }

                // =================================	
                // Variables.
                // =================================

                // ...

                // =================================	
                // Functions.
                // =================================

                // Returns a force-scaled direction vector.

                public static Vector3 seek(Vector3 position, Vector3 target, float force)
                {
                    return (target - position).normalized * force;
                }

                // Seek with arrival.
                // Passing 0.0f as radius is same as calling the first seek.

                public static Vector3 seek(Vector3 position, Vector3 target, float force, float arrivalRadius)
                {
                    Vector3 targetDirection = target - position;

                    float distanceToTargetSqr = targetDirection.sqrMagnitude;
                    float arrivalRadiusSqr = arrivalRadius * arrivalRadius;

                    targetDirection.Normalize();

                    if (distanceToTargetSqr < arrivalRadiusSqr)
                    {
                        targetDirection *= distanceToTargetSqr / arrivalRadiusSqr;
                    }

                    return targetDirection * force;
                }

                // Seek with arrival if within range.
                // Only return a force-scaled direction vector if target is within range.

                // If scale by distance, greater distance == less driving force within vision radius.

                // If position is within arriv<*ed*> radius, stop seeking entirely.

                public static Vector3 seek(Vector3 position, Vector3 target, float force, float arrivalRadius, float arrivedRadius, float visionRadius = Mathf.Infinity, bool scaleByDistance = true)
                {
                    Vector3 targetDirection = target - position;
                    float distanceToTargetSqr = targetDirection.sqrMagnitude;

                    // First, make sure we're NOT already arrived.
                    // Distance to target must be greater than arrived radius.

                    if (distanceToTargetSqr > arrivedRadius * arrivedRadius)
                    {
                        // Next, check if target is within range.
                        // Distance to target must be within vision radius.

                        float visionRadiusSqr = visionRadius * visionRadius;

                        if (distanceToTargetSqr < visionRadiusSqr)
                        {
                            targetDirection.Normalize();

                            float arrivalRadiusSqr = arrivalRadius * arrivalRadius;

                            // If within arrival radius, slow down.
                            // Closer to target == less steering force.

                            if (distanceToTargetSqr < arrivalRadiusSqr)
                            {
                                targetDirection *= distanceToTargetSqr / arrivalRadiusSqr;
                            }

                            // Greater distance from target == less force.

                            if (scaleByDistance)
                            {
                                targetDirection *= 1.0f - (distanceToTargetSqr / visionRadiusSqr);
                            }

                            return targetDirection * force;
                        }
                    }

                    return Vector3.zero;
                }

                // *****NOT IMPLEMENTED*****
                // TO-DO: Implement this in a way that makes sense...
                //        Might not be possible as a static because it iterates.

                public static Vector3 wander(Vector3 velocity, float circleDistance, float circleRadius)
                {
                    //Vector3 circleCenter = velocity.normalized * circleDistance;
                    //Vector3 displacementForce = Vector3.up * circleRadius;

                    //Vector2 s = displacementForce;

                    return Vector3.zero;
                }

                // Is boidB in boidA's vision range?
                // Adapted from Craig Reynold's C++ OpenSteer implementation.

                // > http://www.red3d.com/cwr/steer/.
                // > http://opensteer.sourceforge.net/.

                static bool inBoidNeighborhood(

                    ParticleSystem.Particle boidA, ParticleSystem.Particle boidB,
                    float minDistance, float maxDistance, float cosMaxAngle)
                {
                    Vector3 offset = boidB.position - boidA.position;
                    float distanceSqr = offset.sqrMagnitude;

                    // In neighborhood if inside minDistance sphere.

                    if (distanceSqr < (minDistance * minDistance))
                    {
                        return true;
                    }
                    else
                    {
                        // Not in neighborhood if outside maxDistance sphere.

                        if (distanceSqr > (maxDistance * maxDistance))
                        {
                            return false;
                        }
                        else
                        {
                            // Otherwise, test angular offset from forward axis.

                            Vector3 unitOffset = offset / Mathf.Sqrt(distanceSqr);
                            float forwardness = Vector3.Dot(boidA.velocity, unitOffset);

                            return forwardness > cosMaxAngle;
                        }
                    }
                }

                // Same as standard flock below, but seperation is not scaled 
                // and reused as opposite of cohesion with weighting multiplied.

                // Everything is inlined where applicable.

                public static Vector3 flockFast(

                    ParticleSystem.Particle boid, float force, float groupRadius,
                    float alignmentWeight, float cohesionWeight, float separationWeight,
                    ParticleSystem.Particle[] boids)
                {

                    Vector3 alignment = Vector3.zero;
                    Vector3 cohesion = Vector3.zero;
                    Vector3 separation = Vector3.zero;

                    // Calculate forces.

                    int boidsInNeighborhood = 0;
                    float groupRadiusSqr = groupRadius * groupRadius;

                    for (int i = 0; i < boids.Length; i++)
                    {
                        if (boid.randomSeed != boids[i].randomSeed)
                        {
                            if ((boids[i].position - boid.position).sqrMagnitude < groupRadiusSqr)
                            {
                                alignment +=
                                    boids[i].velocity;

                                cohesion +=
                                    boids[i].position;

                                boidsInNeighborhood++;
                            }
                        }
                    }

                    if (boidsInNeighborhood != 0)
                    {
                        Vector3 steerForce = Vector3.zero;

                        cohesion = ((cohesion / boidsInNeighborhood) - boid.position).normalized;
                        separation = -cohesion * separationWeight;

                        // Return weighted results.

                        steerForce =

                            ((alignment.normalized * alignmentWeight) +
                            (cohesion * cohesionWeight) +
                            (separation)).normalized *

                            force;

                        return steerForce;
                    }

                    return Vector3.zero;
                }

                // ...

                // Straightforward implementation of flocking.
                // Applies the three steering forces based on boids within groupRadius.

                // Seperation is scaled by distance to center of flock.

                public static Vector3 flock(

                    ParticleSystem.Particle boid, float force, float groupRadius,
                    float alignmentWeight, float cohesionWeight, float separationWeight,
                    ParticleSystem.Particle[] boids, int boidCount, bool weightEntireNeighboorHoodCohesion = false)
                {
                    Vector3 alignment = Vector3.zero;
                    Vector3 cohesion = Vector3.zero;
                    Vector3 separation = Vector3.zero;

                    // Calculate forces.

                    int boidsInNeighborhood = 0;
                    float groupRadiusSqr = groupRadius * groupRadius;

                    for (int i = 0; i < boidCount; i++)
                    {
                        if (boid.randomSeed != boids[i].randomSeed)
                        {
                            float distanceSqr =
                                (boids[i].position - boid.position).sqrMagnitude;

                            if (distanceSqr < groupRadiusSqr)
                            {
                                alignment +=
                                    boids[i].velocity;

                                cohesion +=
                                    boids[i].position;

                                // Scaled separation by distance. 

                                Vector3 offset = boids[i].position - boid.position;

                                distanceSqr = offset.sqrMagnitude;
                                separation += offset / distanceSqr;

                                boidsInNeighborhood++;
                            }
                        }
                    }

                    Vector3 steerForce = Vector3.zero;

                    if (boidsInNeighborhood != 0)
                    {
                        // Alignment: average direction of neighborhood boids.

                        alignment.Normalize();

                        // Cohesion: seek towards center (average position) of neighborhood boids.

                        cohesion = ((cohesion / (weightEntireNeighboorHoodCohesion ? boidCount : boidsInNeighborhood)) - boid.position).normalized;

                        // Separation: opposite of cohesion. Flee (inverse seek) from center of neighborhood boids.

                        separation = -(separation / boidsInNeighborhood).normalized;

                        // Return weighted results.

                        steerForce += alignment * alignmentWeight;
                        steerForce += cohesion * cohesionWeight;
                        steerForce += separation * separationWeight;

                        steerForce.Normalize();

                        steerForce *= force;
                    }

                    return steerForce;
                }

                // Every force parameter can be adjusted.

                public static Vector3 flockPrecise(

                    ParticleSystem.Particle boid, float force, float minRadius,

                    float alignmentRadius, float alignmentAngle, float alignmentWeight,
                    float cohesionRadius, float cohesionAngle, float cohesionWeight,
                    float separationRadius, float separationAngle, float separationWeight,

                    ParticleSystem.Particle[] boids, int boidCount)
                {
                    Vector3 alignment = Vector3.zero;
                    Vector3 cohesion = Vector3.zero;
                    Vector3 separation = Vector3.zero;

                    // Calculate forces.

                    int boidsInAlignmentNeighborhood = 0;
                    int boidsInCohesionNeighborhood = 0;
                    int boidsInSeparationNeighborhood = 0;

                    for (int i = 0; i < boidCount; i++)
                    {
                        if (boid.randomSeed != boids[i].randomSeed)
                        {
                            // Alignment.

                            if (inBoidNeighborhood(boid, boids[i], minRadius, alignmentRadius, Mathf.Cos(alignmentAngle)))
                            {
                                alignment +=
                                    boids[i].velocity.normalized;

                                boidsInAlignmentNeighborhood++;
                            }

                            if (inBoidNeighborhood(boid, boids[i], minRadius, cohesionRadius, Mathf.Cos(cohesionAngle)))
                            {
                                cohesion +=
                                    boids[i].position;

                                boidsInCohesionNeighborhood++;
                            }
                            if (inBoidNeighborhood(boid, boids[i], minRadius, separationRadius, Mathf.Cos(separationAngle)))
                            {
                                // Scaled by distance.

                                Vector3 offset = boids[i].position - boid.position;
                                float distanceSqr = offset.sqrMagnitude;

                                separation += (offset / distanceSqr);

                                boidsInSeparationNeighborhood++;
                            }
                        }
                    }

                    // Alignment: average direction of neighborhood boids.

                    if (boidsInAlignmentNeighborhood != 0)
                    {
                        alignment.Normalize();
                    }

                    // Cohesion: seek towards center (average position) of neighborhood boids.

                    if (boidsInCohesionNeighborhood != 0)
                    {
                        cohesion = ((cohesion / boidsInCohesionNeighborhood) - boid.position).normalized;
                    }

                    // Separation: opposite of cohesion. Flee (inverse seek) from center of neighborhood boids.

                    if (boidsInSeparationNeighborhood != 0)
                    {
                        separation = -(separation / boidsInSeparationNeighborhood).normalized;
                    }

                    // Return weighted results.

                    Vector3 steerForce = Vector3.zero;

                    if (boidsInAlignmentNeighborhood + boidsInCohesionNeighborhood + boidsInSeparationNeighborhood != 0)
                    {
                        steerForce += alignment * alignmentWeight;
                        steerForce += cohesion * cohesionWeight;
                        steerForce += separation * separationWeight;

                        steerForce.Normalize();

                        steerForce *= force;
                    }

                    return steerForce;
                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================	
// --END-- //
// =================================
