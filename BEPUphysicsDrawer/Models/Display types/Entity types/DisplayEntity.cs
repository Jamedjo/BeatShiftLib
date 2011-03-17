using BEPUphysics.Entities;

namespace BEPUphysicsDrawer.Models
{
    /// <summary>
    /// Superclass of display objects that follow entities.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public abstract class DisplayEntity<T> : ModelDisplayObject<T> where T : Entity
    {
        /// <summary>
        /// Constructs a new 
        /// </summary>
        /// <param name="drawer">Drawer to use.</param>
        /// <param name="entity">Entity to draw.</param>
        protected DisplayEntity(ModelDrawer drawer, T entity)
            : base(drawer, entity)
        {
        }


        public override void Update()
        {
            WorldTransform = DisplayedObject.WorldTransform;
        }
    }
}