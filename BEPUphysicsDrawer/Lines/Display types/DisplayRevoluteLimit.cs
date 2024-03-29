﻿/*
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
    /// Graphical representation of a ball socket joint.
    /// </summary>
    public class DisplayRevoluteLimit : LineDisplayObject<RevoluteLimit>
    {
        private readonly Line bottom;
        private readonly Line bottomLeft;
        private readonly Line bottomRight;
        private readonly Line middle;
        private readonly Line testAxis;
        private readonly Line top;
        private readonly Line topLeft;
        private readonly Line topRight;

        public DisplayRevoluteLimit(RevoluteLimit constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            topRight = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            top = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            topLeft = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            bottomRight = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            bottom = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            bottomLeft = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            middle = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            testAxis = new Line(Color.DarkRed, Color.DarkRed, drawer);

            myLines.Add(topRight);
            myLines.Add(top);
            myLines.Add(topLeft);
            myLines.Add(bottomRight);
            myLines.Add(bottom);
            myLines.Add(bottomLeft);
            myLines.Add(middle);
            myLines.Add(testAxis);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            Vector3 left = LineObject.ConnectionB.CenterPosition - LineObject.Basis.PrimaryAxis;
            Vector3 right = LineObject.ConnectionB.CenterPosition + LineObject.Basis.PrimaryAxis;

            Vector3 upwardsOffset = Vector3.TransformNormal(LineObject.Basis.XAxis, Matrix.CreateFromAxisAngle(LineObject.Basis.PrimaryAxis, LineObject.MaximumAngle));
            Vector3 topRightPosition = right + upwardsOffset;
            Vector3 topLeftPosition = left + upwardsOffset;

            Vector3 downwardsOffset = Vector3.TransformNormal(LineObject.Basis.XAxis, Matrix.CreateFromAxisAngle(LineObject.Basis.PrimaryAxis, LineObject.MinimumAngle));
            Vector3 bottomRightPosition = right + downwardsOffset;
            Vector3 bottomLeftPosition = left + downwardsOffset;

            middle.PositionA = left;
            middle.PositionB = right;

            topLeft.PositionA = left;
            topLeft.PositionB = topLeftPosition;

            topRight.PositionA = right;
            topRight.PositionB = topRightPosition;

            top.PositionA = topLeftPosition;
            top.PositionB = topRightPosition;

            bottomLeft.PositionA = left;
            bottomLeft.PositionB = bottomLeftPosition;

            bottomRight.PositionA = right;
            bottomRight.PositionB = bottomRightPosition;

            bottom.PositionA = bottomLeftPosition;
            bottom.PositionB = bottomRightPosition;

            testAxis.PositionA = LineObject.ConnectionB.CenterPosition;
            testAxis.PositionB = LineObject.ConnectionB.CenterPosition + LineObject.TestAxis;
        }
    }
}