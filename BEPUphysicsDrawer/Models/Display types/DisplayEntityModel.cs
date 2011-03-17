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


using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Display object of a model that follows an entity.
    /// </summary>
    public class DisplayEntityModelTest : SelfDrawingModelDisplayObject
    {
        private Model myModel;

        private Texture2D myTexture;

        /// <summary>
        /// Bone transformations of meshes in the model.
        /// </summary>
        private Matrix[] transforms;

        /// <summary>
        /// Constructs a new display model.
        /// </summary>
        /// <param name="entity">Entity to follow.</param>
        /// <param name="model">Model to draw on the entity.</param>
        /// <param name="modelDrawer">Model drawer to use.</param>
        public DisplayEntityModelTest(Entity entity, Model model, ModelDrawer modelDrawer)
            : base(modelDrawer)
        {
            OffsetTransform = Matrix.Identity;
            Entity = entity;
            Model = model;
        }

        /// <summary>
        /// Gets or sets the entity to base the model's world matrix on.
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// Gets or sets the model to display.
        /// </summary>
        public Model Model
        {
            get { return myModel; }
            set
            {
                myModel = value;
                transforms = new Matrix[myModel.Bones.Count];
                for (int i = 0; i < Model.Meshes.Count; i++)
                {
                    for (int j = 0; j < Model.Meshes[i].Effects.Count; j++)
                    {
                        var effect = Model.Meshes[i].Effects[j] as BasicEffect;
                        if (effect != null)
                            effect.EnableDefaultLighting();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the texture drawn on this model.
        /// </summary>
        public Texture2D Texture
        {
            get { return myTexture; }
            set
            {
                myTexture = value;
                for (int i = 0; i < Model.Meshes.Count; i++)
                {
                    for (int j = 0; j < Model.Meshes[i].Effects.Count; j++)
                    {
                        var effect = Model.Meshes[i].Effects[j] as BasicEffect;
                        if (effect != null)
                        {
                            if (value != null)
                            {
                                effect.TextureEnabled = true;
                                effect.Texture = Texture;
                            }
                            else
                                effect.TextureEnabled = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the extra world transform of the model.
        /// This transform acts as an offset from the entity's world transform.
        /// </summary>
        public Matrix OffsetTransform { get; set; }

        /// <summary>
        /// Gets the world transformation applied to the model.
        /// </summary>
        public Matrix WorldTransform { get; private set; }

        /// <summary>
        /// Updates the display object.
        /// </summary>
        public override void Update()
        {
            WorldTransform = OffsetTransform * Entity.WorldTransform;
        }

        /// <summary>
        /// Draws the display object.
        /// </summary>
        /// <param name="viewMatrix">Current view matrix.</param>
        /// <param name="projectionMatrix">Current projection matrix.</param>
        public override void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            //This is not a particularly fast method of drawing.
            //It's used very rarely in the demos.
            myModel.CopyAbsoluteBoneTransformsTo(transforms);
            for (int i = 0; i < Model.Meshes.Count; i++)
            {
                for (int j = 0; j < Model.Meshes[i].Effects.Count; j++)
                {
                    var effect = Model.Meshes[i].Effects[j] as BasicEffect;
                    if (effect != null)
                    {
                        effect.World = transforms[Model.Meshes[i].ParentBone.Index] * WorldTransform;
                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
                    }
                }
                Model.Meshes[i].Draw();
            }
        }
    }
}