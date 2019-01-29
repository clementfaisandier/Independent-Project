﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;



namespace Physics_Simulator {

    public static class LessonSimulationLiason {

        public static int config = 1;
    }

    public sealed partial class SimulationPage : Page {

        private Ellipse[] UIObjects; // Visual Representations of object
        private EngineCircle[] eObjects; // Engine Objects
        private Vector[] vectors;

        private int fps = 60; // Frames per second
        private double g = -5; // Gravity of simulation
        private double gA = -Math.PI / 2; // Direction of Gravity

        private DispatcherTimer timer; // Timer that requests engine to calculate and move objects at every interval

        private Engine simEngine; // Physics Engine

        public SimulationPage() {
            this.InitializeComponent();

            //BuildDebugSim(); // | DEBUG ONLY BUILD|

            BuildLessonSim(); // | LESSON DEMO BUILD |
            
            // Build Engine
            simEngine = new Engine(eObjects, vectors, fps, g, gA);

            // Build Dispatcher
            timer = new DispatcherTimer();
            timer.Tick += Dispatch;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (1000 / fps));
            timer.Start();
        }

        /// <summary>
        /// Runs once for every frame, changes everything that has to be changed during the frame
        /// </summary>
        private void Dispatch(object sender, object e) {

            simEngine.ExecuteNext();
            RefreshDisplay();
        }

        private void RefreshDisplay() {
            EngineCircle[] pos = simEngine.Positions();

            for (int i = 0; i < pos.Length; i++) {
                UIObjects[i].SetValue(Canvas.LeftProperty, (pos[i].GetXPos() - pos[i].GetRadius()) * 10);
                UIObjects[i].SetValue(Canvas.TopProperty, (pos[i].GetYPos() - pos[i].GetRadius()) * 10);
            }
        }

        /// <summary>
        /// Variables are in meters, meters per second, degrees and kilograms
        /// </summary>
        public void BuildEllipse(int index, double xPos, double yPos, double radius, byte a, byte r, byte g, byte b, double magnitude, double angle, double xV, double yV, double mass, double elasticity) {
            UIObjects[index] = new Ellipse();
            UIObjects[index].Height = radius*20;
            UIObjects[index].Width = radius*20;
            UIObjects[index].Fill = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            SimCanvas.Children.Add(UIObjects[index]);
            UIObjects[index].SetValue(Canvas.LeftProperty, (xPos-radius)*10);
            UIObjects[index].SetValue(Canvas.TopProperty, (yPos-radius)*10);
            eObjects[index] = new EngineCircle(xPos, yPos, radius, mass, elasticity);
            vectors[index] = new Vector(magnitude, angle, xV, yV);
        }

        public void BuildLessonSim() {

            int n = 0;

            switch (LessonSimulationLiason.config) {

                case 1:
                    n = 2;
                    UIObjects = new Ellipse[n];
                    eObjects = new EngineCircle[n];
                    vectors = new Vector[n];
                    //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                    BuildEllipse(00, 10, 20, 2, 255, 255, 087, 051, 0, 0, 10, 00, 1, 1.0);
                    BuildEllipse(01, 80, 30, 2, 255, 051, 087, 255, 0, 0,-10, 00, 1, 1.0);
                    g = 0;
                    break;

                case 2:
                    n = 2;
                    UIObjects = new Ellipse[n];
                    eObjects = new EngineCircle[n];
                    vectors = new Vector[n];
                    //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                    BuildEllipse(00, 35, 20, 2, 255, 255, 087, 051, 0, 0, 00, 00, 1, 1.0);
                    BuildEllipse(01, 40, 15, 2, 255, 255, 087, 051, 0, 0, 00, -10, 1, 1.0);
                    g = -5;
                    break;

                case 3:
                    n = 4;
                    UIObjects = new Ellipse[n];
                    eObjects = new EngineCircle[n];
                    vectors = new Vector[n];
                    //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                    BuildEllipse(00, 35, 20, 2, 255, 255, 087, 051, 10, 1, 00, 00, 1, 1.0);
                    BuildEllipse(01, 40, 15, 1, 255, 255, 051, 087, 10, -2, 00, 00, 1, 1.0);
                    BuildEllipse(02, 35, 30, 3, 255, 087, 051, 255, 10, -0.75, 00, 00, 1, 1.0);
                    BuildEllipse(03, 45, 18, 2, 255, 051, 087, 255, 10, -0.3, 00, 00, 1, 1.0);
                    g = -5;
                    break;

                case 4:
                    n = 1;
                    UIObjects = new Ellipse[n];
                    eObjects = new EngineCircle[n];
                    vectors = new Vector[n];
                    //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                    BuildEllipse(00, 35, 20, 2, 255, 255, 087, 051, 0, 0, 10, -10, 1, 1.0);
                    g = -10;
                    break;

                case 5:
                    n = 4;
                    UIObjects = new Ellipse[n];
                    eObjects = new EngineCircle[n];
                    vectors = new Vector[n];
                    //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
                    BuildEllipse(00, 35, 20, 2, 255, 051, 087, 255, 0, 0, 20, 00, 1, 0);
                    BuildEllipse(01, 70, 20, 2, 255, 255, 087, 051, 0, 0,-20, 00, 1, 0);
                    BuildEllipse(02, 35, 30, 2, 255, 051, 087, 255, 0, 0, 20, 00, 1, 0);
                    BuildEllipse(03, 50, 30, 2, 255, 255, 087, 051, 0, 0, 0, 00, 1, 0);
                    g = 0;
                    break;
            }

        }

        public void BuildDebugSim() {

            int numObjects = 11;

            UIObjects = new Ellipse[numObjects];
            eObjects = new EngineCircle[numObjects];
            vectors = new Vector[numObjects];

            // Set up all objects
            //           i   x   y   r  a    r    g    b    v  0  vx  vy  m  e
            BuildEllipse(00, 10, 05, 2, 255, 000, 000, 000, 0, 0, 10, 00, 1, 1.0); // constant
            BuildEllipse(01, 10, 10, 2, 255, 250, 000, 000, 0, 0, 10, 00, 1, 0.0); // 0 elasticity
            BuildEllipse(02, 20, 10, 2, 255, 250, 000, 000, 0, 0, 00, 00, 1, 0.0);
            BuildEllipse(03, 10, 20, 2, 255, 000, 250, 000, 0, 0, 10, 00, 1, 0.5); // 0.5 elasticity
            BuildEllipse(04, 20, 20, 2, 255, 000, 250, 000, 0, 0, 00, 00, 1, 0.5);
            BuildEllipse(05, 10, 30, 2, 255, 000, 000, 250, 0, 0, 10, 00, 1, 1.0); // 1 elasticity
            BuildEllipse(06, 20, 30, 2, 255, 000, 000, 250, 0, 0, 00, 00, 1, 1.0);
            BuildEllipse(07, 10, 40, 2, 255, 000, 250, 250, 0, 0, 10, 00, 1, 2.0); // 2 elasticity
            BuildEllipse(08, 20, 40, 2, 255, 000, 250, 250, 0, 0, 00, 00, 1, 2.0);
            BuildEllipse(09, 10, 50, 2, 255, 250, 250, 000, 0, 0, 10, 10, 1, 1.0); // 1 elasticity diagonal
            BuildEllipse(10, 20, 60, 2, 255, 250, 250, 000, 0, 0, 00, 00, 1, 1.0);
            // | CHANGE SIZE OF ARRAY WHEN ADDING OR REMOVING OBJECT |
        }
    }
}
