using UnityEngine;

namespace PacMan.AI
{
    /// <summary>
    /// Pinky - The pink ghost that ambushes the player
    /// Behavior: Targets 4 spaces ahead of the player's current direction
    /// </summary>
    public class Pinky : Ghost
    {
        [Header("Pinky Specific")]
        [SerializeField] private float ambushDistance = 4f;
        
        protected override void HandleChaseBehavior()
        {
            // Pinky targets 4 spaces ahead of the player's current direction
            if (playerTransform != null)
            {
                // Get player's forward direction (simplified for 2D movement)
                Vector3 playerDirection = playerTransform.forward;
                Vector3 ambushTarget = playerTransform.position + (playerDirection * ambushDistance);
                
                Vector3 directionToAmbush = (ambushTarget - transform.position).normalized;
                _moveDirection = directionToAmbush;
            }
        }
        
        protected override void HandleScatterBehavior()
        {
            // Pinky moves to the top-left corner in scatter mode
            Vector3 scatterTarget = new Vector3(-100f, transform.position.y, 100f); // Top-left corner
            Vector3 directionToScatter = (scatterTarget - transform.position).normalized;
            _moveDirection = directionToScatter;
        }
        
        protected override void HandleFrightenedBehavior()
        {
            // Pinky runs away from the player when frightened
            base.HandleFrightenedBehavior();
        }
        
        protected override void HandleRecoveringBehavior()
        {
            // Pinky transitions back to normal behavior
            base.HandleRecoveringBehavior();
        }
        
        protected override void HandleReturningHomeBehavior()
        {
            // Pinky returns to the ghost house
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