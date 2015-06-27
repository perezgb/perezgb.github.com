---
layout: post
title: "Exposing events in Web User Controls"
description: ""
date: 2008-05-17 12:00:00 UTC
category: 
tags: [asp.net, user, controls]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Web User Controls are a way encapsulating code that would have to be repeated  in various pages, they are definitely a huge help.</p>
<p>VisualStudio makes so simple to create these user controls that even novice  users can do it. When the page doesn&rsquo;t need to interact with the user control it  is really simple, and there&rsquo;s nothing to it but putting the controls you want in  the user control. However the controls that are inside the user control are  isolated from the Page and you can&rsquo;t use it&rsquo;s events. This post will show you  how easy it is to overcome this issue.</p>
<p>Let&rsquo;s say we want to create a simple control with a DropDownList, a Button  and a Label. When the button is clicked the selected value in the DropDownList  is copied to the label. Here&rsquo;s the code for it:</p>
<pre title="code" class="brush: csharp">
&lt;%@ Control Language=&quot;C#&quot; ClassName=&quot;MyUserControl&quot; %&gt;

&lt;script runat=&quot;server&quot;&gt;

    protected void ButtonCopy_Click(object sender, EventArgs e)
    {
        LabelText.Text = DropDownListNames.SelectedValue;
    }
&lt;/script&gt;
&lt;asp:DropDownList ID=&quot;DropDownListNames&quot; runat=&quot;server&quot;&gt;
    &lt;asp:ListItem&gt;Elisa&lt;/asp:ListItem&gt;
    &lt;asp:ListItem&gt;Gabriel&lt;/asp:ListItem&gt;
    &lt;asp:ListItem&gt;Rafaela&lt;/asp:ListItem&gt;
&lt;/asp:DropDownList&gt;
&lt;br /&gt;
&lt;br /&gt;
&lt;asp:Button ID=&quot;ButtonCopy&quot; runat=&quot;server&quot; Text=&quot;Copy SelectedValue to Label&quot; 
    onclick=&quot;ButtonCopy_Click&quot; /&gt;
&lt;br /&gt;
&lt;br /&gt;
&lt;asp:Label ID=&quot;LabelText&quot; runat=&quot;server&quot; Text=&quot;Label&quot; Font-Size=&quot;X-Large&quot;&gt;&lt;/asp:Label&gt;

This is simple enough, right? All you have to do is place this control in any page and this behavior is going to be replicated. What I notice that most novice programmers miss is if you need the control to send some kind of information to the page.

Say you want to have a label in your page showing the time that the Copy button of the user control is clicked. It would be easy if you could access the button click event of the button inside the user control but unfortunately you can&rsquo;t. What you want is to make the event of the inner button accessible to outside the control, this is done creating a new event exposing the event you want.


&lt;%@ Control Language=&quot;C#&quot; ClassName=&quot;MyUserControl&quot; %&gt;

&lt;script runat=&quot;server&quot;&gt;

    //this is the event that will be exposed
    public event EventHandler ButtonClick;

    protected void ButtonCopy_Click(object sender, EventArgs e)
    {
        LabelText.Text = DropDownListNames.SelectedValue;

        //this tests if the event has been subscribed by any method
        if (ButtonClick != null)
        {
            //fires the event passing the same arguments of the button
            //click event
            ButtonClick(sender, e);
        }
    }
&lt;/script&gt;
&lt;asp:DropDownList ID=&quot;DropDownListNames&quot; runat=&quot;server&quot;&gt;
    &lt;asp:ListItem&gt;Elisa&lt;/asp:ListItem&gt;
    &lt;asp:ListItem&gt;Gabriel&lt;/asp:ListItem&gt;
    &lt;asp:ListItem&gt;Rafaela&lt;/asp:ListItem&gt;
&lt;/asp:DropDownList&gt;
&lt;br /&gt;
&lt;br /&gt;
&lt;asp:Button ID=&quot;ButtonCopy&quot; runat=&quot;server&quot; Text=&quot;Copy SelectedValue to Label&quot; 
    onclick=&quot;ButtonCopy_Click&quot; /&gt;
&lt;br /&gt;
&lt;br /&gt;
&lt;asp:Label ID=&quot;LabelText&quot; runat=&quot;server&quot; Text=&quot;Label&quot; Font-Size=&quot;X-Large&quot;&gt;&lt;/asp:Label&gt;</pre>
<p>Now you are able to put the control in a page and use it&rsquo;s event, like in the  following example:</p>
<pre title="code" class="brush: csharp">
&lt;%@ Page Language=&quot;C#&quot; %&gt;

&lt;%@ Register Src=&quot;MyUserControl.ascx&quot; TagName=&quot;MyUserControl&quot; TagPrefix=&quot;uc1&quot; %&gt;
&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;

&lt;script runat=&quot;server&quot;&gt;
    protected void MyUserControl1_ButtonClick(object sender, EventArgs e)
    {
        LabelTime.Text = DateTime.Now.ToString();
    }
&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
        &lt;!-- Notice here that I use ButtonClick event I just created in the user control --&gt;
        &lt;!-- Also notice that I didn't misspell the event, the On is added by a VisualStudio convention   --&gt;
        &lt;uc1:MyUserControl ID=&quot;MyUserControl1&quot; runat=&quot;server&quot; OnButtonClick=&quot;MyUserControl1_ButtonClick&quot; /&gt;
        &lt;br /&gt;
        &lt;asp:Label ID=&quot;LabelTime&quot; runat=&quot;server&quot; Text=&quot;Label&quot; Font-Bold=&quot;True&quot; 
            Font-Italic=&quot;True&quot; ForeColor=&quot;#FF3300&quot;&gt;&lt;/asp:Label&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;

</pre>
<p>You could also subscribe to the event in the code behind just like you do  with asp.net controls. Here is the code for it:</p>
<pre title="code" class="brush: csharp">
&lt;%@ Page Language=&quot;C#&quot; %&gt;

&lt;%@ Register Src=&quot;MyUserControl.ascx&quot; TagName=&quot;MyUserControl&quot; TagPrefix=&quot;uc1&quot; %&gt;
&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;

&lt;script runat=&quot;server&quot;&gt;
    public void Page_Load(object sender, EventArgs e)
    {
        MyUserControl1.ButtonClick += MyUserControl1_ButtonClick;
    }

    protected void MyUserControl1_ButtonClick(object sender, EventArgs e)
    {
        LabelTime.Text = DateTime.Now.ToString();
    }
&lt;/script&gt;

&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
&lt;head runat=&quot;server&quot;&gt;
    &lt;title&gt;Untitled Page&lt;/title&gt;
&lt;/head&gt;
&lt;body&gt;
    &lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
        &lt;!-- Notice here that I use ButtonClick event I just created in the user control --&gt;
        &lt;!-- Also notice that I didn't misspell the event, the On is added by a VisualStudio convention   --&gt;
        &lt;uc1:MyUserControl ID=&quot;MyUserControl1&quot; runat=&quot;server&quot; /&gt;
        &lt;br /&gt;
        &lt;asp:Label ID=&quot;LabelTime&quot; runat=&quot;server&quot; Text=&quot;Label&quot; Font-Bold=&quot;True&quot; 
            Font-Italic=&quot;True&quot; ForeColor=&quot;#FF3300&quot;&gt;&lt;/asp:Label&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;
</pre>
<p>I wrote this post because I helped a few people in the <a href="http://forums.asp.net/">asp.net forums</a> and I hope it can help other  people that are beginning to write web user controls. Happy coding guys.</p>
</div>