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


using System;
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
    public class DisplayWrappedBodyTest : DisplayEntity<WrappedBody>
    {
        /// <summary>
        /// Number of sides of spherical sampling to take when creating graphical representations.
        /// </summary>
        public static int NumSamples = 16;

        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayWrappedBodyTest(ModelDrawer drawer, WrappedBody displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 200; //It's hard to know how many triangles it will take before doing a convex hull.
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            var points = new List<Vector3>();
            Vector3 max;
            var direction = new Vector3();
            float margin = DisplayedObject.CollisionMargin - DisplayedObject.AllowedPenetration;
            float angleChange = MathHelper.TwoPi / NumSamples;

            Vector3 centerPosition = DisplayedObject.CenterPosition;
            Matrix transposeOrientation = Matrix.Transpose(DisplayedObject.OrientationMatrix);
            for (int i = 1; i < NumSamples / 2 - 1; i++)
            {
                float phi = MathHelper.PiOver2 - i * angleChange;
                var sinPhi = (float) Math.Sin(phi);
                var cosPhi = (float) Math.Cos(phi);
                for (int j = 0; j < NumSamples; j++)
                {
                    float theta = j * angleChange;
                    direction.X = (float) Math.Cos(theta) * cosPhi;
                    direction.Y = sinPhi;
                    direction.Z = (float) Math.Sin(theta) * cosPhi;

                    DisplayedObject.GetExtremePoint(ref direction, margin, out max);
                    points.Add(Vector3.TransformNormal(max - centerPosition, transposeOrientation));
                }
            }

            DisplayedObject.GetExtremePoint(ref Toolbox.UpVector, margin, out max);
            points.Add(Vector3.TransformNormal(max - centerPosition, transposeOrientation));
            DisplayedObject.GetExtremePoint(ref Toolbox.DownVector, margin, out max);
            points.Add(Vector3.TransformNormal(max - centerPosition, transposeOrientation));

            var hullTriangleVertices = new List<Vector3>();
            var hullTriangleIndices = new List<int>();
            Toolbox.GetConvexHull(points, hullTriangleIndices, hullTriangleVertices);
                //The hull triangle vertices are used as a dummy to get the unnecessary hull vertices, which are cleared afterwards.
            hullTriangleVertices.Clear();
            foreach (int i in hullTriangleIndices)
            {
                hullTriangleVertices.Add(points[i]);
            }

            for (ushort i = 0; i < hullTriangleVertices.Count; i += 3)
            {
                Vector3 normal = Vector3.Normalize(Vector3.Cross(hullTriangleVertices[i + 2] - hullTriangleVertices[i], hullTriangleVertices[i + 1] - hullTriangleVertices[i]));
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