using Assets.MT_Sync.Tests;
using Assets.UITB.Components;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;



namespace Assets.UITB.Tests {
  public class AnchorTransformTest {
    #region Methods and others

    [UnityTest]
    public IEnumerator SetLocalPosRot_UsingProperties_ExpectCorrectLocalPosRotRelativeToAnchor() {
      var (goRoot_0, goRoot_0_0) = createStub(MethodBase.GetCurrentMethod());
      goRoot_0.transform.localPosition = new Vector3(1, 2, 3);

      var tfLink = goRoot_0_0.GetComponent<AnchorTransform>();

      var anchor = tfLink.AnchorOverride;
      var parent = goRoot_0_0.transform.parent;
      var tfExpected = new GameObject("expected transform").transform;
      tfExpected.parent = tfLink.AnchorOverride;

      var n = 100;
      for (var i = 0; i < n; i++) {
        var pose = GetSamplePose(i, n);

        tfLink.localPosition = pose.position;
        tfLink.localRotation = pose.rotation;

        tfExpected.parent = anchor;
        tfExpected.position = pose.position;
        tfExpected.rotation = pose.rotation;
        tfExpected.parent = parent;

        yield return null; //skip a frame to trigger update values
        //Assert.AreEqual( tfExpected.localRotation.eulerAngles, goRoot_0_0.transform.localRotation.eulerAngles );
        Asserts.AreApproximatelyEqual(tfExpected.localPosition, goRoot_0_0.transform.localPosition);
        Asserts.AreApproximatelyEqual(tfExpected.localRotation, goRoot_0_0.transform.localRotation);
      }
    }



    [UnityTest]
    public IEnumerator SetLocalPosRot_UsingAnimationClip_ExpectCorrectPosRot() {
      var (_, gameObject) = createStub(MethodBase.GetCurrentMethod());

      var n = 100;
      var clip = CreateSamplePosesAnimationClip(n);

      for (var i = 0; i < n; i++) {
        clip.SampleAnimation(gameObject, i);
        var pose = GetSamplePose(i, n);

        yield return null; //skip a frame to trigger update values
        Asserts.AreApproximatelyEqual(pose.position, gameObject.transform.localPosition);
        Asserts.AreApproximatelyEqual(pose.rotation, gameObject.transform.localRotation);
      }
    }



    private static AnimationClip CreateSamplePosesAnimationClip(int nSamples) {
      var clip = new AnimationClip {legacy = true, frameRate = 1};

      var posX = new AnimationCurve();
      var posY = new AnimationCurve();
      var posZ = new AnimationCurve();
      var rotX = new AnimationCurve();
      var rotY = new AnimationCurve();
      var rotZ = new AnimationCurve();
      var rotW = new AnimationCurve();

      for (var i = 0; i < nSamples; i++) {
        var pose = GetSamplePose(i, nSamples);
        posX.AddKey(i, pose.position.x);
        posY.AddKey(i, pose.position.y);
        posZ.AddKey(i, pose.position.z);
        rotX.AddKey(i, pose.rotation.x);
        rotY.AddKey(i, pose.rotation.y);
        rotZ.AddKey(i, pose.rotation.z);
        rotW.AddKey(i, pose.rotation.w);
      }

      clip.SetCurve("", typeof(AnchorTransform), "localPosition.x", posX);
      clip.SetCurve("", typeof(AnchorTransform), "localPosition.y", posY);
      clip.SetCurve("", typeof(AnchorTransform), "localPosition.z", posZ);
      clip.SetCurve("", typeof(AnchorTransform), "localRotation.x", rotX);
      clip.SetCurve("", typeof(AnchorTransform), "localRotation.y", rotY);
      clip.SetCurve("", typeof(AnchorTransform), "localRotation.z", rotZ);
      clip.SetCurve("", typeof(AnchorTransform), "localRotation.w", rotW);
      return clip;
    }



    private static Pose GetSamplePose(int i, int nSamples) {
      var phi = i * 2 * Math.PI / nSamples;
      var pos = new Vector3(
        (float)(2 * Math.Sin(phi)),
        (float)(3 * Math.Cos(phi)),
        (float)(4 * Math.Sin((2 * phi) + (Math.PI / 3)))
      );

      var a = i * 360f / nSamples;
      var rotEuler = new Vector3(a, a + 120, (a * 2) - 120);
      var rot = Quaternion.Euler(rotEuler);
      return new Pose(pos, rot);
    }



    /// <summary>
    /// </summary>
    /// <returns></returns>
    private (GameObject goRoot_0, GameObject goRoot_0_0)
      createStub([NotNull] MethodBase callingMethod) {
      var primType = PrimitiveType.Cube;

      var goRoot = new GameObject($"Test {callingMethod.ReflectedType?.Name}.{callingMethod.Name}");
      var goRoot_0 = new GameObject("0") {transform = {parent = goRoot.transform}};
      var goRoot_0_0 = GameObject.CreatePrimitive(primType);
      goRoot_0_0.name = "0.0";
      goRoot_0_0.transform.parent = goRoot_0.transform;

      var tfLink = goRoot_0_0.AddComponent<AnchorTransform>();
      tfLink.AnchorOverride = goRoot.transform;
      tfLink.LinkPosition = true;
      tfLink.LinkRotation = true;
      tfLink.LinkScale = true;

      return (goRoot_0, goRoot_0_0);
    }

    #endregion
  }
}
