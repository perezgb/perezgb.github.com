---
layout: post
title: "Add Controls Dynamically - Part 4"
description: ""
date: 2008-07-22 12:00:00 UTC
category: 
tags: [controls, asp.net, dynamic]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The part 3 of this series was getting a bit long for my taste so I decided to  break it in 2. Here we&rsquo;ll continue dealing with the controls added dynamically  to our page only now we&rsquo;re going to access them by ID.</p>
<p>Related posts:</p>
<ul>
    <li><a href="http://www.gbogea.com/2008/7/6/add-controls-dynamically">Add  Controls Dynamically &ndash; Part 1</a></li>
    <li><a href="http://www.gbogea.com/2008/7/14/add-controls-dynamically-part-2">Add  Controls Dynamically &ndash; Part 2</a></li>
    <li><a href="http://www.gbogea.com/2008/7/22/add-controls-dynamically-part-3">Add  Controls Dynamically &ndash; Part 3</a></li>
    <li><a href="http://www.gbogea.com/2008/7/22/add-controls-dynamically-part-4">Add  Controls Dynamically &ndash; Part 4</a></li>
    <li><a href="http://www.gbogea.com/2008/7/27/add-controls-dynamically-part-5">Add  Controls Dynamically &ndash; Part 5</a></li>
</ul>
<h2>Naming the controls</h2>
<p>By now you have noticed that we haven&rsquo;t explicitly set the ID of any of our  controls. Of course the controls have ID&rsquo;s but they are defined automatically by  the Asp.Net framework when the controls are instantiated. The reason this works  is because the framework will create the names consistently the same, again and  again as long as you create the controls always in the same order (we are doing  that).</p>
<p>It is this automatically naming feature that allows our components to keep  their state between requests. When the page is submitted all of the controls  states are sent back from the browser to the server, but the server doesn&rsquo;t know  how to create the controls that&rsquo;s why we had to do it every time the page is  submitted. After the control has been created the Asp.Net framework is able to  get it&rsquo;s state from the page based on it&rsquo;s name.</p>
<p>Ok, enough theory. Let&rsquo;s give our controls and ID manually. Since the  controls are added dynamically the ID&rsquo;s have to be something that can be  predicted. Let&rsquo;s say: TextBox + a number. For this will change the  <i>createDynamicControls</i> method to add a parameter which will be the number  of the control added.</p>
<pre title="code" class="brush: csharp">
//this method takes care of creating the controls
    private void createDynamicControls(int count)
    {
        TextBox tb = new TextBox();
        tb.TextChanged += TextBox_TextChanged;
        //now we set the control ID manually
        tb.ID = &quot;TextBox&quot; + count;
        PlaceHolder1.Controls.Add(tb);
        controlsList.Add(tb);
    }

</pre>
<p>When the add control button is clicked we need to pass the number to the method.  We can use the control count that we&rsquo;re storing in the ViewState.</p>
<pre title="code" class="brush: csharp">
protected void Button1_Click(object sender, EventArgs e)
    {
        createDynamicControls((int)ViewState[&quot;count&quot;]);
        //increment the number of controls
        ViewState[&quot;count&quot;] = (int)ViewState[&quot;count&quot;] + 1;
    }
</pre>
<p>Another place we need to change is the Page_Load event when the controls are  re-created. Here for loop when we pass in the new parameter:</p>
<pre title="code" class="brush: csharp">
 for (int i = 0; i &lt; controlCount; i++)
            {
                //notice that now we pass the i as parameter
                createDynamicControls(i);
            }

</pre>
<p>To test our new functionality we will add TextBox, a Button and a Label. We are  going to inform the ID of the TextBox we want to get the value for. When the  button is clicked the event will get the control and display it&rsquo;s value in the  label.</p>
<pre title="code" class="brush: csharp">
&lt;asp:TextBox ID=&quot;TextBoxControlId&quot; runat=&quot;server&quot;&gt;&lt;/asp:TextBox&gt;
        &lt;asp:button ID=&quot;ButtonGetValue&quot; runat=&quot;server&quot; text=&quot;Get value&quot; 
            onclick=&quot;ButtonGetValue_Click&quot; /&gt;
        Valor: &lt;asp:Label ID=&quot;LabelValue&quot; runat=&quot;server&quot; Text=&quot;&quot;&gt;&lt;/asp:Label&gt;

</pre>
<p>And here is the event where we get the TextBox we want:</p>
<pre title="code" class="brush: csharp">
protected void ButtonGetValue_Click(object sender, EventArgs e)
    {
        //find the control we want using the name informed by the user in the 
        //TextBoxControlId control
        TextBox tb = (TextBox)PlaceHolder1.FindControl(TextBoxControlId.Text);
        LabelValue.Text = tb.Text;
    }

</pre>
<p>Now if you add 3 TextBoxes (which will have id&rsquo;s TextBox0, TextBox1 and  TextBox2) and you input TextBox0 in the interface and hit the button the  LabelValue control will show the value in the first TextBox.</p>
<p>This one was a bit longer than I would like to so I&rsquo;ll try to make the next  article shorter.</p>
<p>Here is the final version of our page after all of our modifications:</p>
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
                //notice that now we pass the i as parameter
                createDynamicControls(i);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        createDynamicControls((int)ViewState[&quot;count&quot;]);
        //increment the number of controls
        ViewState[&quot;count&quot;] = (int)ViewState[&quot;count&quot;] + 1;
    }

    //this method takes care of creating the controls
    private void createDynamicControls(int count)
    {
        TextBox tb = new TextBox();
        tb.TextChanged += TextBox_TextChanged;
        //now we set the control ID manually
        tb.ID = &quot;TextBox&quot; + count;
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

    protected void ButtonGetValue_Click(object sender, EventArgs e)
    {
        //find the control we want using the name informed by the user in the 
        //TextBoxControlId control
        TextBox tb = (TextBox)PlaceHolder1.FindControl(TextBoxControlId.Text);
        LabelValue.Text = tb.Text;
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
        &lt;br /&gt;
        &lt;br /&gt;
        &lt;asp:TextBox ID=&quot;TextBoxControlId&quot; runat=&quot;server&quot;&gt;&lt;/asp:TextBox&gt;
        &lt;asp:button ID=&quot;ButtonGetValue&quot; runat=&quot;server&quot; text=&quot;Get value&quot; 
            onclick=&quot;ButtonGetValue_Click&quot; /&gt;
        Value: &lt;asp:Label ID=&quot;LabelValue&quot; runat=&quot;server&quot; Text=&quot;&quot;&gt;&lt;/asp:Label&gt;
    &lt;/div&gt;
    &lt;/form&gt;
&lt;/body&gt;
&lt;/html&gt;

</pre>
<p>See you next time.</p>
</div>