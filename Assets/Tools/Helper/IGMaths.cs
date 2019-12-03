﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace InviGiant.Tools
{
    /// <summary>
    /// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
    /// </summary>

    public static class IGMaths
    {
        /// <summary>
        /// Takes a Vector3 and turns it into a Vector2
        /// </summary>
        /// <returns>The vector2.</returns>
        /// <param name="target">The Vector3 to turn into a Vector2.</param>
        public static Vector2 Vector3ToVector2(Vector3 target)
        {
            return new Vector2(target.x, target.y);
        }

        /// <summary>
        /// Takes a Vector2 and turns it into a Vector3 with a null z value
        /// </summary>
        /// <returns>The vector3.</returns>
        /// <param name="target">The Vector2 to turn into a Vector3.</param>
        public static Vector3 Vector2ToVector3(Vector2 target)
        {
            return new Vector3(target.x, target.y, 0);
        }

        /// <summary>
        /// Takes a Vector2 and turns it into a Vector3 with the specified z value 
        /// </summary>
        /// <returns>The vector3.</returns>
        /// <param name="target">The Vector2 to turn into a Vector3.</param>
        /// <param name="newZValue">New Z value.</param>
        public static Vector3 Vector2ToVector3(Vector2 target, float newZValue)
        {
            return new Vector3(target.x, target.y, newZValue);
        }

        /// <summary>
        /// Rounds all components of a Vector3.
        /// </summary>
        /// <returns>The vector3.</returns>
        /// <param name="vector">Vector.</param>
        public static Vector3 RoundVector3(Vector3 vector)
        {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }

        /// <summary>
        /// Returns a random vector3 from 2 defined vector3.
        /// </summary>
        /// <returns>The vector3.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Maximum.</param>
        public static Vector3 RandomVector3(Vector3 minimum, Vector3 maximum)
        {
            return new Vector3(UnityEngine.Random.Range(minimum.x, maximum.x),
                                             UnityEngine.Random.Range(minimum.y, maximum.y),
                                             UnityEngine.Random.Range(minimum.z, maximum.z));
        }

        /// <summary>
        /// Rotates a point around the given pivot.
        /// </summary>
        /// <returns>The new point position.</returns>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot's position.</param>
        /// <param name="angle">The angle we want to rotate our point.</param>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
        {
            angle = angle * (Mathf.PI / 180f);
            var rotatedX = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
            var rotatedY = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
            return new Vector3(rotatedX, rotatedY, 0);
        }

        /// <summary>
        /// Rotates a point around the given pivot.
        /// </summary>
        /// <returns>The new point position.</returns>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot's position.</param>
        /// <param name="angles">The angle as a Vector3.</param>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
        {
            // we get point direction from the point to the pivot
            Vector3 direction = point - pivot;
            // we rotate the direction
            direction = Quaternion.Euler(angle) * direction;
            // we determine the rotated point's position
            point = direction + pivot;
            return point;
        }

        /// <summary>
        /// Rotates a point around the given pivot.
        /// </summary>
        /// <returns>The new point position.</returns>
        /// <param name="point">The point to rotate.</param>
        /// <param name="pivot">The pivot's position.</param>
        /// <param name="angles">The angle as a Vector3.</param>
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion quaternion)
        {
            // we get point direction from the point to the pivot
            Vector3 direction = point - pivot;
            // we rotate the direction
            direction = quaternion * direction;
            // we determine the rotated point's position
            point = direction + pivot;
            return point;
        }

        /// <summary>
        /// Computes and returns the angle between two vectors, on a 360° scale
        /// </summary>
        /// <returns>The <see cref="System.Single"/>.</returns>
        /// <param name="vectorA">Vector a.</param>
        /// <param name="vectorB">Vector b.</param>
        public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
        {
            float angle = Vector2.Angle(vectorA, vectorB);
            Vector3 cross = Vector3.Cross(vectorA, vectorB);

            if (cross.z > 0)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        /// <summary>
        /// Returns the sum of all the int passed in parameters
        /// </summary>
        /// <param name="thingsToAdd">Things to add.</param>
        public static int Sum(params int[] thingsToAdd)
        {
            int result = 0;
            for (int i = 0; i < thingsToAdd.Length; i++)
            {
                result += thingsToAdd[i];
            }
            return result;
        }

        /// <summary>
        /// Returns the result of rolling a dice of the specified number of sides
        /// </summary>
        /// <returns>The result of the dice roll.</returns>
        /// <param name="numberOfSides">Number of sides of the dice.</param>
        public static int RollADice(int numberOfSides)
        {
            return (UnityEngine.Random.Range(1, numberOfSides));
        }

        /// <summary>
        /// Returns a random success based on X% of chance.
        /// Example : I have 20% of chance to do X, Chance(20) > true, yay!
        /// </summary>
        /// <param name="percent">Percent of chance.</param>
        public static bool Chance(int percent)
        {
            return (UnityEngine.Random.Range(0, 100) <= percent);
        }

        /// <summary>
        /// Moves from "from" to "to" by the specified amount and returns the corresponding value
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="amount">Amount.</param>
        public static float Approach(float from, float to, float amount)
        {
            if (from < to)
            {
                from += amount;
                if (from > to)
                {
                    return to;
                }
            }
            else
            {
                from -= amount;
                if (from < to)
                {
                    return to;
                }
            }
            return from;
        }


        /// <summary>
        /// Remaps a value x in interval [A,B], to the proportional value in interval [C,D]
        /// </summary>
        /// <param name="x">The value to remap.</param>
        /// <param name="A">the minimum bound of interval [A,B] that contains the x value</param>
        /// <param name="B">the maximum bound of interval [A,B] that contains the x value</param>
        /// <param name="C">the minimum bound of target interval [C,D]</param>
        /// <param name="D">the maximum bound of target interval [C,D]</param>
        public static float Remap(float x, float A, float B, float C, float D)
        {
            float remappedValue = C + (x - A) / (B - A) * (D - C);
            return remappedValue;
        }

        public static float RoundToClosest(float value, float[] possibleValues)
        {
            if (possibleValues.Length == 0)
            {
                return 0f;
            }

            float closestValue = possibleValues[0];

            foreach (float possibleValue in possibleValues)
            {
                if (Mathf.Abs(closestValue - value) > Mathf.Abs(possibleValue - value))
                {
                    closestValue = possibleValue;
                }
            }
            return closestValue;

        }

        /// <summary>
        /// return Angle of vector2d in unity
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static float CalculateAngle(Vector2 velocity)
        {
            float _angleTemp = 0;
            if (velocity.x > 0 && velocity.y > 0)
            {
                float temp = Mathf.Atan(velocity.x / velocity.y);
                _angleTemp = -temp;
            }
            if (velocity.x > 0 && velocity.y == 0)
            {
                _angleTemp = 1.5f * Mathf.PI;
            }
            if (velocity.x < 0 && velocity.y > 0)
            {
                _angleTemp = Mathf.Atan(-velocity.x / velocity.y);

            }
            if (velocity.x < 0 && velocity.y == 0)
            {
                _angleTemp = 0.5f * Mathf.PI;
            }
            if (velocity.x < 0 && velocity.y < 0)
            {
                _angleTemp = Mathf.Atan(velocity.y / velocity.x);
                _angleTemp += Mathf.PI / 2;
            }
            if (velocity.x == 0 && velocity.y < 0)
            {
                _angleTemp = Mathf.PI;
            }
            if (velocity.x > 0 && velocity.y < 0)
            {
                float temp = Mathf.Atan(-velocity.y / velocity.x);
                _angleTemp = -Mathf.PI / 2 - temp;
            }
            if (velocity.x == 0 && velocity.y > 0)
            {
                _angleTemp = 0f;
            }

            return _angleTemp * 180 / Mathf.PI;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (System.Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        public static void Shuffle<T>(this Stack<T> stack)
        {
            var values = stack.ToArray();
            stack.Clear();
            for (int i = 0; i < values.Length; i++)
                stack.Push(values[i]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static List<T> GetRandomNoDuplicate<T>(this List<T> List, int Number)
        {
            var listTmp = List.Clone();
            List<T> lt = new List<T>();
            for (int i = 0; i < Number; i++)
            {
                bool cont = true;
                while (cont)
                {
                    int ran = UnityEngine.Random.Range(0, listTmp.Count);
                    if (!lt.Contains(listTmp[ran]))
                    {
                        cont = false;
                        lt.Add(listTmp[ran]);
                        listTmp.RemoveAt(ran);
                    }
                }
            }
            return lt;
        }

        public static List<float> SolveCubicFunction(float a, float b, float c, float d)
        {
            List<float> tmp = new List<float>();
            float delta, k, x1, x2, x3, x0, x, X;
            delta = (float)Mathf.Pow(b, 2) - 3 * a * c;
            k = (float)(9 * a * b * c - 2 * Mathf.Pow(b, 3) - 27 * Mathf.Pow(a, 2) * d) / (2 * Mathf.Sqrt(Mathf.Abs(Mathf.Pow(delta, 3))));

            if (delta > 0)
            {
                if (Mathf.Abs(k) <= 1)
                {
                    x1 = (2 * Mathf.Sqrt(delta) * Mathf.Cos((Mathf.Acos(k) / 3)) - b) / (3 * a);
                    x2 = (2 * Mathf.Sqrt(delta) * Mathf.Cos((Mathf.Acos(k) / 3 - (2 * Mathf.PI / 3))) - b) / (3 * a);
                    x3 = (2 * Mathf.Sqrt(delta) * Mathf.Cos((Mathf.Acos(k) / 3 + (2 * Mathf.PI / 3))) - b) / (3 * a);
                    tmp.Add(x1);
                    tmp.Add(x2);
                    tmp.Add(x3);
                }
                else
                {
                    x0 = ((Mathf.Sqrt(delta) * Mathf.Abs(k)) / (3 * a * k)) * (Mathf.Pow(Mathf.Abs(k) + Mathf.Sqrt(Mathf.Pow(k, 2) - 1), 1 / 3) + Mathf.Pow(Mathf.Abs(k) - Mathf.Sqrt(Mathf.Pow(k, 2) - 1), 1 / 3)) - (b / (3 * a));
                    tmp.Add(x0);
                }
            }


            if (delta == 0)
            {
                X = (-b + Mathf.Pow(Mathf.Pow(b, 3) - 27 * Mathf.Pow(a, 2) * d, 1 / 3)) / (3 * a);
                tmp.Add(X);
            }
            if (delta < 0)
            {
                x = (Mathf.Sqrt(Mathf.Abs(delta)) / (3 * a)) * (Mathf.Pow(k + Mathf.Sqrt(Mathf.Pow(k, 2) + 1), 1 / 3) + Mathf.Pow(k - Mathf.Sqrt(Mathf.Pow(k, 2) + 1), 1 / 3)) - (b / (3 * a));
                tmp.Add(x);
            }
            return tmp;
        }
    }
}