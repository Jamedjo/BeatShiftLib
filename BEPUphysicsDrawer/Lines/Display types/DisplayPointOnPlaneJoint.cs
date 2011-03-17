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
    public class DisplayPointOnPlaneJoint : LineDisplayObject<PointOnPlaneJoint>
    {
        private readonly Line aToConnection;
        private readonly Line bToConnection;
        private readonly Line error;
        private readonly Line gridColumn1;
        private readonly Line gridColumn2;
        private readonly Line gridColumn3;
        private readonly Line gridRow1;
        private readonly Line gridRow2;
        private readonly Line gridRow3;

        public DisplayPointOnPlaneJoint(PointOnPlaneJoint constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            gridRow1 = new Line(Color.Gray, Color.Gray, drawer);
            gridRow2 = new Line(Color.Gray, Color.Gray, drawer);
            gridRow3 = new Line(Color.Gray, Color.Gray, drawer);

            gridColumn1 = new Line(Color.Gray, Color.Gray, drawer);
            gridColumn2 = new Line(Color.Gray, Color.Gray, drawer);
            gridColumn3 = new Line(Color.Gray, Color.Gray, drawer);

            aToConnection = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            bToConnection = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            error = new Line(Color.Red, Color.Red, drawer);

            myLines.Add(gridRow1);
            myLines.Add(gridRow2);
            myLines.Add(gridRow3);
            myLines.Add(gridColumn1);
            myLines.Add(gridColumn2);
            myLines.Add(gridColumn3);
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
            PointOnPlaneJoint constraint = LineObject;
            Vector3 planeAnchor = constraint.PlaneAnchor;
            Vector3 y = Vector3.Cross(constraint.ConnectionA.OrientationMatrix.Up, constraint.PlaneNormal);
            if (y.LengthSquared() < .001f)
            {
                y = Vector3.Cross(constraint.ConnectionA.OrientationMatrix.Right, constraint.PlaneNormal);
            }
            Vector3 x = Vector3.Cross(constraint.PlaneNormal, y);

            //Grid
            gridRow1.PositionA = planeAnchor - 1.5f * x + y;
            gridRow1.PositionB = planeAnchor + 1.5f * x + y;

            gridRow2.PositionA = planeAnchor - 1.5f * x;
            gridRow2.PositionB = planeAnchor + 1.5f * x;

            gridRow3.PositionA = planeAnchor - 1.5f * x - y;
            gridRow3.PositionB = planeAnchor + 1.5f * x - y;

            gridColumn1.PositionA = planeAnchor + x - 1.5f * y;
            gridColumn1.PositionB = planeAnchor + x + 1.5f * y;

            gridColumn2.PositionA = planeAnchor - 1.5f * y;
            gridColumn2.PositionB = planeAnchor + 1.5f * y;

            gridColumn3.PositionA = planeAnchor - x - 1.5f * y;
            gridColumn3.PositionB = planeAnchor - x + 1.5f * y;

            //Connection and error
            aToConnection.PositionA = constraint.ConnectionA.CenterOfMass;
            aToConnection.PositionB = constraint.PlaneAnchor;

            bToConnection.PositionA = constraint.ConnectionB.CenterOfMass;
            bToConnection.PositionB = constraint.PointAnchor;

            error.PositionA = aToConnection.PositionB;
            error.PositionB = bToConnection.PositionB;
        }
    }
}