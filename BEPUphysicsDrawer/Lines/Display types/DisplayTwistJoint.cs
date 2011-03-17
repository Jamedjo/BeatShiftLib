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
    /// Graphical representation of a twist joint
    /// </summary>
    public class DisplayTwistJoint : LineDisplayObject<TwistJoint>
    {
        private readonly Line axisA;
        private readonly Line axisB;


        public DisplayTwistJoint(TwistJoint constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            axisA = new Line(Color.DarkRed, Color.DarkRed, drawer);
            axisB = new Line(Color.DarkRed, Color.DarkRed, drawer);
            myLines.Add(axisA);
            myLines.Add(axisB);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            axisA.PositionA = LineObject.ConnectionA.CenterOfMass;
            axisA.PositionB = LineObject.ConnectionA.CenterOfMass + LineObject.WorldAxisA;

            axisB.PositionA = LineObject.ConnectionB.CenterOfMass;
            axisB.PositionB = LineObject.ConnectionB.CenterOfMass + LineObject.WorldAxisB;
        }
    }
}