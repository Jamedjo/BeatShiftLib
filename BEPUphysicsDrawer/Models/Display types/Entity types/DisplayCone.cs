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
    /// Simple display object for cones.  Creates a triangulated representation and keeps track of it during simulation.
    /// </summary>
    public class DisplayConeTest : DisplayEntity<Cone>
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
        public DisplayConeTest(ModelDrawer drawer, Cone displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 2 * NumSides - 2;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            float verticalOffset = -DisplayedObject.Height / 4;
            float angleBetweenFacets = MathHelper.TwoPi / NumSides;
            float radius = DisplayedObject.Radius + DisplayedObject.CollisionMargin - DisplayedObject.AllowedPenetration;

            //Create the vertex list

            var topVertexPosition = new Vector3(0, DisplayedObject.Height + verticalOffset, 0);

            for (int i = 0; i < NumSides; i++)
            {
                float theta = i * angleBetweenFacets;
                var position = new Vector3((float) Math.Cos(theta) * radius, verticalOffset, (float) Math.Sin(theta) * radius);
                Vector3 offset = topVertexPosition - position;
                Vector3 normal = Vector3.Normalize(Vector3.Cross(Vector3.Cross(offset, Vector3.Up), offset));
                //Top vertex
                vertices.Add(new VertexPositionNormalTexture(topVertexPosition, normal, Vector2.Zero));
                //Sloped vertices
                vertices.Add(new VertexPositionNormalTexture(position, normal, Vector2.Zero));
                //Bottom vertices
                vertices.Add(new VertexPositionNormalTexture(position, Vector3.Down, Vector2.Zero));
            }


            //Create the index list
            for (ushort i = 0; i < vertices.Count; i += 3)
            {
                //Each iteration, the loop advances to the next vertex 'column.'
                //Four triangles per column (except for the four degenerate cap triangles).

                //Sloped Triangles
                indices.Add(i);
                indices.Add((ushort) (i + 1));
                indices.Add((ushort) ((i + 4) % vertices.Count));

                //Bottom cap triangles.
                var nextIndex = (ushort) ((i + 5) % vertices.Count);
                if (nextIndex != 2) //Don't add cap indices if it's going to be a degenerate triangle.
                {
                    indices.Add((ushort) (i + 2));
                    indices.Add(2);
                    indices.Add(nextIndex);
                }
            }
        }
    }
}