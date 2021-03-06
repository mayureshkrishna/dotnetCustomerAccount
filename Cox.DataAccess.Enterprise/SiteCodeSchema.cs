﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Cox.DataAccess.Enterprise {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class SiteCodeSchema : DataSet {
        
        private CompanyDivisionSitesDataTable tableCompanyDivisionSites;
        
        private CoxSitesDataTable tableCoxSites;
        
        public SiteCodeSchema() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected SiteCodeSchema(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["tblCompanyDivision"] != null)) {
                    this.Tables.Add(new CompanyDivisionSitesDataTable(ds.Tables["tblCompanyDivision"]));
                }
                if ((ds.Tables["tblSite"] != null)) {
                    this.Tables.Add(new CoxSitesDataTable(ds.Tables["tblSite"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public CompanyDivisionSitesDataTable CompanyDivisionSites {
            get {
                return this.tableCompanyDivisionSites;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public CoxSitesDataTable CoxSites {
            get {
                return this.tableCoxSites;
            }
        }
        
        public override DataSet Clone() {
            SiteCodeSchema cln = ((SiteCodeSchema)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["tblCompanyDivision"] != null)) {
                this.Tables.Add(new CompanyDivisionSitesDataTable(ds.Tables["tblCompanyDivision"]));
            }
            if ((ds.Tables["tblSite"] != null)) {
                this.Tables.Add(new CoxSitesDataTable(ds.Tables["tblSite"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tableCompanyDivisionSites = ((CompanyDivisionSitesDataTable)(this.Tables["tblCompanyDivision"]));
            if ((this.tableCompanyDivisionSites != null)) {
                this.tableCompanyDivisionSites.InitVars();
            }
            this.tableCoxSites = ((CoxSitesDataTable)(this.Tables["tblSite"]));
            if ((this.tableCoxSites != null)) {
                this.tableCoxSites.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "SiteCodeSchema";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/SiteCodeSchema.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableCompanyDivisionSites = new CompanyDivisionSitesDataTable();
            this.Tables.Add(this.tableCompanyDivisionSites);
            this.tableCoxSites = new CoxSitesDataTable();
            this.Tables.Add(this.tableCoxSites);
        }
        
        private bool ShouldSerializeCompanyDivisionSites() {
            return false;
        }
        
        private bool ShouldSerializeCoxSites() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void CompanyDivisionSiteChangeEventHandler(object sender, CompanyDivisionSiteChangeEvent e);
        
        public delegate void CoxSiteChangeEventHandler(object sender, CoxSiteChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CompanyDivisionSitesDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnSiteId;
            
            private DataColumn columnCompanyNumber;
            
            private DataColumn columnDivisionNumber;
            
            internal CompanyDivisionSitesDataTable() : 
                    base("tblCompanyDivision") {
                this.InitClass();
            }
            
            internal CompanyDivisionSitesDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn SiteIdColumn {
                get {
                    return this.columnSiteId;
                }
            }
            
            internal DataColumn CompanyNumberColumn {
                get {
                    return this.columnCompanyNumber;
                }
            }
            
            internal DataColumn DivisionNumberColumn {
                get {
                    return this.columnDivisionNumber;
                }
            }
            
            public CompanyDivisionSite this[int index] {
                get {
                    return ((CompanyDivisionSite)(this.Rows[index]));
                }
            }
            
            public event CompanyDivisionSiteChangeEventHandler CompanyDivisionSiteChanged;
            
            public event CompanyDivisionSiteChangeEventHandler CompanyDivisionSiteChanging;
            
            public event CompanyDivisionSiteChangeEventHandler CompanyDivisionSiteDeleted;
            
            public event CompanyDivisionSiteChangeEventHandler CompanyDivisionSiteDeleting;
            
            public void AddCompanyDivisionSite(CompanyDivisionSite row) {
                this.Rows.Add(row);
            }
            
            public CompanyDivisionSite AddCompanyDivisionSite(int SiteId, int CompanyNumber, int DivisionNumber) {
                CompanyDivisionSite rowCompanyDivisionSite = ((CompanyDivisionSite)(this.NewRow()));
                rowCompanyDivisionSite.ItemArray = new object[] {
                        SiteId,
                        CompanyNumber,
                        DivisionNumber};
                this.Rows.Add(rowCompanyDivisionSite);
                return rowCompanyDivisionSite;
            }
            
            public CompanyDivisionSite FindBySiteIdCompanyNumberDivisionNumber(int SiteId, int CompanyNumber, int DivisionNumber) {
                return ((CompanyDivisionSite)(this.Rows.Find(new object[] {
                            SiteId,
                            CompanyNumber,
                            DivisionNumber})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CompanyDivisionSitesDataTable cln = ((CompanyDivisionSitesDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new CompanyDivisionSitesDataTable();
            }
            
            internal void InitVars() {
                this.columnSiteId = this.Columns["cpdvSiteId"];
                this.columnCompanyNumber = this.Columns["cpdvCompanyNumber"];
                this.columnDivisionNumber = this.Columns["cpdvDivisionNumber"];
            }
            
            private void InitClass() {
                this.columnSiteId = new DataColumn("cpdvSiteId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSiteId);
                this.columnCompanyNumber = new DataColumn("cpdvCompanyNumber", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCompanyNumber);
                this.columnDivisionNumber = new DataColumn("cpdvDivisionNumber", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDivisionNumber);
                this.Constraints.Add(new UniqueConstraint("tblCompanyDivisionKey1", new DataColumn[] {
                                this.columnSiteId,
                                this.columnCompanyNumber,
                                this.columnDivisionNumber}, true));
                this.columnSiteId.AllowDBNull = false;
                this.columnCompanyNumber.AllowDBNull = false;
                this.columnDivisionNumber.AllowDBNull = false;
            }
            
            public CompanyDivisionSite NewCompanyDivisionSite() {
                return ((CompanyDivisionSite)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CompanyDivisionSite(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CompanyDivisionSite);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CompanyDivisionSiteChanged != null)) {
                    this.CompanyDivisionSiteChanged(this, new CompanyDivisionSiteChangeEvent(((CompanyDivisionSite)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CompanyDivisionSiteChanging != null)) {
                    this.CompanyDivisionSiteChanging(this, new CompanyDivisionSiteChangeEvent(((CompanyDivisionSite)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CompanyDivisionSiteDeleted != null)) {
                    this.CompanyDivisionSiteDeleted(this, new CompanyDivisionSiteChangeEvent(((CompanyDivisionSite)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CompanyDivisionSiteDeleting != null)) {
                    this.CompanyDivisionSiteDeleting(this, new CompanyDivisionSiteChangeEvent(((CompanyDivisionSite)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCompanyDivisionSite(CompanyDivisionSite row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CompanyDivisionSite : DataRow {
            
            private CompanyDivisionSitesDataTable tableCompanyDivisionSites;
            
            internal CompanyDivisionSite(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCompanyDivisionSites = ((CompanyDivisionSitesDataTable)(this.Table));
            }
            
            public int SiteId {
                get {
                    return ((int)(this[this.tableCompanyDivisionSites.SiteIdColumn]));
                }
                set {
                    this[this.tableCompanyDivisionSites.SiteIdColumn] = value;
                }
            }
            
            public int CompanyNumber {
                get {
                    return ((int)(this[this.tableCompanyDivisionSites.CompanyNumberColumn]));
                }
                set {
                    this[this.tableCompanyDivisionSites.CompanyNumberColumn] = value;
                }
            }
            
            public int DivisionNumber {
                get {
                    return ((int)(this[this.tableCompanyDivisionSites.DivisionNumberColumn]));
                }
                set {
                    this[this.tableCompanyDivisionSites.DivisionNumberColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CompanyDivisionSiteChangeEvent : EventArgs {
            
            private CompanyDivisionSite eventRow;
            
            private DataRowAction eventAction;
            
            public CompanyDivisionSiteChangeEvent(CompanyDivisionSite row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CompanyDivisionSite Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CoxSitesDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnSiteId;
            
            private DataColumn columnSiteCode;
            
            internal CoxSitesDataTable() : 
                    base("tblSite") {
                this.InitClass();
            }
            
            internal CoxSitesDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn SiteIdColumn {
                get {
                    return this.columnSiteId;
                }
            }
            
            internal DataColumn SiteCodeColumn {
                get {
                    return this.columnSiteCode;
                }
            }
            
            public CoxSite this[int index] {
                get {
                    return ((CoxSite)(this.Rows[index]));
                }
            }
            
            public event CoxSiteChangeEventHandler CoxSiteChanged;
            
            public event CoxSiteChangeEventHandler CoxSiteChanging;
            
            public event CoxSiteChangeEventHandler CoxSiteDeleted;
            
            public event CoxSiteChangeEventHandler CoxSiteDeleting;
            
            public void AddCoxSite(CoxSite row) {
                this.Rows.Add(row);
            }
            
            public CoxSite AddCoxSite(int SiteId, string SiteCode) {
                CoxSite rowCoxSite = ((CoxSite)(this.NewRow()));
                rowCoxSite.ItemArray = new object[] {
                        SiteId,
                        SiteCode};
                this.Rows.Add(rowCoxSite);
                return rowCoxSite;
            }
            
            public CoxSite FindBySiteId(int SiteId) {
                return ((CoxSite)(this.Rows.Find(new object[] {
                            SiteId})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                CoxSitesDataTable cln = ((CoxSitesDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new CoxSitesDataTable();
            }
            
            internal void InitVars() {
                this.columnSiteId = this.Columns["stId"];
                this.columnSiteCode = this.Columns["stCode"];
            }
            
            private void InitClass() {
                this.columnSiteId = new DataColumn("stId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSiteId);
                this.columnSiteCode = new DataColumn("stCode", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSiteCode);
                this.Constraints.Add(new UniqueConstraint("tblSiteKey1", new DataColumn[] {
                                this.columnSiteId}, true));
                this.columnSiteId.AllowDBNull = false;
                this.columnSiteId.Unique = true;
                this.columnSiteCode.AllowDBNull = false;
            }
            
            public CoxSite NewCoxSite() {
                return ((CoxSite)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new CoxSite(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(CoxSite);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.CoxSiteChanged != null)) {
                    this.CoxSiteChanged(this, new CoxSiteChangeEvent(((CoxSite)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.CoxSiteChanging != null)) {
                    this.CoxSiteChanging(this, new CoxSiteChangeEvent(((CoxSite)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.CoxSiteDeleted != null)) {
                    this.CoxSiteDeleted(this, new CoxSiteChangeEvent(((CoxSite)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.CoxSiteDeleting != null)) {
                    this.CoxSiteDeleting(this, new CoxSiteChangeEvent(((CoxSite)(e.Row)), e.Action));
                }
            }
            
            public void RemoveCoxSite(CoxSite row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CoxSite : DataRow {
            
            private CoxSitesDataTable tableCoxSites;
            
            internal CoxSite(DataRowBuilder rb) : 
                    base(rb) {
                this.tableCoxSites = ((CoxSitesDataTable)(this.Table));
            }
            
            public int SiteId {
                get {
                    return ((int)(this[this.tableCoxSites.SiteIdColumn]));
                }
                set {
                    this[this.tableCoxSites.SiteIdColumn] = value;
                }
            }
            
            public string SiteCode {
                get {
                    return ((string)(this[this.tableCoxSites.SiteCodeColumn]));
                }
                set {
                    this[this.tableCoxSites.SiteCodeColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class CoxSiteChangeEvent : EventArgs {
            
            private CoxSite eventRow;
            
            private DataRowAction eventAction;
            
            public CoxSiteChangeEvent(CoxSite row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public CoxSite Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
