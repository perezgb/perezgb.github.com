---
layout: post
title: "ASP.NET MVC: ListBox"
description: ""
date: 2008-11-06 12:00:00 UTC
category: 
tags: [asp.net, mvc]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The Html.ListBox helper is a good choice if you need to represent  many-to-many relationships in a form.</p>
<p>Lets say you have a many-to-many between the Posts table and the Categories  table. When you are creating a new post you need to select all the categories  that it belongs to. When you are editing a post you need to show the categories  it already belongs to in order to make a new selection (or not).</p>
<p><img alt="" src="http://www.perezgb.com/upload/PostCategory.jpg" /></p>
<p>&nbsp;The ListBox would allow you to make all the selections necessary, so the Action  and the View for the New Post would need something like this:</p>
<p>Action:</p>
<pre class="brush: csharp" title="code">
[AcceptVerbs(&quot;GET&quot;)]
public ActionResult New()
{
        ViewData[&quot;Categories&quot;] = _postService.GetCategories();
        return View();
}
</pre>
<p>View:</p>
<pre class="brush: csharp" title="code">
&lt;%= Html.ListBox(&quot;CategoryList&quot;, new MultiSelectList((IList&lt;Category&gt;)ViewData[&quot;Categories&quot;], &quot;ID&quot;, &quot;Name&quot;))%&gt;

</pre>
<p>And the View and Action for the Edit would be like this:</p>
<p>Action:</p>
<pre class="brush: csharp" title="code">
[AcceptVerbs(&quot;GET&quot;)]
public ActionResult Edit(int? id)
{
        int postId = id ?? 0;
        //get the post that is being edited
        Post post = _postService.GetPost(postId);
        //get all the categories
        ViewData[&quot;Categories&quot;] = _postService.GetCategories();
        //get the id's of the categories to which the post belongs
        ViewData[&quot;CategoryIDs&quot;] = post.Categories.Select(c =&gt; c.ID);
        return View(post);
}</pre>
<p>View:</p>
<pre class="brush: csharp" title="code">
&lt;%= Html.ListBox(&quot;CategoryList&quot;, new MultiSelectList((IList&lt;Category&gt;)ViewData[&quot;Categories&quot;], &quot;ID&quot;, &quot;Name&quot;, (IEnumerable&lt;int&gt;)ViewData[&quot;CategoryIDs&quot;]))%&gt;

</pre>
<p>Note that this time I had to pass an extra parameter to the ListBox method  which is a list of the categories ID&rsquo;s that are already associated with the  Post. This list will be used to make the initial selection in the ListBox  rendered in your <span class="caps">HTML</span>.</p>
<p>One last thing you need to know is how to get the ID&rsquo;s of the selected  categories back in your action. This is pretty simple as well. The Form will  have a CategoryList item that will have a comma separated string with all the  selected lines, all you need to do is split this in a array of strings and them  save them to the database as you see fit.</p>
<pre class="brush: csharp" title="code">
string[] selected = Request.Form[&quot;CategoryList&quot;].Split(',');
</pre>
<p>I hope this tip is useful to others that are testing the <span class="caps">MVC</span> Framework.</p>
</div>