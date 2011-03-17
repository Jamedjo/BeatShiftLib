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


using BEPUphysics.Constraints;
using Microsoft.Xna.Framework;

namespace BEPUphysicsDrawer.Lines
{
    /// <summary>
    /// Graphical representation of a PointOnPlaneConstraint
    /// </summary>
    public class DisplayDistanceJoint : LineDisplayObject<DistanceJoint>
    {
        private readonly Line aToConnection;
        private readonly Line bToConnection;
        private readonly Line error;

        public DisplayDistanceJoint(DistanceJoint constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            aToConnection = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            bToConnection = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            error = new Line(Color.Red, Color.Red, drawer);
            myLines.Add(aToConnection);
            myLines.Add(bToConnection);
            myLines.Add(error);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            aToConnection.PositionA = LineObject.ConnectionA.CenterOfMass;
            aToConnection.PositionB = LineObject.WorldAnchorA;

            bToConnection.PositionA = LineObject.ConnectionB.CenterOfMass;
            bToConnection.PositionB = LineObject.WorldAnchorB;

            error.PositionA = aToConnection.PositionB;
            error.PositionB = bToConnection.PositionB;
        }
    }
}