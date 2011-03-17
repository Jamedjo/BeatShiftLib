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
using BEPUphysics.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Simple display object for triangles.
    /// </summary>
    public class DisplayTriangleMeshTest : ModelDisplayObject<TriangleMesh>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayTriangleMeshTest(ModelDrawer drawer, TriangleMesh displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return DisplayedObject.Indices.Length / 3;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            var tempVertices = new VertexPositionNormalTexture[DisplayedObject.Vertices.Length];
            var numNormalContributions = new int[DisplayedObject.Vertices.Length];
            for (int i = 0; i < DisplayedObject.Vertices.Length; i++)
            {
                tempVertices[i] = new VertexPositionNormalTexture(DisplayedObject.Vertices[i].Position, Vector3.Zero, Vector2.Zero);
            }

            for (int i = 0; i < DisplayedObject.Indices.Length; i++)
            {
                indices.Add((ushort) DisplayedObject.Indices[i]);
            }
            for (int i = 0; i < indices.Count; i += 3)
            {
                int a = indices[i];
                int b = indices[i + 1];
                int c = indices[i + 2];
                Vector3 normal = Vector3.Normalize(Vector3.Cross(
                    tempVertices[c].Position - tempVertices[a].Position,
                    tempVertices[b].Position - tempVertices[a].Position));
                tempVertices[a].Normal += normal;
                tempVertices[b].Normal += normal;
                tempVertices[c].Normal += normal;
                numNormalContributions[a]++;
                numNormalContributions[b]++;
                numNormalContributions[c]++;
            }

            for (int i = 0; i < tempVertices.Length; i++)
            {
                tempVertices[i].Normal /= numNormalContributions[i];
                vertices.Add(tempVertices[i]);
            }
        }

        public override void Update()
        {
            WorldTransform = DisplayedObject.WorldMatrix;
        }
    }
}