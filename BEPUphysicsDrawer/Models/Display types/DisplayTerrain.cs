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
    public class DisplayTerrainTest : ModelDisplayObject<Terrain>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayTerrainTest(ModelDrawer drawer, Terrain displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return DisplayedObject.Heights.Length * 2;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            Vector3 normal;
            int numColumns = DisplayedObject.Heights.GetLength(0);
            int numRows = DisplayedObject.Heights.GetLength(1);
            for (int i = 0; i < numColumns; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    normal = DisplayedObject.GetNormal(i, j);
                    vertices.Add(new VertexPositionNormalTexture(
                                     new Vector3(i * DisplayedObject.GetSpacingX(), DisplayedObject.Heights[i, j], j * DisplayedObject.GetSpacingZ()),
                                     normal,
                                     new Vector2(i, j)));
                }
            }
            for (int i = 0; i < numColumns - 1; i++)
            {
                for (int j = 0; j < numRows - 1; j++)
                {
                    for (int k = DisplayedObject.QuadTriangles.Length - 1; k >= 0; k--)
                    {
                        switch (DisplayedObject.QuadTriangles[k])
                        {
                            case 0:
                                indices.Add((ushort) (numRows * j + i));
                                break;
                            case 1:
                                indices.Add((ushort) (numRows * j + i + 1));
                                break;
                            case 2:
                                indices.Add((ushort) (numRows * (j + 1) + i));
                                break;
                            case 3:
                                indices.Add((ushort) (numRows * (j + 1) + i + 1));
                                break;
                        }
                    }
                }
            }
        }

        public override void Update()
        {
            WorldTransform = Matrix.CreateTranslation(DisplayedObject.GetPosition());
        }
    }
}