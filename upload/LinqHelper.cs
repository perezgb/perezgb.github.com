using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace LinqTests
{
    class LinqHelper
    {

        DataTable dtPerson;
        DataTable dtJob;

        public LinqHelper()
        {
            createDataTables();
            populateDataTables();
        }

        private void createDataTables()
        {
            dtPerson = new DataTable("Person");
            DataColumn dc;

            dc = new DataColumn("Id", typeof(int));
            dtPerson.Columns.Add(dc);

            dc = new DataColumn("Name", typeof(string));
            dtPerson.Columns.Add(dc);

            dc = new DataColumn("Age", typeof(int));
            dtPerson.Columns.Add(dc);

            dtJob = new DataTable("Job");

            dc = new DataColumn("PersonId", typeof(int));
            dtJob.Columns.Add(dc);

            dc = new DataColumn("Position", typeof(string));
            dtJob.Columns.Add(dc);
        }

        private void populateDataTables()
        {
            DataRow dr = dtPerson.NewRow();
            dr[0] = 1;
            dr[1] = "Gabriel";
            dr[2] = 31;
            dtPerson.Rows.Add(dr);

            dr = dtPerson.NewRow();
            dr[0] = 2;
            dr[1] = "Elisa";
            dr[2] = 27;
            dtPerson.Rows.Add(dr);

            dr = dtJob.NewRow();
            dr[0] = 1;
            dr[1] = "Programmer";
            dtJob.Rows.Add(dr);

            dr = dtJob.NewRow();
            dr[0] = 2;
            dr[1] = "Manager";
            dtJob.Rows.Add(dr);
        }

        public DataTable DoQuery()
        {
            var query = (from p in dtPerson.AsEnumerable()
                         join j in dtJob.AsEnumerable() on p.Field<int>("Id") equals j.Field<int>("PersonId")
                         select new
                         {
                             Id = p.Field<int>("Id"),
                             Name = p.Field<string>("Name"),
                             Job = j.Field<string>("Position")
                         }).ToArray().ToDataTable();
            return query;
        }
    }

    public static class LinqExtensions
    {
        public static DataTable ToDataTable(this IEnumerable objectList)
        {
            //if the result is null or if the number of
            //objects is less then 1, return null
            if (objectList == null ||
                objectList.OfType<object>().Count() < 1)
            {
                return null;
            }
            //create a new list based on the IEnumerable
            List<object> list = new List<object>(objectList.OfType<object>());

            //take the first object in the list
            object o = list[0];
            //get the type of the object
            Type t = o.GetType();
            //read all the info of the properties
            PropertyInfo[] properties = t.GetProperties();

            DataTable dt = new DataTable();
            //for each property create a new column
            //in the DataTable
            foreach (var pi in properties)
            {
                //the new column has the name of the property
                //and it's type
                DataColumn dc = new DataColumn(pi.Name, pi.PropertyType);
                //add the column to the DataTable
                dt.Columns.Add(dc);
            }

            //add the rows to the DataTable
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                //each property represents a column 
                foreach (var pi in properties)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
