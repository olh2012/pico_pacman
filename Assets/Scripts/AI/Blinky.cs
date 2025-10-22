using UnityEngine;

namespace PacMan.AI
{
    /// <summary>
    /// Blinky - The red ghost that directly chases the player
    /// Behavior: Aggressive and persistent, always targeting the player's position
    /// </summary>
    public class Blinky : Ghost
    {
        [Header("Blinky Specific")]
        [SerializeField] private float targetingDistance = 5f;
        
        protected override void HandleChaseBehavior()
        {
            // Blinky directly targets the player's position
            if (playerTransform != null)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                _moveDirection = directionToPlayer;
            }
        }
        
        protected override void HandleScatterBehavior()
        {
            // Blinky moves to the top-right corner in scatter mode
            // In a full implementation, this would target a specific corner of the maze
            Vector3 scatterTarget = new Vector3(100f, transform.position.y, 100f); // Top-right corner
            Vector3 directionToScatter = (scatterTarget - transform.position).normalized;
            _moveDirection = directionToScatter;
        }
        
        protected override void HandleFrightenedBehavior()
        {
            // Blinky runs away from the player when frightened
            base.HandleFrightenedBehavior();
        }
        
        protected override void HandleRecoveringBehavior()
        {
            // Blinky transitions back to normal behavior
            base.HandleRecoveringBehavior();
        }
        
        protected override void HandleReturningHomeBehavior()
        {
            // Blinky returns to the ghost house
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