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
    public class ApplicationGroupSchema : DataSet {
        
        private ApplicationGroupsDataTable tableApplicationGroups;
        
        private IcomsCredentialsDataTable tableIcomsCredentials;
        
        private UsersDataTable tableUsers;
        
        public ApplicationGroupSchema() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected ApplicationGroupSchema(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["tblApplicationGroup"] != null)) {
                    this.Tables.Add(new ApplicationGroupsDataTable(ds.Tables["tblApplicationGroup"]));
                }
                if ((ds.Tables["tblIcomsCredential"] != null)) {
                    this.Tables.Add(new IcomsCredentialsDataTable(ds.Tables["tblIcomsCredential"]));
                }
                if ((ds.Tables["tblUser"] != null)) {
                    this.Tables.Add(new UsersDataTable(ds.Tables["tblUser"]));
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
        public ApplicationGroupsDataTable ApplicationGroups {
            get {
                return this.tableApplicationGroups;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public IcomsCredentialsDataTable IcomsCredentials {
            get {
                return this.tableIcomsCredentials;
            }
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public UsersDataTable Users {
            get {
                return this.tableUsers;
            }
        }
        
        public override DataSet Clone() {
            ApplicationGroupSchema cln = ((ApplicationGroupSchema)(base.Clone()));
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
            if ((ds.Tables["tblApplicationGroup"] != null)) {
                this.Tables.Add(new ApplicationGroupsDataTable(ds.Tables["tblApplicationGroup"]));
            }
            if ((ds.Tables["tblIcomsCredential"] != null)) {
                this.Tables.Add(new IcomsCredentialsDataTable(ds.Tables["tblIcomsCredential"]));
            }
            if ((ds.Tables["tblUser"] != null)) {
                this.Tables.Add(new UsersDataTable(ds.Tables["tblUser"]));
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
            this.tableApplicationGroups = ((ApplicationGroupsDataTable)(this.Tables["tblApplicationGroup"]));
            if ((this.tableApplicationGroups != null)) {
                this.tableApplicationGroups.InitVars();
            }
            this.tableIcomsCredentials = ((IcomsCredentialsDataTable)(this.Tables["tblIcomsCredential"]));
            if ((this.tableIcomsCredentials != null)) {
                this.tableIcomsCredentials.InitVars();
            }
            this.tableUsers = ((UsersDataTable)(this.Tables["tblUser"]));
            if ((this.tableUsers != null)) {
                this.tableUsers.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "ApplicationGroupSchema";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/ApplicationGroupSchema.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableApplicationGroups = new ApplicationGroupsDataTable();
            this.Tables.Add(this.tableApplicationGroups);
            this.tableIcomsCredentials = new IcomsCredentialsDataTable();
            this.Tables.Add(this.tableIcomsCredentials);
            this.tableUsers = new UsersDataTable();
            this.Tables.Add(this.tableUsers);
            ForeignKeyConstraint fkc;
            fkc = new ForeignKeyConstraint("tblIcomsCredentialtblApplicationGroup", new DataColumn[] {
                        this.tableIcomsCredentials.IdColumn}, new DataColumn[] {
                        this.tableApplicationGroups.CredentialIdColumn});
            this.tableApplicationGroups.Constraints.Add(fkc);
            fkc.AcceptRejectRule = System.Data.AcceptRejectRule.None;
            fkc.DeleteRule = System.Data.Rule.Cascade;
            fkc.UpdateRule = System.Data.Rule.Cascade;
            fkc = new ForeignKeyConstraint("tblApplicationGrouptblUser", new DataColumn[] {
                        this.tableApplicationGroups.IdColumn}, new DataColumn[] {
                        this.tableUsers.usrGroupIdColumn});
            this.tableUsers.Constraints.Add(fkc);
            fkc.AcceptRejectRule = System.Data.AcceptRejectRule.None;
            fkc.DeleteRule = System.Data.Rule.Cascade;
            fkc.UpdateRule = System.Data.Rule.Cascade;
        }
        
        private bool ShouldSerializeApplicationGroups() {
            return false;
        }
        
        private bool ShouldSerializeIcomsCredentials() {
            return false;
        }
        
        private bool ShouldSerializeUsers() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void ApplicationGroupChangeEventHandler(object sender, ApplicationGroupChangeEvent e);
        
        public delegate void IcomsCredentialChangeEventHandler(object sender, IcomsCredentialChangeEvent e);
        
        public delegate void UserChangeEventHandler(object sender, UserChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ApplicationGroupsDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnId;
            
            private DataColumn columnName;
            
            private DataColumn columnCredentialId;
            
            internal ApplicationGroupsDataTable() : 
                    base("tblApplicationGroup") {
                this.InitClass();
            }
            
            internal ApplicationGroupsDataTable(DataTable table) : 
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
            
            internal DataColumn IdColumn {
                get {
                    return this.columnId;
                }
            }
            
            internal DataColumn NameColumn {
                get {
                    return this.columnName;
                }
            }
            
            internal DataColumn CredentialIdColumn {
                get {
                    return this.columnCredentialId;
                }
            }
            
            public ApplicationGroup this[int index] {
                get {
                    return ((ApplicationGroup)(this.Rows[index]));
                }
            }
            
            public event ApplicationGroupChangeEventHandler ApplicationGroupChanged;
            
            public event ApplicationGroupChangeEventHandler ApplicationGroupChanging;
            
            public event ApplicationGroupChangeEventHandler ApplicationGroupDeleted;
            
            public event ApplicationGroupChangeEventHandler ApplicationGroupDeleting;
            
            public void AddApplicationGroup(ApplicationGroup row) {
                this.Rows.Add(row);
            }
            
            public ApplicationGroup AddApplicationGroup(string Name, int CredentialId) {
                ApplicationGroup rowApplicationGroup = ((ApplicationGroup)(this.NewRow()));
                rowApplicationGroup.ItemArray = new object[] {
                        null,
                        Name,
                        CredentialId};
                this.Rows.Add(rowApplicationGroup);
                return rowApplicationGroup;
            }
            
            public ApplicationGroup FindById(int Id) {
                return ((ApplicationGroup)(this.Rows.Find(new object[] {
                            Id})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                ApplicationGroupsDataTable cln = ((ApplicationGroupsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new ApplicationGroupsDataTable();
            }
            
            internal void InitVars() {
                this.columnId = this.Columns["grpId"];
                this.columnName = this.Columns["grpName"];
                this.columnCredentialId = this.Columns["grpCredentialId"];
            }
            
            private void InitClass() {
                this.columnId = new DataColumn("grpId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnId);
                this.columnName = new DataColumn("grpName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnName);
                this.columnCredentialId = new DataColumn("grpCredentialId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCredentialId);
                this.Constraints.Add(new UniqueConstraint("ApplicationGroupSchemaKey1", new DataColumn[] {
                                this.columnId}, true));
                this.columnId.AutoIncrement = true;
                this.columnId.AllowDBNull = false;
                this.columnId.ReadOnly = true;
                this.columnId.Unique = true;
                this.columnName.AllowDBNull = false;
                this.columnCredentialId.AllowDBNull = false;
            }
            
            public ApplicationGroup NewApplicationGroup() {
                return ((ApplicationGroup)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new ApplicationGroup(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(ApplicationGroup);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.ApplicationGroupChanged != null)) {
                    this.ApplicationGroupChanged(this, new ApplicationGroupChangeEvent(((ApplicationGroup)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.ApplicationGroupChanging != null)) {
                    this.ApplicationGroupChanging(this, new ApplicationGroupChangeEvent(((ApplicationGroup)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.ApplicationGroupDeleted != null)) {
                    this.ApplicationGroupDeleted(this, new ApplicationGroupChangeEvent(((ApplicationGroup)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.ApplicationGroupDeleting != null)) {
                    this.ApplicationGroupDeleting(this, new ApplicationGroupChangeEvent(((ApplicationGroup)(e.Row)), e.Action));
                }
            }
            
            public void RemoveApplicationGroup(ApplicationGroup row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ApplicationGroup : DataRow {
            
            private ApplicationGroupsDataTable tableApplicationGroups;
            
            internal ApplicationGroup(DataRowBuilder rb) : 
                    base(rb) {
                this.tableApplicationGroups = ((ApplicationGroupsDataTable)(this.Table));
            }
            
            public int Id {
                get {
                    return ((int)(this[this.tableApplicationGroups.IdColumn]));
                }
                set {
                    this[this.tableApplicationGroups.IdColumn] = value;
                }
            }
            
            public string Name {
                get {
                    return ((string)(this[this.tableApplicationGroups.NameColumn]));
                }
                set {
                    this[this.tableApplicationGroups.NameColumn] = value;
                }
            }
            
            public int CredentialId {
                get {
                    return ((int)(this[this.tableApplicationGroups.CredentialIdColumn]));
                }
                set {
                    this[this.tableApplicationGroups.CredentialIdColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ApplicationGroupChangeEvent : EventArgs {
            
            private ApplicationGroup eventRow;
            
            private DataRowAction eventAction;
            
            public ApplicationGroupChangeEvent(ApplicationGroup row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public ApplicationGroup Row {
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
        public class IcomsCredentialsDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnId;
            
            private DataColumn columnUsername;
            
            private DataColumn columnPassword;
            
            internal IcomsCredentialsDataTable() : 
                    base("tblIcomsCredential") {
                this.InitClass();
            }
            
            internal IcomsCredentialsDataTable(DataTable table) : 
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
            
            internal DataColumn IdColumn {
                get {
                    return this.columnId;
                }
            }
            
            internal DataColumn UsernameColumn {
                get {
                    return this.columnUsername;
                }
            }
            
            internal DataColumn PasswordColumn {
                get {
                    return this.columnPassword;
                }
            }
            
            public IcomsCredential this[int index] {
                get {
                    return ((IcomsCredential)(this.Rows[index]));
                }
            }
            
            public event IcomsCredentialChangeEventHandler IcomsCredentialChanged;
            
            public event IcomsCredentialChangeEventHandler IcomsCredentialChanging;
            
            public event IcomsCredentialChangeEventHandler IcomsCredentialDeleted;
            
            public event IcomsCredentialChangeEventHandler IcomsCredentialDeleting;
            
            public void AddIcomsCredential(IcomsCredential row) {
                this.Rows.Add(row);
            }
            
            public IcomsCredential AddIcomsCredential(string Username, string Password) {
                IcomsCredential rowIcomsCredential = ((IcomsCredential)(this.NewRow()));
                rowIcomsCredential.ItemArray = new object[] {
                        null,
                        Username,
                        Password};
                this.Rows.Add(rowIcomsCredential);
                return rowIcomsCredential;
            }
            
            public IcomsCredential FindById(int Id) {
                return ((IcomsCredential)(this.Rows.Find(new object[] {
                            Id})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                IcomsCredentialsDataTable cln = ((IcomsCredentialsDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new IcomsCredentialsDataTable();
            }
            
            internal void InitVars() {
                this.columnId = this.Columns["icId"];
                this.columnUsername = this.Columns["icUsername"];
                this.columnPassword = this.Columns["icPassword"];
            }
            
            private void InitClass() {
                this.columnId = new DataColumn("icId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnId);
                this.columnUsername = new DataColumn("icUsername", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUsername);
                this.columnPassword = new DataColumn("icPassword", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPassword);
                this.Constraints.Add(new UniqueConstraint("ApplicationGroupSchemaKey2", new DataColumn[] {
                                this.columnId}, true));
                this.columnId.AutoIncrement = true;
                this.columnId.AllowDBNull = false;
                this.columnId.ReadOnly = true;
                this.columnId.Unique = true;
                this.columnUsername.AllowDBNull = false;
                this.columnPassword.AllowDBNull = false;
            }
            
            public IcomsCredential NewIcomsCredential() {
                return ((IcomsCredential)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new IcomsCredential(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(IcomsCredential);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.IcomsCredentialChanged != null)) {
                    this.IcomsCredentialChanged(this, new IcomsCredentialChangeEvent(((IcomsCredential)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.IcomsCredentialChanging != null)) {
                    this.IcomsCredentialChanging(this, new IcomsCredentialChangeEvent(((IcomsCredential)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.IcomsCredentialDeleted != null)) {
                    this.IcomsCredentialDeleted(this, new IcomsCredentialChangeEvent(((IcomsCredential)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.IcomsCredentialDeleting != null)) {
                    this.IcomsCredentialDeleting(this, new IcomsCredentialChangeEvent(((IcomsCredential)(e.Row)), e.Action));
                }
            }
            
            public void RemoveIcomsCredential(IcomsCredential row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IcomsCredential : DataRow {
            
            private IcomsCredentialsDataTable tableIcomsCredentials;
            
            internal IcomsCredential(DataRowBuilder rb) : 
                    base(rb) {
                this.tableIcomsCredentials = ((IcomsCredentialsDataTable)(this.Table));
            }
            
            public int Id {
                get {
                    return ((int)(this[this.tableIcomsCredentials.IdColumn]));
                }
                set {
                    this[this.tableIcomsCredentials.IdColumn] = value;
                }
            }
            
            public string Username {
                get {
                    return ((string)(this[this.tableIcomsCredentials.UsernameColumn]));
                }
                set {
                    this[this.tableIcomsCredentials.UsernameColumn] = value;
                }
            }
            
            public string Password {
                get {
                    return ((string)(this[this.tableIcomsCredentials.PasswordColumn]));
                }
                set {
                    this[this.tableIcomsCredentials.PasswordColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class IcomsCredentialChangeEvent : EventArgs {
            
            private IcomsCredential eventRow;
            
            private DataRowAction eventAction;
            
            public IcomsCredentialChangeEvent(IcomsCredential row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public IcomsCredential Row {
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
        public class UsersDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnusrId;
            
            private DataColumn columnusrUserName;
            
            private DataColumn columnusrGroupId;
            
            internal UsersDataTable() : 
                    base("tblUser") {
                this.InitClass();
            }
            
            internal UsersDataTable(DataTable table) : 
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
            
            internal DataColumn usrIdColumn {
                get {
                    return this.columnusrId;
                }
            }
            
            internal DataColumn usrUserNameColumn {
                get {
                    return this.columnusrUserName;
                }
            }
            
            internal DataColumn usrGroupIdColumn {
                get {
                    return this.columnusrGroupId;
                }
            }
            
            public User this[int index] {
                get {
                    return ((User)(this.Rows[index]));
                }
            }
            
            public event UserChangeEventHandler UserChanged;
            
            public event UserChangeEventHandler UserChanging;
            
            public event UserChangeEventHandler UserDeleted;
            
            public event UserChangeEventHandler UserDeleting;
            
            public void AddUser(User row) {
                this.Rows.Add(row);
            }
            
            public User AddUser(string usrUserName, int usrGroupId) {
                User rowUser = ((User)(this.NewRow()));
                rowUser.ItemArray = new object[] {
                        null,
                        usrUserName,
                        usrGroupId};
                this.Rows.Add(rowUser);
                return rowUser;
            }
            
            public User FindByusrId(int usrId) {
                return ((User)(this.Rows.Find(new object[] {
                            usrId})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                UsersDataTable cln = ((UsersDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new UsersDataTable();
            }
            
            internal void InitVars() {
                this.columnusrId = this.Columns["usrId"];
                this.columnusrUserName = this.Columns["usrUserName"];
                this.columnusrGroupId = this.Columns["usrGroupId"];
            }
            
            private void InitClass() {
                this.columnusrId = new DataColumn("usrId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnusrId);
                this.columnusrUserName = new DataColumn("usrUserName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnusrUserName);
                this.columnusrGroupId = new DataColumn("usrGroupId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnusrGroupId);
                this.Constraints.Add(new UniqueConstraint("ApplicationGroupSchemaKey5", new DataColumn[] {
                                this.columnusrId}, true));
                this.columnusrId.AutoIncrement = true;
                this.columnusrId.AllowDBNull = false;
                this.columnusrId.ReadOnly = true;
                this.columnusrId.Unique = true;
                this.columnusrUserName.AllowDBNull = false;
                this.columnusrGroupId.AllowDBNull = false;
            }
            
            public User NewUser() {
                return ((User)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new User(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(User);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.UserChanged != null)) {
                    this.UserChanged(this, new UserChangeEvent(((User)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.UserChanging != null)) {
                    this.UserChanging(this, new UserChangeEvent(((User)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.UserDeleted != null)) {
                    this.UserDeleted(this, new UserChangeEvent(((User)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.UserDeleting != null)) {
                    this.UserDeleting(this, new UserChangeEvent(((User)(e.Row)), e.Action));
                }
            }
            
            public void RemoveUser(User row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class User : DataRow {
            
            private UsersDataTable tableUsers;
            
            internal User(DataRowBuilder rb) : 
                    base(rb) {
                this.tableUsers = ((UsersDataTable)(this.Table));
            }
            
            public int usrId {
                get {
                    return ((int)(this[this.tableUsers.usrIdColumn]));
                }
                set {
                    this[this.tableUsers.usrIdColumn] = value;
                }
            }
            
            public string usrUserName {
                get {
                    return ((string)(this[this.tableUsers.usrUserNameColumn]));
                }
                set {
                    this[this.tableUsers.usrUserNameColumn] = value;
                }
            }
            
            public int usrGroupId {
                get {
                    return ((int)(this[this.tableUsers.usrGroupIdColumn]));
                }
                set {
                    this[this.tableUsers.usrGroupIdColumn] = value;
                }
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class UserChangeEvent : EventArgs {
            
            private User eventRow;
            
            private DataRowAction eventAction;
            
            public UserChangeEvent(User row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public User Row {
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
