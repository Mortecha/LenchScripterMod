﻿using System.Reflection;
using UnityEngine;

namespace LenchScripterMod.Blocks
{
    public class Flamethrower : Block
    {
        private FlamethrowerController fc;
        private MToggle holdToFire;

        internal Flamethrower(BlockBehaviour bb) : base(bb)
        {
            fc = bb.GetComponent<FlamethrowerController>();
            FieldInfo holdFieldInfo = fc.GetType().GetField("holdToFire", BindingFlags.NonPublic | BindingFlags.Instance);
            holdToFire = holdFieldInfo.GetValue(fc) as MToggle;
        }

        public override void action(string actionName)
        {
            actionName = actionName.ToUpper();
            if (actionName == "IGNITE")
            {
                Ignite();
                return;
            }
            throw new ActionNotFoundException("Block " + name + " has no " + actionName + " action.");
        }

        public void Ignite()
        {
            if (holdToFire.IsActive)
            {
                if (fc.timeOut || STATLORD.infiniteAmmoMode)
                {
                    fc.Flame();
                }
            }
            else if (fc || STATLORD.infiniteAmmoMode)
            {
                fc.FlameOn();
            }

            if (fc.isFlaming)
            {
                fc.timey = fc.timey + Time.deltaTime;
            }
            if (fc.timey >= 10)
            {
                fc.TimeOut();
            }
        }

        internal static bool isFlamethrower(BlockBehaviour bb)
        {
            return bb.GetComponent<FlamethrowerController>() != null;
        }
    }
}
