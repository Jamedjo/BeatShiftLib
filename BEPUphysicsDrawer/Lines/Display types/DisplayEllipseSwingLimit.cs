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


using System;
using BEPUphysics.Constraints;
using Microsoft.Xna.Framework;

namespace BEPUphysicsDrawer.Lines
{
    /// <summary>
    /// Graphical representation of a twist joint
    /// </summary>
    public class DisplayEllipseSwingLimit : LineDisplayObject<EllipseSwingLimit>
    {
        /// <summary>
        /// Number of facets to use when representing the swing limit boundary.
        /// </summary>
        private static int limitFacetCount = 32;

        private readonly Line axis;
        private readonly Line[] limitLines;


        public DisplayEllipseSwingLimit(EllipseSwingLimit constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            axis = new Line(Color.Red, Color.Red, drawer);
            myLines.Add(axis);
            //Create the lines that represent the outline of the limit.
            limitLines = new Line[limitFacetCount * 2];
            for (int i = 0; i < limitFacetCount; i++)
            {
                limitLines[i * 2] = new Line(Color.DarkRed, Color.DarkRed, drawer);
                limitLines[i * 2 + 1] = new Line(Color.DarkRed, Color.DarkRed, drawer);
            }
            myLines.AddRange(limitLines);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            axis.PositionA = LineObject.ConnectionB.CenterOfMass;
            axis.PositionB = LineObject.ConnectionB.CenterOfMass + LineObject.TwistAxisB * 1.5f;


            float angleIncrement = 4 * MathHelper.Pi / limitLines.Length; //Each loop iteration moves this many radians forward.
            for (int i = 0; i < limitLines.Length / 2; i++)
            {
                Line pointToPreviousPoint = limitLines[2 * i];
                Line centerToPoint = limitLines[2 * i + 1];

                float currentAngle = i * angleIncrement;

                //Using the parametric equation for an ellipse, compute the axis of rotation and angle.
                Vector3 rotationAxis = LineObject.Basis.XAxis * LineObject.MaximumAngleX * (float) Math.Cos(currentAngle) +
                                       LineObject.Basis.YAxis * LineObject.MaximumAngleY * (float) Math.Sin(currentAngle);
                float angle = rotationAxis.Length();
                rotationAxis /= angle;

                pointToPreviousPoint.PositionA = LineObject.ConnectionB.CenterOfMass +
                                                 //Rotate the primary axis to the ellipse boundary...
                                                 Vector3.TransformNormal(LineObject.Basis.PrimaryAxis, Matrix.CreateFromAxisAngle(rotationAxis, angle));

                centerToPoint.PositionA = pointToPreviousPoint.PositionA;
                centerToPoint.PositionB = LineObject.ConnectionB.CenterOfMass;
            }
            for (int i = 0; i < limitLines.Length / 2; i++)
            {
                //Connect all the pointToPreviousPoint lines to the previous points.
                limitLines[2 * i].PositionB = limitLines[2 * ((i + 1) % (limitLines.Length / 2))].PositionA;
            }
        }
    }
}