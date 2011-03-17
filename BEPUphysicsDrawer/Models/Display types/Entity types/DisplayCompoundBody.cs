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
    public class DisplayCompoundBodyTest : DisplayEntity<CompoundBody>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayCompoundBodyTest(ModelDrawer drawer, CompoundBody displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 100;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            var tempIndices = new List<ushort>();
            var tempVertices = new List<VertexPositionNormalTexture>();
            for (int i = 0; i < DisplayedObject.SubBodies.Count; i++)
            {
                ModelDisplayObjectBase displayObject = Drawer.GetDisplayObject(DisplayedObject.SubBodies[i]);

                displayObject.GetVertexData(tempVertices, tempIndices);
                for (int j = 0; j < tempIndices.Count; j++)
                {
                    indices.Add((ushort) (tempIndices[j] + vertices.Count));
                }
                for (int j = 0; j < tempVertices.Count; j++)
                {
                    VertexPositionNormalTexture vertex = tempVertices[j];
                    vertex.Position = Vector3.Transform(vertex.Position, DisplayedObject.SubBodyLocalRotations[i]) + DisplayedObject.SubBodyLocalOffsets[i];
                    vertex.Normal = Vector3.Transform(vertex.Normal, DisplayedObject.SubBodyLocalRotations[i]);
                    vertices.Add(vertex);
                }

                tempVertices.Clear();
                tempIndices.Clear();
            }
        }
    }
}