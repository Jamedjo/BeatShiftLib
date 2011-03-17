/*
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


using Microsoft.Xna.Framework;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Superclass of display types that can draw themselves.
    /// These types do not use any instancing techniques.
    /// </summary>
    public abstract class SelfDrawingModelDisplayObject
    {
        protected SelfDrawingModelDisplayObject(ModelDrawer modelDrawer)
        {
            ModelDrawer = modelDrawer;
        }

        /// <summary>
        /// Gets the drawer that manages this display object.
        /// </summary>
        public ModelDrawer ModelDrawer { get; private set; }

        /// <summary>
        /// Updates the display object.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Draws the display object.
        /// </summary>
        /// <param name="viewMatrix">Current view matrix.</param>
        /// <param name="projectionMatrix">Current projection matrix.</param>
        public abstract void Draw(Matrix viewMatrix, Matrix projectionMatrix);
    }
}