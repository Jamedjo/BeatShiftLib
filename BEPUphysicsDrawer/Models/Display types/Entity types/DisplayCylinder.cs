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
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Simple display object for cylinders.  Creates a triangulated representation and keeps track of it during simulation.
    /// </summary>
    public class DisplayCylinderTest : DisplayEntity<Cylinder>
    {
        /// <summary>
        /// Number of sides to build geometry with.
        /// </summary>
        public static int NumSides = 24;

        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayCylinderTest(ModelDrawer drawer, Cylinder displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 4 * NumSides - 4;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            float verticalOffset = DisplayedObject.Height / 2;
            float angleBetweenFacets = MathHelper.TwoPi / NumSides;
            float radius = DisplayedObject.Radius + DisplayedObject.CollisionMargin - DisplayedObject.AllowedPenetration;

            //Create the vertex list
            for (int i = 0; i < NumSides; i++)
            {
                float theta = i * angleBetweenFacets;
                float x = (float) Math.Cos(theta) * radius;
                float z = (float) Math.Sin(theta) * radius;
                //Top cap
                vertices.Add(new VertexPositionNormalTexture(new Vector3(x, verticalOffset, z), Vector3.Up, Vector2.Zero));
                //Top part of body
                vertices.Add(new VertexPositionNormalTexture(new Vector3(x, verticalOffset, z), new Vector3(x, 0, z), Vector2.Zero));
                //Bottom part of body
                vertices.Add(new VertexPositionNormalTexture(new Vector3(x, -verticalOffset, z), new Vector3(x, 0, z), Vector2.Zero));
                //Bottom cap
                vertices.Add(new VertexPositionNormalTexture(new Vector3(x, -verticalOffset, z), Vector3.Down, Vector2.Zero));
            }


            //Create the index list
            //The vertices are arranged a little nonintuitively.
            //0 is part of the top cap, 1 is the upper body, 2 is lower body, and 3 is bottom cap.
            for (ushort i = 0; i < vertices.Count; i += 4)
            {
                //Each iteration, the loop advances to the next vertex 'column.'
                //Four triangles per column (except for the four degenerate cap triangles).

                //Top cap triangles
                var nextIndex = (ushort) ((i + 4) % vertices.Count);
                if (nextIndex != 0) //Don't add cap indices if it's going to be a degenerate triangle.
                {
                    indices.Add(i);
                    indices.Add(nextIndex);
                    indices.Add(0);
                }

                //Body triangles
                nextIndex = (ushort) ((i + 5) % vertices.Count);
                indices.Add((ushort) (i + 1));
                indices.Add((ushort) (i + 2));
                indices.Add(nextIndex);

                indices.Add(nextIndex);
                indices.Add((ushort) (i + 2));
                indices.Add((ushort) ((i + 6) % vertices.Count));

                //Bottom cap triangles.
                nextIndex = (ushort) ((i + 7) % vertices.Count);
                if (nextIndex != 3) //Don't add cap indices if it's going to be a degenerate triangle.
                {
                    indices.Add((ushort) (i + 3));
                    indices.Add(3);
                    indices.Add(nextIndex);
                }
            }
        }
    }
}