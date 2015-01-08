using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
 
/// <summary>
/// レイヤー名を定数で管理するクラスを作成するスクリプト
/// </summary>
public static class LayerNameCreator{
 
    private const string ITEM_NAME  = "Tools/Create/Layer Name";    // コマンド名
    private const string PATH       = "Assets/LayerName.cs";        // ファイルパス
 
    private static readonly string FILENAME                     = Path.GetFileName(PATH);                   // ファイル名(拡張子あり)
    private static readonly string FILENAME_WITHOUT_EXTENSION   = Path.GetFileNameWithoutExtension(PATH);   // ファイル名(拡張子なし)
 
    /// <summary>
    /// レイヤー名を定数で管理するクラスを作成します
    /// </summary>
    [MenuItem(ITEM_NAME)]
    public static void Create()
    {
        if (!CanCreate())
        {
            return;
        }
 
        CreateScript();
        
        EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
    }
 
    /// <summary>
    /// スクリプトを作成します
    /// </summary>
    public static void CreateScript()
    {
        var builder = new StringBuilder();
        
        builder.AppendLine("/// <summary>");
        builder.AppendLine("/// レイヤー名を定数で管理するクラス");
        builder.AppendLine("/// </summary>");
        builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
        builder.AppendLine("{");
 
        foreach (var n in InternalEditorUtility.layers.
            Select(c => new { var = TrimInvalidChar(c), val = LayerMask.NameToLayer(c) }))
        {
            builder.Append("\t").AppendFormat(@"public const int {0} = {1};", n.var, n.val).AppendLine();
        }
        foreach (var n in InternalEditorUtility.layers.
            Select(c => new { var = TrimInvalidChar(c), val = 1 << LayerMask.NameToLayer(c) }))
        {
            builder.Append("\t").AppendFormat(@"public const int {0}Mask = {1};", n.var, n.val).AppendLine();
        }
 
        builder.AppendLine("}");
        
        var directoryName = Path.GetDirectoryName(PATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        
        File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }
 
    /// <summary>
    /// レイヤー名を定数で管理するクラスを作成できるかどうかを取得します
    /// </summary>
    [MenuItem(ITEM_NAME, true)]
    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }
 


    //使用できない文字を削除
    public static string TrimInvalidChar(string str){
        return str.Replace(" ", "");   //とりあえず空白だけでいいや
    }
}