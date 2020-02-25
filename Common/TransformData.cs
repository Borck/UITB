using System;
using Assets.UITB.Attributes;
using Assets.UITB.Extensions;
using JetBrains.Annotations;
using UnityEngine;



namespace Assets.UITB.Common {
  [Serializable]
  [Author( "Christian Borck", EMail = "Christian.Borck@b-tu.de" )]
  public struct TransformData {
    public Vector3 position;
    public Vector3 scale;


    public Quaternion rotation;


    public static readonly TransformData Identity = new TransformData(
      Vector3.zero, Vector3.zero, Vector3.one );


    public TransformData(Vector3 position, Quaternion rotation, Vector3 scale) {
      this.position = position;
      this.rotation = rotation;
      this.scale = scale;

    }



    public TransformData(Vector3 position, Vector3 rotation, Vector3 scale) {
      this.position = position;
      this.rotation = Quaternion.Euler( rotation );
      this.scale = scale;
    }


    public void ApplyLocalTRSTo(Transform transform) {
      transform.localPosition = position;
      transform.localRotation = rotation;
      transform.localScale = scale;
    }

    public void ApplyWorldTRSTo(Transform transform) {
      transform.position = position;
      transform.rotation = rotation;
      transform.SetLossyScale( scale );
    }


    public Matrix4x4 ToMatrix4x4() {
      return Matrix4x4.TRS( position, rotation, scale );
    }



    public static TransformData FromLocalTransform([NotNull]Transform transform)
      => new TransformData( transform.localPosition, transform.localRotation, transform.localScale );



    public static TransformData FromWorldTransform([NotNull]Transform transform)
          => new TransformData( transform.position, transform.rotation, transform.lossyScale );





    public override bool Equals(object obj) {
      return obj is TransformData transformData && Equals( transformData );
    }



    public bool Equals(Vector3 position, Vector3 rotationEuler, Vector3 scale) {
      return this.position.Equals( position ) &&
             rotation.Equals( Quaternion.Euler( rotationEuler ) ) &&
             this.scale.Equals( scale );
    }


    public bool Equals(Vector3 position, Quaternion rotation, Vector3 scale) {
      return this.position.Equals( position ) &&
             this.rotation.Equals( rotation ) &&
             this.scale.Equals( scale );
    }



    public bool Equals(TransformData other) {
      return position.Equals( other.position ) &&
             rotation.Equals( other.rotation ) &&
             scale.Equals( other.scale );
    }



    public override int GetHashCode() {
      unchecked {
        var hashCode = position.GetHashCode();
        hashCode = ( hashCode * 397 ) ^ rotation.GetHashCode();
        hashCode = ( hashCode * 397 ) ^ scale.GetHashCode();
        return hashCode;
      }
    }



    public static bool operator ==(TransformData a, TransformData b) {
      return a.Equals( b );
    }



    public static bool operator !=(TransformData a, TransformData b) {
      return !a.Equals( b );
    }





    public static TransformData operator *(TransformData a, TransformData b) {

      return new TransformData(
        b.scale.ComponentWiseMultiply( b.rotation * a.position ) + b.position,
        a.rotation * b.rotation,
        ( b.rotation * a.scale ).ComponentWiseMultiply( b.scale ) );//TODO validate scale
    }



    public TransformData Inverse() {
      return new TransformData(
        -position,
        Quaternion.Inverse( rotation ),
        new Vector3( 1 / scale.x, 1 / scale.y, 1 / scale.z ) );
    }

  }
}
