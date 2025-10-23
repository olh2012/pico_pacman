using UnityEngine;

namespace PacMan.AI
{
    /// <summary>
    /// Clyde - The orange ghost with fearful behavior
    /// Behavior: Chases player when far, flees when close
    /// </summary>
    public class Clyde : Ghost
    {
        [Header("Clyde Specific")]
        [SerializeField] private float fleeDistance = 8f;
        [SerializeField] private float scatterDistance = 4f;
        
        protected override void HandleChaseBehavior()
        {
            // Clyde's behavior:
            // - If player is more than 8 tiles away, chase like Blinky
            // - If player is less than 8 tiles away, run to bottom-left corner
            
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                
                if (distanceToPlayer > fleeDistance)
                {
                    // Chase the player directly (like Blinky)
                    Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    _moveDirection = directionToPlayer;
                }
                else
                {
                    // Run to bottom-left corner (fearful behavior)
                    Vector3 fleeTarget = new Vector3(-100f, transform.position.y, -100f); // Bottom-left corner
                    Vector3 directionToFlee = (fleeTarget - transform.position).normalized;
                    _moveDirection = directionToFlee;
                }
            }
        }
        
        protected override void HandleScatterBehavior()
        {
            // Clyde moves to the bottom-left corner in scatter mode
            Vector3 scatterTarget = new Vector3(-100f, transform.position.y, -100f); // Bottom-left corner
            Vector3 directionToScatter = (scatterTarget - transform.position).normalized;
            _moveDirection = directionToScatter;
        }
        
        protected override void HandleFrightenedBehavior()
        {
            // Clyde runs away from the player when frightened
            base.HandleFrightenedBehavior();
        }
        
        protected override void HandleRecoveringBehavior()
        {
            // Clyde transitions back to normal behavior
            base.HandleRecoveringBehavior();
        }
        
        protected override void HandleReturningHomeBehavior()
        {
            // Clyde returns to the ghost house
            base.HandleReturningHomeBehavior();
        }
        
        public override void SetFrightened()
        {
            base.SetFrightened();
        }
        
        public override void SetRecovering()
        {
            base.SetRecovering();
        }
        
        public override void SetReturningHome()
        {
            base.SetReturningHome();
        }
        
        public override void ResetGhost()
        {
            base.ResetGhost();
        }
    }
}