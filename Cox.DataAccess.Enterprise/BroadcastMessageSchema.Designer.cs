﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace Cox.DataAccess.Enterprise {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.ComponentModel.ToolboxItem(true)]
    [System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
    [System.Xml.Serialization.XmlRootAttribute("BroadcastMessageSchema")]
    [System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
    public partial class BroadcastMessageSchema : System.Data.DataSet {
        
        private BroadcastMessageDataTable tableBroadcastMessage;
        
        private System.Data.SchemaSerializationMode _schemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public BroadcastMessageSchema() {
            this.BeginInit();
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            base.Relations.CollectionChanged += schemaChangedHandler;
            this.EndInit();
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected BroadcastMessageSchema(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context, false) {
            if ((this.IsBinarySerialized(info, context) == true)) {
                this.InitVars(false);
                System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                this.Tables.CollectionChanged += schemaChangedHandler1;
                this.Relations.CollectionChanged += schemaChangedHandler1;
                return;
            }
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((this.DetermineSchemaSerializationMode(info, context) == System.Data.SchemaSerializationMode.IncludeSchema)) {
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["BroadcastMessage"] != null)) {
                    base.Tables.Add(new BroadcastMessageDataTable(ds.Tables["BroadcastMessage"]));
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
                this.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public BroadcastMessageDataTable BroadcastMessage {
            get {
                return this.tableBroadcastMessage;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.BrowsableAttribute(true)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public override System.Data.SchemaSerializationMode SchemaSerializationMode {
            get {
                return this._schemaSerializationMode;
            }
            set {
                this._schemaSerializationMode = value;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new System.Data.DataTableCollection Tables {
            get {
                return base.Tables;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new System.Data.DataRelationCollection Relations {
            get {
                return base.Relations;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void InitializeDerivedDataSet() {
            this.BeginInit();
            this.InitClass();
            this.EndInit();
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override System.Data.DataSet Clone() {
            BroadcastMessageSchema cln = ((BroadcastMessageSchema)(base.Clone()));
            cln.InitVars();
            cln.SchemaSerializationMode = this.SchemaSerializationMode;
            return cln;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void ReadXmlSerializable(System.Xml.XmlReader reader) {
            if ((this.DetermineSchemaSerializationMode(reader) == System.Data.SchemaSerializationMode.IncludeSchema)) {
                this.Reset();
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXml(reader);
                if ((ds.Tables["BroadcastMessage"] != null)) {
                    base.Tables.Add(new BroadcastMessageDataTable(ds.Tables["BroadcastMessage"]));
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
                this.ReadXml(reader);
                this.InitVars();
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new System.Xml.XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new System.Xml.XmlTextReader(stream), null);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars() {
            this.InitVars(true);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars(bool initTable) {
            this.tableBroadcastMessage = ((BroadcastMessageDataTable)(base.Tables["BroadcastMessage"]));
            if ((initTable == true)) {
                if ((this.tableBroadcastMessage != null)) {
                    this.tableBroadcastMessage.InitVars();
                }
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitClass() {
            this.DataSetName = "BroadcastMessageSchema";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/BroadcastMessageSchema.xsd";
            this.EnforceConstraints = true;
            this.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.tableBroadcastMessage = new BroadcastMessageDataTable();
            base.Tables.Add(this.tableBroadcastMessage);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerializeBroadcastMessage() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(System.Xml.Schema.XmlSchemaSet xs) {
            BroadcastMessageSchema ds = new BroadcastMessageSchema();
            System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
            System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
            xs.Add(ds.GetSchemaSerializable());
            System.Xml.Schema.XmlSchemaAny any = new System.Xml.Schema.XmlSchemaAny();
            any.Namespace = ds.Namespace;
            sequence.Items.Add(any);
            type.Particle = sequence;
            return type;
        }
        
        public delegate void BroadcastMessageRowChangeEventHandler(object sender, BroadcastMessageRowChangeEvent e);
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [System.Serializable()]
        [System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class BroadcastMessageDataTable : System.Data.DataTable, System.Collections.IEnumerable {
            
            private System.Data.DataColumn columnBroadcastMessageId;
            
            private System.Data.DataColumn columnApplicationTypeId;
            
            private System.Data.DataColumn columnChannelId;
            
            private System.Data.DataColumn columnAuthorizationUserId;
            
            private System.Data.DataColumn columnStartDate;
            
            private System.Data.DataColumn columnEndDate;
            
            private System.Data.DataColumn columnCreateDate;
            
            private System.Data.DataColumn columnValue;
            
            private System.Data.DataColumn columnLogin;
            
            private System.Data.DataColumn columnFirstName;
            
            private System.Data.DataColumn columnMiddleName;
            
            private System.Data.DataColumn columnLastName;
            
            private System.Data.DataColumn columnEmailAddress;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageDataTable() {
                this.TableName = "BroadcastMessage";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal BroadcastMessageDataTable(System.Data.DataTable table) {
                this.TableName = table.TableName;
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
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected BroadcastMessageDataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn BroadcastMessageIdColumn {
                get {
                    return this.columnBroadcastMessageId;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn ApplicationTypeIdColumn {
                get {
                    return this.columnApplicationTypeId;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn ChannelIdColumn {
                get {
                    return this.columnChannelId;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn AuthorizationUserIdColumn {
                get {
                    return this.columnAuthorizationUserId;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn StartDateColumn {
                get {
                    return this.columnStartDate;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn EndDateColumn {
                get {
                    return this.columnEndDate;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn CreateDateColumn {
                get {
                    return this.columnCreateDate;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn ValueColumn {
                get {
                    return this.columnValue;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn LoginColumn {
                get {
                    return this.columnLogin;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn FirstNameColumn {
                get {
                    return this.columnFirstName;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn MiddleNameColumn {
                get {
                    return this.columnMiddleName;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn LastNameColumn {
                get {
                    return this.columnLastName;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn EmailAddressColumn {
                get {
                    return this.columnEmailAddress;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageRow this[int index] {
                get {
                    return ((BroadcastMessageRow)(this.Rows[index]));
                }
            }
            
            public event BroadcastMessageRowChangeEventHandler BroadcastMessageRowChanging;
            
            public event BroadcastMessageRowChangeEventHandler BroadcastMessageRowChanged;
            
            public event BroadcastMessageRowChangeEventHandler BroadcastMessageRowDeleting;
            
            public event BroadcastMessageRowChangeEventHandler BroadcastMessageRowDeleted;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AddBroadcastMessageRow(BroadcastMessageRow row) {
                this.Rows.Add(row);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageRow AddBroadcastMessageRow(int ApplicationTypeId, int ChannelId, int AuthorizationUserId, System.DateTime StartDate, System.DateTime EndDate, System.DateTime CreateDate, string Value, string Login, string FirstName, string MiddleName, string LastName, string EmailAddress) {
                BroadcastMessageRow rowBroadcastMessageRow = ((BroadcastMessageRow)(this.NewRow()));
                rowBroadcastMessageRow.ItemArray = new object[] {
                        null,
                        ApplicationTypeId,
                        ChannelId,
                        AuthorizationUserId,
                        StartDate,
                        EndDate,
                        CreateDate,
                        Value,
                        Login,
                        FirstName,
                        MiddleName,
                        LastName,
                        EmailAddress};
                this.Rows.Add(rowBroadcastMessageRow);
                return rowBroadcastMessageRow;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageRow FindByBroadcastMessageId(int BroadcastMessageId) {
                return ((BroadcastMessageRow)(this.Rows.Find(new object[] {
                            BroadcastMessageId})));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override System.Data.DataTable Clone() {
                BroadcastMessageDataTable cln = ((BroadcastMessageDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Data.DataTable CreateInstance() {
                return new BroadcastMessageDataTable();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnBroadcastMessageId = base.Columns["BroadcastMessageId"];
                this.columnApplicationTypeId = base.Columns["ApplicationTypeId"];
                this.columnChannelId = base.Columns["ChannelId"];
                this.columnAuthorizationUserId = base.Columns["AuthorizationUserId"];
                this.columnStartDate = base.Columns["StartDate"];
                this.columnEndDate = base.Columns["EndDate"];
                this.columnCreateDate = base.Columns["CreateDate"];
                this.columnValue = base.Columns["Value"];
                this.columnLogin = base.Columns["Login"];
                this.columnFirstName = base.Columns["FirstName"];
                this.columnMiddleName = base.Columns["MiddleName"];
                this.columnLastName = base.Columns["LastName"];
                this.columnEmailAddress = base.Columns["EmailAddress"];
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnBroadcastMessageId = new System.Data.DataColumn("BroadcastMessageId", typeof(int), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnBroadcastMessageId);
                this.columnApplicationTypeId = new System.Data.DataColumn("ApplicationTypeId", typeof(int), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnApplicationTypeId);
                this.columnChannelId = new System.Data.DataColumn("ChannelId", typeof(int), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnChannelId);
                this.columnAuthorizationUserId = new System.Data.DataColumn("AuthorizationUserId", typeof(int), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnAuthorizationUserId);
                this.columnStartDate = new System.Data.DataColumn("StartDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnStartDate);
                this.columnEndDate = new System.Data.DataColumn("EndDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnEndDate);
                this.columnCreateDate = new System.Data.DataColumn("CreateDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnCreateDate);
                this.columnValue = new System.Data.DataColumn("Value", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnValue);
                this.columnLogin = new System.Data.DataColumn("Login", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnLogin);
                this.columnFirstName = new System.Data.DataColumn("FirstName", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnFirstName);
                this.columnMiddleName = new System.Data.DataColumn("MiddleName", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnMiddleName);
                this.columnLastName = new System.Data.DataColumn("LastName", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnLastName);
                this.columnEmailAddress = new System.Data.DataColumn("EmailAddress", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnEmailAddress);
                this.Constraints.Add(new System.Data.UniqueConstraint("BroadcastMessageSchemaKey1", new System.Data.DataColumn[] {
                                this.columnBroadcastMessageId}, true));
                this.columnBroadcastMessageId.AutoIncrement = true;
                this.columnBroadcastMessageId.AllowDBNull = false;
                this.columnBroadcastMessageId.ReadOnly = true;
                this.columnBroadcastMessageId.Unique = true;
                this.columnApplicationTypeId.AllowDBNull = false;
                this.columnChannelId.AllowDBNull = false;
                this.columnAuthorizationUserId.AllowDBNull = false;
                this.columnCreateDate.AllowDBNull = false;
                this.columnValue.AllowDBNull = false;
                this.columnLogin.AllowDBNull = false;
                this.columnFirstName.AllowDBNull = false;
                this.columnLastName.AllowDBNull = false;
                this.columnEmailAddress.AllowDBNull = false;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageRow NewBroadcastMessageRow() {
                return ((BroadcastMessageRow)(this.NewRow()));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder) {
                return new BroadcastMessageRow(builder);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Type GetRowType() {
                return typeof(BroadcastMessageRow);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.BroadcastMessageRowChanged != null)) {
                    this.BroadcastMessageRowChanged(this, new BroadcastMessageRowChangeEvent(((BroadcastMessageRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.BroadcastMessageRowChanging != null)) {
                    this.BroadcastMessageRowChanging(this, new BroadcastMessageRowChangeEvent(((BroadcastMessageRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.BroadcastMessageRowDeleted != null)) {
                    this.BroadcastMessageRowDeleted(this, new BroadcastMessageRowChangeEvent(((BroadcastMessageRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.BroadcastMessageRowDeleting != null)) {
                    this.BroadcastMessageRowDeleting(this, new BroadcastMessageRowChangeEvent(((BroadcastMessageRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemoveBroadcastMessageRow(BroadcastMessageRow row) {
                this.Rows.Remove(row);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet xs) {
                System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
                System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
                BroadcastMessageSchema ds = new BroadcastMessageSchema();
                xs.Add(ds.GetSchemaSerializable());
                System.Xml.Schema.XmlSchemaAny any1 = new System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                System.Xml.Schema.XmlSchemaAny any2 = new System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                System.Xml.Schema.XmlSchemaAttribute attribute1 = new System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                System.Xml.Schema.XmlSchemaAttribute attribute2 = new System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "BroadcastMessageDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                return type;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class BroadcastMessageRow : System.Data.DataRow {
            
            private BroadcastMessageDataTable tableBroadcastMessage;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal BroadcastMessageRow(System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tableBroadcastMessage = ((BroadcastMessageDataTable)(this.Table));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int BroadcastMessageId {
                get {
                    return ((int)(this[this.tableBroadcastMessage.BroadcastMessageIdColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.BroadcastMessageIdColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int ApplicationTypeId {
                get {
                    return ((int)(this[this.tableBroadcastMessage.ApplicationTypeIdColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.ApplicationTypeIdColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int ChannelId {
                get {
                    return ((int)(this[this.tableBroadcastMessage.ChannelIdColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.ChannelIdColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int AuthorizationUserId {
                get {
                    return ((int)(this[this.tableBroadcastMessage.AuthorizationUserIdColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.AuthorizationUserIdColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.DateTime StartDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableBroadcastMessage.StartDateColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'StartDate\' in table \'BroadcastMessage\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableBroadcastMessage.StartDateColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.DateTime EndDate {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableBroadcastMessage.EndDateColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'EndDate\' in table \'BroadcastMessage\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableBroadcastMessage.EndDateColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.DateTime CreateDate {
                get {
                    return ((System.DateTime)(this[this.tableBroadcastMessage.CreateDateColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.CreateDateColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string Value {
                get {
                    return ((string)(this[this.tableBroadcastMessage.ValueColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.ValueColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string Login {
                get {
                    return ((string)(this[this.tableBroadcastMessage.LoginColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.LoginColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string FirstName {
                get {
                    return ((string)(this[this.tableBroadcastMessage.FirstNameColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.FirstNameColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string MiddleName {
                get {
                    try {
                        return ((string)(this[this.tableBroadcastMessage.MiddleNameColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'MiddleName\' in table \'BroadcastMessage\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableBroadcastMessage.MiddleNameColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string LastName {
                get {
                    return ((string)(this[this.tableBroadcastMessage.LastNameColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.LastNameColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string EmailAddress {
                get {
                    return ((string)(this[this.tableBroadcastMessage.EmailAddressColumn]));
                }
                set {
                    this[this.tableBroadcastMessage.EmailAddressColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsStartDateNull() {
                return this.IsNull(this.tableBroadcastMessage.StartDateColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetStartDateNull() {
                this[this.tableBroadcastMessage.StartDateColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsEndDateNull() {
                return this.IsNull(this.tableBroadcastMessage.EndDateColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetEndDateNull() {
                this[this.tableBroadcastMessage.EndDateColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsMiddleNameNull() {
                return this.IsNull(this.tableBroadcastMessage.MiddleNameColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetMiddleNameNull() {
                this[this.tableBroadcastMessage.MiddleNameColumn] = System.Convert.DBNull;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class BroadcastMessageRowChangeEvent : System.EventArgs {
            
            private BroadcastMessageRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageRowChangeEvent(BroadcastMessageRow row, System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public BroadcastMessageRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}

#pragma warning restore 1591