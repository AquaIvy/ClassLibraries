﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Aquaivy.Unity
{
    /// <summary>
    /// Transform类的扩展方法
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// 返回一个物体的完整层级路径
        /// </summary>
        /// <param name="transform">The transform you wish a full path to.</param>
        /// <param name="delimiter">The delimiter with which each object is delimited in the string.</param>
        /// <param name="prefix">Prefix with which the full path to the object should start.</param>
        /// <returns>A delimited string that is the full path to the game object in the hierarchy.</returns>
        public static string GetFullPath(this Transform transform, string delimiter = "/", string prefix = "/")
        {
            StringBuilder stringBuilder = new StringBuilder();
            GetFullPath(stringBuilder, transform, delimiter, prefix);
            return stringBuilder.ToString();
        }

        private static void GetFullPath(StringBuilder stringBuilder, Transform transform, string delimiter, string prefix)
        {
            if (transform.parent == null)
            {
                stringBuilder.Append(prefix);
            }
            else
            {
                GetFullPath(stringBuilder, transform.parent, delimiter, prefix);
                stringBuilder.Append(delimiter);
            }
            stringBuilder.Append(transform.name);
        }

        /// <summary>
        /// Enumerates all children in the hierarchy starting at the root object.
        /// </summary>
        /// <param name="root">Start point of the traversion set</param>
        public static IEnumerable<Transform> EnumerateHierarchy(this Transform root)
        {
            if (root == null) { throw new ArgumentNullException("root"); }
            return root.EnumerateHierarchyCore(new List<Transform>(0));
        }

        /// <summary>
        /// Enumerates all children in the hierarchy starting at the root object except for the branches in ignore.
        /// </summary>
        /// <param name="root">Start point of the traversion set</param>
        /// <param name="ignore">Transforms and all its children to be ignored</param>
        public static IEnumerable<Transform> EnumerateHierarchy(this Transform root, ICollection<Transform> ignore)
        {
            if (root == null) { throw new ArgumentNullException("root"); }
            if (ignore == null)
            {
                throw new ArgumentNullException("ignore", "Ignore collection can't be null, use EnumerateHierarchy(root) instead.");
            }
            return root.EnumerateHierarchyCore(ignore);
        }

        /// <summary>
        /// Enumerates all children in the hierarchy starting at the root object except for the branches in ignore.
        /// </summary>
        /// <param name="root">Start point of the traversion set</param>
        /// <param name="ignore">Transforms and all its children to be ignored</param>
        private static IEnumerable<Transform> EnumerateHierarchyCore(this Transform root, ICollection<Transform> ignore)
        {
            var transformQueue = new Queue<Transform>();
            transformQueue.Enqueue(root);

            while (transformQueue.Count > 0)
            {
                var parentTransform = transformQueue.Dequeue();

                if (!parentTransform || ignore.Contains(parentTransform)) { continue; }

                for (var i = 0; i < parentTransform.childCount; i++)
                {
                    transformQueue.Enqueue(parentTransform.GetChild(i));
                }

                yield return parentTransform;
            }
        }

        /// <summary>
        /// Calculates the bounds of all the colliders attached to this GameObject and all it's children
        /// </summary>
        /// <param name="transform">Transform of root GameObject the colliders are attached to </param>
        /// <returns>The total bounds of all colliders attached to this GameObject. 
        /// If no colliders attached, returns a bounds of center and extents 0</returns>
        public static Bounds GetColliderBounds(this Transform transform)
        {
            Collider[] colliders = transform.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0) { return new Bounds(); }

            Bounds bounds = colliders[0].bounds;
            for (int i = 1; i < colliders.Length; i++)
            {
                bounds.Encapsulate(colliders[i].bounds);
            }
            return bounds;
        }

        /// <summary>
        /// 检查2个Transform是否为父子关系（即使爷/子也为true）
        /// </summary>
        /// <returns>如果2个Transform相同，返回false</returns>
        public static bool IsParentOrChildOf(this Transform transform1, Transform transform2)
        {
            return (transform1.IsChildOf(transform2) || transform2.IsChildOf(transform1))
                && (transform1 != transform2);
        }

        public static void SetX(this Transform transform, float x)
        {
            var p = transform.localPosition;
            transform.localPosition = new Vector3(x, p.y, p.z);
        }

        public static void SetY(this Transform transform, float y)
        {
            var p = transform.localPosition;
            transform.localPosition = new Vector3(p.x, y, p.z);
        }

        public static void SetZ(this Transform transform, float z)
        {
            var p = transform.localPosition;
            transform.localPosition = new Vector3(p.x, p.y, z);
        }

        public static void SetRotationX(this Transform transform, float x)
        {
            var r = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(x, r.y, r.z);
        }

        public static void SetRotationY(this Transform transform, float y)
        {
            var r = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(r.x, y, r.z);
        }

        public static void SetRotationZ(this Transform transform, float z)
        {
            var r = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(r.x, r.y, z);
        }
    }
}
