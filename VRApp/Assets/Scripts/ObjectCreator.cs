using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class ObjectCreator
{
    private string _path;
    private string _meshName; 
    private GameObject _obj;
    public bool _splitByMaterial = false;

    // structures
    struct OBJFace
    {
        public string materialName;
        public string meshName;
        public int[] indexes;
    }

    public class LineTokenizer
    {
        public string[] Tokens;
        public string Type;
        public string Data;
        
        public LineTokenizer(string line)
        {
            string l = line.Trim().Replace("  ", " ");
            Tokens = l.Split(' ');
            Type = Tokens[0];
            Data = l.Remove(0, l.IndexOf(' ') + 1);
        }
    }

    // OBJ LISTS
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();

    // UMESH LISTS
    List<Vector3> uvertices = new List<Vector3>();
    List<Vector3> unormals = new List<Vector3>();
    List<Vector2> uuvs = new List<Vector2>();

    // MESH CONSTRUCTION
    List<string> materialNames = new List<string>();

    List<string> objectNames = new List<string>();
    Dictionary<string, int> hashtable = new Dictionary<string, int>();
//    List<OBJFace> faceList = new List<OBJFace>();
    private HashSet<OBJFace> faceList = new HashSet<OBJFace>();
    string cmaterial = "";
    string cmesh = "default";

    private bool _hasNormals = false;

    // CACHE
    Material[] materialCache = null;
    // Save this info for later

    FileInfo OBJFileInfo;


    public ObjectCreator(string path)
    {
        _path = path;
        OBJFileInfo = new FileInfo(path);
        _meshName = Path.GetFileNameWithoutExtension(path);
        ParseFile();
        
        if (objectNames.Count == 0)
        {
            objectNames.Add("default");
        }
        
        if (objectNames.Count == 1)
        {
            string objName = objectNames[0];
            _obj = CreateObject(objName);
        }

        else
        {
            _obj = new GameObject(_meshName);
            foreach (string obj in objectNames)
            {
                GameObject subObject = CreateObject(obj);
                subObject.transform.parent = _obj.transform;
                subObject.transform.localScale = new Vector3(-1, 1, 1);   
            }
        }

        if (_obj.name == "default")
        {
            _obj.name = _meshName;
        }
    }

    public GameObject GetObj()
    {
        return _obj;
    }

    private void ParseFile()
    {
        StreamReader sr = new StreamReader(_path, Encoding.Default);
        string ln;

        while (!sr.EndOfStream)
        {
            ln = sr.ReadLine();
            if (ln.Length > 0 && ln[0] != '#')
            {
                LineTokenizer lk = new LineTokenizer(ln);

                if (lk.Type == "mtllib")
                {
                    // Loads cache
                    string mtlPath = OBJLoader.OBJGetFilePath(lk.Data, OBJFileInfo.Directory.FullName + Path.DirectorySeparatorChar, _meshName);

                    if (mtlPath != null)
                    {
                        materialCache = OBJLoader.LoadMTLFile(mtlPath);
                    }
                }

                else if ((lk.Type == "g" || lk.Type == "o") && _splitByMaterial == false)
                {
                    cmesh = lk.Data;
                    if (!objectNames.Contains(cmesh))
                    {
                        objectNames.Add(cmesh);
                    }
                }

                else if (lk.Type == "usemtl")
                {
                    cmaterial = lk.Data;
                    if (!materialNames.Contains(cmaterial))
                    {
                        materialNames.Add(cmaterial);
                    }

                    if (_splitByMaterial)
                    {
                        if (!objectNames.Contains(cmaterial))
                        {
                            objectNames.Add(cmaterial);
                        }
                    }
                }

                // Vertices
                else if (lk.Type == "v")
                {
                    vertices.Add(OBJLoader.ParseVectorFromCMPS(lk.Tokens));
                }

                // Vertex normals
                else if (lk.Type == "vn")
                {
                    normals.Add(OBJLoader.ParseVectorFromCMPS(lk.Tokens));
                }

                // Textures coordinates
                else if (lk.Type == "vt")
                {
                    uvs.Add(OBJLoader.ParseVectorFromCMPS(lk.Tokens));
                }

                // Polygonal face element
                else if (lk.Type == "f")
                {
                    int[] indexes = new int[lk.Tokens.Length - 1];
                    for (int i = 1; i < lk.Tokens.Length; i++)
                    {
                        string felement = lk.Tokens[i];
                        int vertexIndex = -1;
                        int normalIndex = -1;
                        int uvIndex = -1;
                        if (felement.Contains("//"))
                        {
                            //doubleslash, no UVS.
                            string[] elementComps = felement.Split('/');
                            vertexIndex = int.Parse(elementComps[0]) - 1;
                            normalIndex = int.Parse(elementComps[2]) - 1;
                        }
                        else if (felement.Count(x => x == '/') == 2)
                        {
                            //contains everything
                            string[] elementComps = felement.Split('/');
                            vertexIndex = int.Parse(elementComps[0]) - 1;
                            uvIndex = int.Parse(elementComps[1]) - 1;
                            normalIndex = int.Parse(elementComps[2]) - 1;
                        }
                        else if (!felement.Contains("/"))
                        {
                            //just vertex inedx
                            vertexIndex = int.Parse(felement) - 1;
                        }
                        else
                        {
                            //vertex and uv
                            string[] elementComps = felement.Split('/');
                            vertexIndex = int.Parse(elementComps[0]) - 1;
                            uvIndex = int.Parse(elementComps[1]) - 1;
                        }
                        string hashEntry = vertexIndex + "|" + normalIndex + "|" + uvIndex;
                        if (hashtable.ContainsKey(hashEntry))
                        {
                            indexes[i - 1] = hashtable[hashEntry];
                        }
                        else
                        {
                            //create a new hash entry
                            indexes[i - 1] = hashtable.Count;
                            hashtable[hashEntry] = hashtable.Count;
                            uvertices.Add(vertices[vertexIndex]);
                            if (normalIndex < 0 || (normalIndex > (normals.Count - 1)))
                            {
                                unormals.Add(Vector3.zero);
                            }
                            else
                            {
                                _hasNormals = true;
                                unormals.Add(normals[normalIndex]);
                            }
                            if (uvIndex < 0 || (uvIndex > (uvs.Count - 1)))
                            {
                                uuvs.Add(Vector2.zero);
                            }
                            else
                            {
                                uuvs.Add(uvs[uvIndex]);
                            }
                        }
                    }
                    
                    if (indexes.Length < 5 && indexes.Length >= 3)
                    {
                        OBJFace f1 = new OBJFace();
                        f1.materialName = cmaterial;
                        f1.indexes = new int[] {indexes[0], indexes[1], indexes[2]};
                        f1.meshName = (_splitByMaterial) ? cmaterial : cmesh;
                        faceList.Add(f1);
                        if (indexes.Length > 3)
                        {
                            OBJFace f2 = new OBJFace();
                            f2.materialName = cmaterial;
                            f2.meshName = (_splitByMaterial) ? cmaterial : cmesh;
                            f2.indexes = new int[] {indexes[2], indexes[3], indexes[0]};
                            faceList.Add(f2);
                        }
                    }
                }
            }
        }
    }
    
    private GameObject CreateObject(string name)
    {
        GameObject obj = new GameObject(name);   
        //Create mesh
        Mesh m = new Mesh();
        m.name = name;
            
        //LISTS FOR REORDERING
        List<Vector3> processedVertices = new List<Vector3>();
        List<Vector3> processedNormals = new List<Vector3>();
        List<Vector2> processedUVs = new List<Vector2>();
        List<int[]> processedIndexes = new List<int[]>();
        Dictionary<int, int> remapTable = new Dictionary<int, int>();
            
        //POPULATE MESH
        List<string> meshMaterialNames = new List<string>();

        OBJFace[] ofaces = faceList.Where(x => x.meshName == obj.name).ToArray();
        foreach (string mn in materialNames)
        {
            OBJFace[] faces = ofaces.Where(x => x.materialName == mn).ToArray();
            if (faces.Length > 0)
            {
                int[] indexes = new int[0];
                foreach (OBJFace f in faces)
                {
                    int l = indexes.Length;
                    System.Array.Resize(ref indexes, l + f.indexes.Length);
                    System.Array.Copy(f.indexes, 0, indexes, l, f.indexes.Length);
                }
                meshMaterialNames.Add(mn);
                if (m.subMeshCount != meshMaterialNames.Count)
                    m.subMeshCount = meshMaterialNames.Count;

                for (int i = 0; i < indexes.Length; i++)
                {
                    int idx = indexes[i];
                    //build remap table
                    if (remapTable.ContainsKey(idx))
                    {
                        //ezpz
                        indexes[i] = remapTable[idx];
                    }
                    else
                    {
                        processedVertices.Add(uvertices[idx]);
                        processedNormals.Add(unormals[idx]);
                        processedUVs.Add(uuvs[idx]);
                        remapTable[idx] = processedVertices.Count - 1;
                        indexes[i] = remapTable[idx];
                    }
                }

                processedIndexes.Add(indexes);
            }
        }

        if (processedVertices.Count != 0)
        {
            m.vertices = processedVertices.ToArray();
        }

        else
        {
            Debug.Log("no process vertices");
            m.vertices = vertices.ToArray();
        }
        
        m.normals = processedNormals.ToArray();
        m.uv = processedUVs.ToArray();

        for (int i = 0; i < processedIndexes.Count; i++)
        {
            m.SetTriangles(processedIndexes[i], i);
        }

        if (!_hasNormals)
        {
            m.RecalculateNormals();
        }
        m.RecalculateBounds();

        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();

        mr.materials = CreateMaterials(meshMaterialNames);
        mf.mesh = m;

//        obj.name = _meshName;
        return obj;
    }

    private Material[] CreateMaterials(List<string> meshMaterialNames)
    {
        Material[] processedMaterials = new Material[meshMaterialNames.Count];
        for (int i = 0; i < meshMaterialNames.Count; i++)
        {
            if (materialCache == null)
            {
                processedMaterials[i] = new Material(Shader.Find("Standard (Specular setup)"));
            }
            else
            {
                Material mfn = Array.Find(materialCache, x => x.name == meshMaterialNames[i]);

                if (mfn == null)
                {
                    processedMaterials[i] = new Material(Shader.Find("Standard (Specular setup)"));
                }
                else
                {
                    processedMaterials[i] = mfn;
                }
            }
            processedMaterials[i].name = meshMaterialNames[i];
        }

        return processedMaterials;
    }
}