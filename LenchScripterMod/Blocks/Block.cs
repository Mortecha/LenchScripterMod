﻿using System;
using System.Collections.Generic;
using UnityEngine;
using LenchScripter.Internal;

namespace LenchScripter.Blocks
{

    /// <summary>
    /// Base class for all block handlers.
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Used to convert radians to degrees in all functions returning radians.
        /// If radians are the desired angle unit, it will be set to 1, otherwise Mathf.Rad2Deg.
        /// </summary>
        protected static float convertToDegrees = Mathf.Rad2Deg;

        /// <summary>
        /// Used to convert degrees to radians in all functions returning degrees.
        /// If degrees are the desired angle unit, it will be set to 1, otherwise Mathf.Deg2Rad.
        /// </summary>
        protected static float convertToRadians = 1;

        /// <summary>
        /// Name of the block. Corresponds to MyBlockInfo.BlockName.
        /// </summary>
        public string BlockName
        {
            get
            {
                return bb.GetComponent<MyBlockInfo>().blockName;
            }
            set
            {
                bb.GetComponent<MyBlockInfo>().blockName = value;
            }
        }

        /// <summary>
        /// Type of the block.
        /// </summary>
        public BlockType BlockType {
            get
            {
                return (BlockType)bb.GetBlockID();
            }
        }

        /// <summary>
        /// BlockBehaviour object of this handler.
        /// </summary>
        protected readonly BlockBehaviour bb;

        /// <summary>
        /// BlockLoaders BlockScript object.
        /// </summary>
        protected readonly MonoBehaviour bs;

        internal BlockBehaviour GetBlockBehaviour()
        {
            return bb;
        }

        /// <summary>
        /// Creates a Block handler.
        /// </summary>
        /// <param name="bb">BlockBehaviour object.</param>
        public Block(BlockBehaviour bb)
        {
            this.bb = bb;
            if (ScripterMod.blockScriptType != null)
                bs = bb.GetComponent(ScripterMod.blockScriptType) as MonoBehaviour;

            Scripter.Instance.OnUpdate += Update;
            Scripter.Instance.OnUpdate += LateUpdate;
            Scripter.Instance.OnUpdate += FixedUpdate;
        }

        /// <summary>
        /// Is called at every Update.
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// Is called at every LateUpdate.
        /// </summary>
        protected virtual void LateUpdate() { }

        /// <summary>
        /// Is called at every FixedUpdate.
        /// </summary>
        protected virtual void FixedUpdate() { }

        /// <summary>
        /// Makes any future calls to angle functions return degrees.
        /// </summary>
        public static void UseDegrees()
        {
            convertToDegrees = Mathf.Rad2Deg;
            convertToRadians = 1;
        }

        /// <summary>
        /// Makes any future calls to angle functions return radians.
        /// </summary>
        public static void UseRadians()
        {
            convertToDegrees = 1;
            convertToRadians = Mathf.Deg2Rad;
        }

        /// <summary>
        /// Invokes the block's action.
        /// Throws ActionNotFoundException if the block does not poses such action.
        /// </summary>
        /// <param name="actionName">Display name of the action.</param>
        public virtual void action(string actionName) {
            actionName = actionName.ToUpper();
            throw new ActionNotFoundException("Block " + BlockName + " has no " + actionName + " action.");
        }

        /// <summary>
        /// Returns true if the block has RigidBody.
        /// </summary>
        /// <returns>Boolean value.</returns>
        public virtual bool exists()
        {
            return bb != null && bb.GetComponent<Rigidbody>() != null;
        }

        /// <summary>
        /// Returns a list of all available toggles.
        /// </summary>
        /// <returns></returns>
        public virtual List<string> getToggles()
        {
            List<string> toggles = new List<string>();
            foreach (MToggle m in bb.Toggles)
            {
                toggles.Add(m.DisplayName.ToUpper());
            }
            return toggles;
        }

        /// <summary>
        /// Returns a list of all available sliders.
        /// </summary>
        /// <returns></returns>
        public virtual List<string> getSliders()
        {
            List<string> sliders = new List<string>();
            foreach (MSlider m in bb.Sliders)
            {
                sliders.Add(m.DisplayName.ToUpper());
            }
            return sliders;
        }

        /// <summary>
        /// Sets the toggle mode of the block, specified by the toggle display name.
        /// </summary>
        /// <param name="toggleName">Toggle property to be set.</param>
        /// <param name="value">Boolean value to be set.</param>
        public virtual void setToggleMode(string toggleName, bool value)
        {
            foreach (MToggle m in bb.Toggles)
            {
                if (m.DisplayName.ToUpper() == toggleName.ToUpper())
                {
                    m.IsActive = value;
                    return;
                }
            }
            throw new PropertyNotFoundException("Toggle " + toggleName + " not found.");
        }

        /// <summary>
        /// Sets the slider value of the block, specified by the slider display name.
        /// </summary>
        /// <param name="sliderName">Slider value to be set.</param>
        /// <param name="value">Float value to be set.</param>
        public virtual void setSliderValue(string sliderName, float value)
        {
            if (float.IsNaN(value))
                throw new ArgumentException("Value is not a number (NaN).");
            foreach (MSlider m in bb.Sliders)
            {
                if (m.DisplayName.ToUpper() == sliderName.ToUpper())
                {
                    m.Value = value;
                    return;
                }
            }
            throw new PropertyNotFoundException("Slider " + sliderName + " not found.");
        }

        /// <summary>
        /// Returns the toggle mode of the block, specified by the toggle display name.
        /// </summary>
        /// <param name="toggleName">Toggle property to be returned.</param>
        /// <returns>Boolean value.</returns>
        public virtual bool getToggleMode(string toggleName)
        {
            foreach (MToggle m in bb.Toggles)
            {
                if (m.DisplayName.ToUpper() == toggleName.ToUpper())
                {
                    return m.IsActive;
                }
            }
            throw new PropertyNotFoundException("Toggle " + toggleName + " not found.");
        }

        /// <summary>
        /// Returns the slider value of the block, specified by the slider display name.
        /// </summary>
        /// <param name="sliderName">Toggle property to be returned.</param>
        /// <returns>Float value.</returns>
        public virtual float getSliderValue(string sliderName)
        {
            foreach (MSlider m in bb.Sliders)
            {
                if (m.DisplayName.ToUpper() == sliderName.ToUpper())
                {
                    return m.Value;
                }
            }
            throw new PropertyNotFoundException("Slider " + sliderName + " not found.");
        }

        /// <summary>
        /// Returns the key mapper's minimum slider value, specified by the slider display name.
        /// </summary>
        /// <param name="sliderName">Minimum slider value to be returned.</param>
        /// <returns>Float value.</returns>
        public virtual float getSliderMin(string sliderName)
        {
            foreach (MSlider m in bb.Sliders)
            {
                if (m.DisplayName.ToUpper() == sliderName.ToUpper())
                {
                    return m.Min;
                }
            }
            throw new PropertyNotFoundException("Slider " + sliderName + " not found.");
        }

        /// <summary>
        /// Returns the key mapper's maximum slider value, specified by the slider display name.
        /// </summary>
        /// <param name="sliderName">Maximum slider value to be returned.</param>
        /// <returns>Float value.</returns>
        public virtual float getSliderMax(string sliderName)
        {
            foreach (MSlider m in bb.Sliders)
            {
                if (m.DisplayName.ToUpper() == sliderName.ToUpper())
                {
                    return m.Max;
                }
            }
            throw new PropertyNotFoundException("Slider " + sliderName + " not found.");
        }

        /// <summary>
        /// Adds key to the specified key bind.
        /// </summary>
        /// <param name="keyName">Key bind to add the key to.</param>
        /// <param name="key">Key value to be added.</param>
        public virtual void addKey(string keyName, KeyCode key)
        {
            foreach (MKey m in bb.Keys)
            {
                if (m.DisplayName.ToUpper() == keyName.ToUpper())
                {
                    for (int i = 0; i < m.KeyCode.Count; i++)
                        if (m.KeyCode[i] == KeyCode.None)
                        {
                            m.AddOrReplaceKey(i, key);
                            return;
                        }
                    m.AddKey(key);
                    return;
                }
            }
            throw new PropertyNotFoundException("Key " + keyName + " not found.");
        }

        /// <summary>
        /// Replaces the first key bound to the specified key bind.
        /// </summary>
        /// <param name="keyName">Key bind to be replaced.</param>
        /// <param name="key">Key value to be replaced with.</param>
        public virtual void replaceKey(string keyName, KeyCode key)
        {
            foreach (MKey m in bb.Keys)
            {
                if (m.DisplayName.ToUpper() == keyName.ToUpper())
                {
                    m.AddOrReplaceKey(0, key);
                }
            }
            throw new PropertyNotFoundException("Key " + keyName + " not found.");
        }

        /// <summary>
        /// Returns the first key value bound of the specified key bind.
        /// </summary>
        /// <param name="keyName">Key bind to be returned.</param>
        /// <returns>Integer value.</returns>
        public virtual KeyCode getKey(string keyName)
        {
            foreach (MKey m in bb.Keys)
            {
                if (m.DisplayName.ToUpper() == keyName.ToUpper())
                {
                    return m.KeyCode[0];
                }
            }
            throw new PropertyNotFoundException("Key " + keyName + " not found.");
        }

        /// <summary>
        /// Clears all keys of the specified key bind.
        /// </summary>
        /// <param name="keyName"></param>
        public virtual void clearKeys(string keyName)
        {
            foreach (MKey m in bb.Keys)
            {
                if (m.DisplayName.ToUpper() == keyName.ToUpper())
                {
                    for (int i = 0; i < m.KeyCode.Count; i++)
                        m.AddOrReplaceKey(i, KeyCode.None);
                    return;
                }
            }
            throw new PropertyNotFoundException("Key " + keyName + " not found.");
        }

        /// <summary>
        /// Returns the block's forward vector.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getForward()
        {
            return bb.transform.forward;
        }

        /// <summary>
        /// Returns the block's up vector.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getUp()
        {
            return bb.transform.up;
        }

        /// <summary>
        /// Returns the block's right vector.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getRight()
        {
            return bb.transform.right;
        }

        /// <summary>
        /// Returns the position of the block.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getPosition()
        {
            return bb.transform.position;
        }

        /// <summary>
        /// Returns the velocity of the block in units per second.
        /// Throws NoRigidBodyException if the block has no RigidBody.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getVelocity()
        {
            Rigidbody body = bb.GetComponent<Rigidbody>();
            if (body != null)
                return body.velocity;
            throw new NoRigidBodyException("Block " + BlockName + " has no rigid body.");
        }

        /// <summary>
        /// Returns the mass of the block.
        /// </summary>
        /// <returns>Float value.</returns>
        public virtual float getMass()
        {
            Rigidbody body = bb.GetComponent<Rigidbody>();
            if (body != null)
                return body.mass;
            throw new NoRigidBodyException("Block " + BlockName + " has no rigid body.");
        }

        /// <summary>
        /// Returns the center of mass of the block, relative to the block's position.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getCenterOfMass()
        {
            Rigidbody body = bb.GetComponent<Rigidbody>();
            if (body != null)
                return body.centerOfMass;
            throw new NoRigidBodyException("Block " + BlockName + " has no rigid body.");
        }

        /// <summary>
        /// Returns the block's rotation in the form of it's Euler angles.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getEulerAngles()
        {
            Vector3 d2r = new Vector3(convertToRadians, convertToRadians, convertToRadians);
            Vector3 euler = bb.transform.eulerAngles;
            euler.Scale(d2r);
            return euler;
        }

        /// <summary>
        /// Returns the block's angular velocity.
        /// Throws NoRigidBodyException if the block has no RigidBody.
        /// </summary>
        /// <returns>UnityEngine.Vector3 vector.</returns>
        public virtual Vector3 getAngularVelocity()
        {
            Rigidbody body = bb.GetComponent<Rigidbody>();
            if (body != null)
            {
                Vector3 convertUnits = new Vector3(convertToDegrees, convertToDegrees, convertToDegrees);
                Vector3 angularVelocity = body.angularVelocity;
                angularVelocity.Scale(convertUnits);
                return angularVelocity;
            }
            throw new NoRigidBodyException("Block " + BlockName + " has no rigid body.");
        }
    }

    /// <summary>
    /// Exception to be thrown when a block is not found.
    /// </summary>
    public class BlockNotFoundException : Exception
    {
        public BlockNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception to be thrown when trying to call an action that does not exist for the current block.
    /// </summary>
    public class ActionNotFoundException : Exception
    {
        public ActionNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception to be thrown when referencing the block's property that does not exist for the current block.
    /// </summary>
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception to be thrown when trying to access the block's rigid body if it does not have one.
    /// </summary>
    public class NoRigidBodyException : Exception
    {
        public NoRigidBodyException(string message) : base(message) { }
    }
}
