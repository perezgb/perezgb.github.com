---
layout: post
title: "When to use GUID over Autoincrement"
description: ""
date: 2009-02-09 12:00:00 UTC
category: 
tags: [ado.net, .net, sqlserver]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The autoincrement feature in SQLServer tables is one that is really very  handy. I&rsquo;ve always like Oracle databases but it&rsquo;s sequences are really much more  complicated to manage than the autoincrement columns in SQLServer. If you are  doing a project that is only going to run on Oralce or SQLServer either approach  is fine and you won&rsquo;t have any trouble.</p>
<p>The project I&rsquo;m working on however has a requirement to work just the same on  Oralce or SQLServer and the possibility that other databases may be added in the  future. From day one my first worry was about if we were going to use  Autoincrement and Sequences or we had to find another way out. The way out in  this case would be to use <a href="http://en.wikipedia.org/wiki/Globally_Unique_Identifier"><span class="caps">GUID</span></a> (Globally Unique Identifiers). In out case we chose  to use Autoincrement for SQLServer and Sequences in Oracle. Mostly this approach  was chosen because the guys that hired me thought that it would be much easier  to search database for a record with a primary key like 1 or 15604 then  something like 3F2504E0-4F89-11D3-9A0C-0305E82C3301. And I agreed with them on  this, it is really simpler to say to the guy next to you to check if the person  with id 2350 exists in the database the to tell him a <span class="caps">GUID</span>.</p>
<p>Of course we had a price to pay for this decision. Our <span class="caps">DAL</span> (Data Access Layer) was much more complicated because we  had to deal with the Autroincrement vs Sequences problem. Imagine that you have  a simple table called person with two columns only: ID and Name. Look at the  difference in the insert clauses for each database:</p>
<pre title="code" class="brush: sql">
Oracle
INSERT INTO PERSON (ID, NAME) VALUES (SEQ_PERSON.NEXTVAL, 'GABRIEL');</pre>
<pre title="code" class="brush: sql">
SQLServer
INSERT INTO PERSON (NAME) VALUES ('GABRIEL');
</pre>
<p>Observe that with Oracle I have to specify the PK column and in SQLServer I  don&rsquo;t and even worst I have to call the sequence <span class="caps">SQL</span>_PERSON to get the next value for the insert. If I want to  return the value of the ID of the record we just inserted I&rsquo;d have to do this:</p>
<pre title="code" class="brush: sql">
Oracle
INSERT INTO PERSON (ID, NAME) VALUES (SEQ_PERSON.NEXTVAL, 'GABRIEL') RETURNING ID INTO :ID;
</pre>
<pre title="code" class="brush: sql">
SQLServer
INSERT INTO PERSON (NAME) VALUES ('GABRIEL');
SELECT CAST(SCOPE_IDENTITY() AS INT);
</pre>
<p>I think you get the point right? There are a lot of differences in both  databases! If we had choosen to use <span class="caps">GUID</span> id&rsquo;s our life  in the <span class="caps">DAL</span> would be much simpler. The <span class="caps">GUID</span> would be generated in the <span class="caps">DAL</span>  which would make the insert statements very similar if not equal most of the  time. Returning the generated <span class="caps">GUID</span> would also be a piece  of cake. It&rsquo;s true that using numeric PK&rsquo;s creates better indexes and would  consume less space but I find that these improvements would&rsquo;ve been superseded  by the ease in development.</p>
<p>So to sum up, when developing a system that should be database independent I  would recommend using the table&rsquo;s primary key&rsquo;s as <span class="caps">GUID</span>  in order to avoid using Autoincrement and Sequences. Your <span class="caps">DAL</span> layer would be more uniform and you could use <span class="caps">ANSI SQL</span> more often.</p>
<p>Just in case you are wondering we did when we couldn&rsquo;t use <span class="caps">ANSI SQL</span> we created a <span class="caps">SQL</span> translator  that would convert all the commands create for SQLServer to Oracle and this  translator is part of our <span class="caps">DAL</span> generator so when I create  a <span class="caps">DAO</span> for SQLServer the generator will create its&rsquo;  structure and also Oracle&rsquo;s all at once.</p>
</div>