---
layout: post
title: "Using Scope_Identity with TableAdapters"
description: ""
date: 2008-04-09 12:00:00 UTC
category: 
tags: [asp.net, scope_identity, tableadapter, .net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>I find TableAdapters to be a very helpful resource so I use them a lot.  However, every now and then I stumble upon some scenario where the TableAdapter  needs some creativity to work. Imagine the following:</p>
<p>Scenario:</p>
<ol>
    <li>Need to insert a Person and an Address in the Database;</li>
    <li>You populate all the data in just one page;</li>
    <li>The table <span class="caps">PERSON</span> has an primary key named ID which  is an Identity, so SQLServer will generate the ID upon the record insertion;</li>
    <li>You need to Insert a record in the <span class="caps">ADDRESS</span> table  that will need the ID of the person just inserted;</li>
    <li>You&rsquo;re using TableAdapters;</li>
</ol>
<p>For this you&rsquo;ll need the <span class="caps">SCOPE</span>_IDENTITY function  provided by SQLServer in the TableAdapter code.</p>
<p>The TableAdapters code is frequently regenerated so you need to protect this  code from being overwritten on regeneration. The best solution for this is  extending your TableAdapter and overriding the method you need to protect.</p>
<h2>First step</h2>
<p>Add a new insert query in the PERSONTableAdapter named InsertQueryReturnID  with a code like this:</p>
<pre class="brush: sql" title="code">
INSERT INTO [PERSON] ([NAME], [PHONE], [EMAIL]) VALUES (@p1, @p2, @p3);
SELECT CAST(SCOPE_IDENTITY() AS INT);
</pre>
<p>The key here is the <span class="caps">SCOPE</span>_IDENTITY() function which  will return the last inserted id in your connection so you&rsquo;re safe about inserts  made by other users.</p>
<h2>Second step</h2>
<p>Add a new class named PERSONTableAdapterExtended to your project. This class  will inhererit from PERSONTableAdapter. Next, copy the code for the  InsertQueryReturnID method from the Designer file from your DataSet and paste  the method to your extended class. Substitute the method&rsquo;s virtual modifier by  an override modifier (because you want to override the method from the base  class).</p>
<p>Another important change is in the way you execute and return the id.  Normally the insert would be executed with a command.ExecuteNonQuery() command  which would return the numbers of rows affected. Our <span class="caps">SQL</span>  will return the ID of the record inserted so will replace that code by  (int)command.ExecuteScalar(). The final method should look like this:</p>
<pre class="brush: csharp" title="code">
public class PERSONTableAdapterExtended : PERSONTableAdapter
    {
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Design.HelpKeywordAttribute(&quot;vs.data.TableAdapter&quot;)]
        [global::System.ComponentModel.DataObjectMethodAttribute(global::System.ComponentModel.DataObjectMethodType.Insert, false)]
        public override int InsertQueryReturnID(string p1, string p2, string p3)
        {
            global::System.Data.SqlServerCe.SqlCeCommand command = this.CommandCollection[1];
            if ((p1 == null))
            {
                command.Parameters[0].Value = global::System.DBNull.Value;
            }
            else
            {
                command.Parameters[0].Value = ((string)(p1));
            }
            if ((p2 == null))
            {
                command.Parameters[1].Value = global::System.DBNull.Value;
            }
            else
            {
                command.Parameters[1].Value = ((string)(p2));
            }
            if ((p3 == null))
            {
                command.Parameters[2].Value = global::System.DBNull.Value;
            }
            else
            {
                command.Parameters[2].Value = ((string)(p3));
            }
            global::System.Data.ConnectionState previousConnectionState = command.Connection.State;
            if (((command.Connection.State &amp; global::System.Data.ConnectionState.Open)
                        != global::System.Data.ConnectionState.Open))
            {
                command.Connection.Open();
            }
            int returnValue;
            try
            {
                returnValue = (int)command.ExecuteScalar();
            }
            finally
            {
                if ((previousConnectionState == global::System.Data.ConnectionState.Closed))
                {
                    command.Connection.Close();
                }
            }
            return returnValue;
        }
    }
</pre>
<p><img src="http://www.gbogea.com/upload/TableAdapterExtended3.jpg" alt="" /></p>
<h2>Step Three</h2>
<p>Now you&rsquo;ll use only the PERSONTableAdapterExtended class instead of the  PERSONTableAdapter generated class. This will protect your code from changes  when the DataSet is regenerated</p>
</div>