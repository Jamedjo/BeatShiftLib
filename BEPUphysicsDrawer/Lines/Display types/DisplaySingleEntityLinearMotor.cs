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
    /// Graphical representation of a single entity linear motor.
    /// </summary>
    public class DisplaySingleEntityLinearMotor : LineDisplayObject<SingleEntityLinearMotor>
    {
        private readonly Line error;
        private readonly Line toPoint;

        public DisplaySingleEntityLinearMotor(SingleEntityLinearMotor constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            toPoint = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            error = new Line(Color.Red, Color.Red, drawer);
            myLines.Add(toPoint);
            myLines.Add(error);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            if (LineObject.IsActive)
            {
                toPoint.PositionA = LineObject.Entity.CenterOfMass;
                toPoint.PositionB = LineObject.Point;

                if (LineObject.Settings.Mode == MotorMode.Servomechanism)
                {
                    error.PositionA = toPoint.PositionB;
                    error.PositionB = LineObject.Settings.Servo.Goal;
                }
                else
                {
                    error.PositionA = toPoint.PositionB;
                    error.PositionB = toPoint.PositionB;
                }
            }
            else
            {
                error.PositionA = toPoint.PositionB;
                error.PositionB = toPoint.PositionB;
            }
        }
    }
}