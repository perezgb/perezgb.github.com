---
layout: post
title: "From Entity Framework to NHibernate"
description: ""
date: 2009-11-30 12:00:00 UTC
category: 
tags: [entityframework, nhibernate, speakout]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Recently I decided to change the data access layer in <a href="http://speakoutblog.codeplex.com/">my blog</a> from Entity Framework to NHibernate. In this post I'll tell you Why and How I did it.</p>
<h2>The WHY</h2>
<p>I still have some issues with the current version of the Entity Framework as I posted in the article <a href="http://www.gbogea.com/2009/04/22/mvc-storefront-migrating-to-the-entity-framework">MVC Storefront: Migrating to the Entity Framework</a>. Many of the things I wanted to do with the EF like using POCOs or LazyLoading will be possible in version 4. Until then I decided to give <a href="http://nhforge.org/">NHibernate </a>a shot. I figured it wouldn't be so hard since I already had some knowledge with Hibernate from my Java days.</p>
<h2>The HOW</h2>
<p>&nbsp;The migration was not that hard. NHibernate has a great <a href="http://nhforge.org/doc/nh/en/index.html">documentation </a>with good examples. I also used the <a href="http://www.manning.com/kuate/">NHibernate in Action</a> book which I highly recommend. It is only 400 pages and very to the point.</p>
<h3>How the Project is Structured</h3>
<p>I have a Website and 4 assemblies:</p>
<p><strong>SpeakOut.Data</strong>: This is where the DAOs live, both their interface and implementation. Also in this assembly are the NHibernate mapping files.</p>
<p><strong>SpeakOut.Model</strong>: This assembly has all the domain model entities. I had to make minor changes to these classes to get them to work best with NHibernate. The collections for instance were changed to use IList&lt;T&gt; instead of the IEnumerable&lt;T&gt; I was using with the EF.</p>
<p><strong>SpeakOut.Service</strong>: This assembly has services (Interfaces and Implementations) that are used by the presentation layer.</p>
<p><strong>SpeakOut.Lib</strong>: Here are all the utility classes, base classes for the pages and control and classes with extension methods.</p>
<h3>The Model</h3>
<p>This is the class diagram for the classes in the model. NHibernate makes populating the dependencies show in the diagram really easy, actually you don't do anything and you still get the benefit of lazy loading for free. This is something that wasn't easy in the EF.</p>
<p><img width="600" src="http://www.gbogea.com/upload/SpeakoutModel.png" alt="" /></p>
<h3>The Mapping Files</h3>
<p>Here are the mapping files for my entities.</p>
<p><strong>Author</strong></p>
<pre class="brush: xhtml" title="code">
&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;hibernate-mapping xmlns=&quot;urn:nhibernate-mapping-2.2&quot;
                   assembly=&quot;SpeakOut.Model&quot;
                   namespace=&quot;SpeakOut.Model&quot;&gt;
  &lt;class name=&quot;Author&quot;&gt;
    &lt;id name=&quot;AuthorId&quot;&gt;
      &lt;generator class=&quot;guid&quot;/&gt;
    &lt;/id&gt;
    &lt;property name=&quot;Name&quot;/&gt;
    &lt;property name=&quot;Login&quot;/&gt;
    &lt;property name=&quot;Password&quot;/&gt;
    &lt;property name=&quot;Email&quot;/&gt;
    &lt;property name=&quot;IsAdmin&quot;/&gt;
    &lt;bag name=&quot;Posts&quot; access=&quot;field.camelcase-underscore&quot; inverse=&quot;true&quot; cascade=&quot;all-delete-orphan&quot;&gt;
      &lt;key column=&quot;AuthorId&quot;/&gt;
      &lt;one-to-many class=&quot;Post&quot;/&gt;
    &lt;/bag&gt;
  &lt;/class&gt;
&lt;/hibernate-mapping&gt;</pre>
<p><strong>Post</strong></p>
<pre class="brush: xhtml" title="code">
&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;hibernate-mapping xmlns=&quot;urn:nhibernate-mapping-2.2&quot;
                   assembly=&quot;SpeakOut.Model&quot;
                   namespace=&quot;SpeakOut.Model&quot;&gt;
  &lt;class name=&quot;Post&quot;&gt;
    &lt;id name=&quot;PostId&quot;&gt;
      &lt;generator class=&quot;guid&quot;/&gt;
    &lt;/id&gt;
    &lt;property name=&quot;Title&quot;/&gt;
    &lt;property name=&quot;Body&quot;/&gt;
    &lt;property name=&quot;Permalink&quot;/&gt;
    &lt;property name=&quot;Published&quot;/&gt;
    &lt;property name=&quot;PublishDate&quot;/&gt;
    &lt;bag name=&quot;Comments&quot; access=&quot;field.camelcase-underscore&quot; inverse=&quot;true&quot; cascade=&quot;all-delete-orphan&quot;&gt;
      &lt;key column=&quot;PostId&quot;/&gt;
      &lt;one-to-many class=&quot;Comment&quot;/&gt;
    &lt;/bag&gt;
    &lt;bag name=&quot;Tags&quot; access=&quot;field.camelcase-underscore&quot; inverse=&quot;true&quot; cascade=&quot;all-delete-orphan&quot;&gt;
      &lt;key column=&quot;PostId&quot;/&gt;
      &lt;one-to-many class=&quot;Tag&quot;/&gt;
    &lt;/bag&gt;
    &lt;bag name=&quot;Categories&quot; access=&quot;field.camelcase-underscore&quot; table=&quot;CategoryPost&quot;&gt;
      &lt;key&gt;
        &lt;column name=&quot;PostId&quot; not-null=&quot;true&quot;/&gt;
      &lt;/key&gt;
      &lt;many-to-many class=&quot;Category&quot;&gt;
        &lt;column name=&quot;CategoryId&quot; not-null=&quot;true&quot;/&gt;
      &lt;/many-to-many&gt;
    &lt;/bag&gt;
    &lt;many-to-one name=&quot;Author&quot; column=&quot;AuthorId&quot; not-null=&quot;true&quot;/&gt;
  &lt;/class&gt;
&lt;/hibernate-mapping&gt;</pre>
<p><strong>Comment</strong></p>
<pre class="brush: xhtml" title="code">
&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;hibernate-mapping xmlns=&quot;urn:nhibernate-mapping-2.2&quot;
                   assembly=&quot;SpeakOut.Model&quot;
                   namespace=&quot;SpeakOut.Model&quot;&gt;
  &lt;class name=&quot;SpeakOut.Model.Comment&quot;&gt;
    &lt;id name=&quot;CommentId&quot;&gt;
      &lt;generator class=&quot;guid&quot;/&gt;
    &lt;/id&gt;
    &lt;property name=&quot;Author&quot;/&gt;
    &lt;property name=&quot;Body&quot;/&gt;
    &lt;property name=&quot;Email&quot;/&gt;
    &lt;property name=&quot;Website&quot;/&gt;
    &lt;property name=&quot;Country&quot;/&gt;
    &lt;property name=&quot;Ip&quot;/&gt;
    &lt;property name=&quot;IsApproved&quot;/&gt;
    &lt;property name=&quot;CreatedAt&quot;/&gt;
    &lt;many-to-one name=&quot;Post&quot; column=&quot;PostId&quot; not-null=&quot;true&quot;/&gt;
  &lt;/class&gt;
&lt;/hibernate-mapping&gt;</pre>
<p><strong>Tag</strong></p>
<pre class="brush: xhtml" title="code">
&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;hibernate-mapping xmlns=&quot;urn:nhibernate-mapping-2.2&quot;
                   assembly=&quot;SpeakOut.Model&quot;
                   namespace=&quot;SpeakOut.Model&quot;&gt;
  &lt;class name=&quot;Tag&quot;&gt;
    &lt;id name=&quot;TagId&quot;&gt;
      &lt;generator class=&quot;guid&quot;/&gt;
    &lt;/id&gt;
    &lt;property name=&quot;Name&quot;/&gt;
    &lt;many-to-one name=&quot;Post&quot; column=&quot;PostId&quot; not-null=&quot;true&quot;/&gt;
  &lt;/class&gt;
&lt;/hibernate-mapping&gt;</pre>
<p><strong>Category</strong></p>
<pre class="brush: xhtml" title="code">
&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
&lt;hibernate-mapping xmlns=&quot;urn:nhibernate-mapping-2.2&quot;
                   assembly=&quot;SpeakOut.Model&quot;
                   namespace=&quot;SpeakOut.Model&quot;&gt;
  &lt;class name=&quot;Category&quot;&gt;
    &lt;id name=&quot;CategoryId&quot;&gt;
      &lt;generator class=&quot;guid&quot;/&gt;
    &lt;/id&gt;
    &lt;property name=&quot;Name&quot;/&gt;
    &lt;bag name=&quot;Posts&quot; table=&quot;CategoryPost&quot;&gt;
      &lt;key&gt;
        &lt;column name=&quot;CategoryId&quot; not-null=&quot;true&quot;/&gt;
      &lt;/key&gt;
      &lt;many-to-many class=&quot;Post&quot;&gt;
        &lt;column name=&quot;PostId&quot; not-null=&quot;true&quot;/&gt;
      &lt;/many-to-many&gt;
    &lt;/bag&gt;
  &lt;/class&gt;
&lt;/hibernate-mapping&gt;</pre>
<h3>&nbsp;The Data Access</h3>
<p>&nbsp;In the data access layer I decided to drop the Repository I was using to go with simple DAOs. The main reason I did it was to test NHibernate as recommended by it's more experienced users. I really think I wouldn't get a lot from the Repository.</p>
<p>Here is the class diagram for the DAOs interfaces:</p>
<p><img width="600" src="http://www.gbogea.com/upload/SpeakoutInterfaces.png" alt="" /><br />
<br />
One of the approaches suggested in the NHibernate in Action book is to use a base DAO that encapsulate the common behavior in all NHibernate DAOs. Here is the code for the this class:<br />
&nbsp;</p>
<pre title="code" class="brush: csharp">
namespace SpeakOut.Data.NHibernate.Daos
{
    public class BaseDao&lt;T&gt; : IBaseDao&lt;T&gt;
    {
        private ISession _session;

        protected ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = NHibernateHelper.GetCurrentSession();
                }
                return _session;
            }
        }

        public T FindById(Guid id)
        {
            return Session.Load&lt;T&gt;(id);
        }

        public T FindByIdAndLock(Guid id)
        {
            return Session.Load&lt;T&gt;(id, LockMode.Upgrade);
        }

        public IList&lt;T&gt; FindAll()
        {
            return Session.CreateCriteria(typeof (T)).List&lt;T&gt;();
        }

        public T MakePersistent(T entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public void MakeTransient(T entity)
        {
            Session.Delete(entity);
        }
    }
}</pre>
<p>&nbsp;All the DAOs derive from this BaseDao and get all the basic functionality. The specialized DAOs are left to implement the functionality that is directly related to them. Look at how simple the implementation of the AuthorDao is:</p>
<pre class="brush: csharp" title="code">
namespace SpeakOut.Data.NHibernate.Daos
{
    public class AuthorDao : BaseDao&lt;Author&gt;, IAuthorDao
    {
        public Author FindByLogin(string login)
        {
            return Session.CreateCriteria&lt;Author&gt;()
                .Add(Expression.Eq(&quot;Login&quot;, login).IgnoreCase())
                .UniqueResult&lt;Author&gt;();
        }
    }
}</pre>
<p>&nbsp;One aspect to pay attention to is how the NHibernate Session works when you have an ASP.NET application. The easiest way to deal with this is to create an HTTP Module that creates a session that will live as long as the duration of the Request. When the Request ends the Transaction is committed and the Session is released. A similar strategy is described in <a href="http://www.beansoftware.com/asp.net-tutorials/nhibernate-log4net.aspx">this post</a>.</p>
<p>&nbsp;Here is the implementation of the HTTP Module.</p>
<pre title="code" class="brush: csharp">
public class NHibernateCurrentSessionWebModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.BeginRequest += Application_BeginRequest;
        context.EndRequest += Application_EndRequest;
    }

    public void Dispose()
    {
    }

    private void Application_BeginRequest(object sender, EventArgs e)
    {
        ISession session = NHibernateHelper.OpenSession();
        session.BeginTransaction();
        CurrentSessionContext.Bind(session);
    }

    private void Application_EndRequest(object sender, EventArgs e)
    {
        ISession session = CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);

        if (session == null) return;

        try
        {
            session.Transaction.Commit();
        }
        catch (Exception)
        {
            session.Transaction.Rollback();
        }
        finally
        {
            session.Close();
        }
    }
}</pre>
<p>Since this module creates the Session and the Transaction we need a way to access these resources. This is done by a helper class like this:</p>
<pre title="code" class="brush: csharp">
namespace SpeakOut.Data.NHibernate
{
    public static class NHibernateHelper
    {
        public static readonly ISessionFactory SessionFactory;

        static NHibernateHelper()
        {
            try
            {
                Configuration configuration = new Configuration();
                configuration.AddAssembly(&quot;SpeakOut.Data&quot;);
                SessionFactory = configuration.Configure().BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw new Exception(&quot;NHibernate initialization failed&quot;, ex);
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }
    }
}</pre>
<p>There are two important things about this class. The first is AddAssembly method being called in the static constructor. I have to pass the name of the assembly that contains the NHibernate Mappings. In this project all the mappings are in the Speakout.Data assembly. The second important point is the GetCurrentSession which is the method that will be used in all the DAOs to get the Session already created in the HTTP Module.</p>
<p>More info about configuring NHibernate when using several assemblies can be found <a href="http://blog.codehangover.com/configuring-nhibernate-in-a-multiple-project-layout/">here</a>.</p>
<h3>The Configuration</h3>
<p>Since I'm using a WebSite all the NHibernate configuration is done in the Web.Config. First a section is defined in the ConfigSections node:</p>
<pre class="brush: xhtml" title="code">
&lt;section name=&quot;hibernate-configuration&quot; type=&quot;NHibernate.Cfg.ConfigurationSectionHandler,NHibernate&quot;/&gt;</pre>
<p>And the create the configuration node:</p>
<pre class="brush: xhtml" title="code">
&lt;hibernate-configuration xmlns=&quot;urn:nhibernate-configuration-2.2&quot;&gt;
  &lt;session-factory&gt;
    &lt;property name=&quot;connection.driver_class&quot;&gt;NHibernate.Driver.SqlClientDriver&lt;/property&gt;
    &lt;property name=&quot;connection.connection_string&quot;&gt;
      Server=(local);initial catalog=your_catalog;user id=your_user;password=your_pass
    &lt;/property&gt;
    &lt;property name=&quot;adonet.batch_size&quot;&gt;10&lt;/property&gt;
    &lt;property name=&quot;show_sql&quot;&gt;true&lt;/property&gt;
    &lt;property name=&quot;dialect&quot;&gt;NHibernate.Dialect.MsSql2005Dialect&lt;/property&gt;
    &lt;property name=&quot;use_outer_join&quot;&gt;true&lt;/property&gt;
    &lt;property name=&quot;command_timeout&quot;&gt;60&lt;/property&gt;
    &lt;property name=&quot;query.substitutions&quot;&gt;true 1, false 0, yes 'Y', no 'N'&lt;/property&gt;
    &lt;property name=&quot;proxyfactory.factory_class&quot;&gt;NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu&lt;/property&gt;
    &lt;property name=&quot;current_session_context_class&quot;&gt;web&lt;/property &gt;
  &lt;/session-factory&gt;
&lt;/hibernate-configuration&gt;</pre>
<h2>&nbsp;Final Words</h2>
<p>I hope my implementation may help you. It simple and small enough that you can go through it very quickly and get a base understanding of what you need to get NHibernate going.</p>
<p>I'll probably give the Entity Framework another chance once my hosting starts to support EF 4. After all changing and trying, and failing and learning is exactly what this blog is all about.</p>
<p>You can get all the source code for this project at <a href="http://speakoutblog.codeplex.com/">Codeplex</a>.</p>
</div>