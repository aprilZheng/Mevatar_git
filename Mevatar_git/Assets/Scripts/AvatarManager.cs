using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance;

    private GameObject source;
    private Transform sourceTrans;

    private GameObject target;
    private Transform targetTrans;

    public Dictionary<string, Dictionary<string, MeshFilter>> data = new Dictionary<string, Dictionary<string, MeshFilter>>();
    public Dictionary<string, Dictionary<string, Material>> materials = new Dictionary<string, Dictionary<string, Material>>();
    
    public Dictionary<string, MeshFilter> meshFilter = new Dictionary<string, MeshFilter>();
    //public Dictionary<string, Material> material = new Dictionary<string, Material>();
    public Dictionary<string, MeshRenderer> meshRenderer = new Dictionary<string, MeshRenderer>();
    public Dictionary<string, Transform> transforms = new Dictionary<string, Transform>();

    public GameObject currentUserObject;
    public Mevatar currentMevatar;
    public string[,] AvatarInfo = new string[,] { { "eyeInL", "1" }, { "eyeOutL", "1" }, { "eyeOutR", "1" }, { "eyebrow", "1" }, { "head", "1" } };

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InstantiateSource();
        InstantiateTarget();
        SaveData();
        //InitAvatar();
    }

    public void InstantiateSource()
    {
        source = Instantiate(Resources.Load("resources")) as GameObject;
        sourceTrans = source.transform;
        source.SetActive(false);
    }

    public void InstantiateTarget()
    {
        target = Instantiate(Resources.Load("target")) as GameObject;
        targetTrans = target.transform;
    }

    public void LoadMevatarFromUser()
    {
        currentMevatar = currentUserObject.GetComponent<Mevatar>();

        int length = AvatarInfo.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            switch (AvatarInfo[i, 0])
            {
                case "eyeInL":
                    AvatarInfo[i,1]=currentMevatar.eyes;
                    //Debug.Log("eyeInL to Mesh Filter");
                    break;
                case "eyeInR":
                    AvatarInfo[i, 1] = currentMevatar.eyes;
                    //Debug.Log("eyeInL to Mesh Filter");
                    break;
                case "eyebrow":
                    AvatarInfo[i, 1] = currentMevatar.eyebrow;
                    //Debug.Log("eyebrow to Mesh Filter");
                    break;
                case "head":
                    AvatarInfo[i, 1] = currentMevatar.head;
                    //Debug.Log("head to Mesh Filter");
                    break;
                default:
                    //Debug.Log("skip");
                    break;
            }
        }
    }

    public void SaveData()
    {
        if (sourceTrans == null)
        {
            Debug.Log("soure tranform is empty");
            return;
        }
        MeshFilter[] parts = sourceTrans.GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRend = sourceTrans.GetComponentsInChildren<MeshRenderer>();
        Transform[] transf = sourceTrans.GetComponentsInChildren<Transform>();

        foreach (Transform t in transf)
        {
            string[] names = t.name.Split('-');

            if (!transforms.ContainsKey(names[0]))
            {
                transforms.Add(names[0], t);
                Debug.Log("mesh render added names[0]: " + names[0]);
            }
        }

        foreach (MeshRenderer meshr in meshRend)//one meshrenderer have a list of materials
        {
            string[] names = meshr.name.Split('-');

            if (!meshRenderer.ContainsKey(names[0]))
            {
                meshRenderer.Add(names[0], meshr);
                Debug.Log("mesh render added names[0]: " + names[0]);
            }
        }

        foreach (var part in parts)
        {
            string[] names = part.name.Split('-');

            if (!data.ContainsKey(names[0]))
            {
                GameObject partGo = new GameObject { name = names[0] };
                partGo.transform.parent = target.transform;
                partGo.transform.position = transforms[names[0]].position;
                partGo.transform.localScale = transforms[names[0]].localScale;
                partGo.transform.rotation = transforms[names[0]].rotation;
                MeshRenderer mr = partGo.AddComponent<MeshRenderer>();

                data.Add(names[0], new Dictionary<string, MeshFilter>());
                meshFilter.Add(names[0], partGo.AddComponent<MeshFilter>());

                switch (names[0].ToString())
                {
                    case "eyeInL":
                        mr.sharedMaterials = meshRenderer["eyeInL"].sharedMaterials;
                        Debug.Log("eyeInL to Mesh Filter");
                        break;
                    case "eyeInR":
                        mr.sharedMaterials = meshRenderer["eyeInR"].sharedMaterials;
                        Debug.Log("eyeInL to Mesh Filter");
                        break;
                    case "eyebrow":
                        mr.sharedMaterials = meshRenderer["eyebrow"].sharedMaterials;
                        Debug.Log("eyebrow to Mesh Filter");
                        break;
                    case "head":
                        mr.sharedMaterials = meshRenderer["head"].sharedMaterials;
                        Debug.Log("head to Mesh Filter");
                        break;
                    case "eyeOutL":
                        mr.sharedMaterials = meshRenderer["eyeOutL"].sharedMaterials;
                        Debug.Log("eyeOutLto Mesh Filter");
                        break;
                    case "eyeOutR":
                        mr.sharedMaterials = meshRenderer["eyeOutR"].sharedMaterials;
                        Debug.Log("eyeOutLto Mesh Filter");
                        break;
                    default:
                        Debug.Log("Something wrong, name[0] = " + names[0]);
                        break;
                }
            }
            data[names[0]].Add(names[1], part);
        }
    }

    public void InitAvatar()
    {
        LoadMevatarFromUser();

        int length = AvatarInfo.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            LoadInitAvatar(AvatarInfo[i, 0], AvatarInfo[i, 1]);
        }
    }

    public void LoadInitAvatar(string part, string num)
    {

        MeshFilter mf = data[part][num];
        meshFilter[part].mesh = mf.mesh;
        if (part == "eyeInL")
        {
            LoadInitAvatar("eyeInR", num);
        }
        Debug.Log("Load init avatar called");

        //SaveAvatarInfo(part, num);
    }

    public void ChangeAvatar(string part, string num)
    {

        MeshFilter mf = data[part][num];
        meshFilter[part].mesh = mf.mesh;
        if (part == "eyeInL")
        {
            ChangeAvatar("eyeInR", num);
        }
        Debug.Log("change avatar called");

        //SaveAvatarInfo(part, num);

    }

    //void SaveAvatarInfo(string part, string num)
    //{
    //    int length = AvatarInfo.GetLength(0);
    //    for (int i = 0; i < length; i++)
    //    {
    //        if (AvatarInfo[i, 0] == part)
    //        {
    //            AvatarInfo[i, 1] = num;
    //        }
    //    }

    //}

    public void DefaultRotationAvatar()
    {
        MeshFilter[] parts = target.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter part in parts)
        {
            string[] names = part.name.Split('-');

            part.transform.position = transforms[names[0]].position;
            part.transform.localScale = transforms[names[0]].localScale;
            part.transform.rotation = transforms[names[0]].rotation;
        }

    }
}
