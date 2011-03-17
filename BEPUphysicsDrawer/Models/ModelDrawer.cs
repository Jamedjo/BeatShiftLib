using System;
using System.Collections.Generic;
using BEPUphysics;
using BEPUphysics.DataStructures;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Manages and draws models.
    /// </summary>
    public abstract class ModelDrawer
    {
        private readonly Dictionary<object, ModelDisplayObjectBase> displayObjects = new Dictionary<object, ModelDisplayObjectBase>();
        private readonly RasterizerState fillState;

        private readonly Dictionary<Type, Type> myDisplayTypes = new Dictionary<Type, Type>();
        private readonly List<SelfDrawingModelDisplayObject> selfDrawingDisplayObjects = new List<SelfDrawingModelDisplayObject>();
        private readonly RasterizerState wireframeState;
        protected Texture2D[] textures = new Texture2D[8];

        protected ModelDrawer(Game game)
        {
            Game = game;
            textures[0] = game.Content.Load<Texture2D>("Textures//red");
            textures[1] = game.Content.Load<Texture2D>("Textures//yellow");
            textures[2] = game.Content.Load<Texture2D>("Textures//purple");
            textures[3] = game.Content.Load<Texture2D>("Textures//orange");
            textures[4] = game.Content.Load<Texture2D>("Textures//green");
            textures[5] = game.Content.Load<Texture2D>("Textures//lightblue");
            textures[6] = game.Content.Load<Texture2D>("Textures//prorange");
            textures[7] = game.Content.Load<Texture2D>("Textures//teal");
            DisplayTypes.Add(typeof (Box), typeof (DisplayBoxTest));
            DisplayTypes.Add(typeof (Sphere), typeof (DisplaySphereTest));
            DisplayTypes.Add(typeof (Capsule), typeof (DisplayCapsuleTest));
            DisplayTypes.Add(typeof (Cylinder), typeof (DisplayCylinderTest));
            DisplayTypes.Add(typeof (Cone), typeof (DisplayConeTest));
            DisplayTypes.Add(typeof (Triangle), typeof (DisplayTriangleTest));
            DisplayTypes.Add(typeof (ConvexHull), typeof (DisplayConvexHullTest));
            DisplayTypes.Add(typeof (MinkowskiSum), typeof (DisplayMinkowskiSumTest));
            DisplayTypes.Add(typeof (WrappedBody), typeof (DisplayWrappedBodyTest));
            DisplayTypes.Add(typeof (CompoundBody), typeof (DisplayCompoundBodyTest));
            DisplayTypes.Add(typeof (FluidVolume), typeof (DisplayFluidTest));
            DisplayTypes.Add(typeof (Terrain), typeof (DisplayTerrainTest));
            DisplayTypes.Add(typeof (TriangleMesh), typeof (DisplayTriangleMeshTest));

            fillState = new RasterizerState();
            wireframeState = new RasterizerState();
            wireframeState.FillMode = FillMode.WireFrame;
        }

        /// <summary>
        /// Gets the map from object types to display object types.
        /// </summary>
        public Dictionary<Type, Type> DisplayTypes
        {
            get { return myDisplayTypes; }
        }

        /// <summary>
        /// Gets the game using this ModelDrawer.
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// Gets or sets whether or not the model drawer is drawing wireframes.
        /// </summary>
        public bool IsWireframe { get; set; }

        /// <summary>
        /// Constructs a new display object for an object.
        /// </summary>
        /// <param name="objectToDisplay">Object to create a display object for.</param>
        /// <returns>Display object for an object.</returns>
        internal ModelDisplayObjectBase GetDisplayObject(object objectToDisplay)
        {
            Type displayType;
            if (!displayObjects.ContainsKey(objectToDisplay) &&
                myDisplayTypes.TryGetValue(objectToDisplay.GetType(), out displayType))
            {
#if !WINDOWS
                return (ModelDisplayObjectBase)displayType.GetConstructor(
                                                     new Type[] { typeof(ModelDrawer), objectToDisplay.GetType() })
                                                     .Invoke(new object[] { this, objectToDisplay });
#else
                return (ModelDisplayObjectBase) Activator.CreateInstance(displayType, new[] {this, objectToDisplay});
#endif
            }
            return null;
        }

        /// <summary>
        /// Attempts to add an object to the ModelDrawer.
        /// </summary>
        /// <param name="objectToDisplay">Object to be added to the model drawer.</param>
        /// <returns>ModelDisplayObject created for the object.  Null if it couldn't be added.</returns>
        public ModelDisplayObjectBase Add(object objectToDisplay)
        {
            ModelDisplayObjectBase displayObject = GetDisplayObject(objectToDisplay);
            if (displayObject != null)
            {
                Add(displayObject);
                displayObjects.Add(objectToDisplay, displayObject);
                return displayObject;
            }
            return null; //Couldn't add it.
        }

        /// <summary>
        /// Adds the display object to the drawer.
        /// </summary>
        /// <param name="displayObject">Display object to add.</param>
        /// <returns>Whether or not the display object was added.</returns>
        public bool Add(SelfDrawingModelDisplayObject displayObject)
        {
            if (!selfDrawingDisplayObjects.Contains(displayObject))
            {
                selfDrawingDisplayObjects.Add(displayObject);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a display object to the drawer.
        /// </summary>
        /// <param name="displayObject">Display object to add.</param>
        protected abstract void Add(ModelDisplayObjectBase displayObject);

        /// <summary>
        /// Removes an object from the drawer.
        /// </summary>
        /// <param name="objectToRemove">Object to remove.</param>
        public void Remove(object objectToRemove)
        {
            Remove(displayObjects[objectToRemove]);
        }

        /// <summary>
        /// Removes an object from the drawer.
        /// </summary>
        /// <param name="displayObject">Display object to remove.</param>
        public void Remove(SelfDrawingModelDisplayObject displayObject)
        {
            selfDrawingDisplayObjects.Remove(displayObject);
        }


        /// <summary>
        /// Removes a display object from the drawer.
        /// </summary>
        /// <param name="displayObject">Object to remove.</param>
        protected abstract void Remove(ModelDisplayObjectBase displayObject);

        /// <summary>
        /// Cleans out the model drawer of any existing display objects.
        /// </summary>
        public void Clear()
        {
            displayObjects.Clear();
            selfDrawingDisplayObjects.Clear();
            ClearManagedModels();
        }

        /// <summary>
        /// Cleans out any data contained by derived drawers.
        /// </summary>
        protected abstract void ClearManagedModels();

        /// <summary>
        /// Determines if the object has an associated display object in this drawer.
        /// </summary>
        /// <param name="displayedObject">Object to check for in the drawer.</param>
        /// <returns>Whether or not the object has an associated display object in this drawer.</returns>
        public bool Contains(object displayedObject)
        {
            return displayObjects.ContainsKey(displayedObject);
        }

        /// <summary>
        /// Updates the drawer and its components.
        /// </summary>
        public void Update()
        {
            foreach (SelfDrawingModelDisplayObject displayObject in selfDrawingDisplayObjects)
                displayObject.Update();
            UpdateManagedModels();
        }

        /// <summary>
        /// Updates the drawer's technique.
        /// </summary>
        protected abstract void UpdateManagedModels();

        /// <summary>
        /// Draws the drawer's models.
        /// </summary>
        /// <param name="viewMatrix">View matrix to use to draw the objects.</param>
        /// <param name="projectionMatrix">Projection matrix to use to draw the objects.</param>
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Game.GraphicsDevice.RasterizerState = IsWireframe ? wireframeState : fillState;

            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (SelfDrawingModelDisplayObject displayObject in selfDrawingDisplayObjects)
                displayObject.Draw(viewMatrix, projectionMatrix);
            DrawManagedModels(viewMatrix, projectionMatrix);
        }

        /// <summary>
        /// Draws the models managed by the drawer using the appropriate technique.
        /// </summary>
        /// <param name="viewMatrix">View matrix to use to draw the objects.</param>
        /// <param name="projectionMatrix">Projection matrix to use to draw the objects.</param>
        protected abstract void DrawManagedModels(Matrix viewMatrix, Matrix projectionMatrix);
    }
}