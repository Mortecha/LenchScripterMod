﻿namespace Lench.Scripter.Blocks
{
    /// <summary>
    ///     Handler for the Grenade block.
    /// </summary>
    public class Grenade : BlockHandler
    {
        private readonly ControllableBomb _cb;

        /// <summary>
        ///     Creates a Block handler.
        /// </summary>
        /// <param name="bb">BlockBehaviour object.</param>
        public Grenade(BlockBehaviour bb) : base(bb)
        {
            _cb = bb.GetComponent<ControllableBomb>();
        }

        /// <summary>
        ///     Invokes the block's action.
        ///     Throws ActionNotFoundException if the block does not posess such action.
        /// </summary>
        /// <param name="actionName">Display name of the action.</param>
        public override void Action(string actionName)
        {
            actionName = actionName.ToUpper();
            switch (actionName)
            {
                case "DETONATE":
                    Detonate();
                    return;
            }
            throw new ActionNotFoundException("Block " + BlockName + " has no " + actionName + " action.");
        }

        /// <summary>
        ///     Detonate the grenade.
        /// </summary>
        public void Detonate()
        {
            _cb.StartCoroutine_Auto(_cb.Explode());
        }
    }
}