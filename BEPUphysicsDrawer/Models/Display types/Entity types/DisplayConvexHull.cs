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
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Simple display object for triangles.
    /// </summary>
    public class DisplayConvexHullTest : DisplayEntity<ConvexHull>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayConvexHullTest(ModelDrawer drawer, ConvexHull displayedObject)
            : base(drawer, displayedObject)
        {
        }


        public override int GetTriangleCountEstimate()
        {
            return DisplayedObject.BodyPoints.Count * 4; //It's hard to know how many triangles it will take before doing a convex hull.
        }


        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            var hullTriangleVertices = new List<Vector3>();
            var hullTriangleIndices = new List<int>();
            Toolbox.GetConvexHull(DisplayedObject.BodyPoints, hullTriangleIndices, hullTriangleVertices);
                //The hull triangle vertices are used as a dummy to get the unnecessary hull vertices, which are cleared afterwards.
            hullTriangleVertices.Clear();
            foreach (int i in hullTriangleIndices)
            {
                hullTriangleVertices.Add(DisplayedObject.BodyPoints[i]);
            }

            var toReturn = new VertexPositionNormalTexture[hullTriangleVertices.Count];
            Vector3 normal;
            for (ushort i = 0; i < hullTriangleVertices.Count; i += 3)
            {
                normal = Vector3.Normalize(Vector3.Cross(hullTriangleVertices[i + 2] - hullTriangleVertices[i], hullTriangleVertices[i + 1] - hullTriangleVertices[i]));
                vertices.Add(new VertexPositionNormalTexture(hullTriangleVertices[i], normal, new Vector2(0, 0)));
                vertices.Add(new VertexPositionNormalTexture(hullTriangleVertices[i + 1], normal, new Vector2(1, 0)));
                vertices.Add(new VertexPositionNormalTexture(hullTriangleVertices[i + 2], normal, new Vector2(0, 1)));
                indices.Add(i);
                indices.Add((ushort) (i + 1));
                indices.Add((ushort) (i + 2));
            }
        }
    }
}