using UnityEngine;

// An original articulated low-poly athlete. Animation never moves the physics root.
public class RunnerHumanVisual : MonoBehaviour
{
    private Transform rig, leftHip, rightHip, leftKnee, rightKnee;
    private Transform leftShoulder, rightShoulder, leftElbow, rightElbow;
    private Rigidbody body;
    private float phase;
    private Material skin, shirt, shorts, shoe, hair;
    private GameObject pack;
    private int previousLevel = -1;

    private Material Make(Color color)
    {
        return new Material(Shader.Find("Standard")) { color = color };
    }

    private Transform Joint(string label, Transform parent, Vector3 position)
    {
        Transform joint = new GameObject(label).transform;
        joint.SetParent(parent, false);
        joint.localPosition = position;
        return joint;
    }

    private void Part(string label, Transform parent, Vector3 position, Vector3 size, Material material, PrimitiveType shape = PrimitiveType.Cube)
    {
        GameObject part = GameObject.CreatePrimitive(shape);
        part.name = label;
        part.transform.SetParent(parent, false);
        part.transform.localPosition = position;
        part.transform.localScale = size;
        Collider collider = part.GetComponent<Collider>();
        collider.enabled = false;
        Destroy(collider);
        part.GetComponent<Renderer>().sharedMaterial = material;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        skin = Make(new Color(.62f, .36f, .23f));
        shirt = Make(new Color(.04f, .65f, .66f));
        shorts = Make(new Color(.08f, .12f, .2f));
        shoe = Make(new Color(.95f, .87f, .65f));
        hair = Make(new Color(.055f, .04f, .035f));
        rig = Joint("Athlete visual", transform, new Vector3(0, -.02f, 0));
        // Local X is forward; Z separates left and right limbs.
        Part("Jersey", rig, new Vector3(0, .34f, 0), new Vector3(.32f, .56f, .48f), shirt);
        Part("Waist", rig, new Vector3(0, .02f, 0), new Vector3(.30f, .18f, .4f), shorts);
        Part("Neck", rig, new Vector3(0, .68f, 0), new Vector3(.16f, .16f, .17f), skin);
        Part("Head", rig, new Vector3(.015f, .85f, 0), new Vector3(.31f, .38f, .32f), skin, PrimitiveType.Sphere);
        Part("Hair", rig, new Vector3(-.025f, 1f, 0), new Vector3(.30f, .12f, .33f), hair);
        Part("Nose", rig, new Vector3(.17f, .84f, 0), new Vector3(.09f, .09f, .10f), skin);
        Part("Eye L", rig, new Vector3(.13f, .91f, -.12f), new Vector3(.025f, .035f, .035f), hair);
        Part("Eye R", rig, new Vector3(.13f, .91f, .12f), new Vector3(.025f, .035f, .035f), hair);
        Part("Jersey stripe", rig, new Vector3(.015f, .4f, -.246f), new Vector3(.27f, .07f, .01f), shoe);
        BuildLeg(-.15f, out leftHip, out leftKnee);
        BuildLeg(.15f, out rightHip, out rightKnee);
        BuildArm(-.31f, out leftShoulder, out leftElbow);
        BuildArm(.31f, out rightShoulder, out rightElbow);
        pack = Joint("Training pack", rig, new Vector3(-.23f, .35f, 0)).gameObject;
        Part("Pack", pack.transform, Vector3.zero, new Vector3(.16f, .35f, .3f), shorts);
    }

    private void BuildLeg(float z, out Transform hip, out Transform knee)
    {
        hip = Joint("Hip", rig, new Vector3(0, -.04f, z));
        Part("Shorts leg", hip, new Vector3(0, -.16f, 0), new Vector3(.22f, .32f, .21f), shorts);
        knee = Joint("Knee", hip, new Vector3(0, -.4f, 0));
        Part("Calf", knee, new Vector3(0, -.19f, 0), new Vector3(.13f, .38f, .14f), skin);
        Part("Sock", knee, new Vector3(0, -.35f, 0), new Vector3(.14f, .12f, .15f), shoe);
        Part("Running shoe", knee, new Vector3(.07f, -.44f, 0), new Vector3(.32f, .13f, .19f), shoe);
    }

    private void BuildArm(float z, out Transform shoulder, out Transform elbow)
    {
        shoulder = Joint("Shoulder", rig, new Vector3(0, .55f, z));
        Part("Sleeve", shoulder, new Vector3(0, -.09f, 0), new Vector3(.21f, .23f, .2f), shirt);
        Part("Upper arm", shoulder, new Vector3(0, -.23f, 0), new Vector3(.13f, .26f, .14f), skin);
        elbow = Joint("Elbow", shoulder, new Vector3(0, -.34f, 0));
        Part("Forearm", elbow, new Vector3(0, -.15f, 0), new Vector3(.12f, .30f, .13f), skin);
        Part("Hand", elbow, new Vector3(0, -.32f, 0), new Vector3(.14f, .16f, .14f), skin, PrimitiveType.Sphere);
    }

    private void LateUpdate()
    {
        RunGameManager game = RunGameManager.Instance;
        if (game == null) return;
        if (previousLevel != game.SpeedLevel)
        {
            previousLevel = game.SpeedLevel;
            pack.SetActive(previousLevel >= 1);
            shirt.color = previousLevel >= 5 ? new Color(.3f, .4f, .85f) : new Color(.04f, .65f, .66f);
        }
        bool airborne = transform.position.y > 1.09f || Mathf.Abs(body.linearVelocity.y) > .5f;
        if (game.IsRunning) phase += Time.deltaTime * Mathf.Clamp(game.CurrentSpeed * 1.9f, 8f, 20f);
        float stride = game.IsRunning ? Mathf.Sin(phase) : 0;
        Pose(leftHip, airborne ? 48 : stride * 38);
        Pose(rightHip, airborne ? -24 : -stride * 38);
        Pose(leftKnee, airborne ? -65 : -Mathf.Max(0, -stride) * 68);
        Pose(rightKnee, airborne ? -85 : -Mathf.Max(0, stride) * 68);
        Pose(leftShoulder, airborne ? -45 : -stride * 42);
        Pose(rightShoulder, airborne ? 35 : stride * 42);
        Pose(leftElbow, 85);
        Pose(rightElbow, 85);
        rig.localRotation = Quaternion.Euler(0, 0, game.IsRunning ? -7 : 9);
        rig.localPosition = new Vector3(0, -.02f + (game.IsRunning && !airborne ? Mathf.Abs(Mathf.Sin(phase)) * .035f : 0), 0);
    }

    private static void Pose(Transform joint, float angle) { joint.localRotation = Quaternion.Euler(0, 0, angle); }
    private void OnDestroy()
    {
        Destroy(skin); Destroy(shirt); Destroy(shorts); Destroy(shoe); Destroy(hair);
    }
}
