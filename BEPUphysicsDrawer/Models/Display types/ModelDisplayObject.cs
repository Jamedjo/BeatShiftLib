﻿/*
      Copyright (C) 2010 Bepu Entertainment LLC.

      This software source code is provided 'as-is', without 
      any express or implied warranty.  In no event will the authors be held 
      liable for any damages arising from the use of this software.

      Permission is granted to anyone to use this software for any purpose,
      including commercial applications, and to alter it and redistribute it
      freely, subject to the following restrictions:

      1. The origin of this software must not be misrepresented; you must not
         claim that you wrote the original software. If you use this software
         in a product, an acknowledgment in the product documentation would be
         appreciated but is not required.
      2. Altered source versions must be plainly marked as such, and must not be
         misrepresented as being the original software.
      3. This notice may not be removed or altered from any source distribution.

    Contact us at:
    contact@bepu-games.com
 */


namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Model-based graphical representation of an object.
    /// </summary>
    /// <typeparam name="T">Type of the object to be displayed.</typeparam>
    public abstract class ModelDisplayObject<T> : ModelDisplayObjectBase
    {
        protected ModelDisplayObject(ModelDrawer drawer, T displayedObject)
            : base(drawer)
        {
            DisplayedObject = displayedObject;
        }

        /// <summary>
        /// Gets the object to represent with a model.
        /// </summary>
        public T DisplayedObject { get; private set; }
    }
}