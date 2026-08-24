using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ServiceTexturerManager
{
    TextureData[] textures = new TextureData[0];

    TextureAtlasData[] textureAtlass = new TextureAtlasData[0]; 

    List<Texture2D> texture2Ds = new List<Texture2D>();



    public ServiceTexturerManager()
    {
        string pathTextureData = Path.Combine(
            Application.streamingAssetsPath,
            "textureData.json"
        );

        string jsonTextureData = File.ReadAllText(pathTextureData);

        TextureDataArray dataTextureData = JsonUtility.FromJson<TextureDataArray>(jsonTextureData);

        textures = dataTextureData.textures;

        string pathTextureAtlas = Path.Combine(
            Application.streamingAssetsPath,
            "textureAtlasData.json"
        );

        string jsonTextureAtlas = File.ReadAllText(pathTextureAtlas);

        TextureAtlasDataArray dataTextureAtlas = JsonUtility.FromJson<TextureAtlasDataArray>(jsonTextureAtlas);

        textureAtlass = dataTextureAtlas.textures;


        for (int i = 0; i < textureAtlass.Length; i++)
        {
            texture2Ds.Add(LoadTexture(textureAtlass[i].path));
        }
        //for( int i = 0; i < textures.Length; i++)
        //{
        //    texture2Ds.Add(LoadTexture(textures[i].Path));
        //}
    }

    public TextureAtlasTextureCoordinates GetTextureById(int id)
    {
        TextureData textureData = textures.FirstOrDefault(x => x.Id == id);

        //Texture2D texture = LoadTexture(textureData.Path);

        return new TextureAtlasTextureCoordinates(textureData.Id, textureData.TextureSizeX, textureData.TextureSizeY, textureData.TileSizeX, textureData.TileSizeY, textureData.TexturesCoordinatesX, textureData.TexturesCoordinatesY, texture2Ds[textureData.IdAtlas]);
    }

    Texture2D LoadTexture(string relativePath)
    {
        string path = Path.Combine(
            Application.streamingAssetsPath,
            relativePath
        );

        if (!File.Exists(path))
        {
            Debug.LogError("Texture not found: " + path);
            return null;
        }

        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(
            2,
            2,
            TextureFormat.RGBA32,
            false
        );

        if (!texture.LoadImage(fileData))
        {
            Debug.LogError("Failed to load texture: " + path);
            return null;
        }

        texture.name = Path.GetFileNameWithoutExtension(path);

        // Atlas settings
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.anisoLevel = 0;

        return texture;
    }

    public void GetTileUV(
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

    Texture2D GetTileTexture(Texture2D atlas, TextureAtlasTextureCoordinates textureCoordinates)
    {
        int x = textureCoordinates.texturesCoordinatesX * textureCoordinates.tileSizeX;
        int y = textureCoordinates.texturesCoordinatesY * textureCoordinates.tileSizeY;

        int width = textureCoordinates.tileSizeX;
        int height = textureCoordinates.tileSizeY;

        Color[] pixels = atlas.GetPixels(x, y, width, height);

        Texture2D tileTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        tileTexture.SetPixels(pixels);
        tileTexture.Apply();

        return tileTexture;
    }

}


[Serializable]
public class TextureData
{
    [SerializeField] private int id;
    [SerializeField] private int idAtlas;
    [SerializeField] private int textureSizeX;
    [SerializeField] private int textureSizeY;
    [SerializeField] private int tileSizeX;
    [SerializeField] private int tileSizeY;
    [SerializeField] private int texturesCoordinatesX;
    [SerializeField] private int texturesCoordinatesY;

    public int Id => id;
    public int IdAtlas => idAtlas;
    public int TextureSizeX => textureSizeX;
    public int TextureSizeY => textureSizeY;
    public int TileSizeX => tileSizeX;
    public int TileSizeY => tileSizeY;
    public int TexturesCoordinatesX => texturesCoordinatesX;
    public int TexturesCoordinatesY => texturesCoordinatesY;
}

[Serializable]
public class TextureDataArray
{
    public TextureData[] textures;
}

public class TextureAtlasTextureCoordinates
{
    public int id;
    public int idAtlas;
    public int textureSizeX;
    public int textureSizeY;
    public int tileSizeX;
    public int tileSizeY;
    public int texturesCoordinatesX;
    public int texturesCoordinatesY;
    public readonly Texture2D texture;

    public TextureAtlasTextureCoordinates(
        int id,
        int textureSizeX,
        int textureSizeY,
        int tileSizeX,
        int tileSizeY,
        int texturesCoordinatesX,
        int texturesCoordinatesY,
        Texture2D texture)
    {
        this.id = id;
        this.textureSizeX = textureSizeX;
        this.textureSizeY = textureSizeY;
        this.tileSizeX = tileSizeX;
        this.tileSizeY = tileSizeY;
        this.texturesCoordinatesX = texturesCoordinatesX;
        this.texturesCoordinatesY = texturesCoordinatesY;

        this.texture = texture;
    }
}

[Serializable]
public class TextureAtlasData
{
    public int id;
    public string path;
}

[Serializable]
public class TextureAtlasDataArray
{
    public TextureAtlasData[] textures;
}
