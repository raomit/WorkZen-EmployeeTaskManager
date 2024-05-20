using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkZen.Models.CustomModels
{
    public class JqueryDatatbleParams
    {
        //public string draw { get; set; }
        //public string start { get; set; }
        //public int length { get; set; }
        //public string searchValue { get; set; }
        //public int orderColumn {  get; set; }
        //public string orderDir { get; set; }

        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Search search { get; set; }
        public bool searchRegex { get; set; }
        public List<Ordering> order { get; set; }
        public List<Column> columns { get; set; }

        public class Ordering
        {
            public int column { get; set; }
            public string dir { get; set; }
        }

        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }

        public class Search
        {
            public string value { get; set; }
        }

    }
}