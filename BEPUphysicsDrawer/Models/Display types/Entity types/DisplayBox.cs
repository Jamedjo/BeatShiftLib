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
    /// Simple display class for boxes.  Creates and manages a triangulated represenation of the entity.
    /// </summary>
    public class DisplayBoxTest : DisplayEntity<Box>
    {
        /// <summary>
        /// Creates the display object for the entity.
        /// </summary>
        /// <param name="drawer">Drawer managing this display object.</param>
        /// <param name="displayedObject">Entity to draw.</param>
        public DisplayBoxTest(ModelDrawer drawer, Box displayedObject)
            : base(drawer, displayedObject)
        {
        }

        public override int GetTriangleCountEstimate()
        {
            return 12;
        }

        public override void GetVertexData(List<VertexPositionNormalTexture> vertices, List<ushort> indices)
        {
            float slop = DisplayedObject.CollisionMargin - DisplayedObject.AllowedPenetration;
            var boundingBox = new BoundingBox(
                new Vector3(-DisplayedObject.HalfWidth - slop,
                            -DisplayedObject.HalfHeight - slop,
                            -DisplayedObject.HalfLength - slop),
                new Vector3(DisplayedObject.HalfWidth + slop,
                            DisplayedObject.HalfHeight + slop,
                            DisplayedObject.HalfLength + slop));


            Vector3[] corners = boundingBox.GetCorners();

            var textureCoords = new Vector2[4];
            textureCoords[0] = new Vector2(0, 0);
            textureCoords[1] = new Vector2(1, 0);
            textureCoords[2] = new Vector2(1, 1);
            textureCoords[3] = new Vector2(0, 1);

            vertices.Add(new VertexPositionNormalTexture(corners[0], Vector3.Backward, textureCoords[0]));
            vertices.Add(new VertexPositionNormalTexture(corners[1], Vector3.Backward, textureCoords[1]));
            vertices.Add(new VertexPositionNormalTexture(corners[2], Vector3.Backward, textureCoords[2]));
            vertices.Add(new VertexPositionNormalTexture(corners[3], Vector3.Backward, textureCoords[3]));
            indices.Add(0);
            indices.Add(1);
            indices.Add(2);
            indices.Add(0);
            indices.Add(2);
            indices.Add(3);

            vertices.Add(new VertexPositionNormalTexture(corners[1], Vector3.Right, textureCoords[0]));
            vertices.Add(new VertexPositionNormalTexture(corners[2], Vector3.Right, textureCoords[3]));
            vertices.Add(new VertexPositionNormalTexture(corners[5], Vector3.Right, textureCoords[1]));
            vertices.Add(new VertexPositionNormalTexture(corners[6], Vector3.Right, textureCoords[2]));
            indices.Add(4);
            indices.Add(6);
            indices.Add(7);
            indices.Add(4);
            indices.Add(7);
            indices.Add(5);

            vertices.Add(new VertexPositionNormalTexture(corners[4], Vector3.Forward, textureCoords[1]));
            vertices.Add(new VertexPositionNormalTexture(corners[5], Vector3.Forward, textureCoords[0]));
            vertices.Add(new VertexPositionNormalTexture(corners[6], Vector3.Forward, textureCoords[3]));
            vertices.Add(new VertexPositionNormalTexture(corners[7], Vector3.Forward, textureCoords[2]));
            indices.Add(9);
            indices.Add(8);
            indices.Add(11);
            indices.Add(9);
            indices.Add(11);
            indices.Add(10);

            vertices.Add(new VertexPositionNormalTexture(corners[0], Vector3.Left, textureCoords[1]));
            vertices.Add(new VertexPositionNormalTexture(corners[3], Vector3.Left, textureCoords[2]));
            vertices.Add(new VertexPositionNormalTexture(corners[4], Vector3.Left, textureCoords[0]));
            vertices.Add(new VertexPositionNormalTexture(corners[7], Vector3.Left, textureCoords[3]));
            indices.Add(14);
            indices.Add(12);
            indices.Add(13);
            indices.Add(14);
            indices.Add(13);
            indices.Add(15);

            vertices.Add(new VertexPositionNormalTexture(corners[0], Vector3.Up, textureCoords[2]));
            vertices.Add(new VertexPositionNormalTexture(corners[1], Vector3.Up, textureCoords[3]));
            vertices.Add(new VertexPositionNormalTexture(corners[4], Vector3.Up, textureCoords[1]));
            vertices.Add(new VertexPositionNormalTexture(corners[5], Vector3.Up, textureCoords[0]));
            indices.Add(16);
            indices.Add(19);
            indices.Add(17);
            indices.Add(16);
            indices.Add(18);
            indices.Add(19);

            vertices.Add(new VertexPositionNormalTexture(corners[2], Vector3.Down, textureCoords[1]));
            vertices.Add(new VertexPositionNormalTexture(corners[3], Vector3.Down, textureCoords[0]));
            vertices.Add(new VertexPositionNormalTexture(corners[6], Vector3.Down, textureCoords[2]));
            vertices.Add(new VertexPositionNormalTexture(corners[7], Vector3.Down, textureCoords[3]));
            indices.Add(21);
            indices.Add(20);
            indices.Add(22);
            indices.Add(21);
            indices.Add(22);
            indices.Add(23);
        }
    }
}