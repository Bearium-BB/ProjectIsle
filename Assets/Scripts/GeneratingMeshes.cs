using UnityEngine;

public class GeneratingMeshes : MonoBehaviour
{

    public int xSize = 10; // Number of segments along X
    public int zSize = 10; // Number of segments along Z

    //public Texture2D[] textures;

    // Your texture atlas
    //public Texture2D textureAtlas;

    // Number of tiles across and down
    //public int atlasColumns = 2;
    //public int atlasRows = 2;

    void Start()
    {
        //GenerateGrid();
    }

    //void GenerateGrid()
    //{
    //    Mesh mesh = new Mesh();
    //    mesh.name = "ProceduralGrid";

    //    // 1. Calculate array sizes
    //    Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];
    //    Vector2[] uv = new Vector2[vertices.Length];

    //    // 2. Generate Vertices and UVs using nested loops
    //    int i = 0;
    //    for (int z = 0; z <= zSize; z++)
    //    {
    //        for (int x = 0; x <= xSize; x++)
    //        {
    //            // Position each vertex 1 unit apart
    //            vertices[i] = new Vector3(x, 0, z);

    //            // Map UV coordinates between 0.0 and 1.0
    //            uv[i] = new Vector2((float)x / xSize, (float)z / zSize);

    //            i++;
    //        }
    //    }

    //    // 3. Generate Triangles using nested loops
    //    int[] triangles = new int[xSize * zSize * 6];
    //    int vert = 0;
    //    int tris = 0;

    //    for (int z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            // First triangle of the square quad
    //            triangles[tris + 0] = vert;
    //            triangles[tris + 1] = vert + xSize + 1;
    //            triangles[tris + 2] = vert + 1;

    //            // Second triangle of the square quad
    //            triangles[tris + 3] = vert + 1;
    //            triangles[tris + 4] = vert + xSize + 1;
    //            triangles[tris + 5] = vert + xSize + 2;

    //            vert++;
    //            tris += 6;
    //        }
    //        vert++; // Skip the last vertex at the end of the row
    //    }

    //    // 4. Assign data to the mesh
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.uv = uv;

    //    // 5. Automatically compute lighting behaviors
    //    mesh.RecalculateNormals();

    //    MeshFilter meshFilter = GetComponent<MeshFilter>();
    //    meshFilter.mesh = mesh;

    //    MeshRenderer renderer = GetComponent<MeshRenderer>();

    //    Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    //    material.color = Color.blue;

    //    renderer.material = material;
    //}

    //void GenerateGrid()
    //{
    //    Mesh mesh = new Mesh();
    //    mesh.name = "ProceduralGrid";

    //    int quadCount = xSize * zSize;

    //    Vector3[] vertices = new Vector3[quadCount * 4];
    //    Vector2[] uv = new Vector2[quadCount * 4];

    //    mesh.subMeshCount = quadCount;

    //    int vertexIndex = 0;

    //    // Create all vertices and UVs first
    //    for (int z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            vertices[vertexIndex + 0] = new Vector3(x, 0, z);
    //            vertices[vertexIndex + 1] = new Vector3(x + 1, 0, z);
    //            vertices[vertexIndex + 2] = new Vector3(x, 0, z + 1);
    //            vertices[vertexIndex + 3] = new Vector3(x + 1, 0, z + 1);

    //            uv[vertexIndex + 0] = new Vector2(0, 0);
    //            uv[vertexIndex + 1] = new Vector2(1, 0);
    //            uv[vertexIndex + 2] = new Vector2(0, 1);
    //            uv[vertexIndex + 3] = new Vector2(1, 1);

    //            vertexIndex += 4;
    //        }
    //    }

    //    // Give vertices to the mesh BEFORE setting triangles
    //    mesh.vertices = vertices;
    //    mesh.uv = uv;

    //    // Now create the submeshes
    //    vertexIndex = 0;
    //    int subMeshIndex = 0;

    //    for (int z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            int[] triangles =
    //            {
    //            vertexIndex + 0,
    //            vertexIndex + 2,
    //            vertexIndex + 1,

    //            vertexIndex + 1,
    //            vertexIndex + 2,
    //            vertexIndex + 3
    //        };

    //            mesh.SetTriangles(triangles, subMeshIndex);

    //            vertexIndex += 4;
    //            subMeshIndex++;
    //        }
    //    }

    //    mesh.RecalculateNormals();

    //    GetComponent<MeshFilter>().mesh = mesh;

    //    // Create materials
    //    Material[] materials = new Material[quadCount];

    //    for (int i = 0; i < quadCount; i++)
    //    {
    //        Material material = new Material(
    //            Shader.Find("Universal Render Pipeline/Lit")
    //        );

    //        Texture2D texture =
    //            textures[Random.Range(0, textures.Length)];

    //        material.mainTexture = texture;

    //        materials[i] = material;
    //    }

    //    GetComponent<MeshRenderer>().materials = materials;
    //}

    //void GenerateGrid()
    //{
    //    Mesh mesh = new Mesh();
    //    mesh.name = "ProceduralGrid";

    //    int quadCount = xSize * zSize;

    //    Vector3[] vertices = new Vector3[quadCount * 4];
    //    Vector2[] uv = new Vector2[quadCount * 4];
    //    int[] triangles = new int[quadCount * 6];

    //    int vertexIndex = 0;
    //    int triangleIndex = 0;

    //    for (int z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            // -------------------------
    //            // Vertices
    //            // -------------------------

    //            vertices[vertexIndex + 0] = new Vector3(x, 0, z);
    //            vertices[vertexIndex + 1] = new Vector3(x + 1, 0, z);
    //            vertices[vertexIndex + 2] = new Vector3(x, 0, z + 1);
    //            vertices[vertexIndex + 3] = new Vector3(x + 1, 0, z + 1);


    //            // -------------------------
    //            // Pick random tile
    //            // -------------------------

    //            int tileX = Random.Range(0, atlasColumns);
    //            int tileY = Random.Range(0, atlasRows);


    //            // Size of one tile in UV space

    //            float tileWidth = 1f / atlasColumns;
    //            float tileHeight = 1f / atlasRows;


    //            // Bottom-left corner of selected tile

    //            float uMin = tileX * tileWidth;
    //            float vMin = tileY * tileHeight;


    //            // Top-right corner

    //            float uMax = uMin + tileWidth;
    //            float vMax = vMin + tileHeight;


    //            // -------------------------
    //            // UVs
    //            // -------------------------

    //            uv[vertexIndex + 0] = new Vector2(uMin, vMin);
    //            uv[vertexIndex + 1] = new Vector2(uMax, vMin);
    //            uv[vertexIndex + 2] = new Vector2(uMin, vMax);
    //            uv[vertexIndex + 3] = new Vector2(uMax, vMax);


    //            // -------------------------
    //            // Triangles
    //            // -------------------------

    //            triangles[triangleIndex + 0] = vertexIndex + 0;
    //            triangles[triangleIndex + 1] = vertexIndex + 2;
    //            triangles[triangleIndex + 2] = vertexIndex + 1;

    //            triangles[triangleIndex + 3] = vertexIndex + 1;
    //            triangles[triangleIndex + 4] = vertexIndex + 2;
    //            triangles[triangleIndex + 5] = vertexIndex + 3;


    //            vertexIndex += 4;
    //            triangleIndex += 6;
    //        }
    //    }

    //    mesh.vertices = vertices;
    //    mesh.uv = uv;
    //    mesh.triangles = triangles;

    //    mesh.RecalculateNormals();

    //    GetComponent<MeshFilter>().mesh = mesh;


    //    // -------------------------
    //    // Material
    //    // -------------------------

    //    Material material = new Material(
    //        Shader.Find("Universal Render Pipeline/Lit")
    //    );

    //    material.mainTexture = textureAtlas;

    //    GetComponent<MeshRenderer>().material = material;
    //}

    //Good One
    //void GenerateGrid()
    //{
    //    Mesh mesh = new Mesh();
    //    mesh.name = "ProceduralGrid";

    //    int quadCount = xSize * zSize;

    //    Vector3[] vertices = new Vector3[quadCount * 4];
    //    Vector2[] uv = new Vector2[quadCount * 4];
    //    int[] triangles = new int[quadCount * 6];

    //    int vertexIndex = 0;
    //    int triangleIndex = 0;

    //    for (int z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            // -------------------------
    //            // Vertices
    //            // -------------------------

    //            vertices[vertexIndex + 0] = new Vector3(x, 0, z);
    //            vertices[vertexIndex + 1] = new Vector3(x + 1, 0, z);
    //            vertices[vertexIndex + 2] = new Vector3(x, 0, z + 1);
    //            vertices[vertexIndex + 3] = new Vector3(x + 1, 0, z + 1);


    //            // -------------------------
    //            // Pick random tile
    //            // -------------------------

    //            int tileX = Random.Range(0, 2);
    //            int tileY = Random.Range(0, 2);

    //            // Flip Y because Unity UVs start
    //            // at the bottom-left
    //            tileY = 1 - tileY;


    //            // -------------------------
    //            // Atlas dimensions
    //            // -------------------------

    //            float tileSize = 128f;
    //            float atlasSize = 256f;


    //            // -------------------------
    //            // Calculate UV coordinates
    //            // -------------------------

    //            float uMin = (tileX * tileSize) / atlasSize;
    //            float uMax = ((tileX + 1) * tileSize) / atlasSize;

    //            float vMin = (tileY * tileSize) / atlasSize;
    //            float vMax = ((tileY + 1) * tileSize) / atlasSize;


    //            // -------------------------
    //            // Assign UVs
    //            // -------------------------

    //            uv[vertexIndex + 0] = new Vector2(uMin, vMin);
    //            uv[vertexIndex + 1] = new Vector2(uMax, vMin);
    //            uv[vertexIndex + 2] = new Vector2(uMin, vMax);
    //            uv[vertexIndex + 3] = new Vector2(uMax, vMax);


    //            // -------------------------
    //            // Triangles
    //            // -------------------------

    //            triangles[triangleIndex + 0] = vertexIndex + 0;
    //            triangles[triangleIndex + 1] = vertexIndex + 2;
    //            triangles[triangleIndex + 2] = vertexIndex + 1;

    //            triangles[triangleIndex + 3] = vertexIndex + 1;
    //            triangles[triangleIndex + 4] = vertexIndex + 2;
    //            triangles[triangleIndex + 5] = vertexIndex + 3;


    //            vertexIndex += 4;
    //            triangleIndex += 6;
    //        }
    //    }


    //    // -------------------------
    //    // Assign mesh data
    //    // -------------------------

    //    mesh.vertices = vertices;
    //    mesh.uv = uv;
    //    mesh.triangles = triangles;

    //    mesh.RecalculateNormals();

    //    GetComponent<MeshFilter>().mesh = mesh;


    //    // -------------------------
    //    // Create material
    //    // -------------------------

    //    Material material = new Material(
    //        Shader.Find("Universal Render Pipeline/Lit")
    //    );

    //    material.mainTexture = textureAtlas;

    //    GetComponent<MeshRenderer>().material = material;
    //}

    public GameObject GenerateGrid(Chunk chunk, Texture2D textureAtlas)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ProceduralGrid";

        int quadCount = xSize * zSize;

        Vector3[] vertices = new Vector3[quadCount * 4];
        Vector2[] uv = new Vector2[quadCount * 4];
        int[] triangles = new int[quadCount * 6];

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                vertices[vertexIndex + 0] = new Vector3(x, 0, z);
                vertices[vertexIndex + 1] = new Vector3(x + 1, 0, z);
                vertices[vertexIndex + 2] = new Vector3(x, 0, z + 1);
                vertices[vertexIndex + 3] = new Vector3(x + 1, 0, z + 1);

                int nodeIndex = z * xSize + x;

                MapNode node = chunk.nodes[nodeIndex];

                float uMin = 0;
                float uMax = 0;
                float vMin = 0;
                float vMax = 0;

                if (node.type == MapNodeType.land)
                {

                    GetTileUV(2048, 2048, 512, 512, 0, 0,out uMin, out uMax, out vMin, out vMax);
 
                }
                else if (node.type == MapNodeType.water)
                {
                    GetTileUV(2048, 2048, 512, 512, 1, 0, out uMin, out uMax, out vMin, out vMax);
                }

                uv[vertexIndex + 0] = new Vector2(uMin, vMin);
                uv[vertexIndex + 1] = new Vector2(uMax, vMin);
                uv[vertexIndex + 2] = new Vector2(uMin, vMax);
                uv[vertexIndex + 3] = new Vector2(uMax, vMax);

                triangles[triangleIndex + 0] = vertexIndex + 0;
                triangles[triangleIndex + 1] = vertexIndex + 2;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + 2;
                triangles[triangleIndex + 5] = vertexIndex + 3;

                vertexIndex += 4;
                triangleIndex += 6;
            }
        }

        // -------------------------
        // Assign mesh data
        // -------------------------

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        GameObject emptyGO = new GameObject("New Engine");

        emptyGO.AddComponent<MeshFilter>().mesh = mesh;


        // -------------------------
        // Create material
        // -------------------------

        Material material = new Material(
            Shader.Find("Universal Render Pipeline/Unlit")
        );

        material.mainTexture = textureAtlas;

        emptyGO.AddComponent<MeshRenderer>().material = material;
        emptyGO.transform.position = new Vector3(chunk.chunkX * 32, 0, chunk.chunkY * 32);

        return emptyGO;

    }

    void GetTileUV(
    int atlasWidth,
    int atlasHeight,
    int tileWidth,
    int tileHeight,
    int tileX,
    int tileY,
    out float uMin,
    out float uMax,
    out float vMin,
    out float vMax)
    {
        uMin = (float)(tileX * tileWidth) / atlasWidth;
        uMax = (float)((tileX + 1) * tileWidth) / atlasWidth;

        // Flip Y because Unity UV coordinates start at the bottom
        vMin = 1.0f - (float)((tileY + 1) * tileHeight) / atlasHeight;
        vMax = 1.0f - (float)(tileY * tileHeight) / atlasHeight;
    }
}
