---
layout: post
title: "Force File Download"
description: ""
date: 2008-05-21 12:00:00 UTC
category: 
tags: [asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>One handy feature that browser&rsquo;s offer is the ability to automatically open  know file types in their respective program. There are however times when you  want to force the user to download the file, you want the <strong>Save  As</strong> dialog box to appear to the user.</p>
<p>In order to avoid the default behavior and stop file from being open  automatically you can use the content-disposition header. This is an <span class="caps">HTTP</span> header, and is not asp.net specific.</p>
<table>
    <tbody>
        <tr>
            <td style="background: rgb(221, 221, 221) none repeat scroll 0% 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;"><strong>Content-disposition: attachment;  filename=fname.ext</strong></td>
        </tr>
    </tbody>
</table>
<p>To do this using asp.net resources you can use the Response object to set the  header like this:</p>
<table>
    <tbody>
        <tr>
            <td style="background: rgb(221, 221, 221) none repeat scroll 0% 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;"><strong>Response.AppendHeader(&ldquo;Content-disposition&rdquo;,  &ldquo;attachment; filename=&rdquo; + fileName);</strong></td>
        </tr>
    </tbody>
</table>
<p>It&rsquo;s very simple and very handy. I wrote a working example below:</p>
<pre title="code" class="brush: csharp">
&lt;%@ Page Language=&quot;C#&quot; %&gt;

&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;

&lt;script runat=&quot;server&quot;&gt;

    protected void Button1_Click(object sender, EventArgs e)
    {
        string fileName = &quot;MyFile.doc&quot;;
        string filePath = Server.MapPath(&quot;MyFiles/&quot;+fileName);
        Response.AppendHeader(&quot;Content-disposition&quot;, &quot;attachment; filename=&quot; + fileName);
        Response.ContentType = &quot;Application/msword&quot;;
        Response.WriteFile(filePath);
        Response.End();
    }
&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
        &lt;asp:Button ID=&quot;Button1&quot; runat=&quot;server&quot; Text=&quot;Download&quot; 
            onclick=&quot;Button1_Click&quot; /&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;

</pre>
<p>&nbsp;</p>
<p>&nbsp;</p>
</div>