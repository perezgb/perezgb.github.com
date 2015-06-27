---
layout: post
title: "Compressing Files and Folders with SharpZipLib"
description: ""
date: 2009-11-10 12:00:00 UTC
category: 
tags: [c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>I have found a lot of examples on how to compress files using C# but only a few that would compress all files and folders (including subfolders). I found an example in this <a href="http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/6f594844-efcb-41d9-95af-8f3d83eb290d">forum </a>that show how to do it using <a href="http://www.icsharpcode.net/OpenSource/SharpZipLib/Download.aspx">SharpZipLib</a>.</p>
<p>Here is what I did with it:</p>
<pre title="code" class="brush: csharp">
static void Main(string[] args)
{
    ZipOutputStream zip = new ZipOutputStream(File.Create(@&quot;d:\my.zip&quot;));
    zip.SetLevel(9);
    string folder = @&quot;D:\Work\AnyDirectory\&quot;;
    ZipFolder(folder, folder, zip);
    zip.Finish();
    zip.Close();
}

public static void ZipFolder(string RootFolder, string CurrentFolder, 
    ZipOutputStream zStream)
{
    string[] SubFolders = Directory.GetDirectories(CurrentFolder);

    //calls the method recursively for each subfolder
    foreach (string Folder in SubFolders)
    {
        ZipFolder(RootFolder, Folder, zStream);
    }

    string relativePath = CurrentFolder.Substring(RootFolder.Length) + &quot;/&quot;;

    //the path &quot;/&quot; is not added or a folder will be created
    //at the root of the file
    if (relativePath.Length &gt; 1)
    {
        ZipEntry dirEntry;
        dirEntry = new ZipEntry(relativePath);
        dirEntry.DateTime = DateTime.Now;
    }

    //adds all the files in the folder to the zip
    foreach (string file in Directory.GetFiles(CurrentFolder))
    {
        AddFileToZip(zStream, relativePath, file);
    }
}

private static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
{
    byte[] buffer = new byte[4096];

    //the relative path is added to the file in order to place the file within
    //this directory in the zip
    string fileRelativePath = (relativePath.Length &gt; 1 ? relativePath : string.Empty) 
                              + Path.GetFileName(file);

    ZipEntry entry = new ZipEntry(fileRelativePath);
    entry.DateTime = DateTime.Now;
    zStream.PutNextEntry(entry);

    using (FileStream fs = File.OpenRead(file))
    {
        int sourceBytes;
        do
        {
            sourceBytes = fs.Read(buffer, 0, buffer.Length);
            zStream.Write(buffer, 0, sourceBytes);
        } while (sourceBytes &gt; 0);
    }
}</pre>
<p>This is not hard but I had to figure out that the file names needed to have the relative path before it in order to correctly place them in the same folders within the compressed zip file.</p>
<p>I hope this helps!</p>
</div>