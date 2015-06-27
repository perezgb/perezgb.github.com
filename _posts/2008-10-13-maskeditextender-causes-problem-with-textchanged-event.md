---
layout: post
title: "MaskEditExtender causes problem with TextChanged event"
description: ""
date: 2008-10-13 12:00:00 UTC
category: 
tags: [asp.net, ajaxcontroltoolkit, .net, maskedit]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Have you ever used the Ajax ControlToolkit MaskedEdit control? Last week I  found an unexpected behavior with it.</p>
<p>Page:</p>
<pre title="code" class="brush: csharp">
&lt;form id=&quot;form1&quot; runat=&quot;server&quot;&gt;
    &lt;div&gt;
    &lt;asp:ScriptManager ID=&quot;ScriptManager1&quot; runat=&quot;server&quot;&gt;
        &lt;/asp:ScriptManager&gt;

        &lt;asp:TextBox ID=&quot;TextBox1&quot; runat=&quot;server&quot; ontextchanged=&quot;TextBox1_TextChanged&quot;&gt;&lt;/asp:TextBox&gt;
        &lt;cc1:MaskedEditExtender ID=&quot;TextBox1_MaskedEditExtender&quot; runat=&quot;server&quot; 
            CultureAMPMPlaceholder=&quot;&quot; CultureCurrencySymbolPlaceholder=&quot;&quot; 
            CultureDateFormat=&quot;&quot; CultureDatePlaceholder=&quot;&quot; CultureDecimalPlaceholder=&quot;&quot; 
            CultureThousandsPlaceholder=&quot;&quot; CultureTimePlaceholder=&quot;&quot; Enabled=&quot;True&quot; 
            Mask=&quot;99-99-99&quot; TargetControlID=&quot;TextBox1&quot; ClearMaskOnLostFocus=&quot;False&quot; 
            MaskType=&quot;Number&quot;&gt;
        &lt;/cc1:MaskedEditExtender&gt;
        &lt;asp:Button ID=&quot;Button1&quot; runat=&quot;server&quot; onclick=&quot;Button1_Click&quot; Text=&quot;Button&quot; /&gt;

        &lt;asp:Button ID=&quot;Button2&quot; runat=&quot;server&quot; onclick=&quot;Button2_Click&quot; Text=&quot;Button&quot; /&gt;

    &lt;/div&gt;
    &lt;/form&gt;

</pre>
<p>CodeBehind:</p>
<pre title="code" class="brush: csharp">
 protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = &quot;101208&quot;;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }
</pre>
<p>This page has one edit with a MaskEditExtender and two buttons. On the first  button click I change the value of the TextBox. On the second button click I do  nothing. If I click the first button then the second, the TextChanged event of  the edit will fire. Funny, right? Here is the reason.</p>
<p>Let&rsquo;s say you have a mask like this: 99-99-99. To apply the value to the  textbox you do:</p>
<pre><code><br />TextBox1.Text = &quot;101208&quot;;<br /></code>
</pre>
<p>The resulting value on the web page will be: 10-12-08. Here is the catch: The  mask is applied to the textbox only after the page is rendered using JavaScript.  So the value for the control in the ViewState is 101208 and the value in the  page is 10-12-08. When another postback occurs the framwork will detect that the  value in the page is different from the value in the ViewState therefore it  fires the TextChanged event when in fact the value hasn&rsquo;t changed.</p>
<p>You can  avoid this behavior by setting the value to the TextBox already with masked  value.</p>
<pre><code><br />TextBox1.Text = &quot;10-12-08&quot;;<br /></code>
</pre>
<p>I hope this can help someone out there.</p>
</div>