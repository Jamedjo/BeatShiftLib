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
using System.Collections.ObjectModel;

namespace BEPUphysicsDrawer.Lines
{
    /// <summary>
    /// Base class of LineDisplayObjects.
    /// </summary>
    public abstract class LineDisplayObjectBase
    {
        private readonly ReadOnlyCollection<Line> myLinesReadOnly;
        private bool myIsActive = true;

        protected List<Line> myLines = new List<Line>();

        protected LineDisplayObjectBase(LineDrawer drawer)
        {
            Drawer = drawer;
            myLinesReadOnly = new ReadOnlyCollection<Line>(myLines);
        }

        /// <summary>
        /// Gets the drawer that this display object belongs to.
        /// </summary>
        public LineDrawer Drawer { get; private set; }

        /// <summary>
        /// Gets or sets whether or not this display object is active.
        /// </summary>
        public bool IsActive
        {
            get { return myIsActive; }
            set
            {
                if (myIsActive && !value)
                    Deactivate();
                else if (!myIsActive && value)
                    Activate();
                myIsActive = value;
            }
        }

        /// <summary>
        /// Gets the list of lines used by this display object.
        /// </summary>
        public ReadOnlyCollection<Line> Lines
        {
            get { return myLinesReadOnly; }
        }

        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public abstract void Update();


        /// <summary>
        /// Removes the lines used by the display constraint from its line drawer.
        /// </summary>
        public void RemoveLines()
        {
            foreach (Line line in myLines)
            {
                Drawer.RemoveLine(line);
            }
        }

        /// <summary>
        /// Makes lines associated with this object invisible, but does not remove them.
        /// </summary>
        private void Deactivate()
        {
            foreach (Line line in myLines)
            {
                Drawer.Deactivate(line);
            }
        }


        /// <summary>
        /// Makes lines associated with this object visible.
        /// </summary>
        private void Activate()
        {
            foreach (Line line in myLines)
            {
                Drawer.Activate(line);
            }
        }
    }
}