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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Manages and draws non-instanced models.
    /// </summary>
    public class BruteModelDrawer : ModelDrawer
    {
        private readonly List<BruteDisplayObjectEntry> displayObjects = new List<BruteDisplayObjectEntry>();
        private readonly BasicEffect effect;

        private VertexDeclaration vertexDeclaration;

        public BruteModelDrawer(Game game)
            : base(game)
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.PreferPerPixelLighting = true;
            effect.LightingEnabled = true;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
            effect.DirectionalLight0.DiffuseColor = new Vector3(.66f, .66f, .66f);
            effect.AmbientLightColor = new Vector3(.5f, .5f, .5f);

            effect.TextureEnabled = true;

            vertexDeclaration = VertexPositionNormalTexture.VertexDeclaration;
        }

        protected override void Add(ModelDisplayObjectBase displayObject)
        {
            displayObjects.Add(new BruteDisplayObjectEntry(this, displayObject));
        }

        protected override void Remove(ModelDisplayObjectBase displayObject)
        {
            for (int i = 0; i < displayObjects.Count; i++)
            {
                if (displayObjects[i].displayObject == displayObject)
                {
                    displayObjects.RemoveAt(i);
                    break;
                }
            }
        }

        protected override void ClearManagedModels()
        {
            displayObjects.Clear();
        }

        protected override void UpdateManagedModels()
        {
            foreach (BruteDisplayObjectEntry entry in displayObjects)
            {
                entry.displayObject.Update();
            }
        }


        /// <summary>
        /// Draws the models managed by the drawer.
        /// </summary>
        /// <param name="viewMatrix">View matrix to use to draw the objects.</param>
        /// <param name="projectionMatrix">Projection matrix to use to draw the objects.</param>
        protected override void DrawManagedModels(Matrix viewMatrix, Matrix projectionMatrix)
        {
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;

            for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
            {
                foreach (BruteDisplayObjectEntry entry in displayObjects)
                {
                    entry.Draw(textures, Game.GraphicsDevice, effect, effect.CurrentTechnique.Passes[i]);
                }
            }
        }
    }
}