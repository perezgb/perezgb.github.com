---
layout: post
title: "Add Controls Dynamically - Part 2"
description: ""
date: 2008-07-14 12:00:00 UTC
category: 
tags: [controls, asp.net, dynamic]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">This post continues what&rsquo;s been done in the<span class="Apple-converted-space">&nbsp;</span><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/6/add-controls-dynamically">Part 1</a></p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">On my last post I talked about creating controls dynamically in asp.net. In this post I&rsquo;m going to continue with common issues or needs that users have based on what I noticed on the<span class="Apple-converted-space">&nbsp;</span><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://forums.asp.net/">Asp.Net Forums</a></p>
Related posts:
<ul>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/6/add-controls-dynamically">Add Controls Dynamically &ndash; Part 1</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/14/add-controls-dynamically-part-2">Add Controls Dynamically &ndash; Part 2</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/22/add-controls-dynamically-part-3">Add Controls Dynamically &ndash; Part 3</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/22/add-controls-dynamically-part-4">Add Controls Dynamically &ndash; Part 4</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/27/add-controls-dynamically-part-5">Add Controls Dynamically &ndash; Part 5</a></li>
</ul>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">First I want to talk about assigning events to the controls that we are creating, this is a simple task but still and important topic.</p>
<h3 style="border-bottom: 1px solid rgb(234, 234, 234); margin: 1.2em 0px 1em; padding: 0px 0px 0.2em; font-weight: normal; font-family: 'Trebuchet MS',Verdana,sans-serif; font-size: 130%; color: rgb(125, 0, 10);">Adding Events</h3>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">The first thing we need to do is create a method that will be the handler for the event. When the event is fired on the control this is the method we want to be called to handle the event. In our example we are using TextBoxes so we want to handle the TextChanged event and make the background of the control yellow. Here is the method:</p>
<pre title="code" class="brush: csharp">
private void TextBox_TextChanged(object sender, EventArgs e)
    {
        //the sender is the control that fired the event
        //that is the control we want to change the color
        TextBox tb = (TextBox)sender;
        tb.BackColor = System.Drawing.Color.Yellow ;
    }
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">The name of the method is irrelevant but it&rsquo;s return type and parameters are not. If you create an event through VisualStudio you&rsquo;ll notice that the signature of the method is the same.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Now that we have the method we need to set this method as the handler for the event on the controls that we are creating. Adding the method to the controls handler is as simples as this:</p>
<pre title="code" class="brush: csharp">
tb.TextChanged += TextBox_TextChanged;
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">If you add your controls now all of them will have this handler and it&rsquo;s background will turn yellow (after submit) if you change the content of the textbox.</p>
<h3 style="border-bottom: 1px solid rgb(234, 234, 234); margin: 1.2em 0px 1em; padding: 0px 0px 0.2em; font-weight: normal; font-family: 'Trebuchet MS',Verdana,sans-serif; font-size: 130%; color: rgb(125, 0, 10);">PlaceHolder</h3>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Up until now we&rsquo;ve been adding the controls directly to the Form. This will cause the controls to be added to the end of the page after all other controls. This is fine as an example but might not be you desired result. We will you the PlaceHolder control as a container to our controls, this way we can put the PlaceHolder wherever we want in the page and the controls will be added within it.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Another advantage of the PlaceHolder control is that it doesn&rsquo;t generate any html, it&rsquo;s just the to serve (as it&rsquo;s own name says) as a place holder. So you don&rsquo;t need to worry about adding extra html just to have a place to put your controls in.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">In our example I will put the PlaceHolder before the button that we are using just to show that the controls will no longer be added to the end of the page.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">The new page with the events and the PlaceHolder control will be like this:</p>
<pre title="code" class="brush: csharp">
&lt;script runat=&quot;server&quot;&gt;

    void Page_Load(object sender, EventArgs e)
    {
        //when the user first enters the page set the count as zero
        if (!IsPostBack)
        {
            ViewState[&quot;count&quot;] = 0;
        }
        //on every subsequent postback, check the control count
        //and recreated all the controls
        else
        {
            int controlCount = (int)ViewState[&quot;count&quot;];
            for (int i = 0; i &lt; controlCount; i++)
            {
                createDynamicControls();
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        createDynamicControls();
        //increment the number of controls
        ViewState[&quot;count&quot;] = (int)ViewState[&quot;count&quot;] + 1;        
    }

    //this method takes care of creating the controls
    private void createDynamicControls()
    {
        TextBox tb = new TextBox();
        tb.TextChanged += TextBox_TextChanged;
        PlaceHolder1.Controls.Add(tb);
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
        //the sender is the control that fired the event
        //that is the control we want to change the color
        TextBox tb = (TextBox)sender;
        tb.BackColor = System.Drawing.Color.Yellow ;
    }
&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head id=&quot;Head1&quot; runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
        &lt;asp:PlaceHolder ID=&quot;PlaceHolder1&quot; runat=&quot;server&quot;&gt;&lt;/asp:PlaceHolder&gt;
        &lt;asp:Button ID=&quot;Button1&quot; runat=&quot;server&quot; Text=&quot;Add TextBox&quot; 
            onclick=&quot;Button1_Click&quot; /&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">Stay tuned for more about dynamically created controls.</span></p>
</span></p>
</span></p>
</span></p>
</div>