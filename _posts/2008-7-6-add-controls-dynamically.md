---
layout: post
title: "Add Controls Dynamically"
description: ""
date: 2008-07-06 12:00:00 UTC
category: 
tags: [asp.net, dynamic, controls]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Dynamic controls are an interesting topic that keep showing up in the asp.net forums. I&rsquo;ll try to explain the main points about creating and working with dynamically created controls in a few articles (this one would be too long otherwise).</p>
First of all there are two points you need to understand:
<ol>
    <li>Adding the controls dynamically is easy, the tricky part is getting them to stick around.</li>
    <li>The controls are not kept between postbacks therefore you have to keep recreating them every time there is a postback.</li>
    <li>The best place to recreate the controls in the Page_Load or Page_Init events.</li>
</ol>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Creating a new control is easy, it&rsquo;s just a matter of instantiate the class. Adding the control to the page is also easy you only need to add the new control to the page controls.</p>
<pre title="code" class="brush: csharp">
//create a new textbox
TextBox tb = new TextBox();
//add the new textbox to the page
Page.Form.Controls.Add(tb);
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">You&rsquo;ve seen how easy it is to add the controls but if you don&rsquo;t keep recreating them when you submit the page the dynamic controls will be gone.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">First understand that this need to be recreated is not exclusive for dynamic created controls. Every time there is a postback the whole page is recreated. The difference is that all the controls that are declared in the aspx page are created automatically what gives the user the impression that the controls are always there (they are not!).</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Since dynamically created controls are not hard coded anywhere (obviously) you are the one responsible for recreating them.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">If you create a control in the Page_Load event you&rsquo;ll notice that it works just like any control already in the aspx page:</p>
<pre title="code" class="brush: xhtml">
&lt;script runat=&quot;server&quot;&gt;

    void Page_Load(object sender, EventArgs e)
    {
        TextBox tb = new TextBox();
        Page.Form.Controls.Add(tb);
    }
&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;

    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">Granted that if you could create the control like this you might declare it in the aspx page as well. Ok, so let&rsquo;s improve our example and create a button that adds one and only one edit to the page. If the edit already exists the button won&rsquo;t do any thing.</span></p>
<pre title="code" class="brush: csharp">
&lt;script runat=&quot;server&quot;&gt;

    void Page_Load(object sender, EventArgs e)
    {
        //means that the control has been created already so you 
        //need to recreate it
        if (ViewState[&quot;tb&quot;] != null)
        {
            createDynamicControls();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //means the control has not been created yet
        //create the control and store the viewstate
        if (ViewState[&quot;tb&quot;] == null)
        {
            createDynamicControls();
            ViewState[&quot;tb&quot;] = true;
        }        
    }

    //this method takes care of creating the controls
    private void createDynamicControls()
    {
        TextBox tb = new TextBox();
        Page.Form.Controls.Add(tb);
    }

&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
        &lt;asp:Button ID=&quot;Button1&quot; runat=&quot;server&quot; Text=&quot;Add TextBox&quot; 
            onclick=&quot;Button1_Click&quot; /&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Notice that in this version of the code I created a method responsible for creating the dynamic control. This will help to encapsulate the method creation and avoid duplicating code. Also notice that I&rsquo;m using the ViewState to help maintain a variable that indicates if the control has been added yet in order to avoid duplication.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">When the user clicks the button I check my ViewState variable to see if the TextBox has been created, if it hasn&rsquo;t I create the TextBox, add it to the page and set the ViewState variable. Now, as long as you don&rsquo;t leave the page the control and it&rsquo;s state will be maintained.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">We could also let the user add an undefined number of TextBoxes to the page making some small changes.</p>
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
        Page.Form.Controls.Add(tb);
    }
&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
        &lt;asp:Button ID=&quot;Button1&quot; runat=&quot;server&quot; Text=&quot;Add TextBox&quot; 
            onclick=&quot;Button1_Click&quot; /&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Now you are able to add any number of controls keeping the state of them all. On my next post I&rsquo;ll keep working with dynamic created controls so stay tuned.</p>
Related posts:
<ul>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/6/add-controls-dynamically">Add Controls Dynamically &ndash; Part 1</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/14/add-controls-dynamically-part-2">Add Controls Dynamically &ndash; Part 2</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/22/add-controls-dynamically-part-3">Add Controls Dynamically &ndash; Part 3</a></li>
    <li><a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://www.perezgb.com/2008/7/22/add-controls-dynamically-part-4">Add Controls Dynamically &ndash; Part 4</a></li>
</ul>
</span></p>
</span></p>
</span></p>
</span></p>
</div>