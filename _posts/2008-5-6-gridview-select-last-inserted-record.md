---
layout: post
title: "GridView: Select last inserted record"
description: ""
date: 2008-05-06 12:00:00 UTC
category: 
tags: [gridview, asp.net, .net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The other day a responded a post on the asp.net forums about this so I think  it might be useful to others.</p>
<p>Imagine if you have a GridView using pagination. When you insert a new record  using a DetailsView or a FormView you want this new record to be automatically  selected on the GridView, even if it&rsquo;s not at the same page (of the GridView)  that you currently have selected.</p>
<pre title="code" class="brush: csharp">
//returns a collection of records, including the one you just inserted
DataRowCollection drc = Dal.GetData();

//RecordID is the id of the record you just inserted, 
//you need to store this somewhere when you insert the record
DataRow dr = drc.Find(RecordID);

//having found the datarow of the record you need to know it's position(index) in the results
int index = drc.IndexOf(dr);

//Get the page the record will appear on
int page = index / GridView1.PageSize;

//If you're not already on the right page, set the PageIndex to the correct page
if (GridView1.PageIndex != page)
{
     GridView1.PageIndex = page;
}

 //gets the index of the record in the page
GridView1.SelectedIndex = index - (page* GridView1.PageSize);

</pre>
<p>&nbsp;</p>
</div>