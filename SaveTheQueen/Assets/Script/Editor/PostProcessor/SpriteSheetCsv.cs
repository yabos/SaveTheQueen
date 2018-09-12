using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SpriteSheetCsv
{
    public int id;
    public string assetName;
    public string spriteName;
    public int numIndex;
    public int numX;
    public int numY;

    public static string HeaderString()
    {
        return string.Format("id,assetName,spriteName,numIndex,numX,numY");
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4},{5}",id,assetName,spriteName,numIndex,numX,numY); 
    }
}
public class ScriptCsv
{
    private List<SpriteSheetCsv> m_lstValue = new List<SpriteSheetCsv>();
    protected string m_path;

    public ScriptCsv(string path)
    {
        m_path = path;
    }
    
    public List<SpriteSheetCsv> LstValue
    {
        get { return m_lstValue; }
    }
    
    public bool Save()
    {
        string[] strContent = new string[LstValue.Count + 1];
        strContent[0] = SpriteSheetCsv.HeaderString();
        for (int line = 0; line < LstValue.Count; line++)
        {
            strContent[line + 1] += LstValue[line].ToString();
        }
        File.WriteAllLines(m_path, strContent);

        return true;
    }
}