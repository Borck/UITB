using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class BoundsX {
    public static Vector3 GetAlignedPosition(this Bounds bounds, BoundsAlign xAlign, BoundsAlign yAlign, BoundsAlign zAlign) {
      Vector3 vector3;
      switch (xAlign) {
        case BoundsAlign.Min:
          vector3.x = bounds.min.x;
          break;
        case BoundsAlign.Center:
          vector3.x = bounds.center.x;
          break;
        case BoundsAlign.Max:
          vector3.x = bounds.max.x;
          break;
        default:
          throw new ArgumentOutOfRangeException( nameof( xAlign ), xAlign, null );
      }
      switch (yAlign) {
        case BoundsAlign.Min:
          vector3.y = bounds.min.y;
          break;
        case BoundsAlign.Center:
          vector3.y = bounds.center.y;
          break;
        case BoundsAlign.Max:
          vector3.y = bounds.max.y;
          break;
        default:
          throw new ArgumentOutOfRangeException( nameof( yAlign ), yAlign, null );
      }
      switch (zAlign) {
        case BoundsAlign.Min:
          vector3.z = bounds.min.z;
          break;
        case BoundsAlign.Center:
          vector3.z = bounds.center.z;
          break;
        case BoundsAlign.Max:
          vector3.z = bounds.max.z;
          break;
        default:
          throw new ArgumentOutOfRangeException( nameof( zAlign ), zAlign, null );
      }

      return vector3;
    }



    public static Bounds EncapsulateAll(this IEnumerable<Bounds> boundsEnumerable)
      => boundsEnumerable.Aggregate(
        (bounds0, bounds1) => {
          bounds0.Encapsulate( bounds1 );
          return bounds0;
        }
      );

  }



  public enum BoundsAlign {
    Min, Center, Max
  }

}
