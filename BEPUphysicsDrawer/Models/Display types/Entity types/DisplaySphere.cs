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
    /// Simple display object for spheres.  Creates a triangulated representation and keeps track of it during simulation.
    /// </summary>
    public class DisplaySphereTest : DisplayEntity<Sphere>
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
        public DisplaySphereTest(ModelDrawer drawer, Sphere displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return NumSides * (NumSides - 2);
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            var n = new Vector3();
            float angleBetweenFacets = MathHelper.TwoPi / NumSides;
            float radius = DisplayedObject.Radius + DisplayedObject.CollisionMargin - DisplayedObject.AllowedPenetration;

            //Create the vertex list
            vertices.Add(new VertexPositionNormalTexture(new Vector3(0, radius, 0), Vector3.Up, Vector2.Zero));
            for (int i = 1; i < NumSides / 2; i++)
            {
                float phi = MathHelper.PiOver2 - i * angleBetweenFacets;
                var sinPhi = (float) Math.Sin(phi);
                var cosPhi = (float) Math.Cos(phi);

                for (int j = 0; j < NumSides; j++)
                {
                    float theta = j * angleBetweenFacets;

                    n.X = (float) Math.Cos(theta) * cosPhi;
                    n.Y = sinPhi;
                    n.Z = (float) Math.Sin(theta) * cosPhi;

                    vertices.Add(new VertexPositionNormalTexture(n * radius, n, Vector2.Zero));
                }
            }
            vertices.Add(new VertexPositionNormalTexture(new Vector3(0, -radius, 0), Vector3.Down, Vector2.Zero));


            //Create the index list
            for (int i = 0; i < NumSides; i++)
            {
                indices.Add((ushort) (vertices.Count - 1));
                indices.Add((ushort) (vertices.Count - 2 - i));
                indices.Add((ushort) (vertices.Count - 2 - (i + 1) % NumSides));
            }

            for (int i = 0; i < NumSides / 2 - 2; i++)
            {
                for (int j = 0; j < NumSides; j++)
                {
                    int nextColumn = (j + 1) % NumSides;

                    indices.Add((ushort) (i * NumSides + nextColumn + 1));
                    indices.Add((ushort) (i * NumSides + j + 1));
                    indices.Add((ushort) ((i + 1) * NumSides + j + 1));

                    indices.Add((ushort) ((i + 1) * NumSides + nextColumn + 1));
                    indices.Add((ushort) (i * NumSides + nextColumn + 1));
                    indices.Add((ushort) ((i + 1) * NumSides + j + 1));
                }
            }

            for (int i = 0; i < NumSides; i++)
            {
                indices.Add(0);
                indices.Add((ushort) (i + 1));
                indices.Add((ushort) ((i + 1) % NumSides + 1));
            }
        }
    }
}