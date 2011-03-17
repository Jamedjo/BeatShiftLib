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


using System.Collections.Generic;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Simple display object for triangles.
    /// </summary>
    public class DisplayTriangleTest : DisplayEntity<Triangle>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayTriangleTest(ModelDrawer drawer, Triangle displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 2;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            vertices.Add(new VertexPositionNormalTexture(DisplayedObject.LocalVertices[0], DisplayedObject.LocalNormal, new Vector2(0, 0)));
            vertices.Add(new VertexPositionNormalTexture(DisplayedObject.LocalVertices[1], DisplayedObject.LocalNormal, new Vector2(0, 1)));
            vertices.Add(new VertexPositionNormalTexture(DisplayedObject.LocalVertices[2], DisplayedObject.LocalNormal, new Vector2(1, 0)));

            vertices.Add(new VertexPositionNormalTexture(DisplayedObject.LocalVertices[0], -DisplayedObject.LocalNormal, new Vector2(0, 0)));
            vertices.Add(new VertexPositionNormalTexture(DisplayedObject.LocalVertices[1], -DisplayedObject.LocalNormal, new Vector2(0, 1)));
            vertices.Add(new VertexPositionNormalTexture(DisplayedObject.LocalVertices[2], -DisplayedObject.LocalNormal, new Vector2(1, 0)));

            indices.Add(0);
            indices.Add(1);
            indices.Add(2);

            indices.Add(3);
            indices.Add(5);
            indices.Add(4);
        }
    }
}