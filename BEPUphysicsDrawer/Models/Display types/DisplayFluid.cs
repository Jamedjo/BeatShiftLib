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
using BEPUphysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Simple display object for triangles.
    /// </summary>
    public class DisplayFluidTest : ModelDisplayObject<FluidVolume>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayFluidTest(ModelDrawer drawer, FluidVolume displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 2 * DisplayedObject.Triangles.Count;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            for (int i = 0; i < DisplayedObject.Triangles.Count; i++)
            {
                vertices.Add(new VertexPositionNormalTexture(DisplayedObject.Triangles[i][0], DisplayedObject.Normal, new Vector2(0, 0)));
                vertices.Add(new VertexPositionNormalTexture(DisplayedObject.Triangles[i][1], DisplayedObject.Normal, new Vector2(0, 1)));
                vertices.Add(new VertexPositionNormalTexture(DisplayedObject.Triangles[i][2], DisplayedObject.Normal, new Vector2(1, 0)));

                vertices.Add(new VertexPositionNormalTexture(DisplayedObject.Triangles[i][0], -DisplayedObject.Normal, new Vector2(0, 0)));
                vertices.Add(new VertexPositionNormalTexture(DisplayedObject.Triangles[i][1], -DisplayedObject.Normal, new Vector2(0, 1)));
                vertices.Add(new VertexPositionNormalTexture(DisplayedObject.Triangles[i][2], -DisplayedObject.Normal, new Vector2(1, 0)));

                indices.Add((ushort) (i * 6));
                indices.Add((ushort) (i * 6 + 1));
                indices.Add((ushort) (i * 6 + 2));

                indices.Add((ushort) (i * 6 + 3));
                indices.Add((ushort) (i * 6 + 5));
                indices.Add((ushort) (i * 6 + 4));
            }
        }

        public override void Update()
        {
            WorldTransform = Matrix.Identity;
        }
    }
}