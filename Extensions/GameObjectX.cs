using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;



namespace Assets.UITB.Extensions {
  public static class GameObjectX {
    public static IEnumerable<GameObject> Traverse(this GameObject gameObject) {
      yield return gameObject;
      foreach (Transform childTransform in gameObject.transform) {
        foreach (var childGameObject in Traverse(childTransform.gameObject)) {
          yield return childGameObject;
        }
      }
    }



    public static IEnumerable<GameObject> Traverse(this GameObject gameObject, uint depth) {
      yield return gameObject;
      if (depth == 0) {
        yield break;
      }

      var childDepth = depth - 1;
      foreach (Transform childTransform in gameObject.transform) {
        foreach (var childGameObject in Traverse(childTransform.gameObject, childDepth)) {
          yield return childGameObject;
        }
      }
    }



    /// <summary>
    ///   Validates if any children exists
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns>true if <see cref="GameObject" /> has any child <see cref="GameObject" />, otherwise false</returns>
    public static bool HasAnyChild(this GameObject gameObject) {
      return gameObject.transform.childCount > 0;
    }



    public static IEnumerable<GameObject> GetChildren(this GameObject gameObject) {
      foreach (Transform childTransform in gameObject.transform) {
        yield return childTransform.gameObject;
      }
    }



    public static bool TryGetComponent<TComponent>(this GameObject gameObject, out TComponent component)
      where TComponent : Component {
      component = gameObject.GetComponent<TComponent>();
      return component != default(TComponent);
    }


    /// <summary>
    /// Destroys if the component already exists and adds a new component
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static TComponent ReAddComponent<TComponent>(this GameObject gameObject) where TComponent : Component {
      if (gameObject.TryGetComponent(out TComponent component)) {
        Object.Destroy(component);
      }
      return gameObject.AddComponent<TComponent>();
    }
    /// <summary>
    /// Destroys if the component already exists and adds a new component
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="componentType"></param>
    /// <returns></returns>
    public static Component ReAddComponent(this GameObject gameObject, Type componentType) {
      if (gameObject.TryGetComponent(componentType, out var component)) {
        Object.Destroy(component);
      }
      return gameObject.AddComponent(componentType);

    }



    public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject)
      where TComponent : Component {
      var component = gameObject.GetComponent<TComponent>();
      return component != default(TComponent)
               ? component
               : gameObject.AddComponent<TComponent>();
    }



    public static Component GetOrAddComponent(this GameObject gameObject, Type componentType) {
      var comp = gameObject.GetComponent(componentType);
      return comp != default
               ? comp
               : gameObject.AddComponent(componentType);
    }



    public static IEnumerable<TComponent> GetComponentsInThisAndChildren<TComponent>(this GameObject gameObject)
      where TComponent : Component {
      return gameObject.GetComponents<TComponent>()
                       .Concat(gameObject.GetComponentsInChildren<TComponent>());
    }



    public static bool HasComponent<T>(this GameObject gameObject)
      where T : Component {
      return gameObject.GetComponent<T>() != default(T);
    }



    /// <summary>
    ///   Returns the geometrical center of the underlying mesh in world coordinates without local transform.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Vector3 GetCenterNormalized(this GameObject gameObject) {
      var pivotTransformed = gameObject.GetComponent<MeshRenderer>().bounds.center;
      var transform = gameObject.transform;
      var transformOffset = transform.TransformVector(Vector3.zero);
      return pivotTransformed - transformOffset;
    }



    /// <summary>
    ///   Returns the previous <see cref="GameObject" />
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static GameObject Previous(this GameObject gameObject) {
      var prev = gameObject.transform.Previous();
      return prev != default
               ? prev.gameObject
               : default;
    }



    /// <summary>
    ///   Returns the next <see cref="GameObject" />
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static GameObject Next(this GameObject gameObject) {
      var next = gameObject.transform.Next();
      return next != default
               ? next.gameObject
               : default;
    }



    /// <summary>
    ///   Traverses through the parents of the game object to find a parent with the given tag
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static bool TryFindParentByTag(this GameObject gameObject, string tag, out GameObject parent) {
      var parentTransform = gameObject.transform.parent;
      while (parentTransform) {
        if (parentTransform.CompareTag(tag)) {
          parent = parentTransform.gameObject;
          return true;
        }

        parentTransform = parentTransform.parent;
      }

      parent = default;
      return false;
    }



    /// <summary>
    ///   Calculates the bounding box encapsulating all collider bounds in the sub tree
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Bounds EncapsulateColliderBounds(this GameObject gameObject)
      => EncapsulateBounds(gameObject, (Collider collider) => collider.bounds);



    /// <summary>
    ///   Calculates the bounding box encapsulating all collider bounds in the sub tree
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Bounds EncapsulateMeshBounds(this GameObject gameObject)
      => EncapsulateBounds(gameObject, (MeshRenderer meshRenderer) => meshRenderer.bounds);



    /// <summary>
    ///   Calculates the bounding box encapsulating all collider bounds in the sub tree
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Bounds EncapsulateBounds<T>(this GameObject gameObject, Func<T, Bounds> getBounds)
      where T : Component {
      return gameObject.GetComponentsInThisAndChildren<T>()
                       .Select(getBounds)
                       .EncapsulateAll();
    }
  }
}
