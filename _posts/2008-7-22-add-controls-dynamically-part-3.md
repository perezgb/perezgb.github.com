---
layout: post
title: "Add Controls Dynamically - Part 3"
description: ""
date: 2008-07-22 12:00:00 UTC
category: 
tags: [controls, asp.net, dynamic]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>On the last article of this series we saw how we could assign event handlers  to dynamically created controls and how the <a href="http://www.google.com.br/url?sa=t&amp;ct=res&amp;cd=1&amp;url=http%3A%2F%2Fmsdn.microsoft.com%2Fen-us%2Flibrary%2Fsystem.web.ui.webcontrols.placeholder.aspx&amp;ei=1LyESM-HHZyQ8wTtoezkCw&amp;usg=AFQjCNHOxV3c6z18CTHqCAGcYa8_ySR--Q&amp;sig2=Tg1qyS0ac61UNUbmV5q4Wg">PlaceHolder  control</a> could help us add the controls in a more organized way.</p>
<p>Related  posts:</p>
<ul>
    <li><a href="http://www.gbogea.com/2008/7/6/add-controls-dynamically">Add  Controls Dynamically &ndash; Part 1</a></li>
    <li><a href="http://www.gbogea.com/2008/7/14/add-controls-dynamically-part-2">Add  Controls Dynamically &ndash; Part 2</a></li>
    <li><a href="http://www.gbogea.com/2008/7/22/add-controls-dynamically-part-3">Add  Controls Dynamically &ndash; Part 3</a></li>
    <li><a href="http://www.gbogea.com/2008/7/22/add-controls-dynamically-part-4">Add  Controls Dynamically &ndash; Part 4</a></li>
    <li><a href="http://www.gbogea.com/2008/7/27/add-controls-dynamically-part-5">Add  Controls Dynamically &ndash; Part 5</a></li>
</ul>
<p>In this article I want to show how you can keep a reference of a dynamically  created control so that you can use it later somewhere else in your code. For  this I&rsquo;ll continue using the code created add the end of the <a href="http://www.gbogea.com/2008/7/14/add-controls-dynamically-part-2">last  article of this series</a>.</p>
<p>Now that you can add as many TextBoxes to the form as you want let&rsquo;s say that  you want to use them to input numeric values and then add up all the values for  display. The first thing then is to decide where do we want to keep the  reference to the controls. I chose to use a generic list of TextBox  (List<textbox>) but you could have chosen another structure that you feel more  comfortable with.</textbox></p>
<pre title="code" class="brush: csharp">
System.Collections.Generic.List&lt;TextBox&gt; controlsList = new System.Collections.Generic.List&lt;TextBox&gt;();
</pre>
<p>Now that the list is declared and created the next step is deciding when we are  going to add the controls to the list. To me using the method that adds the  controls dynamically feels like the best choice, maybe someone has another idea  (please share if you do). So the method would look like this:</p>
<pre title="code" class="brush: csharp">
 private void createDynamicControls()
    {
        TextBox tb = new TextBox();
        tb.TextChanged += TextBox_TextChanged;
        PlaceHolder1.Controls.Add(tb);
        controlsList.Add(tb);
    }

</pre>
<p>Easy, right? Ok, the last step now is adding a button to the form that will call  the logic to add the values within all the controls and add a label that will  display the result of the addition. The logic for adding the values is very  simple, once you have a list with all the controls you only need to iterate  through the list, get the value of the control and add it to a variable. Here is  the code:</p>
<pre title="code" class="brush: csharp">
 protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        int value = 0;
        foreach (TextBox tb in controlsList)
        {
            value += int.Parse(tb.Text);
        }
        LabelTotal.Text = value.ToString();
    }

</pre>
<p>Please notice that I have assumed that all the TextBoxes are provided with valid  numeric values. We could improve the code to check for empty values and  conversion errors however this is not the main goal of this article. Here is a  complete version of the page after all our changes were made:</p>
<pre title="code" class="brush: csharp">
&lt;%@ Page Language=&quot;C#&quot; %&gt;

&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;

&lt;script runat=&quot;server&quot;&gt;

    System.Collections.Generic.List&lt;TextBox&gt; controlsList = new System.Collections.Generic.List&lt;TextBox&gt;();

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
        controlsList.Add(tb);
    }

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
        //the sender is the control that fired the event
        //that is the control we want to change the color
        TextBox tb = (TextBox)sender;
        tb.BackColor = System.Drawing.Color.Yellow ;
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        int value = 0;
        foreach (TextBox tb in controlsList)
        {
            value += int.Parse(tb.Text);
        }
        LabelTotal.Text = value.ToString();
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
        &lt;br /&gt;
        &lt;asp:Button ID=&quot;Button1&quot; runat=&quot;server&quot; Text=&quot;Add TextBox&quot; 
            onclick=&quot;Button1_Click&quot; /&gt;
        &lt;br /&gt;
        &lt;asp:Button ID=&quot;ButtonAdd&quot; runat=&quot;server&quot; Text=&quot;Add all values&quot; 
            onclick=&quot;ButtonAdd_Click&quot; /&gt;
        &lt;br /&gt;
        Total:&lt;asp:Label ID=&quot;LabelTotal&quot; runat=&quot;server&quot; Text=&quot;&quot;&gt;&lt;/asp:Label&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;

</pre>
<p>&nbsp;</p>
</div>