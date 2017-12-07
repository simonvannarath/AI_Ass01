using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using GGL;

namespace ZombieSlalom
{
    public class PathFollowing : SteeringBehaviour
    {
        public Transform target;                                                    // Get to the tarjey!
        public float nodeRadius = 0.1f;                                             // How big each node is for the agent to seek to
        public float targetRadius = 3f;                                             // Separate from the nodes that the agent follows
        public bool isAtTarget = false;                                             // Has the agent reached the target node?
        public int currentNode = 0;                                                 // Keep track of the current node the agent is following

        private NavMeshAgent nav;                                                   // Reference to the agent component
        private NavMeshPath path;                                                   // Stores the calculated path in this variable

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
        }

        // Draw out the path calculated by the agent
        void Update()
        {
            // Is the path calculated?
            if(path != null)
            {
                Vector3[] corners = path.corners;   // Corners refer to the nodes that Unity generated through A*
                // Has generated corners for the path?
                if (corners.Length > 0)
                {
                    Vector3 targetPos = corners[corners.Length - 1]; // Store the last corner into target pos
                    // Draw the target
                    GizmosGL.color = new Color(1, 0, 0, 0.3f);
                    GizmosGL.AddSphere(targetPos, targetRadius * 2f);
                    float distance = Vector3.Distance(transform.position, targetPos); // Calculate distance from agent to target
                    // Is the distance greater than target radius?
                    if (distance >= targetRadius)
                    {
                        GizmosGL.color = Color.cyan;
                        for (int i = 0; i < corners.Length - 1; i++)
                        {
                            Vector3 nodeA = corners[i];
                            Vector3 nodeB = corners[i + 1];
                            GizmosGL.AddLine(nodeA, nodeB, 0.1f, 0.1f);
                            GizmosGL.AddSphere(nodeB, 1f);
                            Gizmos.color = Color.red;
                        }
                    }
                }
            }
        }

        // Modified seek function
        Vector3 Seek(Vector3 target)
        {
            Vector3 force = Vector3.zero;
            float distanceToTarget = Vector3.Distance(target, transform.position); // Get the direction to target
            
            // Calculate distance - Ternary Operator 
            // return value = <condition> ? <statement a> : <statement b>
            float radius = isAtTarget ? targetRadius : nodeRadius;
            
            // Is the magnitude greater than distance?
            if (distanceToTarget > radius)
            {
                Vector3 direction = (target - transform.position).normalized * weighting;      // Apply weighting to force
                force = direction - owner.velocity;                                 // Apply desired force to force (removing current owner's velocity)
            }

            return force;                                                           // Return force
        }

        public override Vector3 GetForce()
        {
            Vector3 force = Vector3.zero;

            // Is there not a target?
            if (!target)
            {
                return force;
            }

            // Calculate path using the nav agent
            if (nav.CalculatePath(target.position, path))
            {
                // Is the path finished calculating?
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    Vector3[] corners = path.corners;
                    // Are there any corners in the path?
                    if (corners.Length > 0)
                    {
                        int lastIndex = corners.Length - 1;
                        // Is currentNode at the end of the list?
                        if(currentNode >= corners.Length)
                        {
                            currentNode = lastIndex;
                        }
                        // Getthe current corner position
                        Vector3 currentPos = corners[currentNode];
                        float distance = Vector3.Distance(transform.position, currentPos);  // Get the distance to current pos
                        // Is the distance with nodeRadius
                        if (distance <= nodeRadius)
                        {
                            currentNode++; // Move to the next node
                        }
                        // Is the agent at the target?
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);
                        isAtTarget = distanceToTarget <= targetRadius;
                        force = Seek(currentPos); // Seek towards current node's position
                    }
                }
            }

            return force;
        }

        #region NOTES
        int SumOf(params int[] values)
        {
            int result = 0;
            foreach (var n in values)
            {
                result += n;
            }
            return result;
        }
        #endregion
    }
}